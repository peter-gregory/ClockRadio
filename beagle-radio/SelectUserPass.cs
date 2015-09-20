using System;

namespace beagleradio {

	[System.ComponentModel.ToolboxItem(true)]
	public partial class SelectUserPass : Gtk.Bin {

		public event EventHandler NextPage;
		public event EventHandler PrevPage;

		private int entryField = 0;

		public String User { get { return entryUser.Text; } }
		public String Password { get { return entryPassword.Text; } }

		public SelectUserPass() {
			this.Build();
			keyboardwidget1.KeyPressed += Keyboardwidget1_KeyPressed;
			keyboardwidget1.KeyDelete += Keyboardwidget1_KeyDelete;
			keyboardwidget1.KeyNext += Keyboardwidget1_KeyNext;
		}
		void Keyboardwidget1_KeyNext (object sender, EventArgs e)
		{
			switch (entryField) {
				case 0:
					if (labelPass.Visible) {
						entryField = 1;
						entryPassword.GrabFocus();
					}
					break;
				case 1:
					if (labelUser.Visible) {
						entryField = 0;
						entryUser.GrabFocus();
					}
					break;
			}
		}

		void Keyboardwidget1_KeyDelete (object sender, EventArgs e)
		{
			switch (entryField) {
				case 0:
					if (entryUser.Text.Length > 0) {
						entryUser.Text = entryUser.Text.Substring(0, entryUser.Text.Length - 1);
					}
					break;
				case 1:
					if (entryPassword.Text.Length > 0) {
						entryPassword.Text = entryPassword.Text.Substring(0, entryPassword.Text.Length - 1);
					}
					break;
			}
		}

		void Keyboardwidget1_KeyPressed (object sender, string e)
		{
			switch (entryField) {
				case 0:
					entryUser.Text += e;
					break;
				case 1:
					entryPassword.Text += e;
					break;
			}
		}

		public void Initialize(WirelessInfo info) {
			if (info.Security.ToLower().Contains("psk")) {
				labelTitle.LabelProp = "Enter Passphrase for " + info.ESSID;
				entryField = 1;
				labelUser.Visible = false;
				entryUser.Visible = false;
				labelPass.Visible = true;
				labelPass.LabelProp = "Phrase";
				entryPassword.Visible = true;
			} else {
				labelTitle.LabelProp = "Enter User and Password for " + info.ESSID;
				entryField = 0;
				labelUser.Visible = true;
				labelUser.LabelProp = "User";
				entryUser.Visible = true;
				labelPass.Visible = true;
				labelPass.LabelProp = "Pass";
				entryPassword.Visible = true;
			}
		}

		protected void OnEntryUserFocusInEvent (object o, Gtk.FocusInEventArgs args) {
			entryField = 0;
		}

		protected void OnEntryPasswordFocusInEvent (object o, Gtk.FocusInEventArgs args) {
			entryField = 1;
		}

		protected void OnButtonPreviousClicked (object sender, EventArgs e) {
			if (PrevPage != null) {
				PrevPage(this, new EventArgs());
			}
		}

		protected void OnButtonNextClicked (object sender, EventArgs e) {
			if (NextPage != null) {
				NextPage(this, new EventArgs());
			}
		}
	}
}

