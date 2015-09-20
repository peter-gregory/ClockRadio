using System;
using System.Collections.Generic;

namespace beagleradio {

	public class WirelessInfo : IComparable {

		public string BSSID { get; set; }
		public string ESSID { get; set; }
		public string Channel { get; set; }
		public int Signal { get; set; }
		public bool Encryption { get; set; }
		public string Security { get; set; }
		public string GroupCypher { get; set; }
		public string PairCypher { get; set; }
		public string Domain { get; set; }
		public string User { get; set; }
		public string Password { get; set; }
		public bool Active { get; set; }

		public WirelessInfo() {
			BSSID = "";
			ESSID = "";
			Channel = "";
			Signal = 0;
			Encryption = false;
			Security = "";
			GroupCypher = "";
			PairCypher = "";
			Domain = "";
			User = "";
			Password = "";
			Active = false;
		}


		public int CompareTo (object obj) {
			WirelessInfo test = (WirelessInfo) obj;
			int result = (test.Signal.CompareTo(Signal));
			if (result == 0) {
				result = ESSID.CompareTo(test.ESSID);
			}
			return result;
		}
	}
}

