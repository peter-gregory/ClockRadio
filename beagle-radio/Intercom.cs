using System;
using Gtk;
using System.IO;
using System.Diagnostics;

namespace beagleradio {

	[System.ComponentModel.ToolboxItem(true)]
	public partial class Intercom : Gtk.Bin {

		private Process process;

		public bool IsBusy { get; set; }
		
		public Intercom() {
			this.Build();
			IsBusy = false;
			nodeSip.NodeStore = new NodeStore(typeof(SipListNode));
			nodeSip.AppendColumn("Name", new Gtk.CellRendererText(), "text", 0);
			nodeSip.AppendColumn("Status", new Gtk.CellRendererText(), "text", 1);
			nodeSip.HeadersVisible = true;
			LoadPhones();
		}

		public void Start() {
			ProcessStartInfo info = new ProcessStartInfo();
			info.Arguments = "";
			info.FileName = "linphonec";
			info.CreateNoWindow = true;
			info.UseShellExecute = false;
			info.RedirectStandardError = true;
			info.RedirectStandardInput = true;
			info.RedirectStandardOutput = true;
			process = Process.Start(info);
			process.ErrorDataReceived += Process_ErrorDataReceived;
			process.OutputDataReceived += Process_OutputDataReceived;
			process.EnableRaisingEvents = true;
			process.BeginErrorReadLine();
			process.BeginOutputReadLine();
			process.Exited += Process_Exited;
		}

		public void Close() {
			if (process != null) {
				process.StandardInput.WriteLine("quit");
			}
		}

		bool FindNode(out TreeIter iter, string match, int column) {
			if (nodeSip.Model.GetIterFirst(out iter)) {
				do {
					string value = nodeSip.Model.GetValue(iter, column).ToString();
					//Console.WriteLine("Matching column " + column + ": '" + value + "' = '" + match + "'");
					if (value == match) {
						//Console.WriteLine("Matching column " + column + ": success!");
						return true;
					}
				} while (nodeSip.Model.IterNext(ref iter));
			}
			return false;
		}

		void Process_Exited (object sender, EventArgs e)
		{
			process.Close();
			process.Dispose();
			process = null;
		}

		void Process_OutputDataReceived (object sender, DataReceivedEventArgs e)
		{
			string line = e.Data;
			//Console.WriteLine("linphone: " + line);
			if (line.Contains("linphonec> Receiving new incoming call from ")) {
				IsBusy = true;
				//Receiving new incoming call from <sip:pgregory@10.128.84.200>, assigned id 1
				string caller = Framework.Extract(line, 0, "<", ">");
				string callerIp = Framework.Extract(caller, 0, "@", "");
				string id = line.Substring(line.LastIndexOf(" ") + 1);
				TreeIter iter;
				if (FindNode(out iter, callerIp, 2)) {
					TreePath path = nodeSip.Model.GetPath(iter);
					SipListNode node = (SipListNode) nodeSip.NodeStore.GetNode(path);
					node.Id = id;
					node.Status = "Ringing";
					nodeSip.Model.EmitRowChanged(path, iter);
				} else {
					nodeSip.NodeStore.AddNode(new SipListNode(callerIp, "Calling", callerIp, id));
				}
			} else if (line.Contains("linphonec> Call ") && line.Contains(" ended ")) {
				//Call 1 with <sip:pgregory@10.128.84.200> ended (No error).
				string id = Framework.Extract(line, 0, " Call ", " with ");
				TreeIter iter;
				if (FindNode(out iter, id, 3)) {
					TreePath path = nodeSip.Model.GetPath(iter);
					SipListNode node = (SipListNode) nodeSip.NodeStore.GetNode(path);
					node.Status = "";
					node.Id = "";
					nodeSip.Model.EmitRowChanged(path, iter);
					IsBusy = false;
				}
			} else if(line.Contains("linphonec> Call ") && line.Contains(" connected.")) {
				// linphonec> Call 1 with <sip:pgregory@192.168.1.170> connected.
				string id = Framework.Extract(line, 0, " Call ", " with ");
				TreeIter iter;
				if (FindNode(out iter, id, 3)) {
					TreePath path = nodeSip.Model.GetPath(iter);
					SipListNode node = (SipListNode) nodeSip.NodeStore.GetNode(path);
					node.Status = "Connected";
					nodeSip.Model.EmitRowChanged(path, iter);
				}
			}
		}

		void Process_ErrorDataReceived (object sender, DataReceivedEventArgs e)
		{
			Console.WriteLine("linphone ERROR: " + e.Data);
		}

		private void LoadPhones() {
			
			NodeStore store = new NodeStore(typeof(SipListNode));

			try {
				FileStream file = new FileStream("sip.txt", FileMode.OpenOrCreate);
				StreamReader reader = new StreamReader(file);
				string line = reader.ReadLine();
				while (line != null) {
					line = reader.ReadLine();
					string[] parts = line.Split(',');
					store.AddNode(new SipListNode(parts[0].Trim(), "", parts[1].Trim(), ""));
				}
				reader.Close();
				reader.Dispose();
				file.Close();
				file.Dispose();

			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}

			nodeSip.NodeStore = store;
			nodeSip.ColumnsAutosize();
		}

		protected void OnButtonCallClicked (object sender, EventArgs e) {
			SipListNode node = (SipListNode)nodeSip.NodeSelection.SelectedNode;
			if (node != null && node.Status == "") {
			}
		}

		protected void OnButtonAnswerClicked (object sender, EventArgs e) {
			TreeIter iter;
			if (FindNode(out iter, "Calling", 1)) {
				TreePath path = nodeSip.Model.GetPath(iter);
				SipListNode node = (SipListNode) nodeSip.NodeStore.GetNode(path);
				node.Status = "Answering";
				process.StandardInput.WriteLine("answer " + node.Id);
				nodeSip.Model.EmitRowChanged(path, iter);
			}
		}

		protected void OnButtonHangUpClicked (object sender, EventArgs e) {
			TreeIter iter;
			if (FindNode(out iter, "Connected", 1)) {
				TreePath path = nodeSip.Model.GetPath(iter);
				SipListNode node = (SipListNode) nodeSip.NodeStore.GetNode(path);
				node.Status = "Hanging Up";
				process.StandardInput.WriteLine("terminate " + node.Id);
				nodeSip.Model.EmitRowChanged(path, iter);
			}
		}

		protected void OnButtonDeleteClicked (object sender, EventArgs e) {
			throw new NotImplementedException();
		}

		[Gtk.TreeNode (ListOnly=true)]
		public class SipListNode : Gtk.TreeNode {

			public SipListNode (string name, string status, string address, string id)
			{
				Name = name;
				Status = status;
				Address = address;
				Id = id;
			}

			[Gtk.TreeNodeValue (Column=0)]
			public string Name { get; set; }

			[Gtk.TreeNodeValue (Column=1)]
			public string Status { get; set; }

			[Gtk.TreeNodeValue (Column=2)]
			public string Address { get; set; }

			[Gtk.TreeNodeValue (Column=3)]
			public string Id { get; set; }
		}
	}
}

