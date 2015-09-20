using System;
using Gtk;

namespace beagleradio {

	[System.ComponentModel.ToolboxItem(true)]
	public partial class KeyboardWidget : Gtk.Bin {

		private string[] charSet = new string[] {
			"QWERTYUIOPASDFGHJKL-ZXCVBNM,./",
			"qwertyuiopasdfghjkl-zxcvbnm,./",
			"1234567890=:;()$&@?!'\"[]{}#%^*",
			"+=_\\|~<>                      "
		};
		private int charSetIndex = 0;

		public event EventHandler<String> KeyPressed;
		public event EventHandler<EventArgs> KeyDelete;
		public event EventHandler<EventArgs> KeyNext;

		public KeyboardWidget() {
			this.Build();
		}
		protected void CharacterPressed (object sender, EventArgs e) {
			String value = ((Gtk.Button)sender).Label;
			if (KeyPressed != null) {
				KeyPressed(this, value);
			}
		}

		protected void OnButtonDeletePressed (object sender, EventArgs e) {
			if (KeyDelete != null) {
				KeyDelete(this, new EventArgs());
			}
		}

		protected void OnSetShiftPressed (object sender, EventArgs e) {
			charSetIndex = (charSetIndex + 1) % 4;

			buttonLetter1.Label = charSet[charSetIndex].Substring(0,1);
			buttonLetter2.Label = charSet[charSetIndex].Substring(1,1);
			buttonLetter3.Label = charSet[charSetIndex].Substring(2,1);
			buttonLetter4.Label = charSet[charSetIndex].Substring(3,1);
			buttonLetter5.Label = charSet[charSetIndex].Substring(4,1);
			buttonLetter6.Label = charSet[charSetIndex].Substring(5,1);
			buttonLetter7.Label = charSet[charSetIndex].Substring(6,1);
			buttonLetter8.Label = charSet[charSetIndex].Substring(7,1);
			buttonLetter9.Label = charSet[charSetIndex].Substring(8,1);
			buttonLetter10.Label = charSet[charSetIndex].Substring(9,1);

			buttonLetter11.Label = charSet[charSetIndex].Substring(10,1);
			buttonLetter12.Label = charSet[charSetIndex].Substring(11,1);
			buttonLetter13.Label = charSet[charSetIndex].Substring(12,1);
			buttonLetter14.Label = charSet[charSetIndex].Substring(13,1);
			buttonLetter15.Label = charSet[charSetIndex].Substring(14,1);
			buttonLetter16.Label = charSet[charSetIndex].Substring(15,1);
			buttonLetter17.Label = charSet[charSetIndex].Substring(16,1);
			buttonLetter18.Label = charSet[charSetIndex].Substring(17,1);
			buttonLetter19.Label = charSet[charSetIndex].Substring(18,1);
			buttonLetter20.Label = charSet[charSetIndex].Substring(19,1);

			buttonLetter21.Label = charSet[charSetIndex].Substring(20,1);
			buttonLetter22.Label = charSet[charSetIndex].Substring(21,1);
			buttonLetter23.Label = charSet[charSetIndex].Substring(22,1);
			buttonLetter24.Label = charSet[charSetIndex].Substring(23,1);
			buttonLetter25.Label = charSet[charSetIndex].Substring(24,1);
			buttonLetter26.Label = charSet[charSetIndex].Substring(25,1);
			buttonLetter27.Label = charSet[charSetIndex].Substring(26,1);
			buttonLetter28.Label = charSet[charSetIndex].Substring(27,1);
			buttonLetter29.Label = charSet[charSetIndex].Substring(28,1);
			buttonLetter30.Label = charSet[charSetIndex].Substring(29,1);
		}

		protected void OnNextPressed (object sender, EventArgs e) {
			if (KeyNext != null) {
				KeyNext(this, new EventArgs());
			}
		}
	}
}

