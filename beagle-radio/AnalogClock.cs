using System;
using System.Threading;
using Gtk;
using Gdk;

namespace beagleradio {

	[System.ComponentModel.ToolboxItem(true)]
	public partial class AnalogClock : Gtk.Bin {

		private Timer timer;
		private bool autoUpdate;
		private Pixbuf clockFace;

		public event EventHandler<DateTime> TimeChanged;

		public DateTime CurrentTime { get; set; }
		public bool AnalogMovement { get; set; }
		public int Spacing { get; set; }

		public AnalogClock() {
			this.Build();
			AnalogMovement = true;
			autoUpdate = false;
		}

		public bool AutoUpdate {
			get {
				return autoUpdate;
			}
			set {
				if (autoUpdate != value) {
					if (value) {
						timer = new Timer(new TimerCallback(delegate(object state) {
							DateTime test = DateTime.Now;
							if (AnalogMovement) {
								test = new DateTime(test.Year, test.Month, test.Day, test.Hour, test.Minute, test.Second, (test.Millisecond / 200) * 200);
							} else {
								test = new DateTime(test.Year, test.Month, test.Day, test.Hour, test.Minute, test.Second, 0);
							}
							if (test != CurrentTime) {
								CurrentTime = test;
								Gtk.Application.Invoke (delegate {
									if (TimeChanged != null) {
										TimeChanged(this, test);
									}
									QueueDraw();
								});
							}
						}));
						timer.Change(100, 100);
					} else {
						timer.Dispose();
						timer = null;
					}
				}
				autoUpdate = value;
			}
		}

		override public void Dispose() {
			if (timer != null) {
				timer.Dispose();
				timer = null;
			}
			base.Dispose();
		}

		protected void OnDrawClockExposeEvent (object o, Gtk.ExposeEventArgs args) {

			Gdk.Color back = Style.Background(StateType.Normal);
			Gdk.Color fore = Style.Foreground(StateType.Normal);

			Drawable draw = args.Event.Window;

			int width;
			int height;
			draw.GetSize(out width, out height);

			Gdk.GC gc = new Gdk.GC(draw);
			gc.Foreground = back;
			gc.Background = back;

			Point start;
			Point stop;
			double xMax;
			double yMax;
			double angle = 0;

			int size = width < height ? width : height;
			size -= Spacing;
			Point center = new Point(width / 2, height / 2);
			Rectangle rect = new Rectangle((width - size) / 2, (height - size) / 2, size, size);

			if (clockFace != null && (clockFace.Width != width || clockFace.Height != height)) {
				clockFace.Dispose();
				clockFace = null;
			}

			if (clockFace == null) {
				
				draw.DrawPixbuf(gc, clockFace, 0, 0, 0, 0, width, height, RgbDither.None, 0, 0);

				draw.DrawRectangle(gc, true, 0, 0, width, height);

				gc.Foreground = fore;

				gc.SetLineAttributes(4, LineStyle.Solid, CapStyle.Round, JoinStyle.Round);
				draw.DrawArc(gc, false, rect.Left, rect.Top, size, size, 0, 360 * 64);
				gc.SetLineAttributes(1, LineStyle.Solid, CapStyle.Round, JoinStyle.Round);

				for (int index = 0; index < 60; index++, angle += Math.PI / 30.0) {
					xMax = Math.Cos(angle) * size / 2;
					yMax = Math.Sin(angle) * size / 2;
					start = center;
					stop = center;
					if ((index % 5) == 0) {
						int hour = ((index / 5) + 3) % 12;
						if (hour == 0) hour = 12;

						start.Offset((int)(xMax / 1.30), (int)(yMax / 1.30));

						Pango.FontDescription font = Pango.FontDescription.FromString("Serif 10");
						Pango.Layout layout = CreatePangoLayout(hour.ToString());
						layout.FontDescription = font;
						layout.Width = Pango.Units.FromPixels(100);

						int textWidth;
						int textHeight;
						layout.GetPixelSize(out textWidth, out textHeight);

						start.Offset(-textWidth / 2, -textHeight / 2);

						draw.DrawLayoutWithColors(gc, start.X, start.Y, layout, fore, back);
						layout.Dispose();

						start = center;
						gc.SetLineAttributes(4, LineStyle.Solid, CapStyle.Round, JoinStyle.Round);
						start.Offset((int)(xMax / 1.05), (int)(yMax / 1.05));
						stop.Offset((int)xMax, (int)yMax);
						draw.DrawLine(gc, start.X, start.Y, stop.X, stop.Y);
						gc.SetLineAttributes(1, LineStyle.Solid, CapStyle.Round, JoinStyle.Round);
					} else {
						start.Offset((int)(xMax / 1.05), (int)(yMax / 1.05));
						stop.Offset((int)xMax, (int)yMax);
						draw.DrawLine(gc, start.X, start.Y, stop.X, stop.Y);
					}
				}
				clockFace = Pixbuf.FromDrawable(draw, draw.Colormap, 0, 0, 0, 0, width, height);

			} else {
				
				draw.DrawPixbuf(gc, clockFace, 0, 0, 0, 0, width, height, RgbDither.None, 0, 0);

			}

			gc.Foreground = fore;

			double startAngle = 1.5 * Math.PI;
			double secondAngle;
			double minuteAngle;
			double hourAngle;

			if (AnalogMovement) {

				double value = (CurrentTime.Second * 1000) + CurrentTime.Millisecond;
				double divisor = 60000.0;
				secondAngle = 2.0 * Math.PI * value / divisor + startAngle;

				value += (double) CurrentTime.Minute * divisor;
				divisor *= 60.0;
				minuteAngle = 2.0 * Math.PI * value / divisor + startAngle;

				value += (double) ((CurrentTime.Hour) % 12) * divisor;
				divisor *= 12.0;
				hourAngle = 2.0 * Math.PI * value / divisor + startAngle;

			} else {

				double value = CurrentTime.Second;
				double divisor = 60;
				secondAngle = 2.0 * Math.PI * value / divisor + startAngle;

				value = CurrentTime.Minute;
				divisor = 60;
				minuteAngle = 2.0 * Math.PI * value / divisor + startAngle;

				value = ((CurrentTime.Hour) % 12);
				divisor = 12;
				hourAngle = 2.0 * Math.PI * value / divisor + startAngle;

			}

			start = center;
			stop = center;
			xMax = Math.Cos(secondAngle) * size / 2;
			yMax = Math.Sin(secondAngle) * size / 2;
			stop.Offset((int) (xMax / 1.2), (int) (yMax / 1.2));
			draw.DrawLine(gc, start.X, start.Y , stop.X, stop.Y);

			stop = center;
			xMax = Math.Cos(minuteAngle) * size / 2;
			yMax = Math.Sin(minuteAngle) * size / 2;
			stop.Offset((int) (xMax / 1.4), (int) (yMax / 1.4));
			gc.SetLineAttributes(4, LineStyle.Solid, CapStyle.Round, JoinStyle.Round);
			draw.DrawLine(gc, start.X, start.Y , stop.X, stop.Y);

			stop = center;
			xMax = Math.Cos(hourAngle) * size / 2;
			yMax = Math.Sin(hourAngle) * size / 2;
			stop.Offset((int) (xMax / 2.2), (int) (yMax / 2.2));
			gc.SetLineAttributes(8, LineStyle.Solid, CapStyle.Round, JoinStyle.Round);
			draw.DrawLine(gc, start.X, start.Y , stop.X, stop.Y);

			gc.Dispose();
		}
	}
}

