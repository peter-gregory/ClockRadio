using System;
using System.Diagnostics;

namespace beagleradio {

	public class VolumeService {
		
		private Process process;

		public bool IsBusy { get; set; }

		public VolumeService() {
			IsBusy = false;
		}

		public void WaitDone() {
			while (IsBusy) {
				Framework.GuiDelay();
			}
		}

		public void SetVolume(int percent) {
			ProcessStartInfo info = new ProcessStartInfo();
			info.Arguments = "-D pulse sset Master " + percent + "%";
			info.FileName = "amixer";
			info.CreateNoWindow = true;
			info.UseShellExecute = false;
			process = Process.Start(info);
			process.EnableRaisingEvents = true;
			process.Exited += Process_Exited;
			IsBusy = true;
		}

		public void SetLcdBrightness(int percent) {
			ProcessStartInfo info = new ProcessStartInfo();
			info.Arguments = "sh -c \"echo " + percent + " >  /sys/class/backlight/backlight.11/brightness\"";
			info.FileName = "sudo";
			info.CreateNoWindow = true;
			info.UseShellExecute = false;
			process = Process.Start(info);
			process.EnableRaisingEvents = true;
			process.Exited += Process_Exited;
			IsBusy = true;
		}

		public void SetUserLedBrightness(int percent) {
			ProcessStartInfo info = new ProcessStartInfo();
			info.Arguments = "sh -c \"echo " + percent + " >  /sys/class/leds/lcd4\\:green\\:usr0/brightness\"";
			info.FileName = "sudo";
			info.CreateNoWindow = true;
			info.UseShellExecute = false;
			process = Process.Start(info);
			process.EnableRaisingEvents = true;
			process.Exited += Process_Exited;
			IsBusy = true;
		}

		void Process_Exited (object sender, EventArgs e)
		{
			process.Close();
			process.Dispose();
			process = null;
			IsBusy = false;
		}

	}
}

