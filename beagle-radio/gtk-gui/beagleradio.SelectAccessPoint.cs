
// This file has been generated by the GUI designer. Do not modify.
namespace beagleradio
{
	public partial class SelectAccessPoint
	{
		private global::Gtk.Table table2;
		
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		
		private global::Gtk.NodeView nodeWiFi;
		
		private global::Gtk.VButtonBox vbuttonbox1;
		
		private global::Gtk.Button buttonPrevious;
		
		private global::Gtk.Button buttonRefresh;
		
		private global::Gtk.Button buttonNext;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget beagleradio.SelectAccessPoint
			global::Stetic.BinContainer.Attach (this);
			this.Name = "beagleradio.SelectAccessPoint";
			// Container child beagleradio.SelectAccessPoint.Gtk.Container+ContainerChild
			this.table2 = new global::Gtk.Table (((uint)(1)), ((uint)(2)), false);
			this.table2.Name = "table2";
			this.table2.RowSpacing = ((uint)(6));
			this.table2.ColumnSpacing = ((uint)(6));
			this.table2.BorderWidth = ((uint)(4));
			// Container child table2.Gtk.Table+TableChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.nodeWiFi = new global::Gtk.NodeView ();
			this.nodeWiFi.CanFocus = true;
			this.nodeWiFi.Name = "nodeWiFi";
			this.nodeWiFi.EnableSearch = false;
			this.GtkScrolledWindow.Add (this.nodeWiFi);
			this.table2.Add (this.GtkScrolledWindow);
			// Container child table2.Gtk.Table+TableChild
			this.vbuttonbox1 = new global::Gtk.VButtonBox ();
			this.vbuttonbox1.Name = "vbuttonbox1";
			this.vbuttonbox1.Spacing = 4;
			this.vbuttonbox1.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(3));
			// Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
			this.buttonPrevious = new global::Gtk.Button ();
			this.buttonPrevious.CanFocus = true;
			this.buttonPrevious.Name = "buttonPrevious";
			this.buttonPrevious.UseUnderline = true;
			this.buttonPrevious.Label = global::Mono.Unix.Catalog.GetString ("Previous");
			this.vbuttonbox1.Add (this.buttonPrevious);
			global::Gtk.ButtonBox.ButtonBoxChild w3 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1 [this.buttonPrevious]));
			w3.Expand = false;
			w3.Fill = false;
			// Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
			this.buttonRefresh = new global::Gtk.Button ();
			this.buttonRefresh.CanFocus = true;
			this.buttonRefresh.Name = "buttonRefresh";
			this.buttonRefresh.UseUnderline = true;
			this.buttonRefresh.Label = global::Mono.Unix.Catalog.GetString ("Refresh");
			this.vbuttonbox1.Add (this.buttonRefresh);
			global::Gtk.ButtonBox.ButtonBoxChild w4 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1 [this.buttonRefresh]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			// Container child vbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
			this.buttonNext = new global::Gtk.Button ();
			this.buttonNext.CanFocus = true;
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.UseUnderline = true;
			this.buttonNext.Label = global::Mono.Unix.Catalog.GetString ("Next");
			this.vbuttonbox1.Add (this.buttonNext);
			global::Gtk.ButtonBox.ButtonBoxChild w5 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.vbuttonbox1 [this.buttonNext]));
			w5.Position = 2;
			w5.Expand = false;
			w5.Fill = false;
			this.table2.Add (this.vbuttonbox1);
			global::Gtk.Table.TableChild w6 = ((global::Gtk.Table.TableChild)(this.table2 [this.vbuttonbox1]));
			w6.LeftAttach = ((uint)(1));
			w6.RightAttach = ((uint)(2));
			w6.XOptions = ((global::Gtk.AttachOptions)(4));
			w6.YOptions = ((global::Gtk.AttachOptions)(4));
			this.Add (this.table2);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.buttonPrevious.Clicked += new global::System.EventHandler (this.OnButtonPreviousClicked);
			this.buttonRefresh.Clicked += new global::System.EventHandler (this.OnButtonRefreshClicked);
			this.buttonNext.Clicked += new global::System.EventHandler (this.OnButtonNextClicked);
		}
	}
}