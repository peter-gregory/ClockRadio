using System;
using System.Collections.Generic;

namespace beagleradio {

	public class EventItem {

		public string Name { get; set; }
		public List<string> Actions { get; set; }
		public List<TimeFrame> OnTimes { get; set; }
		public List<TimeFrame> OffTimes { get; set; }

		public EventItem() {
			Actions = new List<string>();
			OnTimes = new List<TimeFrame>();
			OffTimes = new List<TimeFrame>();
		}

		public bool IsExpired() {
			bool isExpired = false;
			if (OnTimes.Count > 0 || OffTimes.Count > 0) {
				isExpired = true;
				foreach (TimeFrame frame in OnTimes) {
					if (!frame.IsExpired()) {
						isExpired = false;
						break;
					}
				}
			}
			return isExpired;
		}

		public bool IsActive(DateTime test) {
			bool isActive = true;
			if (OnTimes.Count > 0 || OffTimes.Count > 0) {
				isActive = false;
				foreach (TimeFrame frame in OnTimes) {
					if (frame.IsActive(test)) {
						isActive = true;
						break;
					}
				}
				if (isActive) {
					foreach (TimeFrame frame in OffTimes) {
						if (frame.IsActive(test)) {
							isActive = false;
							break;
						}
					}
				}
			}
			return isActive;
		}
	}
}

