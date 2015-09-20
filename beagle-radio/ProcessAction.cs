using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gtk;

namespace beagleradio {

	public class ProcessAction {

		private WeatherService service;
		private EventList events;
		private object[] Buffers { get; set; }

		public event EventHandler<string> PlayRadio;
		public event EventHandler<int> ChangeTab;
		public event EventHandler<string> SpeakText;
		public event EventHandler<int> SetVolume;

		public ProcessAction(WeatherService service, EventList events) {
			this.service = service;
			this.Buffers = new object[10];
			this.events = events;
		}

		public void Process(List<string> actions) {
			foreach (string action in actions) {
				foreach (string command in action.Split(';')) {
					if (command.Trim().Length > 0) {
						Process(command.Trim());
					}
				}
			}
		}

		public void Process(string action) {
			Evaluator eval = new Evaluator(action);
			eval.FindVariable += Eval_FindVariable;
			eval.ExecuteMethod += Eval_ExecuteMethod;
			while (eval.Offset < eval.Source.Length) {
				eval.Evaluate();
			}
		}

		private int GetVarIndex(string varName) {
			if (varName.ToLower().StartsWith("var")) {
				return int.Parse(varName.Substring(3));
			}
			return 0;
		}

		void Eval_ExecuteMethod (object sender, Evaluator.MethodInfo e)
		{
			//Console.WriteLine("Functon " + e.Name);
			switch (e.Name.ToLower()) {
				case "set":
					Buffers[GetVarIndex(e.Args[0].ToString())] = e.Args[1];
					break;
				case "speak":
					if (SpeakText != null) {
						SpeakText(this, e.Args[0].ToString());
					}
					break;
				case "age":
					DateTime dob = Evaluator.ConvertToDateTime(e.Args[0]);
					int years = DateTime.Now.Year - dob.Year;
					if (DateTime.Now.Month == dob.Month && DateTime.Now.Day < dob.Day) {
						years--;
					} else if (DateTime.Now.Month < dob.Month) {
						years--;
					}
					e.ReturnValue = years;
					break;
				case "ordinal":
					e.ReturnValue = Framework.NumberToWordsOrdinal((int)e.Args[0]);
					break;
				case "period":
					if (e.ArgumentCount == 0) {
						e.ReturnValue = Framework.PeriodOfDayToWords(DateTime.Now);
					} else {
						e.ReturnValue = Framework.PeriodOfDayToWords(Evaluator.ConvertToDateTime(e.Args[0]));
					}
					break;
				case "time":
					if (e.ArgumentCount == 0) {
						e.ReturnValue = Framework.TimeToWords(DateTime.Now);
					} else {
						e.ReturnValue = Framework.TimeToWords(Evaluator.ConvertToDateTime(e.Args[0]));
					}
					break;
				case "date":
					if (e.ArgumentCount == 0) {
						e.ReturnValue = Framework.DateToWords(DateTime.Now);
					} else {
						e.ReturnValue = Framework.DateToWords(Evaluator.ConvertToDateTime(e.Args[0]));
					}
					break;
				case "weather":
					e.ReturnValue = "";
					foreach (WeatherService.WeatherPeriod info in service.Forecasts) {
						//WeatherService.WeatherPeriod info = service.Forecasts[0];
						e.ReturnValue += "The weather forecast for " + info.Title + " is " + info.Forecast + ". ";
						break;
					}
					break;
				case "events":
					List<EventItem> items = events.FindActive(DateTime.Now);
					foreach (EventItem item in items) {
						Process(item.Actions);
					}
					break;
				case "playradio":
					if (PlayRadio != null) {
						PlayRadio(this, e.Args[0].ToString());
					}
					break;
				case "showtime":
					if (ChangeTab != null) {
						ChangeTab(this, 1);
					}
					break;
				case "showweather":
					if (ChangeTab != null) {
						ChangeTab(this, 3);
					}
					break;
				case "showradio":
					if (ChangeTab != null) {
						ChangeTab(this, 2);
					}
					break;
				case "setvolume":
					if (SetVolume != null) {
						SetVolume(this, Evaluator.ConvertToInteger(e.Args[0]));
					}
					break;
					
			}
		}

		void Eval_FindVariable (object sender, Evaluator.VariableInfo e)
		{
			if (e.Name.ToLower().StartsWith("var")) {
				int index = int.Parse(e.Name.Substring(3));
				e.Result = Buffers[index];
			}
		}
	}
}

