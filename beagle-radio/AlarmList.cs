using System;
using System.Collections.Generic;
using System.IO;

namespace beagleradio {

	public class AlarmList {

		public bool Enabled { get; set; }
		public List<AlarmItem> Alarms { get; private set; }

		public AlarmList() {
			Alarms = new List<AlarmItem>();
		}

		public DateTime NextActiveAlarm() {
			DateTime test = DateTime.Now;
			test = new DateTime(test.Year, test.Month, test.Day, test.Hour, test.Minute, 0);
			DateTime next = DateTime.MaxValue;
			foreach (AlarmItem item in Alarms) {
				DateTime nextTest = item.NextActive(test);
				if (nextTest < next) {
					next = nextTest;
				}
			}
			return next;
		}

		public List<AlarmItem> FindOn(DateTime test) {
			test = new DateTime(test.Year, test.Month, test.Day, test.Hour, test.Minute, 0);
			List<AlarmItem> results = new List<AlarmItem>();
			List<AlarmItem> expired = new List<AlarmItem>();
			foreach (AlarmItem item in Alarms) {
				if (item.IsOn(test)) {
					results.Add(item);
				} else if (item.IsExpired()) {
					expired.Add(item);
				}
			}
			foreach (AlarmItem item in expired) {
				Alarms.Remove(item);
			}
			return results;
		}

		public List<AlarmItem> FindOff(DateTime test) {
			test = new DateTime(test.Year, test.Month, test.Day, test.Hour, test.Minute, 0);
			List<AlarmItem> results = new List<AlarmItem>();
			List<AlarmItem> expired = new List<AlarmItem>();
			foreach (AlarmItem item in Alarms) {
				if (item.IsOff(test)) {
					results.Add(item);
				} else if (item.IsExpired()) {
					expired.Add(item);
				}
			}
			foreach (AlarmItem item in expired) {
				Alarms.Remove(item);
			}
			return results;
		}

		public void Load() {
			Alarms.Clear();
			try {
				FileStream file = new FileStream("alarms.txt", FileMode.Open);
				StreamReader reader = new StreamReader(file);
				string line = reader.ReadLine();
				AlarmItem item = null;
				TimeFrame onframe = null;
				TimeFrame offframe = null;
				while (line != null) {
					line = line.Trim();
					if (!line.StartsWith("#")) {
						string value;
						if (Framework.ExtractKey(out value, line, "name:")) {
							if (item != null) {
								if (onframe != null) {
									item.OnTimes.Add(onframe);
									onframe = null;
								}
								if (offframe != null) {
									item.OffTimes.Add(offframe);
									offframe = null;
								}
								Alarms.Add(item);
							}
							item = new AlarmItem();
							item.Name = value;
						} else if (Framework.ExtractKey(out value, line, "on_action:")) {
							item.OnActions.Add(value);
						} else if (Framework.ExtractKey(out value, line, "off_action:")) {
							item.OffActions.Add(value);
						} else if (Framework.ExtractKey(out value, line, "on_frame:")) {
							if (onframe != null) item.OnTimes.Add(onframe);
							onframe = new TimeFrame();
						} else if (Framework.ExtractKey(out value, line, "on_start_date:")) {
							onframe.StartDate = DateTime.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "on_stop_date:")) {
							onframe.StopDate = DateTime.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "on_start_time:")) {
							onframe.StartTime = DateTime.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "on_stop_time:")) {
							onframe.StopTime = DateTime.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "on_recurrence:")) {
							onframe.Recurrence = (TimeFrame.RecurringType) Enum.Parse(typeof(TimeFrame.RecurringType), value);
						} else if (Framework.ExtractKey(out value, line, "on_recurrence_week:")) {
							onframe.RecurrenceWeek = (TimeFrame.RecurringWeek) Enum.Parse(typeof(TimeFrame.RecurringWeek), value);
						} else if (Framework.ExtractKey(out value, line, "on_weekdays:")) {
							onframe.WeekDays = value;
						} else if (Framework.ExtractKey(out value, line, "on_month:")) {
							onframe.Month = int.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "on_day:")) {
							onframe.Day = int.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "off_frame:")) {
							if (offframe != null) item.OnTimes.Add(offframe);
							offframe = new TimeFrame();
						} else if (Framework.ExtractKey(out value, line, "off_start_date:")) {
							offframe.StartDate = DateTime.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "off_stop_date:")) {
							offframe.StopDate = DateTime.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "off_start_time:")) {
							offframe.StartTime = DateTime.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "off_stop_time:")) {
							offframe.StopTime = DateTime.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "off_recurrence:")) {
							offframe.Recurrence = (TimeFrame.RecurringType) Enum.Parse(typeof(TimeFrame.RecurringType), value);
						} else if (Framework.ExtractKey(out value, line, "off_recurrence_week:")) {
							offframe.RecurrenceWeek = (TimeFrame.RecurringWeek) Enum.Parse(typeof(TimeFrame.RecurringWeek), value);
						} else if (Framework.ExtractKey(out value, line, "off_weekdays:")) {
							offframe.WeekDays = value;
						} else if (Framework.ExtractKey(out value, line, "off_month:")) {
							offframe.Month = int.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "off_day:")) {
							offframe.Day = int.Parse(value);
						}
					}
					line = reader.ReadLine();
				}
				reader.Close();
				reader.Dispose();
				file.Close();
				file.Dispose();
				if (item != null) {
					if (onframe != null) {
						item.OnTimes.Add(onframe);
					}
					if (offframe != null) {
						item.OffTimes.Add(offframe);
					}
					Alarms.Add(item);
				}
			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}
		}
	}
}

