using System;
using System.Diagnostics;
using Gtk;
using System.Threading;

namespace beagleradio {

	public class SpeechRecognition {

		private Process process;

		public event EventHandler<String> Closed;
		public event EventHandler<String> StatusChanged;
		public event EventHandler<String> LogMessage;
		public event EventHandler<String> Recognized;

		public bool IsBusy { get; private set; }
		public bool IsListening { get; private set; }
		public string Status { get; private set; }
		public string Noise { get; private set; }
		public string BufferSize { get; private set; }
		public string KeywordRecognized { get; private set; }
		public DateTime TimeRecognized { get; private set; }

		public SpeechRecognition() {
			Status = "Idle";
		}

		public void Start() {
			if (process == null) {
				IsBusy = true;
				Console.WriteLine("Starting recognizer");
				ProcessStartInfo info = new ProcessStartInfo();
				info.FileName = "recognize";
				info.CreateNoWindow = true;
				info.UseShellExecute = false;
				info.RedirectStandardInput = true;
				info.RedirectStandardOutput = true;
				process = Process.Start(info);
				process.EnableRaisingEvents = true;
				process.OutputDataReceived += Process_OutputDataReceived;
				process.BeginOutputReadLine();
				process.Exited += Process_Done;
				Status = "Starting";
				Noise = "Idle";
				BufferSize = "0";
				if (StatusChanged != null) {
					StatusChanged(this, Status);
				}
			}
		}

		public void StartListening() {
			if (process != null) {
				process.StandardInput.Write("M\n");
			}
		}

		public void StopListening() {
			if (process != null) {
				process.StandardInput.Write("Q\n");
			}
		}

		public void Close() {
			if (process != null) {
				process.StandardInput.Write("X\n");
			}
		}

		void Process_OutputDataReceived (object sender, DataReceivedEventArgs e)
		{
			if (LogMessage != null) {
				LogMessage(this, e.Data);
			}
			string line = e.Data;
			if (line.StartsWith("STATUS:")) {
				Status = line.Substring(line.IndexOf(":") + 1).Trim();
				if (StatusChanged != null) {
					StatusChanged(this, Status);
				}
			}
			if (line.StartsWith("NOISE:")) {
				Noise = line.Substring(line.IndexOf(":") + 1).Trim();
				if (StatusChanged != null) {
					StatusChanged(this, Status);
				}
			}
			if (line.StartsWith("BUFFER_SIZE:")) {
				BufferSize = line.Substring(line.IndexOf(":") + 1).Trim();
				if (StatusChanged != null) {
					StatusChanged(this, Status);
				}
			}
			if (line.StartsWith("RECOGNIZED:")) {
				Console.WriteLine("Received recognized keyword: " + line);
				TimeRecognized = DateTime.Now;
				KeywordRecognized = line.Substring(line.IndexOf(":") + 1).Trim();
				if (Recognized != null) {
					Recognized(this, KeywordRecognized);
				}
			}
		}

		void Process_Done (object sender, EventArgs e)
		{
			process.Close();
			process.Dispose();
			process = null;
			IsBusy = false;
			if (Closed != null) {
				Closed(this, null);
			}
		}
	}
}

