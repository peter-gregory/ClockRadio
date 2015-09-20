using System;
using System.Collections.Generic;
using System.IO;

namespace beagleradio {

	public class EventList {

		public List<EventItem> Events { get; set; }

		public EventList() {
			Events = new List<EventItem>();
		}

		public List<EventItem> FindActive(DateTime test) {
			test = new DateTime(test.Year, test.Month, test.Day, test.Hour, test.Minute, 0);
			List<EventItem> results = new List<EventItem>();
			List<EventItem> expired = new List<EventItem>();
			foreach (EventItem item in Events) {
				if (item.IsActive(test)) {
					results.Add(item);
				} else if (item.IsExpired()) {
					expired.Add(item);
				}
			}
			foreach (EventItem item in expired) {
				Events.Remove(item);
			}
			return results;
		}

		public void Load() {
			Events.Clear();
			try {
				FileStream file = new FileStream("events.txt", FileMode.Open);
				StreamReader reader = new StreamReader(file);
				string line = reader.ReadLine();
				EventItem item = null;
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
								Events.Add(item);
							}
							item = new EventItem();
							item.Name = value;
						} else if (Framework.ExtractKey(out value, line, "action:")) {
							item.Actions.Add(value);
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
					Events.Add(item);
				}
			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}
		}
	}
}

