using System;
using Gtk;
using System.Collections.Generic;

namespace beagleradio {


	[System.ComponentModel.ToolboxItem(true)]
	public partial class SelectAccessPoint : Gtk.Bin {

		private WirelessWrapper wireless;

		public event EventHandler NextPage;
		public event EventHandler PrevPage;

		public SelectAccessPoint() {
			this.Build();
			wireless = new WirelessWrapper();
			nodeWiFi.NodeStore = new NodeStore(typeof(WiFiListNode));
			nodeWiFi.AppendColumn("Name", new Gtk.CellRendererText(), "text", 0);
			nodeWiFi.AppendColumn("Strength", new Gtk.CellRendererText(), "text", 1);
			nodeWiFi.AppendColumn("Encryption", new Gtk.CellRendererText(), "text", 2);
			nodeWiFi.HeadersVisible = true;
			nodeWiFi.ColumnsAutosize();
		}

		public WirelessInfo SelectedAccessPoint { 
			get {
				WiFiListNode node = (WiFiListNode)nodeWiFi.NodeSelection.SelectedNode;
				if (node != null) return node.Info;
				return null;
			}
		}

		private void RefreshWiFi() {
			NodeStore store = new NodeStore(typeof(WiFiListNode));

			List<WirelessInfo> infoList = wireless.Scan();
			infoList.Sort();
			foreach (WirelessInfo info in infoList) {
				if (info.ESSID.Length > 0) {
					store.AddNode(new WiFiListNode(info));
				}
			}
			nodeWiFi.NodeStore = store;
			nodeWiFi.ColumnsAutosize();
		}

		protected void OnButtonPreviousClicked (object sender, EventArgs e) {
			if (PrevPage != null) {
				PrevPage(this, new EventArgs());
			}
		}

		protected void OnButtonRefreshClicked (object sender, EventArgs e) {
			RefreshWiFi();
		}

		protected void OnButtonNextClicked (object sender, EventArgs e) {
			if (SelectedAccessPoint != null) {
				if (NextPage != null) {
					NextPage(this, new EventArgs());
				}
			}
		}

		[Gtk.TreeNode (ListOnly=true)]
		public class WiFiListNode : Gtk.TreeNode {

			public WiFiListNode (WirelessInfo info)
			{
				this.Info = info;
			}

			public WirelessInfo Info { get; set; }

			[Gtk.TreeNodeValue (Column=0)]
			public string Name { get { return Info.ESSID; } }

			[Gtk.TreeNodeValue (Column=1)]
			public string Strength { get { return Info.Signal.ToString(); } }

			[Gtk.TreeNodeValue (Column=2)]
			public string Encryption { get { return Info.Security.ToString(); } }
		}
	}
}

