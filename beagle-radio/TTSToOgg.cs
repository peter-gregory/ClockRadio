using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.Text;
using System.Net;
using Gtk;

namespace beagleradio {

	public class TTSToOgg {

		private WebClient client;
		private Timer timer;

		public event EventHandler<string> Status;
		public event EventHandler Closed;

		public bool IsBusy { get; private set; }
		public string Filename { get; private set; }

		public TTSToOgg() {
			client = new WebClient();
			client.DownloadDataCompleted += Client_DownloadDataCompleted;
		}

		public void Convert(string filename, string speech, int timeout = 60000) {
			ConvertAsync(filename, speech, timeout);
			while (IsBusy) {
				Framework.GuiDelay();
			}
		}

		public void ConvertAsync(string filename, string speech, int timeout = 60000) {
			Filename = filename;
			IsBusy = true;
			if (Status != null) {
				Status(this, "Convert text to wave file");
			}
			speech = Framework.ExpandToWords(speech);
			string url = "http://api.voicerss.org/?key=d272fdbfb66d4a3fa8b77ef0b2b6f492&hl=en-us&c=ogg&f=44khz_16bit_mono&src=" + speech;
			Uri uri = new Uri(url);
			client.DownloadDataAsync(uri);
			timer = new Timer(new TimerCallback(delegate(object state) {
				client.CancelAsync();
			}));
			timer.Change(timeout, timeout);
		}

		public void WaitDone() {
			while (IsBusy) {
				Framework.GuiDelay();
			}
		}

		void Finished() {
			IsBusy = false;
			if (Closed != null) {
				Closed(this, null);
			}
		}

		void Client_DownloadDataCompleted (object sender, DownloadDataCompletedEventArgs e) {

			timer.Dispose();
			timer = null;

			if (e.Cancelled) {
				if (Status != null) {
					Status(this, "Conversion timed out");
				}
			} else {
				if (Status != null) {
					Status(this, "Success");
				}

				FileStream file = new FileStream(Filename, FileMode.Create);
				file.Write(e.Result, 0, e.Result.Length);
				file.Close();
				file.Dispose();
			}

			Finished();
		}

		public void Close() {
			if (IsBusy) {
				client.CancelAsync();
			}
		}
	}
}

