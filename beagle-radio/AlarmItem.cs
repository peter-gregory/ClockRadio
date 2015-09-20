using System;
using System.Collections.Generic;

namespace beagleradio {

	public class AlarmItem {
		public string Name { get; set; }
		public List<string> OnActions { get; private set; }
		public List<string> OffActions { get; private set; }
		public List<TimeFrame> OnTimes { get; private set; }
		public List<TimeFrame> OffTimes { get; private set; }

		public AlarmItem() {
			OnTimes = new List<TimeFrame>();
			OffTimes = new List<TimeFrame>();
			OnActions = new List<string>();
			OffActions = new List<string>();
		}

		public bool IsExpired() {
			bool isExpired = false;
			if (OnTimes.Count > 0) {
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

		public DateTime NextActive(DateTime test) {
			DateTime nextTime = DateTime.MaxValue;
			foreach (TimeFrame frame in OnTimes) {
				bool isValid = false;
				DateTime next = test;
				while (!isValid) {
					isValid = true;
					next = frame.NextOn(next);
					if (next != DateTime.MaxValue) {
						foreach (TimeFrame offFrame in OffTimes) {
							if (offFrame.IsActive(test)) {
								next = next.AddMinutes(1);
								isValid = false;
								break;
							}
						}
					}
				}
				if (nextTime > next) {
					nextTime = next;
				}
			}
			return nextTime;
		}

		public bool IsOn(DateTime test) {
			bool isOn = false;
			foreach (TimeFrame frame in OnTimes) {
				if (frame.IsOn(test)) {
					isOn = true;
					break;
				}
			}
			if (isOn) {
				foreach (TimeFrame frame in OffTimes) {
					if (frame.IsActive(test)) {
						isOn = false;
						break;
					}
				}
			}
			return isOn;
		}

	
		public bool IsOff(DateTime test) {
			bool isOff = false;
			foreach (TimeFrame frame in OnTimes) {
				if (frame.IsOff(test)) {
					isOff = true;
					break;
				}
			}
			if (isOff) {
				foreach (TimeFrame frame in OffTimes) {
					if (frame.IsActive(test)) {
						isOff = false;
						break;
					}
				}
			}
			return isOff;
		}
	}
}

