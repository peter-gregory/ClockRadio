using System;
using Gtk;
using System.IO;
using Gdk;
using System.Collections.Generic;

namespace beagleradio {

	[System.ComponentModel.ToolboxItem(true)]
	public partial class WeatherView : Gtk.Bin {

		public WeatherService Weather { get; private set; }
		public int SelectedForecast { get; set; }

		public WeatherView() {
			this.Build();

			imageMorning1.RedrawOnAllocate = true;
			imageDay1.RedrawOnAllocate = true;
			imageMorning2.RedrawOnAllocate = true;
			imageDay2.RedrawOnAllocate = true;
			imageMorning3.RedrawOnAllocate = true;
			imageDay3.RedrawOnAllocate = true;
			imageMorning4.RedrawOnAllocate = true;
			imageDay4.RedrawOnAllocate = true;

			Weather = new WeatherService();
			Weather.Completed += Weather_Completed;
		}

		public void ChangeColors(Color fg, Color bk) {
			ModifyBg(StateType.Normal, bk);
			ModifyFg(StateType.Normal, fg);
			labelDay1.ModifyFg(StateType.Normal, fg);
			labelDay2.ModifyFg(StateType.Normal, fg);
			labelDay3.ModifyFg(StateType.Normal, fg);
			labelDay4.ModifyFg(StateType.Normal, fg);
		}

		public void UpdateWeather() {
			if (!Weather.IsBusy) {
				Weather.GetWeatherAsync();
			}
		}

		void Weather_Completed (object sender, string e) {
			Gtk.Application.Invoke(delegate {
				try {
					Console.WriteLine("Updating " + Weather.Forecasts.Count + " forecasts");
					for (int index = 0; index < Weather.Forecasts.Count; index++) {
						WeatherService.WeatherPeriod info = Weather.Forecasts[index];
						Console.WriteLine("Index " + index + " image1 = " + (info.Image == null ? "null" : "Loaded"));
						switch (index) {
							case 0:
								labelDay1.Text = info.Title;
								imageMorning1.Pixbuf = info.Image;
								imageMorning1.QueueDraw();
								eventMorning1.ResizeChildren();
								break;
							case 1:
								imageDay1.Pixbuf = info.Image;
								imageDay1.QueueDraw();
								eventDay1.ResizeChildren();
								break;
							case 2:
								labelDay2.Text = info.Title;
								imageMorning2.Pixbuf = info.Image;
								imageMorning2.QueueDraw();
								eventMorning2.ResizeChildren();
								break;
							case 3:
								imageDay2.Pixbuf = info.Image;
								imageDay2.QueueDraw();
								eventDay2.ResizeChildren();
								break;
							case 4:
								labelDay3.Text = info.Title;
								imageMorning3.Pixbuf = info.Image;
								imageMorning3.QueueDraw();
								eventMorning3.ResizeChildren();
								break;
							case 5:
								imageDay3.Pixbuf = info.Image;
								imageDay3.QueueDraw();
								eventDay3.ResizeChildren();
								break;
							case 6:
								labelDay4.Text = info.Title;
								imageMorning4.Pixbuf = info.Image;
								imageMorning4.QueueDraw();
								eventMorning4.ResizeChildren();
								break;
							case 7:
								imageDay4.Pixbuf = info.Image;
								imageDay4.QueueDraw();
								eventDay4.ResizeChildren();
								break;
						}
					}
					tableWeather.ResizeChildren();
					table13.ResizeChildren();

				} catch (Exception ex) {
					Console.WriteLine(ex.Source);
					Console.WriteLine(ex.StackTrace);
				}
			});
		}

		protected void OnDrawWeatherExposeEvent (object o, ExposeEventArgs args) {
			Gdk.Color back = Style.Background(StateType.Normal);
			Gdk.Color fore = Style.Foreground(StateType.Normal);

			Drawable draw = drawWeather.GdkWindow;
			Gdk.GC gc = new Gdk.GC(draw);

			try {
				gc.Foreground = back;
				gc.Background = back;

				int width;
				int height;
				draw.GetSize(out width, out height);
				draw.DrawRectangle(gc, true, 0, 0, width, height);

				gc.Foreground = fore;

				String text = "";
				if (Weather.Forecasts.Count > SelectedForecast) {
					WeatherService.WeatherPeriod info = Weather.Forecasts[SelectedForecast];
					text = info.Title + " - " + info.Forecast + "\n";
				}

				Pango.FontDescription font = Pango.FontDescription.FromString("Serif 14");
				Pango.Layout layout = drawWeather.CreatePangoLayout(text);
				layout.FontDescription = font;
				layout.Width = Pango.Units.FromPixels(width);
				draw.DrawLayoutWithColors(gc, 0, 0, layout, fore, back);
				layout.Dispose();

			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}

			gc.Dispose();
		}

		protected void OnEventMorning1ButtonPressEvent (object o, ButtonPressEventArgs args) {
			SelectedForecast = 0;
			tableWeather.QueueDraw();
			drawWeather.QueueDraw();
		}

		protected void OnEventDay1ButtonPressEvent (object o, ButtonPressEventArgs args) {
			SelectedForecast = 1;
			tableWeather.QueueDraw();
			drawWeather.QueueDraw();
		}

		protected void OnEventMorning2ButtonPressEvent (object o, ButtonPressEventArgs args) {
			SelectedForecast = 2;
			tableWeather.QueueDraw();
			drawWeather.QueueDraw();
		}

		protected void OnEventDay2ButtonPressEvent (object o, ButtonPressEventArgs args) {
			SelectedForecast = 3;
			tableWeather.QueueDraw();
			drawWeather.QueueDraw();
		}

		protected void OnEventMorning3ButtonPressEvent (object o, ButtonPressEventArgs args) {
			SelectedForecast = 4;
			tableWeather.QueueDraw();
			drawWeather.QueueDraw();
		}

		protected void OnEventDay3ButtonPressEvent (object o, ButtonPressEventArgs args) {
			SelectedForecast = 5;
			tableWeather.QueueDraw();
			drawWeather.QueueDraw();
		}

		protected void OnEventMorning4ButtonPressEvent (object o, ButtonPressEventArgs args) {
			SelectedForecast = 6;
			tableWeather.QueueDraw();
			drawWeather.QueueDraw();
		}

		protected void OnEventDay4ButtonPressEvent (object o, ButtonPressEventArgs args) {
			SelectedForecast = 7;
			tableWeather.QueueDraw();
			drawWeather.QueueDraw();
		}

		protected void OnTableWeatherExposeEvent (object o, ExposeEventArgs args) {
			Color fore = Style.Foreground(StateType.Normal);

			Drawable draw = args.Event.Window;
			Gdk.GC gc = new Gdk.GC(draw);

			Rectangle rect;
			switch (SelectedForecast) {
				case 0:
					rect = imageMorning1.Allocation;
					break;
				case 1:
					rect = imageDay1.Allocation;
					break;
				case 2:
					rect = imageMorning2.Allocation;
					break;
				case 3:
					rect = imageDay2.Allocation;
					break;
				case 4:
					rect = imageMorning3.Allocation;
					break;
				case 5:
					rect = imageDay3.Allocation;
					break;
				case 6:
					rect = imageMorning4.Allocation;
					break;
				default:
					rect = imageDay4.Allocation;
					break;
			}

			gc.Foreground = fore;
			gc.Background = fore;
			gc.SetLineAttributes(4, LineStyle.Solid, CapStyle.Round, JoinStyle.Round);
			draw.DrawRectangle(gc, false, rect);

			gc.Dispose();
		}
	}
}

