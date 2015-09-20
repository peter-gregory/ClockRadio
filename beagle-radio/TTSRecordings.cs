using System;
using System.Collections.Generic;
using System.IO;

namespace beagleradio {

	public class TTSRecordings {
		
		private const string SPEECH_PATH = "speech/";
		private List<string> filenames;

		public event EventHandler<string> Status;
		public event EventHandler<List<string>> MissingWords;

		public TTSRecordings() {
		}

		public class CompareLengths : IComparer<string> {
			public int Compare (string x, string y) {
				return y.Length - x.Length;
			}
		}

		public void Build(string filename, string speech) {
			LoadFilenames();
			FileStream file = new FileStream(filename, FileMode.Create);
			SpeakWordsToFile(file, speech);
			file.Close();
			file.Dispose();
		}

		private void LoadFilenames() {
			if (filenames == null) {
				filenames = new List<string>();
				foreach (string filename in Directory.EnumerateFiles(SPEECH_PATH)) {
					if (Path.GetExtension(filename) == ".raw") {
						filenames.Add(Path.GetFileNameWithoutExtension(filename));
					}
				}
				filenames.Sort(new CompareLengths());
			}
		}

		private void SpeakFileToFile(FileStream buffer, string filename) {

			FileStream file = new FileStream(SPEECH_PATH + filename, FileMode.Open);
			byte[] data = new byte[file.Length];
			file.Read(data, 0, (int) file.Length);
			file.Close();
			file.Dispose();
			buffer.Write(data, 0, data.Length);
		}

		private bool SpeakWordsToFile(FileStream buffer, string words) {

			List<String> missing = new List<string>();

			words = " " + Framework.ExpandToWords(words) + " ";

			if (Status != null) {
				Status(this, words.Trim());
			}

			words = words.Replace("-", " silence200.raw ");
			words = words.Replace(":", " silence200.raw ");
			words = words.Replace(";", " silence200.raw ");
			words = words.Replace(".", " silence200.raw ");
			words = words.Replace(",", " silence200.raw ");
			words = words.Replace("!", " silence200.raw ");
			words = words.Replace("?", " silence200.raw ");

			foreach (string phrase in filenames) {
				while (words.Contains(" " + phrase.Replace("_", " ") + " ")) {
					words = words.Replace(" " + phrase.Replace("_", " ") + " ", " " + phrase + ".raw ");
				}
			}

			foreach (string item in words.Trim().Split(' ')) {
				string word = item.Trim();
				if (word.Length > 0) {
					if (!word.EndsWith(".raw")) {
						missing.Add(word);
					} else {
						SpeakFileToFile(buffer, word);
					}
				}
			}

			if (missing.Count > 0 && MissingWords != null) {
				MissingWords(this, missing);
				filenames = null;
			}

			return missing.Count == 0;
		}
	}
}

