using System;
using System.Diagnostics;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Gtk;
using Gdk;
using System.IO;

namespace beagleradio {

	public class WeatherService {

		public class WeatherPeriod {
			public string Period { get; set; }
			public string Title { get; set; }
			public string Icon { get; set; }
			public Pixbuf Image { get; set; }
			public string Forecast { get; set; }
			public string ForecastMetric { get; set; }
			public string Conditions { get; set; }
			public string High { get; set; }
			public string Low { get; set; }
			public string HighMetric { get; set; }
			public string LowMetric { get; set; }
		}

		private WebClient client;
		private Timer timer;
		private string imageName;
		private string state;
		private string city;
		private string key;

		public event EventHandler<string> Completed;
		public event EventHandler<string> Status;

		public DateTime LastSuccess { get; private set; }
		public DateTime LastAttempt { get; private set; }
		public List<WeatherPeriod> Forecasts { get; private set; }
		public bool IsBusy { get; set; }


		public WeatherService() {


			IsBusy = false;
			Forecasts = new List<WeatherPeriod>();
			client = new WebClient();
			client.DownloadStringCompleted += Client_DownloadStringCompleted;
			client.DownloadDataCompleted += Client_DownloadDataCompleted;
			LoadCredentials();
		}

		private void LoadCredentials() {
			try {
				
				FileStream file = new FileStream("weather.txt", FileMode.Open);
				StreamReader reader = new StreamReader(file);
				string line = reader.ReadLine();
				while (line != null) {
					line = line.Trim();
					if (!line.StartsWith("#")) {
						string value;
						if (Framework.ExtractKey(out value, line, "state:")) {
							Console.WriteLine("Weather state = '" + value + "'");
							state = value;
						}
						if (Framework.ExtractKey(out value, line, "city:")) {
							Console.WriteLine("Weather city = '" + value + "'");
							city = value;
						}
						if (Framework.ExtractKey(out value, line, "key:")) {
							Console.WriteLine("Weather key = '" + value + "'");
							key = value;
						}
					}
					line = reader.ReadLine();
				}
				reader.Close();
				reader.Dispose();
				file.Close();
				file.Dispose();

			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}
		}

		public void GetWeather(int timeout = 60000) {
			GetWeatherAsync(timeout);
			while (IsBusy) {
				Framework.GuiDelay();
			}
		}

		public void GetWeatherAsync(int timeout = 60000) {
			try {
				if (!IsBusy) {
					Console.WriteLine("Request for weather");
					if (key != "" && DateTime.Now.Subtract(LastAttempt).TotalMinutes >= 1.0) {
						//Console.WriteLine("Getting current weather report");
						if (Status != null) {
							Status(this, "Getting current weather report");
						}
						LastAttempt = DateTime.Now;
						IsBusy = true;
						//Uri uri = new Uri("http://api.wunderground.com/api/f94133718a01963c/forecast/q/"+state+"/"+city+".json");

						Uri uri = new Uri("http://api.wunderground.com/api/" + key + "/forecast/q/" + state + "/" + city + ".json");
						Console.WriteLine("Send weather request: " + uri.ToString());
						timer = new Timer(new TimerCallback(delegate(object s) {
							Close();
						}));
						timer.Change(timeout, -1);
						client.DownloadStringAsync(uri);
					} else {
						//Console.WriteLine("Can't request weather : key=" + key + ", minutes=" + DateTime.Now.Subtract(LastAttempt).TotalMinutes);
					}
				}
			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}
		}

		public void Close() {
			if (IsBusy) {
				client.CancelAsync();
			}
		}

		private bool CheckDownload(byte[] data) {

			if (data != null) {
				Pixbuf image = new Pixbuf(data);
				foreach (WeatherPeriod info in Forecasts) {
					if (info.Icon == imageName) {
						info.Image = image;
					}
				}
			}
			foreach (WeatherPeriod info in Forecasts) {
				if (info.Icon.Length > 0 && info.Image == null) {
					imageName = info.Icon;
					Console.WriteLine("Download image " + info.Icon);
					Uri uri = new Uri(info.Icon);
					client.DownloadDataAsync(uri);
					return true;
				}
			}
			return false;
		}

		private void Client_DownloadStringCompleted (object sender, DownloadStringCompletedEventArgs e)
		{
			try {
				timer.Dispose();
				timer = null;

				Console.WriteLine("Request for weather finished");

				if (e.Cancelled) {
					Console.WriteLine("Weather timed out");
					IsBusy = false;
					if (Status != null) {
						Status(this, "Timed out getting the weather");
					}
				} else {

					Console.WriteLine("Got weather reply");

					Forecasts.Clear();

					// Process the weather result
					string text = e.Result;

					WeatherPeriod info;
					bool isHigh = false;
					bool isLow = false;
					foreach (string line in text.Split('\n')) {
						string value;
						if (Framework.ExtractKey(out value, line, "\"period\"", ":", ",")) {
							info = null;
							foreach (WeatherPeriod test in Forecasts) {
								if (test.Period == value) {
									info = test;
									break;
								}
							}
							if (info == null) {
								info = new WeatherPeriod();
								info.Period = value;
								Forecasts.Add(info);
							}
						}
						if (line.Contains("\"high\":")) {
							isHigh = true;
							isLow = false;
						} else if (line.Contains("\"low\":")) {
							isHigh = false;
							isLow = true;
						}


						if (Framework.ExtractKey(out value, line, "\"icon_url\":", "\"", "\"")) info.Icon = value;
						if (Framework.ExtractKey(out value, line, "\"title\":", "\"", "\"")) info.Title = value;
						if (Framework.ExtractKey(out value, line, "\"title\":", "\"", "\"")) info.Title = value;
						if (Framework.ExtractKey(out value, line, "\"fcttext\":", "\"", "\"")) info.Forecast = value;
						if (Framework.ExtractKey(out value, line, "\"fcttext_metric\":", "\"", "\"")) info.ForecastMetric = value;
						if (Framework.ExtractKey(out value, line, "\"conditions\":", "\"", "\"")) info.Conditions = value;
						if (Framework.ExtractKey(out value, line, "\"fahrenheit\":", "\"", "\"")) {
							if (isHigh) info.High = value;
							if (isLow) info.Low = value;
						}
						if (Framework.ExtractKey(out value, line, "\"celsius\":", "\"", "\"")) {
							if (isHigh) info.HighMetric = value;
							if (isLow) info.LowMetric = value;
						}
					}

					if (Status != null) {
						Status(this, "Got " + Forecasts.Count + " forecasts");
					}
				}

				if (!CheckDownload(null)) {
					IsBusy = false;
					LastSuccess = DateTime.Now;
					Console.WriteLine("No images to download - finished");
				}

			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
				Console.WriteLine("Error - clear busy");
				IsBusy = false;
			}

			if (!IsBusy) {
				if (Completed != null) {
					Completed(this, null);
				}
			}
		}

		void Client_DownloadDataCompleted (object sender, DownloadDataCompletedEventArgs e)
		{
			try {

				Console.WriteLine("Finished download");

				if (e.Cancelled) {
					IsBusy = false;
				} else {
					if (!CheckDownload(e.Result)) {
						Console.WriteLine("No images to download - finished");
						LastSuccess = DateTime.Now;
						IsBusy = false;
					}
				}
			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}

			if (!IsBusy) {
				if (Completed != null) {
					Completed(this, null);
				}
			}
		}
	}
}

