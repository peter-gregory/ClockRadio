using System;
using System.Collections.Generic;

namespace beagleradio {

	public class TimeFrame {

		public enum RecurringType {
			None, // Just use the start date / end date
			Daily, // Start Date / End Date, Every Monday, Tuesday ...
			Weekly, // Start Date / End Date, Every first Monday, Tuesday...
			Monthly, // Every Dec, First Tuesday
			Yearly, // Every Dec 25th 
			YearlyRange // Every year between the starttime and endtime
		};

		public enum RecurringWeek {
			None,
			First,
			Second,
			Third,
			Fourth,
			Fifth,
			Last
		};

		public DateTime StartDate { get; set; }  // Year, Month, Day
		public DateTime StopDate { get; set; }  // Year, Month, Day
		public DateTime StartTime { get; set; }  // Hour, Minute, Second
		public DateTime StopTime { get; set; }  // Hour, Minute, Second
		public RecurringType Recurrence { get; set; }
		public RecurringWeek RecurrenceWeek { get; set; }
		public string WeekDays { get; set; }
		public int Month { get; set; }
		public int Day { get; set; }
		public int DayOfMonth { get; set; }

		public TimeFrame() {
			StartDate = DateTime.MinValue;
			StopDate = DateTime.MaxValue;
			StartTime = DateTime.MinValue;
			StopTime = DateTime.MaxValue;
			Recurrence = RecurringType.None;
		}

		public bool IsExpired() {
			return (StopDate <= DateTime.Now);
		}

		public bool IsActive(DateTime test) {
			switch (Recurrence) {
				case RecurringType.Daily:
					return IsActiveDaily(test);
				case RecurringType.Weekly:
					return IsActiveWeekly(test);
				case RecurringType.Monthly:
					return IsActiveMonthly(test);
				case RecurringType.Yearly:
					return IsActiveYearly(test);
				case RecurringType.YearlyRange:
					return IsActiveYearlyRange(test);
				default:
					return IsActiveNone(test);
			}
		}

		bool IsActiveNone(DateTime test) {
			return StartDate.Date <= test.Date && 
				StopDate.Date > test.Date && 
				test.TimeOfDay >= StartTime.TimeOfDay && 
				test.TimeOfDay < StopTime.TimeOfDay;
		}

		// Every Monday, Tuesday, Wednesday...
		bool IsActiveDaily(DateTime test) {
			if (StartTime.TimeOfDay <= test.TimeOfDay && StopTime.TimeOfDay > test.TimeOfDay) {
				if (StartDate.Date <= test.Date && StopDate.Date > test.Date) {
					string days = WeekDays.ToLower();
					days = days.Replace("weekdays", "monday tuesday wednesday thursday friday");
					days = days.Replace("weekends", "saturday sunday");
					if (days.Contains(test.DayOfWeek.ToString().ToLower())) {
						return true;
					}
				}
			}
			return false;
		}

		// Every Nth Monday, Tuesday, Wednesday...
		bool IsActiveWeekly(DateTime test) {
			if (StartTime.TimeOfDay <= test.TimeOfDay && StopTime.TimeOfDay > test.TimeOfDay) {
				if (StartDate.Date <= test.Date && StopDate.Date > test.Date) {
					string days = WeekDays.ToLower();
					days = days.Replace("weekdays", "monday tuesday wednesday thursday friday");
					days = days.Replace("weekends", "saturday sunday");
					if (days.Contains(test.DayOfWeek.ToString().ToLower())) {
						switch (RecurrenceWeek) {
							case RecurringWeek.First:
								return (test.Day - 1) / 7 == 0;
							case RecurringWeek.Second:
								return (test.Day - 1) / 7 == 1;
							case RecurringWeek.Third:
								return (test.Day - 1) / 7 == 2;
							case RecurringWeek.Fourth:
								return (test.Day - 1) / 7 == 3;
							case RecurringWeek.Fifth:
								return (test.Day - 1) / 7 == 4;
							case RecurringWeek.Last:
								return test.AddDays(7).Month != test.Month;
						}
					}
				}
			}
			return false;
		}

		// Every Nth Thursday of January
		bool IsActiveMonthly(DateTime test) {
			if (StartTime.TimeOfDay <= test.TimeOfDay && StopTime.TimeOfDay > test.TimeOfDay) {
				if (StartDate.Date <= test.Date && StopDate.Date > test.Date) {
					string days = WeekDays.ToLower();
					days = days.Replace("weekdays", "monday tuesday wednesday thursday friday");
					days = days.Replace("weekends", "saturday sunday");
					if (days.Contains(test.DayOfWeek.ToString().ToLower())) {
						if (test.Month == Month) {
							switch (RecurrenceWeek) {
								case RecurringWeek.First:
									return (test.Day - 1) / 7 == 0;
								case RecurringWeek.Second:
									return (test.Day - 1) / 7 == 1;
								case RecurringWeek.Third:
									return (test.Day - 1) / 7 == 2;
								case RecurringWeek.Fourth:
									return (test.Day - 1) / 7 == 3;
								case RecurringWeek.Fifth:
									return (test.Day - 1) / 7 == 4;
								case RecurringWeek.Last:
									return test.AddDays(7).Month != test.Month;
							}
						}
					}
				}
			}
			return false;
		}

		// Every dec 25th
		bool IsActiveYearly(DateTime test) {
			if (StartTime.TimeOfDay <= test.TimeOfDay && StopTime.TimeOfDay > test.TimeOfDay) {
				if (StartDate.Date <= test.Date && StopDate.Date > test.Date) {
					if (test.Month == Month) {
						if (test.Day == Day) {
							return true;
						}
					}
				}
			}
			return false;
		}

		// Every year mm/dd to mm/dd
		bool IsActiveYearlyRange(DateTime test) {
			if (StartDate.Date <= test.Date && StopDate.Date > test.Date) {
				if (StartTime.ToString("MMdd").CompareTo(test.ToString("MMdd")) <= 0) {
					if (StopTime.ToString("MMdd").CompareTo(test.ToString("MMdd")) > 0) {
						return true;
					}
				}
			}
			return false;
		}

		public bool IsOn(DateTime test) {
			switch (Recurrence) {
				case RecurringType.Daily:
					return IsOnDaily(test);
				case RecurringType.Weekly:
					return IsOnWeekly(test);
				case RecurringType.Monthly:
					return IsOnMonthly(test);
				case RecurringType.Yearly:
					return IsOnYearly(test);
				case RecurringType.YearlyRange:
					return IsOnYearlyRange(test);
				default:
					return IsOnNone(test);
			}
		}

		bool IsOnNone(DateTime test) {
			return StartDate.Date <= test.Date && 
				StopDate.Date > test.Date && 
				test.TimeOfDay == StartTime.TimeOfDay;
		}

		// Every Monday, Tuesday, Wednesday...
		bool IsOnDaily(DateTime test) {
			if (StartTime.TimeOfDay == test.TimeOfDay) {
				if (StartDate.Date <= test.Date && StopDate.Date > test.Date) {
					string days = WeekDays.ToLower();
					days = days.Replace("weekdays", "monday tuesday wednesday thursday friday");
					days = days.Replace("weekends", "saturday sunday");
					if (days.Contains(test.DayOfWeek.ToString().ToLower())) {
						return true;
					}
				}
			}
			return false;
		}

		// Every Nth Monday, Tuesday, Wednesday...
		bool IsOnWeekly(DateTime test) {
			if (StartTime.TimeOfDay == test.TimeOfDay) {
				if (StartDate.Date <= test.Date && StopDate.Date > test.Date) {
					string days = WeekDays.ToLower();
					days = days.Replace("weekdays", "monday tuesday wednesday thursday friday");
					days = days.Replace("weekends", "saturday sunday");
					if (days.Contains(test.DayOfWeek.ToString().ToLower())) {
						switch (RecurrenceWeek) {
							case RecurringWeek.First:
								return (test.Day - 1) / 7 == 0;
							case RecurringWeek.Second:
								return (test.Day - 1) / 7 == 1;
							case RecurringWeek.Third:
								return (test.Day - 1) / 7 == 2;
							case RecurringWeek.Fourth:
								return (test.Day - 1) / 7 == 3;
							case RecurringWeek.Fifth:
								return (test.Day - 1) / 7 == 4;
							case RecurringWeek.Last:
								return test.AddDays(7).Month != test.Month;
						}
					}
				}
			}
			return false;
		}

		// Every Nth Thursday of January
		bool IsOnMonthly(DateTime test) {
			if (StartTime.TimeOfDay == test.TimeOfDay) {
				if (StartDate.Date <= test.Date && StopDate.Date > test.Date) {
					string days = WeekDays.ToLower();
					days = days.Replace("weekdays", "monday tuesday wednesday thursday friday");
					days = days.Replace("weekends", "saturday sunday");
					if (days.Contains(test.DayOfWeek.ToString().ToLower())) {
						if (test.Month == Month) {
							switch (RecurrenceWeek) {
								case RecurringWeek.First:
									return (test.Day - 1) / 7 == 0;
								case RecurringWeek.Second:
									return (test.Day - 1) / 7 == 1;
								case RecurringWeek.Third:
									return (test.Day - 1) / 7 == 2;
								case RecurringWeek.Fourth:
									return (test.Day - 1) / 7 == 3;
								case RecurringWeek.Fifth:
									return (test.Day - 1) / 7 == 4;
								case RecurringWeek.Last:
									return test.AddDays(7).Month != test.Month;
							}
						}
					}
				}
			}
			return false;
		}

		// Every dec 25th
		bool IsOnYearly(DateTime test) {
			if (StartTime.TimeOfDay == test.TimeOfDay) {
				if (StartDate.Date <= test.Date && StopDate.Date > test.Date) {
					if (test.Month == Month) {
						if (test.Day == Day) {
							return true;
						}
					}
				}
			}
			return false;
		}

		// Every year mm/dd to mm/dd
		bool IsOnYearlyRange(DateTime test) {
			if (StartDate.Date <= test.Date && StopDate.Date > test.Date) {
				if (StartTime.ToString("MMdd").CompareTo(test.ToString("MMdd")) == 0) {
					return true;
				}
			}
			return false;
		}

		public bool IsOff(DateTime test) {
			switch (Recurrence) {
				case RecurringType.Daily:
					return IsOffDaily(test);
				case RecurringType.Weekly:
					return IsOffWeekly(test);
				case RecurringType.Monthly:
					return IsOffMonthly(test);
				case RecurringType.Yearly:
					return IsOffYearly(test);
				case RecurringType.YearlyRange:
					return IsOffYearlyRange(test);
				default:
					return IsOffNone(test);
			}
		}

		bool IsOffNone(DateTime test) {
			return StartDate.Date <= test.Date && 
				StopDate.Date > test.Date && 
				test.TimeOfDay == StopTime.TimeOfDay;
		}

		// Every Monday, Tuesday, Wednesday...
		bool IsOffDaily(DateTime test) {
			if (StopTime.TimeOfDay == test.TimeOfDay) {
				if (StartDate.Date <= test.Date && StopDate.Date > test.Date) {
					string days = WeekDays.ToLower();
					days = days.Replace("weekdays", "monday tuesday wednesday thursday friday");
					days = days.Replace("weekends", "saturday sunday");
					if (days.Contains(test.DayOfWeek.ToString().ToLower())) {
						return true;
					}
				}
			}
			return false;
		}

		// Every Nth Monday, Tuesday, Wednesday...
		bool IsOffWeekly(DateTime test) {
			if (StopTime.TimeOfDay == test.TimeOfDay) {
				if (StartDate.Date <= test.Date && StopDate.Date > test.Date) {
					string days = WeekDays.ToLower();
					days = days.Replace("weekdays", "monday tuesday wednesday thursday friday");
					days = days.Replace("weekends", "saturday sunday");
					if (days.Contains(test.DayOfWeek.ToString().ToLower())) {
						switch (RecurrenceWeek) {
							case RecurringWeek.First:
								return (test.Day - 1) / 7 == 0;
							case RecurringWeek.Second:
								return (test.Day - 1) / 7 == 1;
							case RecurringWeek.Third:
								return (test.Day - 1) / 7 == 2;
							case RecurringWeek.Fourth:
								return (test.Day - 1) / 7 == 3;
							case RecurringWeek.Fifth:
								return (test.Day - 1) / 7 == 4;
							case RecurringWeek.Last:
								return test.AddDays(7).Month != test.Month;
						}
					}
				}
			}
			return false;
		}

		// Every Nth Thursday of January
		bool IsOffMonthly(DateTime test) {
			if (StopTime.TimeOfDay == test.TimeOfDay) {
				if (StartDate.Date <= test.Date && StopDate.Date > test.Date) {
					string days = WeekDays.ToLower();
					days = days.Replace("weekdays", "monday tuesday wednesday thursday friday");
					days = days.Replace("weekends", "saturday sunday");
					if (days.Contains(test.DayOfWeek.ToString().ToLower())) {
						if (test.Month == Month) {
							switch (RecurrenceWeek) {
								case RecurringWeek.First:
									return (test.Day - 1) / 7 == 0;
								case RecurringWeek.Second:
									return (test.Day - 1) / 7 == 1;
								case RecurringWeek.Third:
									return (test.Day - 1) / 7 == 2;
								case RecurringWeek.Fourth:
									return (test.Day - 1) / 7 == 3;
								case RecurringWeek.Fifth:
									return (test.Day - 1) / 7 == 4;
								case RecurringWeek.Last:
									return test.AddDays(7).Month != test.Month;
							}
						}
					}
				}
			}
			return false;
		}

		// Every dec 25th
		bool IsOffYearly(DateTime test) {
			if (StopTime.TimeOfDay == test.TimeOfDay) {
				if (StartDate.Date <= test.Date && StopDate.Date > test.Date) {
					if (test.Month == Month) {
						if (test.Day == Day) {
							return true;
						}
					}
				}
			}
			return false;
		}

		// Every year mm/dd to mm/dd
		bool IsOffYearlyRange(DateTime test) {
			if (StopDate.Date <= test.Date && StopDate.Date > test.Date) {
				if (StartTime.ToString("MMdd").CompareTo(test.ToString("MMdd")) == 0) {
					return true;
				}
			}
			return false;
		}

		public DateTime NextOn(DateTime test) {
			switch (Recurrence) {
				case RecurringType.Daily:
					return NextOnDaily(test);
				case RecurringType.Weekly:
					return NextOnWeekly(test);
				case RecurringType.Monthly:
					return NextOnMonthly(test);
				case RecurringType.Yearly:
					return NextOnYearly(test);
				case RecurringType.YearlyRange:
					return NextOnYearly(test);
				default:
					return NextOnNone(test);
			}
		}

		DateTime NextOnNone(DateTime test) {
			if (test > StopDate) return DateTime.MaxValue;
			if (test < StartDate) test = StartDate;
			if (test.TimeOfDay < StartTime.TimeOfDay) test = new DateTime(test.Year, test.Month, test.Day, StartTime.Hour, StartTime.Minute, 0);
			if (test.TimeOfDay > StartDate.TimeOfDay) return DateTime.MaxValue;
			return test;

		}

		DateTime NextOnDaily(DateTime test) {
			if (test > StopDate) return DateTime.MaxValue;
			if (test < StartDate) test = StartDate;
			if (test.TimeOfDay < StartTime.TimeOfDay) test = new DateTime(test.Year, test.Month, test.Day, StartTime.Hour, StartTime.Minute, 0);
			if (StartTime.TimeOfDay != test.TimeOfDay) {
				test = test.AddDays(1);
				test = new DateTime(test.Year, test.Month, test.Day, StartTime.Hour, StartTime.Minute, StartTime.Second); 
			}
			int maxScan = 7;
			string days = WeekDays.ToLower();
			days = days.Replace("weekdays", "monday tuesday wednesday thursday friday");
			days = days.Replace("weekends", "saturday sunday");
			if (!days.Contains(test.DayOfWeek.ToString().ToLower()) && maxScan-- > 0) {
				test = test.AddDays(1);
				if (test >= StopDate) return DateTime.MaxValue;
			}
			return test;
		}

		DateTime NextOnWeekly(DateTime test) {
			if (test > StopDate) return DateTime.MaxValue;
			if (test < StartDate) test = StartDate;
			if (test.TimeOfDay < StartTime.TimeOfDay) test = new DateTime(test.Year, test.Month, test.Day, StartTime.Hour, StartTime.Minute, 0);
			if (StartTime.TimeOfDay != test.TimeOfDay) {
				test = test.AddDays(1);
				test = new DateTime(test.Year, test.Month, test.Day, StartTime.Hour, StartTime.Minute, StartTime.Second); 
			}
			bool isFound = false;
			int maxScan = 31;
			string days = WeekDays.ToLower();
			days = days.Replace("weekdays", "monday tuesday wednesday thursday friday");
			days = days.Replace("weekends", "saturday sunday");
			while (!isFound && test < StopDate && maxScan-- > 0) {
				if (days.Contains(test.DayOfWeek.ToString().ToLower())) {
					switch (RecurrenceWeek) {
						case RecurringWeek.First:
							isFound = (test.Day - 1) / 7 == 0;
							break;
						case RecurringWeek.Second:
							isFound =  (test.Day - 1) / 7 == 1;
							break;
						case RecurringWeek.Third:
							isFound =  (test.Day - 1) / 7 == 2;
							break;
						case RecurringWeek.Fourth:
							isFound =  (test.Day - 1) / 7 == 3;
							break;
						case RecurringWeek.Fifth:
							isFound =  (test.Day - 1) / 7 == 4;
							break;
						case RecurringWeek.Last:
							isFound =  test.AddDays(7).Month != test.Month;
							break;
					}
				}
				if (!isFound) {
					test = test.AddDays(1);
				}
			}
			if (!isFound) return DateTime.MaxValue;
			return test;
		}

		// Every Nth Thursday of January
		DateTime NextOnMonthly(DateTime test) {
			if (test > StopDate) return DateTime.MaxValue;
			if (test < StartDate) test = StartDate;
			if (test.Month < Month) test = new DateTime(test.Year, Month, 1, StartTime.Hour, StartTime.Minute, StartTime.Second);
			if (test.Month > Month) test = new DateTime(test.Year + 1, Month, 1, StartTime.Hour, StartTime.Minute, StartTime.Second);
			if (StartDate.TimeOfDay < test.TimeOfDay) test = new DateTime(test.Year, test.Month, test.Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
			if (StartTime.TimeOfDay != test.TimeOfDay) {
				test = test.AddDays(1);
				test = new DateTime(test.Year, test.Month, test.Day, StartTime.Hour, StartTime.Minute, StartTime.Second); 
			}
			bool isFound = false;
			string days = WeekDays.ToLower();
			days = days.Replace("weekdays", "monday tuesday wednesday thursday friday");
			days = days.Replace("weekends", "saturday sunday");
			while (!isFound && test < StopDate && test.Month == Month) {
				if (days.Contains(test.DayOfWeek.ToString().ToLower())) {
					switch (RecurrenceWeek) {
						case RecurringWeek.First:
							isFound = (test.Day - 1) / 7 == 0;
							break;
						case RecurringWeek.Second:
							isFound = (test.Day - 1) / 7 == 1;
							break;
						case RecurringWeek.Third:
							isFound = (test.Day - 1) / 7 == 2;
							break;
						case RecurringWeek.Fourth:
							isFound = (test.Day - 1) / 7 == 3;
							break;
						case RecurringWeek.Fifth:
							isFound = (test.Day - 1) / 7 == 4;
							break;
						case RecurringWeek.Last:
							isFound = test.AddDays(7).Month != test.Month;
							break;
					}
				}
			}
			if (!isFound) return DateTime.MaxValue;
			return test;
		}

		// Every year mm/dd to mm/dd
		DateTime NextOnYearly(DateTime test) {
			if (test > StopDate) return DateTime.MaxValue;
			if (test < StartDate) test = StartDate;
			if ((test.Month < Month) || (test.Month == Month && test.Day < Day)) test = new DateTime(test.Year, Month, Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
			if ((test.Month > Month) || (test.Month == Month && test.Day > Day)) test = new DateTime(test.Year + 1, Month, Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
			if (StartDate.TimeOfDay < test.TimeOfDay) test = new DateTime(test.Year, Month, Day, StartTime.Hour, StartTime.Minute, StartTime.Second);
			if (StartTime.TimeOfDay != test.TimeOfDay) {
				test = new DateTime(test.Year + 1, Month, Day, StartTime.Hour, StartTime.Minute, StartTime.Second);				test = new DateTime(test.Year, test.Month, test.Day, StartTime.Hour, StartTime.Minute, StartTime.Second); 
			}
			if (test > StopDate) return DateTime.MaxValue;
			return test;
		}
	}
}

