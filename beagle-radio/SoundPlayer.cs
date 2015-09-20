using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace beagleradio {

	public class SoundPlayer {

		private Process process;

		public event EventHandler<string> Status;
		public event EventHandler Closed;

		public bool IsBusy { get; private set; }
		public List<string> Files { get; set; }

		public SoundPlayer() {
			Files = new List<string>();
		}

		public void PlayAsync(string file) {
			Files.Add(file);
			if (!IsBusy) {
				IsBusy = true;
				PlayNext();
			}
		}

		public void Play(string file) {
			PlayAsync(file);
			WaitDone();
		}

		public void WaitDone() {
			while (IsBusy) {
				Framework.GuiDelay();
			}
		}

		private void PlayNext() {
			ProcessStartInfo info = new ProcessStartInfo();
			string filename = Files[0];
			Files.RemoveAt(0);

			if (filename.EndsWith(".raw")) {
				info.Arguments = "-p --rate=44100 --format=s16le --channels=1 " + filename;
				info.FileName = "/usr/bin/pacat";
			} else {
				info.Arguments = filename;
				info.FileName = "paplay";
			}
			info.CreateNoWindow = true;
			info.UseShellExecute = false;
			process = Process.Start(info);
			process.EnableRaisingEvents = true;
			process.Exited += Process_Exited;
		}

		private void Finished() {
			if (Status != null) {
				Status(this, "Finished TTS");
			}
			IsBusy = false;
			if (Closed != null) {
				Closed(this, null);
			}
		}

		void Process_Exited (object sender, EventArgs e) {
			process.Close();
			process.Dispose();
			process = null;

			if (Files.Count == 0) {
				Finished();
			} else {
				PlayNext();
			}
		}


		public void Close() {
			if (process != null) {
				process.Kill();
			}
		}
	}
}

