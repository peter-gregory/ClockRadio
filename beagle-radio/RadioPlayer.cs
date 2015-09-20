using System;
using System.Diagnostics;
using System.Threading;
using Gtk;

namespace beagleradio {

	public class RadioPlayer {

		private Process process;

		public event EventHandler<String> MetadataReceived;
		public event EventHandler<String> Closed;

		public bool IsBusy { get; set; }

		public RadioPlayer() {
			IsBusy = false;
		}

		public void Start(String filename) {
			if (!IsBusy) {
				IsBusy = true;
				ProcessStartInfo info = new ProcessStartInfo();
				info.Arguments = "-prefer-ipv4 -cache-min 10 -slave -quiet -idle -playlist " + filename;
				info.FileName = "mplayer";
				info.CreateNoWindow = true;
				info.UseShellExecute = false;
				info.RedirectStandardInput = true;
				info.RedirectStandardOutput = true;
				process = Process.Start(info);
				process.EnableRaisingEvents = true;
				process.OutputDataReceived += Process_OutputDataReceived;
				process.Exited += Process_Exited;
				process.BeginOutputReadLine();
			}
		}

		public void Stop() {
			StopAsync();
			while (IsBusy) {
				Framework.GuiDelay();
			}
		}

		public void StopAsync() {
			try {
				if (IsBusy && process != null) {
					process.StandardInput.Write("quit\n");
				}
			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}
		}

		private void Process_Exited (object sender, EventArgs e)
		{
			process.CancelOutputRead();
			process.Close();
			process.Dispose();
			process = null;

			IsBusy = false;
			if (Closed != null) {
				Closed(this, null);
			}
		}

		private void Process_OutputDataReceived (object sender, DataReceivedEventArgs e) {
			if (MetadataReceived != null && e != null && e.Data != null) {
				MetadataReceived(this, e.Data);
			}
		}
	}
}

