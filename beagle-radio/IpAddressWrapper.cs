using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace beagleradio {

	public class IpAddressWrapper {
		public IpAddressWrapper() {
		}

		public static String QueryIpAddress() {
			string ethAddress = "";
			string wlanAddr = "";

			ProcessStartInfo info = new ProcessStartInfo();
			info.FileName = "/sbin/ifconfig";
			info.CreateNoWindow = true;
			info.UseShellExecute = false;
			info.RedirectStandardOutput = true;
			Process process = Process.Start(info);
			process.WaitForInputIdle();
			string test = process.StandardOutput.ReadToEnd();
			process.Close();
			process.Dispose();

			string value;
			string interfaceType = "";
			foreach (string line in test.Split('\n')) {
				if (line.Length > 0) {
					if (!line.StartsWith(" ")) {
						interfaceType = Framework.Extract(line, 0, "", " ");
					}
					if (Framework.ExtractKey(out value, line, " inet addr:", "", " ")) {
						if (value != "127.0.0.1" && value != "0.0.0.0") {
							if (interfaceType == "eth0") ethAddress = value;
							if (interfaceType == "wlan0") wlanAddr = value;
						}
					}
				}
			}
			if (ethAddress != "") return ethAddress;
			return wlanAddr;
		}
	}
}

