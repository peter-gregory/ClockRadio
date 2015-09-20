using System;
using Gtk;
using System.Threading;
using System.Text;

namespace beagleradio {

	public class Framework {
		public Framework() {
		}

		public static void GuiDelay() {
			if (Application.EventsPending()) {
				Application.RunIteration();
			} else {
				Thread.Sleep(100);
			}
		}

		public static string Extract(string value, int index=0, string delim1="", string delim2="") {
			if (delim1 != "") {
				int startIndex = value.IndexOf(delim1, index);
				if (startIndex > 0) {
					index = startIndex + delim1.Length;
				}
			}
			int endIndex = value.Length;
			if (delim2 != "") {
				int test = value.IndexOf(delim2, index);
				if (test > 0) {
					endIndex = test;
				}
			}
			return value.Substring(index, endIndex - index).Trim();
		}

		public static bool ExtractKey (out string result, string value, string key, string delim1="", string delim2="") {
			result = "";
			int index = value.IndexOf(key);
			if (index < 0) return false;
			index += key.Length;
			result = Extract(value, index, delim1, delim2);
			return true;
		}

		public static string DoubleToWords(double number) {
			if (number == 0) return "zero";
			string words;
			int fractionPart;
			if (number < 0) {
				words = NumberToWords((int)Math.Ceiling(number));
				fractionPart = (int)(-(number - Math.Ceiling(number)) * 100);
			} else {
				words = NumberToWords((int)Math.Floor(number));
				fractionPart = (int)(Math.Abs(number - Math.Floor(number)) * 100);
			}
			if (fractionPart != 0) {
				words += " point ";
				if (fractionPart < 10) {
					words += " oh ";
				}
				words += NumberToWords(fractionPart);
			}
			return words;
		}

		public static string NumberToWords(int number)
		{
			if (number == 0)
				return "zero";

			if (number < 0)
				return "minus " + NumberToWords(Math.Abs(number));

			string words = "";

			if ((number / 1000000) > 0)
			{
				words += NumberToWords(number / 1000000) + " million ";
				number %= 1000000;
			}

			if ((number / 1000) > 0)
			{
				words += NumberToWords(number / 1000) + " thousand ";
				number %= 1000;
			}

			if ((number / 100) > 0)
			{
				words += NumberToWords(number / 100) + " hundred ";
				number %= 100;
			}

			if (number > 0)
			{
				if (words != "")
					words += "and ";

				var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
				var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

				if (number < 20)
					words += unitsMap[number];
				else
				{
					words += tensMap[number / 10];
					if ((number % 10) > 0)
						words += " " + unitsMap[number % 10];
				}
			}

			return words;
		}

		public static string NumberToRange(int number) {
			string words = NumberToWords(number);
			String lastWord = words;
			int index = lastWord.LastIndexOf(" ");
			if (index > 0) {
				lastWord = lastWord.Substring(index + 1);
			}
			words = words.Substring(0, words.Length - lastWord.Length);
			switch (lastWord) {
				case "six":
					lastWord = "sixes";
					break;
				case "ten":
					lastWord = "teens";
					break;
				case "twenty":
					lastWord = "twenties";
					break;
				case "thirty":
					lastWord = "thirties";
					break;
				case "forty":
					lastWord = "fouties";
					break;
				case "fifty":
					lastWord = "fifties";
					break;
				case "sixty":
					lastWord = "sixties";
					break;
				case "seventy":
					lastWord = "seventies";
					break;
				case "eighty":
					lastWord = "eighties";
					break;
				case "ninety":
					lastWord = "nineties";
					break;
				default:
					lastWord += "s";
					break;
			}
			return words + " " + lastWord;
		}

		public static string NumberToWordsOrdinal(int number) {
			String words = NumberToWords(number);
			String lastWord = words;
			int index = lastWord.LastIndexOf(" ");
			if (index > 0) {
				lastWord = lastWord.Substring(index + 1);
			}
			words = words.Substring(0, words.Length - lastWord.Length);
			switch (lastWord) {
				case "zero":
					lastWord = "zeroth";
					break;
				case "one":
					lastWord = "first";
					break;
				case "two":
					lastWord = "second";
					break;
				case "three":
					lastWord = "third";
					break;
				case "five":
					lastWord = "fifth";
					break;
				case "eight":
					lastWord = "eighth";
					break;
				case "twenty":
					lastWord = "twentieth";
					break;
				case "thirty":
					lastWord = "thirtieth";
					break;
				case "forty":
					lastWord = "fortieth";
					break;
				case "fifty":
					lastWord = "fiftieth";
					break;
				case "sixty":
					lastWord = "sixtieth";
					break;
				case "seventy":
					lastWord = "seventieth";
					break;
				case "eighty":
					lastWord = "eightieth";
					break;
				case "ninety":
					lastWord = "ninetieth";
					break;
				default:
					lastWord += "th";
					break;
			}
			return words + " " + lastWord;
		}

		public static string PeriodOfDayToWords(DateTime cur) {
			string period = "morning";
			if (cur.Hour >= 12) {
				period = "afternoon";
				if (cur.Hour >= 17) {
					period = "evening";
				}
			}
			return period;
		}

		public static string DateToWords(DateTime cur) {
			return cur.ToString("dddd MMMM ") + NumberToWordsOrdinal(cur.Day) + ", " + NumberToWords(cur.Year / 100) + " " + NumberToWords(cur.Year % 100);
		}

		public static string TimeToWords(DateTime cur) {
			string result = cur.Hour % 12 == 0 ? "12" : (cur.Hour % 12).ToString();
			if (cur.Minute == 0) {
				result += " oh clock";
			} else if (cur.Minute < 10) {
				result += " oh " + NumberToWords(cur.Minute);
			} else {
				result += " " + NumberToWords(cur.Minute);
			}

			if (cur.Hour > 11) {
				result += " p m";
			} else {
				result += " a m";
			}
			return result;
		}

		public static string ExpandToWords(string words) {

			int testValue;
			double testDouble;
			StringBuilder builder = new StringBuilder();

			words = words.Replace(".", " . ");
			words = words.Replace("?", " ? ");
			words = words.Replace("!", " ! ");
			words = words.Replace(",", " , ");
			words = words.Replace(":", " : ");
			words = words.Replace(";", " ; ");
			words = words.Replace("-", " - ");

			words = words.Replace(" N ", " north ");
			words = words.Replace(" NW ", " north west ");
			words = words.Replace(" NNW ", " north north west ");
			words = words.Replace(" NNE ", " north north east ");
			words = words.Replace(" NE ", " north east ");
			words = words.Replace(" NNE ", " north north east ");
			words = words.Replace(" NNW ", " north north west ");

			words = words.Replace(" S ", " south ");
			words = words.Replace(" SW ", " south west ");
			words = words.Replace(" SE ", " south east ");
			words = words.Replace(" SSW ", " south south west ");
			words = words.Replace(" SSE ", " south south east ");

			words = words.Replace(" W ", " west ");
			words = words.Replace(" WN ", " west north ");
			words = words.Replace(" WNW ", " west north west ");
			words = words.Replace(" WNE ", " west north east ");
			words = words.Replace(" WS ", " west south ");
			words = words.Replace(" WSW ", " west south west ");
			words = words.Replace(" WSE ", " west south east ");

			words = words.Replace(" E ", " east ");
			words = words.Replace(" EN ", " east north ");
			words = words.Replace(" ENW ", " east north west ");
			words = words.Replace(" ENE ", " east north east ");
			words = words.Replace(" ES ", " east south ");
			words = words.Replace(" ESW ", " east south west ");
			words = words.Replace(" ESE ", " east south east ");

			words = words.Replace(" MPH ", " miles per hour ");
			words = words.Replace(" mph ", " miles per hour ");

			foreach (string item in words.Split(new char[] {' ', '\n', '\t', '\r'})) {
				string word = item.Trim().ToLower();
				if (word.Length > 0) {
					if (builder.Length > 0) builder.Append(" ");
					if (word.Length > 0) {
						if (word[word.Length - 1] == 'c' && int.TryParse(word.Substring(0, word.Length - 1), out testValue)) {
							builder.Append(Framework.NumberToWords(testValue));
							builder.Append(" degrees centigrade");
						} else if (word[word.Length - 1] == 'f' && int.TryParse(word.Substring(0, word.Length - 1), out testValue)) {
							builder.Append(Framework.NumberToWords(testValue));
							builder.Append(" degrees farenheit");
						} else if (word[word.Length - 1] == 's' && int.TryParse(word.Substring(0, word.Length - 1), out testValue)) {
							builder.Append(Framework.NumberToRange(testValue));
						} else if (word[word.Length - 1] == '%' && int.TryParse(word.Substring(0, word.Length - 1), out testValue)) {
							builder.Append(Framework.NumberToWords(testValue));
							builder.Append(" percent");
						} else if (int.TryParse(word, out testValue)) {
							builder.Append(Framework.NumberToWords(testValue));
						} else if (double.TryParse(word, out testDouble)) {
							builder.Append(Framework.DoubleToWords(testDouble));
						} else {
							builder.Append(word);
						}
					}
				}
			}
			return builder.ToString();
		}
	}
}

