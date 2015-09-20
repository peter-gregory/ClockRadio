using System;

namespace beagleradio {

	public class StationInfo {

		public String Url { get; set; }
		public String Name { get; set; }
		public String CallLetters { get; set; }
		public String Location { get; set; }

		public StationInfo(String url, String name, String callLetters, String location) {
			this.Url = url;
			this.Name = name;
			this.CallLetters = callLetters;
			this.Location = location;
		}

		public StationInfo() {

		}
	}
}

