
// This file has been generated by the GUI designer. Do not modify.
namespace beagleradio
{
	public partial class Intercom
	{
		private global::Gtk.Table table1;
		
		private global::Gtk.ScrolledWindow GtkScrolledWindow;
		
		private global::Gtk.NodeView nodeSip;
		
		private global::Gtk.HButtonBox hbuttonbox1;
		
		private global::Gtk.Button buttonCall;
		
		private global::Gtk.Button buttonAnswer;
		
		private global::Gtk.Button buttonHangUp;
		
		private global::Gtk.Button buttonDelete;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget beagleradio.Intercom
			global::Stetic.BinContainer.Attach (this);
			this.Name = "beagleradio.Intercom";
			// Container child beagleradio.Intercom.Gtk.Container+ContainerChild
			this.table1 = new global::Gtk.Table (((uint)(2)), ((uint)(1)), false);
			this.table1.Name = "table1";
			this.table1.RowSpacing = ((uint)(6));
			this.table1.ColumnSpacing = ((uint)(6));
			// Container child table1.Gtk.Table+TableChild
			this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
			this.GtkScrolledWindow.Name = "GtkScrolledWindow";
			this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
			// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
			this.nodeSip = new global::Gtk.NodeView ();
			this.nodeSip.CanFocus = true;
			this.nodeSip.Name = "nodeSip";
			this.GtkScrolledWindow.Add (this.nodeSip);
			this.table1.Add (this.GtkScrolledWindow);
			// Container child table1.Gtk.Table+TableChild
			this.hbuttonbox1 = new global::Gtk.HButtonBox ();
			this.hbuttonbox1.Name = "hbuttonbox1";
			this.hbuttonbox1.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(1));
			// Container child hbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
			this.buttonCall = new global::Gtk.Button ();
			this.buttonCall.CanFocus = true;
			this.buttonCall.Name = "buttonCall";
			this.buttonCall.UseUnderline = true;
			this.buttonCall.Label = global::Mono.Unix.Catalog.GetString ("Call");
			this.hbuttonbox1.Add (this.buttonCall);
			global::Gtk.ButtonBox.ButtonBoxChild w3 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox1 [this.buttonCall]));
			w3.Expand = false;
			w3.Fill = false;
			// Container child hbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
			this.buttonAnswer = new global::Gtk.Button ();
			this.buttonAnswer.CanFocus = true;
			this.buttonAnswer.Name = "buttonAnswer";
			this.buttonAnswer.UseUnderline = true;
			this.buttonAnswer.Label = global::Mono.Unix.Catalog.GetString ("Answer");
			this.hbuttonbox1.Add (this.buttonAnswer);
			global::Gtk.ButtonBox.ButtonBoxChild w4 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox1 [this.buttonAnswer]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			// Container child hbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
			this.buttonHangUp = new global::Gtk.Button ();
			this.buttonHangUp.CanFocus = true;
			this.buttonHangUp.Name = "buttonHangUp";
			this.buttonHangUp.UseUnderline = true;
			this.buttonHangUp.Label = global::Mono.Unix.Catalog.GetString ("Hang Up");
			this.hbuttonbox1.Add (this.buttonHangUp);
			global::Gtk.ButtonBox.ButtonBoxChild w5 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox1 [this.buttonHangUp]));
			w5.Position = 2;
			w5.Expand = false;
			w5.Fill = false;
			// Container child hbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
			this.buttonDelete = new global::Gtk.Button ();
			this.buttonDelete.CanFocus = true;
			this.buttonDelete.Name = "buttonDelete";
			this.buttonDelete.UseUnderline = true;
			this.buttonDelete.Label = global::Mono.Unix.Catalog.GetString ("Delete");
			this.hbuttonbox1.Add (this.buttonDelete);
			global::Gtk.ButtonBox.ButtonBoxChild w6 = ((global::Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox1 [this.buttonDelete]));
			w6.Position = 3;
			w6.Expand = false;
			w6.Fill = false;
			this.table1.Add (this.hbuttonbox1);
			global::Gtk.Table.TableChild w7 = ((global::Gtk.Table.TableChild)(this.table1 [this.hbuttonbox1]));
			w7.TopAttach = ((uint)(1));
			w7.BottomAttach = ((uint)(2));
			w7.YOptions = ((global::Gtk.AttachOptions)(4));
			this.Add (this.table1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.buttonCall.Clicked += new global::System.EventHandler (this.OnButtonCallClicked);
			this.buttonAnswer.Clicked += new global::System.EventHandler (this.OnButtonAnswerClicked);
			this.buttonHangUp.Clicked += new global::System.EventHandler (this.OnButtonHangUpClicked);
			this.buttonDelete.Clicked += new global::System.EventHandler (this.OnButtonDeleteClicked);
		}
	}
}
