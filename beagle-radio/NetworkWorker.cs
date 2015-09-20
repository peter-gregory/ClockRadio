using System;
using System.Threading;
using System.Collections.Generic;

namespace beagleradio {

	public class NetworkWorker {

		private Timer timer;
		private WirelessWrapper wireless;

		public String IpAddress { get; private set; }
		public WirelessInfo WiFi { get; private set; }

		public NetworkWorker() {
			wireless = new WirelessWrapper();
		}

		public void Start() {
			timer = new Timer(new TimerCallback(UpdateNetworkInfo));
			timer.Change(4000, -1);
		}

		public void Close() {
			if (timer != null) {
				timer.Dispose();
				timer = null;
			}
		}

		protected void UpdateNetworkInfo(object sender) {
			
			try {
				
				List<WirelessInfo> infoList = wireless.ConnectionStatus();
				WirelessInfo connect = null;
				foreach (WirelessInfo test in infoList) {
					if (test.Active) {
						connect = test;
						break;
					}
				}
				WiFi = connect;

				IpAddress = IpAddressWrapper.QueryIpAddress();

				if (timer != null) timer.Change(4000, -1);

			} catch (Exception ex) {
				Console.WriteLine("UpdateNetworkInfo: " + ex.ToString());
			}
		}
	}
}

