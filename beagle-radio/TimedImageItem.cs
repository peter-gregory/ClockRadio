using System;
using System.Collections.Generic;

namespace beagleradio {

	public class TimedImageItem {

		public string Name { get; set; }
		public string Filename { get; set; }
		public List<TimeFrame> OnTimes { get; private set; }
		public List<TimeFrame> OffTimes { get; private set; }

		public TimedImageItem() {
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

