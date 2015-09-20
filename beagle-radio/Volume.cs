using System;
using Gdk;
using Gtk;

namespace beagleradio {

	[System.ComponentModel.ToolboxItem(true)]
	public partial class Volume : Gtk.Bin {

		public int Percentage { get; set; }

		public Volume() {
			this.Build();
		}

		protected void OnDrawVolumeExposeEvent (object o, Gtk.ExposeEventArgs args) {

			Gdk.Color back = Style.Background(StateType.Normal);
			Gdk.Color fore = Style.Foreground(StateType.Normal);

			Drawable draw = args.Event.Window;

			int width;
			int height;
			draw.GetSize(out width, out height);

			Gdk.GC gc = new Gdk.GC(draw);

			gc.Foreground = back;
			gc.Background = back;
			draw.DrawRectangle(gc, true, 0, 0, width, height);

			//Color lightSlateGray = new Color(119,136,153);

			//gc.Colormap.AllocColor(ref lightSlateGray, false, true);

			int outside = height * 8 / 10;
			int middle = height * 6 / 10;
			int inside = height * 4 / 10;

			int startOut = 24;
			int startMiddle = 30;
			int startInside = 36;

			int endOut = width - 24;
			int endMiddle = width - 30;
			int endInside = width - 36;

			gc.Foreground = fore;
			gc.Foreground = fore;
			gc.SetLineAttributes(outside, LineStyle.Solid, CapStyle.Round, JoinStyle.Round);
			draw.DrawLine(gc, startOut, height / 2, endOut, height / 2);

			gc.Foreground = back;
			gc.Background = back;
			gc.SetLineAttributes(middle, LineStyle.Solid, CapStyle.Round, JoinStyle.Round);
			draw.DrawLine(gc, startMiddle, height / 2, endMiddle, height / 2);


			int endX = (endInside - startInside) * Percentage / 100 + startInside;
			gc.Foreground = fore;
			gc.Foreground = fore;
			gc.SetLineAttributes(inside, LineStyle.Solid, CapStyle.Round, JoinStyle.Round);
			draw.DrawLine(gc, startInside, height / 2, endX, height / 2);


			gc.Dispose();
		}

		protected void OnEventVolumeButtonPressEvent (object o, ButtonPressEventArgs args) {
			int width;
			int height;
			drawVolume.GdkWindow.GetSize(out width, out height);

			int x = (int) args.Event.X;
			int startInside = 36;
			int endInside = width - 36;
			if (x < startInside) {
				Percentage = 0;
			} else if (x > endInside) {
				Percentage = 100;
			} else {
				Percentage = (x - startInside) * 100 / (endInside - startInside);
			}

			QueueDraw();
		}

		protected void OnEventVolumeMotionNotifyEvent (object o, MotionNotifyEventArgs args) {
			int width;
			int height;
			drawVolume.GdkWindow.GetSize(out width, out height);

			int x = (int) args.Event.X;
			int startInside = 36;
			int endInside = width - 36;

			if (x < startInside) {
				Percentage = 0;
			} else if (x > endInside) {
				Percentage = 100;
			} else {
				Percentage = (x - startInside) * 100 / (endInside - startInside);
			}

			QueueDraw();
		}
	}
}

