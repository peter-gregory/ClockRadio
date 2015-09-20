using System;
using Gdk;
using System.Collections.Generic;
using Gtk;

namespace beagleradio {

	public class GuiVolume {

		public int Volume { get; set; }

		public GuiVolume() {
			Volume = 0;
		}

		public void Draw(Drawable draw, Gdk.Color back) {
			Gdk.GC gc = new Gdk.GC(draw);

			try {

				int width;
				int height;
				draw.GetSize(out width, out height);

				int xPoint = (width * Volume) / 100;
				int yTop = height * Volume / 100;

				List<Point> points = new List<Point>();
				points.Add(new Point(0,0));
				points.Add(new Point(xPoint, yTop));
				points.Add(new Point(xPoint, height));
				points.Add(new Point(0,height));
				points.Add(new Point(0,0));

				gc.Foreground = back;
				gc.Background = back;
				draw.DrawPolygon(gc, true, points.ToArray());
				draw.DrawRectangle(gc, true, xPoint, 0, width, height);

				points.Clear();
				points.Add(new Point(0,0));
				points.Add(new Point(xPoint, yTop));
				points.Add(new Point(xPoint, 0));
				points.Add(new Point(0,0));
				gc.Foreground = new Gdk.Color(0,128,0);
				gc.Background = new Gdk.Color(0,128,0);
				draw.DrawPolygon(gc, true, points.ToArray());


			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}

			gc.Dispose();
		}
	}
}

