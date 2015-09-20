using System;
using Gtk;
using Gdk;
using System.Threading;
using beagleradio;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

public partial class MainWindow: Gtk.Window {

	public enum ProgramState {
		Recognizing,
		Radio,
		Intercom}

	;

	private Timer timer;
	private Timer background;
	private VolumeService volumeService;
	private RadioPlayer radio;
	private SpeechRecognition recognizer;
	private FindArtwork findArtwork;
	private TTSRecordings ttsRecordings;
	private SoundPlayer player;
	private WirelessWrapper wireless;
	private NetworkWorker network;
	private EventList clockEvents;
	private AlarmList clockAlarms;
	private string artist = "";
	private string song = "";
	private string ipAddress = "";
	private string prevIpAddress = "";
	private List<StationInfo> stations;
	private int stationIndex = 0;
	private int prevVolume = -1;
	private StationInfo station;
	private StationInfo playingStation;
	private int wifiState = 0;
	private int lastMinute = -1;
	private Pixbuf[] images;
	private string status = "";
	private string lastStatus = "";
	private string lastRecognized = "";
	private string lastNoise = "";
	private string lastBuffer = "";
	private WirelessInfo activeConnection;
	private string activeAlarm = "";
	private string nextAlarm = "";
	private bool isAlarmActive = false;
	private List<string> wordList;

	private const int WIFI_0 = 0;
	private const int WIFI_50 = 1;
	private const int WIFI_80 = 2;
	private const int WIFI_100 = 3;
	private const int WIFI_SEARCH1 = 4;
	private const int WIFI_SEARCH2 = 5;
	private const int UNKNOWN_MUSIC = 6;


	public MainWindow() : base(Gtk.WindowType.Toplevel) {
		Build();

		notebook.CurrentPage = 1;
		volume.Percentage = 70;

		Gdk.Color bk = new Gdk.Color(17, 118, 207);
		Gdk.Color fg = new Gdk.Color(255, 255, 255);
		this.ModifyBg(StateType.Normal, bk);
		this.ModifyFg(StateType.Normal, fg);
		ctlArtwork.ModifyBg(StateType.Normal, bk);
		ctlArtwork.ModifyFg(StateType.Normal, fg);
		ctlDrawingMeta.ModifyBg(StateType.Normal, bk);
		ctlDrawingMeta.ModifyFg(StateType.Normal, fg);
		drawWiFi.ModifyBg(StateType.Normal, bk);
		drawWiFi.ModifyFg(StateType.Normal, fg);
		eventLeft.ModifyBg(StateType.Normal, bk);
		eventLeft.ModifyFg(StateType.Normal, fg);
		eventRight.ModifyBg(StateType.Normal, bk);
		eventRight.ModifyFg(StateType.Normal, fg);
		analogclock.ModifyBg(StateType.Normal, bk);
		analogclock.ModifyFg(StateType.Normal, fg);
		notebook.ModifyBg(StateType.Normal, bk);
		notebook.ModifyFg(StateType.Normal, fg);
		notebook.ModifyBg(StateType.Active, bk);
		notebook.ModifyFg(StateType.Active, fg);
		volume.ModifyBg(StateType.Normal, bk);
		volume.ModifyFg(StateType.Normal, fg);
		labelClockInfo.ModifyBg(StateType.Normal, bk);
		labelClockInfo.ModifyFg(StateType.Normal, fg);
		labelClockInfo.ModifyFont(Pango.FontDescription.FromString("Serif 12"));
		weatherview.ChangeColors(fg, bk);

		loadImages();

		wordList = new List<string>();
		if (File.Exists("missing_words.txt")) {
			FileStream file = new FileStream("missing_words.txt", FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(file);
			string line = reader.ReadLine();
			while (line != null) {
				wordList.Add(line);
				line = reader.ReadLine();
			}
			reader.Close();
			reader.Dispose();
			file.Close();
			file.Dispose();
		}

		clockEvents = new EventList();
		clockEvents.Load();
		Console.WriteLine("Events ******************");
		foreach (EventItem item in clockEvents.Events) {
			Console.WriteLine("  " + item.Name);
		}
		Console.WriteLine("*************************");
		Console.WriteLine("Loaded " + clockEvents.Events.Count + " events");
		clockAlarms = new AlarmList();
		clockAlarms.Load();
		Console.WriteLine("Alarms ******************");
		foreach (AlarmItem item in clockAlarms.Alarms) {
			Console.WriteLine("  " + item.Name);
		}
		Console.WriteLine("*************************");
		Console.WriteLine("Loaded " + clockAlarms.Alarms.Count + " alarms");

		wireless = new WirelessWrapper();
		wireless.Status += Wireless_Status;

		network = new NetworkWorker();
		network.Start();

		findArtwork = new FindArtwork();
		volumeService = new VolumeService();

		stations = new List<StationInfo>();
		LoadStations();

		ttsRecordings = new TTSRecordings();
		ttsRecordings.MissingWords += TtsRecordings_MissingWords;
		ttsRecordings.Status += TtsRecordings_Status;

		player = new SoundPlayer();

		radio = new RadioPlayer();
		radio.MetadataReceived += Player_MetadataReceived;

		recognizer = new SpeechRecognition();
		recognizer.StatusChanged += Recognizer_StatusChanged;
		recognizer.Recognized += Recognizer_Recognized;
		//recognizer.LogMessage += Recognizer_LogMessage;
		Console.WriteLine("Starting recognizer");
		recognizer.Start();

		analogclock.TimeChanged += Analogclock_TimeChanged;

		timer = new Timer(new TimerCallback(timerTick));
		timer.Change(250, 250);

		background = new Timer(new TimerCallback(backgroundTick));
		background.Change(2000, 2000);

		intercomView.Start();
		timedimage.Start();
	}

	void Analogclock_TimeChanged (object sender, DateTime e)
	{
		labelClockInfo.Text = DateTime.Now.ToString("dddd MMMM dd, yyyy hh:mm:ss tt");
	}

	void Recognizer_LogMessage (object sender, string e) {
		Gtk.Application.Invoke(delegate {
			try {
				Console.WriteLine("Recognizer: " + e);
			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}
		});
	}

	void Recognizer_Recognized (object sender, string e) {
		Gtk.Application.Invoke(delegate {
			try {
				Console.WriteLine("recognized " + e);
				if (e.Contains("what time is it")) {
					SpeakTime();
				} else if (e.Contains("whats happening")) {
					SpeakEvents();
				} else if (e.Contains("weather")) {
					SpeakWeather(false);
				} else if (e.Contains("night") || e.Contains("sleep")) {
					NightMode();
				} else if (e.Contains("wake") || e.Contains("morning")) {
					DayMode();
				}
			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}
		});
	}

	void Wireless_Status (object sender, string e) {
		Gtk.Application.Invoke(delegate {
			try {
				Console.WriteLine("Network: " + e);
			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}
		});
	}

	void TtsRecordings_Status (object sender, string e) {
		Gtk.Application.Invoke(delegate {
			try {
				Console.WriteLine("TTS: " + e);
			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}
		});
	}

	void TtsRecordings_MissingWords (object sender, List<string> e) {
		Gtk.Application.Invoke(delegate {
			try {
				foreach (string word in e) {
					if (!wordList.Contains(word)) {
						FileStream file = new FileStream("missing_words.txt", FileMode.Append, FileAccess.Write);
						StreamWriter writer = new StreamWriter(file);
						writer.WriteLine(word);
						writer.Close();
						writer.Dispose();
						file.Close();
						file.Dispose();
						wordList.Add(word);
					}
				}
			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}
		});
	}

	void LightsOff () {
		volumeService.SetLcdBrightness(0);
	}

	void LightsOn () {
		volumeService.SetLcdBrightness(100);
	}

	void SpeakTime () {
		isAlarmActive = true;
		volumeService.SetUserLedBrightness(100);
		ChangeTab(1);
		DateTime curr = DateTime.Now;
		string timeString = "The current time is " + Framework.TimeToWords(curr) + ".";
		Console.WriteLine("Playing " + timeString);
		status = "Playing";
		ttsRecordings.Build("time.raw", timeString);
		player.Play("time.raw");
		volumeService.SetUserLedBrightness(0);
		isAlarmActive = false;
	}

	void SpeakWeather (bool isSpeakAll) {
		isAlarmActive = true;
		volumeService.SetUserLedBrightness(100);
		string message = "Weather is not available at this time";
		if (weatherview.Weather.Forecasts.Count == 0) {
			ttsRecordings.Build("temp.raw", message);
			status = "Playing";
			player.Play("temp.raw");
		} else {
			message = "";
			ChangeTab(3);
			int index = 0;
			foreach (WeatherService.WeatherPeriod info in weatherview.Weather.Forecasts) {
				weatherview.SelectedForecast = index++;
				weatherview.QueueDraw();
				message = "The weather forecast for " + info.Title + " is " + info.Forecast + ". ";
				ttsRecordings.Build("temp.raw", message);
				status = "Playing";
				player.Play("temp.raw");
				if (!isSpeakAll) break;
			}
		}
		volumeService.SetUserLedBrightness(0);
		isAlarmActive = false;
	}

	void SpeakEvents () {
		isAlarmActive = true;
		volumeService.SetUserLedBrightness(100);
		ChangeTab(1);
		List<EventItem> active = clockEvents.FindActive(DateTime.Now);
		if (active.Count == 0) {
			ttsRecordings.Build("temp.raw", "There are no events for today");
			player.Play("temp.raw");
		} else {
			foreach (EventItem item in active) {
				ProcessActions(item.Actions);
			}
		}
		volumeService.SetUserLedBrightness(0);
		isAlarmActive = false;
	}

	void NightMode () {
		isAlarmActive = true;
		volumeService.SetUserLedBrightness(100);
		string message = "Good night";
		ttsRecordings.Build("temp.raw", message);
		player.Play("temp.raw");
		volumeService.SetUserLedBrightness(0);
		LightsOff();
		isAlarmActive = false;
	}

	void DayMode () {
		isAlarmActive = true;
		volumeService.SetUserLedBrightness(100);
		LightsOn();
		ChangeTab(1);
		string message = "Good morning";
		ttsRecordings.Build("temp.raw", message);
		player.Play("temp.raw");
		volumeService.SetUserLedBrightness(0);
		isAlarmActive = false;
	}

	void ProcessActions (List<string> actions) {
		Console.WriteLine("Processing " + actions.Count + " actions.");
		ProcessAction proc = new ProcessAction(weatherview.Weather, clockEvents);
		proc.PlayRadio += (object sender, string e) => PlayRadioStation(e);
		proc.ChangeTab += (object sender, int e) => ChangeTab(e);
		proc.SetVolume += (object sender, int e) => ChangeVolume(e);
		proc.SpeakText += (object sender, string e) => SpeakText(e);
		proc.Process(actions);
	}

	void SpeakText (string text) {
		ttsRecordings.Build("temp.raw", text);
		player.Play("temp.raw");
	}

	void PlayRadioStation (string stationCallLetters) {
		for (stationIndex = 1; stationIndex < stations.Count; stationIndex++) {
			if (stations[stationIndex].CallLetters == stationCallLetters) {
				break;
			}
		}
		if (stationIndex == stations.Count) stationIndex = 0;
		station = stations[stationIndex];
		ctlDrawingMeta.QueueDraw();
	}

	void ChangeVolume (int percentage) {
		volume.Percentage = percentage;
		volume.QueueDraw();
		if (!volumeService.IsBusy) {
			prevVolume = percentage;
			volumeService.SetVolume(percentage);
		}
	}

	void ChangeTab (int index) {
		if (notebook.CurrentPage != index) {
			notebook.CurrentPage = index;
			tableTabs.QueueDraw();
		}
	}

	void Recognizer_StatusChanged (object sender, string e) {
		Gtk.Application.Invoke(delegate {
			try {
				status = e;
			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}
		});
	}

	private void loadImages () {
		images = new Pixbuf[7];

		images[WIFI_0] = Pixbuf.LoadFromResource("beagleradio.Properties.wifi-0.png");
		images[WIFI_50] = Pixbuf.LoadFromResource("beagleradio.Properties.wifi-50.png");
		images[WIFI_SEARCH1] = Pixbuf.LoadFromResource("beagleradio.Properties.wifi-search1.png");
		images[WIFI_SEARCH2] = Pixbuf.LoadFromResource("beagleradio.Properties.wifi-search2.png");
		images[WIFI_100] = Pixbuf.LoadFromResource("beagleradio.Properties.wifi-100.png");
		images[WIFI_80] = Pixbuf.LoadFromResource("beagleradio.Properties.wifi-80.png");
		images[UNKNOWN_MUSIC] = Pixbuf.LoadFromResource("beagleradio.Properties.unknown.png");
	}

	private void Cleanup () { 
		
		station = null;

		if (background != null) {
			background.Dispose();
			background = null;
		}
		if (timer != null) {
			timer.Dispose();
			timer = null;
		}

		network.Close();
		timedimage.Close();
		recognizer.Close();
		radio.Stop();
		player.Close();
		intercomView.Close();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a) {
		Cleanup();
		Application.Quit();
		a.RetVal = true;
	}

	protected void OnDestroyEvent (object sender, DestroyEventArgs a) {
		Cleanup();
	}

	private void LoadStations () {
		stations.Add(null);
		stations.Add(new StationInfo("http://provisioning.streamtheworld.com/pls/WRLTFMAAC.pls", "Radio Lightning 100", "WRLT FM", "Nashville, TN"));
		stations.Add(new StationInfo("http://wpln.streamguys.org/wplnfm.mp3.m3u", "Nashville Public Radio", "WPLN FM", "Nashville, TN"));
	}

	void Player_MetadataReceived (object sender, string e) {
		Gtk.Application.Invoke(delegate {
			try {
				Console.WriteLine(e);
				int index = e.IndexOf("StreamTitle='");
				if (index > 0) {
					index += 13;
					int endIndex = e.IndexOf("';", index);
					if (endIndex > 0) {
						e = e.Substring(index, endIndex - index);
						index = e.IndexOf(" - ");
						if (index > 0) {
							artist = e.Substring(0, index);
							song = e.Substring(index + 3);
							Console.WriteLine("Search for artist " + artist + ", song " + song);
							findArtwork.Find(artist, song);
							ctlDrawingMeta.QueueDraw();
						}
					}
				}
			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}
		});
	}

	protected void backgroundTick (object sender) {
		Gtk.Application.Invoke(delegate {

			try {

				string test = "";
				if (recognizer.TimeRecognized != DateTime.MinValue) {
					test = recognizer.TimeRecognized.ToShortTimeString() + ": " + recognizer.KeywordRecognized;
				}
				if (test != lastRecognized || 
					recognizer.Noise != lastNoise || 
					recognizer.BufferSize != lastBuffer ||
					status != lastStatus) {
					drawWiFi.QueueDraw();
				}

				activeConnection = network.WiFi;
				ipAddress = network.IpAddress;

				if (prevIpAddress != ipAddress) {
					Console.WriteLine("IP Address changed to " + ipAddress);
					drawWiFi.QueueDraw();
				}
				prevIpAddress = ipAddress;

				if (ipAddress != "") {
					if (DateTime.Now.Subtract(weatherview.Weather.LastSuccess).TotalHours > 4) {
						weatherview.UpdateWeather();
					}
				}

				if (intercomView.IsBusy) {
					ChangeTab(4);
					stationIndex = 0;
					station = null;
				}

				if (recognizer.Status == "Ready" && station != null && playingStation == null) {
					if (ipAddress.Length > 0) {
						status = "Radio " + station.CallLetters;
						Console.WriteLine("Start playing radio station " + station.CallLetters);
						radio.Start(station.Url);
						playingStation = station;
					}
				} else if (playingStation != null && playingStation != station) {
					Console.WriteLine("Start playing radio station " + playingStation.CallLetters);
					radio.Stop();
					playingStation = null;
					findArtwork.Artwork = null;
					artist = "";
					song = "";
					status = "";
					ctlDrawingMeta.QueueDraw();
				} else if (recognizer.Status == "Ready" && !isAlarmActive && station == null && playingStation == null && !intercomView.IsBusy) {
					Console.WriteLine("Recognizer is ready and idle - start listening");
					recognizer.StartListening();
				} else if (recognizer.Status != "Ready" && (station != null || isAlarmActive || intercomView.IsBusy)) {
					recognizer.StopListening();
				}

				if (DateTime.Now > DateTime.Parse("08/01/2015")) {

					analogclock.AutoUpdate = true;

					if (lastMinute != DateTime.Now.Minute) {
						lastMinute = DateTime.Now.Minute;

						List<AlarmItem> alarms = clockAlarms.FindOn(DateTime.Now);
						activeAlarm = "";
						foreach (AlarmItem item in alarms) {
							activeAlarm += item.Name + " ";
						}
						if (alarms.Count > 0) {
							isAlarmActive = true;
							recognizer.StopListening();
							volumeService.SetUserLedBrightness(100);
							LightsOn();
							foreach (AlarmItem item in alarms) {
								ProcessActions(item.OnActions);
							}
							volumeService.SetUserLedBrightness(0);
							isAlarmActive = false;
						}

						alarms = clockAlarms.FindOff(DateTime.Now);
						activeAlarm = "";
						foreach (AlarmItem item in alarms) {
							activeAlarm += item.Name + " ";
						}
						if (alarms.Count > 0) {
							isAlarmActive = true;
							recognizer.StopListening();
							volumeService.SetUserLedBrightness(100);
							foreach (AlarmItem item in alarms) {
								ProcessActions(item.OffActions);
							}
							volumeService.SetUserLedBrightness(0);
							isAlarmActive = false;
						}

						nextAlarm = "";
						DateTime nextTime = clockAlarms.NextActiveAlarm();
						if (nextTime != DateTime.MaxValue) {
							nextAlarm = nextTime.ToShortDateString() + " " + nextTime.ToShortTimeString() + ":";
							alarms = clockAlarms.FindOn(nextTime);
							foreach (AlarmItem item in alarms) {
								nextAlarm += " " + item.Name;
							}
						}
					}

					System.GC.Collect();
				}


			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}
		});
	}

	protected void timerTick (object sender) {
		Gtk.Application.Invoke(delegate {

			try {

				wifiState = (wifiState + 1) % 6;

				Pixbuf image = null;
				if (activeConnection == null) {
					switch (wifiState) {
						case 0:
							image = images[WIFI_0];
							break;
						case 1:
							image = images[WIFI_50];
							break;
						case 2:
							image = images[WIFI_SEARCH1];
							break;
						case 3:
							image = images[WIFI_SEARCH2];
							break;
						case 4:
							image = images[WIFI_SEARCH1];
							break;
						case 5:
							image = images[WIFI_50];
							break;
					}
				} else {
					if (activeConnection.Signal > 90) {
						image = images[WIFI_100];
					} else if (activeConnection.Signal > 80) {
						image = images[WIFI_80];
					} else if (activeConnection.Signal > 70) {
						image = images[WIFI_50];
					} else {
						image = images[WIFI_0];
					}
				}
				if (imageTabWiFi.Pixbuf != image) {
					imageTabWiFi.Pixbuf = image;
					imageTabWiFi.QueueDraw();
				}

				int newVolume = volume.Percentage;
				if (newVolume != prevVolume && !volumeService.IsBusy) {
					prevVolume = newVolume;
					volumeService.SetVolume(prevVolume);
				}

				if (findArtwork.Artwork != null && findArtwork.Artwork != ctlArtwork.Pixbuf) {
					ctlArtwork.Pixbuf = findArtwork.Artwork;
					ctlArtwork.QueueDraw();
				} else if (findArtwork.Artwork == null && ctlArtwork.Pixbuf != images[UNKNOWN_MUSIC]) {
					ctlArtwork.Pixbuf = images[UNKNOWN_MUSIC];
					ctlArtwork.QueueDraw();
				}

			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}
		});
	}

	protected void OnDrawWiFiExposeEvent (object o, ExposeEventArgs args) {

		Gdk.Color back = drawWiFi.Style.Background(StateType.Normal);
		Gdk.Color fore = drawWiFi.Style.Foreground(StateType.Normal);

		Drawable draw = drawWiFi.GdkWindow;
		Gdk.GC gc = new Gdk.GC(draw);

		try {
			gc.Foreground = back;
			gc.Background = back;

			int width;
			int height;
			draw.GetSize(out width, out height);
			draw.DrawRectangle(gc, true, 0, 0, width, height);

			gc.Foreground = fore;

			lastNoise = recognizer.Noise;
			lastRecognized = recognizer.TimeRecognized.ToShortTimeString() + ": " + recognizer.KeywordRecognized;
			lastBuffer = recognizer.BufferSize;
			lastStatus = status;

			String text = "IP: " + ipAddress + "\n" +
			              "Status: " + lastStatus + "\n" +
				          "Recognized: " + lastRecognized + "\n" +
            			  "Buffer: " + lastBuffer + "\n" +
            			  "Noise: " + lastNoise + "\n" +
			              "Active Alarm: " + activeAlarm + "\n" +
			              "Next Alarm: " + nextAlarm + "\n";

			if (activeConnection == null) {
				text += "WiFi: Not connected\n";
			} else {
				text += "WiFi: " + activeConnection.ESSID + "\n";
				text += "BSSID: " + activeConnection.BSSID + "\n";
				text += "Channel: " + activeConnection.Channel + "\n";
				text += "Signal: " + activeConnection.Signal + "\n";
				text += "Encryption: " + activeConnection.Encryption + "\n";
			}
			
			Pango.FontDescription font = Pango.FontDescription.FromString("Serif 10");
			Pango.Layout layout = drawWiFi.CreatePangoLayout(text);
			layout.FontDescription = font;
			layout.Width = Pango.Units.FromPixels(width);
			draw.DrawLayoutWithColors(gc, 0, 0, layout, fore, back);
			layout.Dispose();

		} catch (Exception ex) {
			Console.WriteLine(ex.Source);
			Console.WriteLine(ex.StackTrace);
		}

		gc.Dispose();
	}

	protected void OnCtlDrawingMetaExposeEvent (object o, ExposeEventArgs args) {

		Gdk.Color back = ctlDrawingMeta.Style.Background(StateType.Normal);
		Gdk.Color fore = ctlDrawingMeta.Style.Foreground(StateType.Normal);

		Drawable draw = ctlDrawingMeta.GdkWindow;
		Gdk.GC gc = new Gdk.GC(draw);
		try {
			gc.Foreground = back;
			gc.Background = back;

			int width;
			int height;
			draw.GetSize(out width, out height);
			draw.DrawRectangle(gc, true, 0, 0, width, height);

			gc.Foreground = fore;

			String stationName = "No station";
			if (station != null) {
				stationName = station.CallLetters + "\n" + station.Location; 
			}
			String text = stationName + "\n" +
			              artist + "\n" +
			              song;

			Pango.FontDescription font = Pango.FontDescription.FromString("Serif 14");
			Pango.Layout layout = ctlDrawingMeta.CreatePangoLayout(text);
			layout.FontDescription = font;
			layout.Width = Pango.Units.FromPixels(width);
			draw.DrawLayoutWithColors(gc, 0, 0, layout, fore, back);
			layout.Dispose();
		} catch (Exception ex) {
			Console.WriteLine(ex.Source);
			Console.WriteLine(ex.StackTrace);
		}

		gc.Dispose();
	}

	protected void OnTableTabsExposeEvent (object o, ExposeEventArgs args) {
		
		Color fore = Style.Foreground(StateType.Normal);

		Drawable draw = args.Event.Window;
		Gdk.GC gc = new Gdk.GC(draw);

		try {
			Rectangle tabRect;
			switch (notebook.CurrentPage) {
				case 0: // WiFi
					tabRect = imageTabWiFi.Allocation;
					break;
				case 1: // Clock
					tabRect = imageTabClock.Allocation;
					break;
				case 2: // Radio
					tabRect = imageTabRadio.Allocation;
					break;
				case 3: // weather
					tabRect = imageTabWeather.Allocation;
					break;
				default:
					tabRect = imageTabIntercom.Allocation;
					break;
			}

			Rectangle rect = notebook.Allocation;
			rect.Inflate(4, 4);
			tabRect.Inflate(4, 4);

			List<Gdk.Point> points = new List<Gdk.Point>();
			points.Add(new Point(rect.Left, rect.Top));
			points.Add(new Point(rect.Right, rect.Top));
			points.Add(new Point(rect.Right, tabRect.Top));
			points.Add(new Point(tabRect.Right, tabRect.Top));
			points.Add(new Point(tabRect.Right, tabRect.Bottom));
			points.Add(new Point(rect.Right, tabRect.Bottom));
			points.Add(new Point(rect.Right, rect.Bottom));
			points.Add(new Point(rect.Left, rect.Bottom));
			points.Add(new Point(rect.Left, rect.Top));

			gc.Foreground = fore;
			gc.Background = fore;
			gc.SetLineAttributes(4, LineStyle.Solid, CapStyle.Round, JoinStyle.Round);
			draw.DrawLines(gc, points.ToArray());

		} catch (Exception ex) {
			Console.WriteLine(ex.Source);
			Console.WriteLine(ex.StackTrace);
		}

		gc.Dispose();
	}

	protected void OnEventTabWiFiButtonPressEvent (object o, ButtonPressEventArgs args) {
		ChangeTab(0);
	}

	protected void OnEventTabClockButtonPressEvent (object o, ButtonPressEventArgs args) {
		ChangeTab(1);
	}

	protected void OnEventTabRadioButtonPressEvent (object o, ButtonPressEventArgs args) {
		ChangeTab(2);
	}

	protected void OnEventTabWeatherButtonPressEvent (object o, ButtonPressEventArgs args) {
		ChangeTab(3);
	}

	protected void OnEventTabIntercomButtonPressEvent (object o, ButtonPressEventArgs args) {
		ChangeTab(4);
	}

	protected void OnEventRightButtonPressEvent (object o, ButtonPressEventArgs args) {
		stationIndex++;
		if (stationIndex >= stations.Count) stationIndex = stations.Count - 1;
		station = stations[stationIndex];
		ctlDrawingMeta.QueueDraw();
	}

	protected void OnEventLeftButtonPressEvent (object o, ButtonPressEventArgs args) {
		stationIndex--;
		if (stationIndex < 0) stationIndex = 0;
		station = stations[stationIndex];
		ctlDrawingMeta.QueueDraw();
	}

	protected void OnButtonDisconnectClicked (object sender, EventArgs e) {
		wireless.Disconnect();
	}

	protected void OnButtonConnectClicked (object sender, EventArgs e) {
		notebookWiFi.Page = 1;
	}

	protected void WiFiPrevPage () {
		notebookWiFi.Page--;
	}

	protected void WiFiNextPage () {
		if (notebookWiFi.Page == 2) {
			WirelessInfo info = selectaccesspoint.SelectedAccessPoint;
			info.User = selectuserpass.User;
			info.Password = selectuserpass.Password;
			wireless.Connect(info);
			notebookWiFi.Page = 0;
		} else if (notebookWiFi.Page == 1) {
			WirelessInfo info = selectaccesspoint.SelectedAccessPoint;
			Console.WriteLine("Wireless security = " + info.Security);
			selectuserpass.Initialize(info);
			notebookWiFi.Page++;
		} else {
			notebookWiFi.Page++;
		}
	}

	protected void OnSelectuserpassNextPage (object sender, EventArgs e) {
		WiFiNextPage();
	}

	protected void OnSelectuserpassPrevPage (object sender, EventArgs e) {
		WiFiPrevPage();
	}

	protected void OnSelectaccesspointNextPage (object sender, EventArgs e) {
		WiFiNextPage();
	}

	protected void OnSelectaccesspointPrevPage (object sender, EventArgs e) {
		WiFiPrevPage();
	}

	protected void OnButtonTestClicked (object sender, EventArgs e) {
		isAlarmActive = true;
		recognizer.StopListening();
		NightMode();
		isAlarmActive = false;
	}

	protected void OnButtonWeatherClicked (object sender, EventArgs e) {
		isAlarmActive = true;
		recognizer.StopListening();
		SpeakWeather(true);
		isAlarmActive = false;
	}

	protected void OnButtonEventsClicked (object sender, EventArgs e) {
		isAlarmActive = true;
		recognizer.StopListening();
		SpeakEvents();
		isAlarmActive = false;
	}

}

