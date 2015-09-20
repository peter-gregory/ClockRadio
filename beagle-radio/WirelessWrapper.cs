using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace beagleradio {

	public class WirelessWrapper {

		public event EventHandler<string> Status;

		public WirelessWrapper() {
		}

		private void ProcessScanResult(List<WirelessInfo> results, string line) {
			WirelessInfo item;

			String value;
			if (Framework.ExtractKey(out value, line, "Address:")) {
				results.Add(new WirelessInfo());
				item = results[results.Count - 1];
				item.BSSID = value; 
			}
			if (results.Count > 0 && Framework.ExtractKey(out value, line, "ESSID:", "\"", "\"")) {
				item = results[results.Count - 1];
				item.ESSID = value; 
			}
			if (Framework.ExtractKey(out value, line, "Frequency:", "(", ")")) {
				item = results[results.Count - 1];
				item.Channel = value.Substring(value.IndexOf(" ") + 1); 
			}
			if (results.Count > 0 && Framework.ExtractKey(out value, line, "Encryption key:")) {
				item = results[results.Count - 1];
				item.Encryption = value == "on";
			}
			if (results.Count > 0 && Framework.ExtractKey(out value, line, "Authentication Suites", ":")) {
				item = results[results.Count - 1];
				item.Security = value;
			}
			if (results.Count > 0 && Framework.ExtractKey(out value, line, "Quality=", "", "/")) {
				item = results[results.Count - 1];
				item.Signal = int.Parse(value);
			}
			if (results.Count > 0 && Framework.ExtractKey(out value, line, "Group Cipher :")) {
				item = results[results.Count - 1];
				item.GroupCypher = value;
			}
			if (results.Count > 0 && Framework.ExtractKey(out value, line, "Pairwise Ciphers", ":")) {
				item = results[results.Count - 1];
				item.PairCypher = value;
			}
		}

		public List<WirelessInfo> Scan() {

			List<WirelessInfo> results = new List<WirelessInfo>();
			ProcessStartInfo info = new ProcessStartInfo();
			info.Arguments = "wlan0 scanning";
			info.FileName = "/sbin/iwlist";
			info.CreateNoWindow = true;
			info.UseShellExecute = false;
			info.RedirectStandardOutput = true;
			Process process = new Process();
			process.StartInfo = info;
			process.Start();
			process.WaitForInputIdle();
			string test = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
			process.Close();
			process.Dispose();
			foreach (string line in test.Split('\n')) {
				ProcessScanResult(results, line);
			}
			return results;
		}

		private void ProcessStatusResult(List<WirelessInfo> results, string line) {
			WirelessInfo item;

			String value;
			if (Framework.ExtractKey(out value, line, "ESSID:", "\"", "\"")) {
				results.Add(new WirelessInfo());
				item = results[results.Count - 1];
				item.ESSID = value; 
				item.Active = true;
			}
			if (results.Count > 0 && Framework.ExtractKey(out value, line, "Access Point:")) {
				item = results[results.Count - 1];
				item.BSSID = value; 
			}
			if (results.Count > 0 && Framework.ExtractKey(out value, line, "Link Quality=", "", "/")) {
				item = results[results.Count - 1];
				item.Signal = int.Parse(value);
			}
		}

		public void Restart() {
			ProcessStartInfo info = new ProcessStartInfo();
			info.Arguments = "service wicd restart";
			info.FileName = "sudo";
			info.CreateNoWindow = true;
			info.UseShellExecute = false;
			Process process = Process.Start(info);
			process.WaitForExit();
			process.Close();
			process.Dispose();
		}

		public List<WirelessInfo> ConnectionStatus() {
			List<WirelessInfo> results = new List<WirelessInfo>();
			ProcessStartInfo info = new ProcessStartInfo();
			info.Arguments = "wlan0";
			info.FileName = "/sbin/iwconfig";
			info.CreateNoWindow = true;
			info.UseShellExecute = false;
			info.RedirectStandardOutput = true;
			Process process = Process.Start(info);
			process.WaitForInputIdle();
			string test = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
			process.Close();
			process.Dispose();
			foreach (string line in test.Split('\n')) {
				ProcessStatusResult(results, line);
			}
			return results;
		}

		private List<string> readConnectionInfo() {
			List<string> body = new List<string>();
			ProcessStartInfo info = new ProcessStartInfo();
			info.Arguments = "cat /etc/wicd/wireless-settings.conf";
			info.FileName = "/usr/bin/sudo";
			info.CreateNoWindow = true;
			info.UseShellExecute = false;
			info.RedirectStandardOutput = true;
			Process process = Process.Start(info);
			process.WaitForInputIdle();
			string test = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
			process.Close();
			process.Dispose();
			foreach (string line in test.Split('\n')) {
				body.Add(line);
			}
			return body;
		}

		private void writeConnectionInfo(List<string> body) {
			FileStream file = new FileStream("temp.txt", FileMode.OpenOrCreate);
			StreamWriter writer = new StreamWriter(file);
			foreach (string line in body) {
				writer.WriteLine(line);
			}
			writer.Close();
			writer.Dispose();
			file.Close();
			file.Dispose();

			ProcessStartInfo info = new ProcessStartInfo();
			info.Arguments = "cp -f -T ./temp.txt /etc/wicd/wireless-settings.conf";
			info.FileName = "/usr/bin/sudo";
			info.CreateNoWindow = true;
			info.UseShellExecute = false;
			info.RedirectStandardError = false;
			info.RedirectStandardOutput = false;
			Process process = Process.Start(info);
			process.WaitForExit();
			process.Close();
			process.Dispose();
		}

		private List<string> removeConnection(List<string> body, WirelessInfo wireless) {
			List<string> result = new List<string>();
			bool found = false;
			foreach (string line in body) {
				if (line.StartsWith("[")) {
					if (Status != null) {
						Status(this, "Compare " + line + " to [" + wireless.BSSID + "]");
					}
					found = (line == "[" + wireless.BSSID + "]");
					if (Status != null) {
						Status(this, "Remove old connection to " + wireless.ESSID + " (" + wireless.BSSID + ")");
					}
				}
				if (!found) {
					result.Add(line);
				}
			}
			return result;
		}

		private void appendNone(List<string> body, WirelessInfo info) {
			body.Add("[" + info.BSSID + "]");
			body.Add("bssid = " + info.BSSID);
			body.Add("ip = None");
			body.Add("dns_domain = None");
			body.Add("gateway = None");
			body.Add("use_global_dns = 0");
			body.Add("encryption = False");
			body.Add("hidden = False");
			body.Add("channel = " + info.Channel);
			body.Add("mode = Master");
			body.Add("netmask = None");
			body.Add("usedhcphostname = 0");
			body.Add("enctype = None");
			body.Add("dns3 = None");
			body.Add("dns2 = None");
			body.Add("dns1 = None");
			body.Add("use_settings_globally = 0");
			body.Add("use_static_dns = 0");
			body.Add("essid = " + info.ESSID);
			body.Add("automatic = 1");
			body.Add("beforescript = None");
			body.Add("afterscript = None");
			body.Add("predisconnectscript = None");
			body.Add("postdisconnectscript = None");
		}

		private void appendLeap(List<string> body, WirelessInfo info) {
			body.Add("[" + info.BSSID + "]");
			body.Add("bssid = " + info.BSSID);
			body.Add("ip = None");
			body.Add("dns_domain = None");
			body.Add("gateway = None");
			body.Add("use_global_dns = 0");
			body.Add("encryption = True");
			body.Add("hidden = False");
			body.Add("channel = " + info.Channel);
			body.Add("mode = Master");
			body.Add("username = " + info.User);
			body.Add("netmask = None");
			body.Add("usedhcphostname = 0");
			body.Add("password = " + info.Password);
			body.Add("identity = " + info.User);
			body.Add("enctype = peap");
			body.Add("dns3 = None");
			body.Add("dns2 = None");
			body.Add("dns1 = None");
			body.Add("use_settings_globally = 0");
			body.Add("use_static_dns = 0");
			body.Add("pac_file = None");
			body.Add("encryption_method = WPA2");
			body.Add("essid = " + info.ESSID);
			body.Add("automatic = 1");
			body.Add("search_domain = None");
			body.Add("beforescript = None");
			body.Add("afterscript = None");
			body.Add("predisconnectscript = None");
			body.Add("postdisconnectscript = None");
		}

		private void appendWpa(List<string> body, WirelessInfo info) {
			body.Add("[" + info.BSSID + "]");
			body.Add("dhcphostname = beaglebone");
			body.Add("bssid = " + info.BSSID);
			body.Add("ip = None");
			body.Add("dns_domain = None");
			body.Add("gateway = None");
			body.Add("use_global_dns = 0");
			body.Add("encryption = True");
			body.Add("hidden = False");
			body.Add("channel = " + info.Channel);
			body.Add("mode = Master");
			body.Add("netmask = None");
			body.Add("key = " + info.Password);
			body.Add("usedhcphostname = 0");
			body.Add("enctype = wpa");
			body.Add("dns3 = None");
			body.Add("dns2 = None");
			body.Add("dns1 = None");
			body.Add("use_settings_globally = 0");
			body.Add("use_static_dns = 0");
			body.Add("encryption_method = WPA2");
			body.Add("essid = " + info.ESSID);
			body.Add("automatic = 1");
			body.Add("search_domain = None");
			body.Add("beforescript = None");
			body.Add("afterscript = None");
			body.Add("predisconnectscript = None");
			body.Add("postdisconnectscript = None");
		}

		public void Connect(WirelessInfo wireless) {
			if (Status != null) {
				Status(this, "Connect to " + wireless.ESSID + " (" + wireless.BSSID + ")");
			}
			List<string> body = readConnectionInfo();
			body = removeConnection(body, wireless);
			if (wireless.Encryption) {
				if (wireless.Security.ToLower().Contains("802.1x")) {
					appendLeap(body, wireless);
				} else if (wireless.Security.ToLower().Contains("psk")) {
					appendWpa(body, wireless);
				} else {
					if (Status != null) {
						Status(this, "Unknown security: " + wireless.Security);
					}
				}
			} else {
				appendNone(body, wireless);
			}
			writeConnectionInfo(body);
			Restart();
		}

		public void Disconnect() {
			List<WirelessInfo> infoList = ConnectionStatus();
			if (infoList.Count > 0) {
				List<string> body = readConnectionInfo();
				body = removeConnection(body, infoList[0]);
				writeConnectionInfo(body);
				Restart();
			}
		}
	}
}

