using System;
using System.Collections.Generic;
using System.IO;
using Gdk;
using System.Threading;

namespace beagleradio {

	[System.ComponentModel.ToolboxItem(true)]
	public partial class TimedImage : Gtk.Bin {

		private List<TimedImageItem> timedImages { get; set; }
		private TimedImageItem activeItem;
		private List<string> images;
		private DateTime lastChecked;
		private DateTime lastImageTime;
		private int imageIndex;
		private int minutesPerImage;
		private string activeImage;
		private Timer nextImageTimer;

		private bool isShutdown;
		private Thread animationThread;

		/// <summary>
		/// Initializes a new instance of the <see cref="beagleradio.TimedImage"/> class.
		/// </summary>
		public TimedImage() {
			this.Build();

			imageIndex = 0;
			minutesPerImage = 1;
			lastChecked = DateTime.MinValue;
			images = new List<string>();
			timedImages = new List<TimedImageItem>();
			Load();
		}

		/// <summary>
		/// Start this instance.
		/// </summary>
		public void Start() {
			if (nextImageTimer == null) {
				nextImageTimer = new Timer(new TimerCallback(ChangeDisplayedImage));
				nextImageTimer.Change(1000, 1000);
			}
			isShutdown = false;
			animationThread = new Thread(new ThreadStart(ChangeImageThread));
			animationThread.Start();
		}

		/// <summary>
		/// Close this instance.
		/// </summary>
		public void Close() {
			
			isShutdown = true;

			if (nextImageTimer != null) {
				nextImageTimer.Dispose();
				nextImageTimer = null;
			}
		}

		private List<TimedImageItem> FindActive(DateTime test) {
			List<TimedImageItem> results = new List<TimedImageItem>();
			List<TimedImageItem> expired = new List<TimedImageItem>();
			foreach (TimedImageItem item in timedImages) {
				if (item.IsActive(test)) {
					results.Add(item);
				} else if (item.IsExpired()) {
					expired.Add(item);
				}
			}
			foreach (TimedImageItem item in expired) {
				timedImages.Remove(item);
			}
			return results;
		}

		private void Load() {
			timedImages.Clear();
			try {
				FileStream file = new FileStream("timedimages.txt", FileMode.Open);
				StreamReader reader = new StreamReader(file);
				string line = reader.ReadLine();
				TimedImageItem item = null;
				TimeFrame onframe = null;
				TimeFrame offframe = null;
				while (line != null) {
					line = line.Trim();
					if (!line.StartsWith("#")) {
						string value;
						if (Framework.ExtractKey(out value, line, "name:")) {
							if (item != null) {
								if (onframe != null) {
									item.OnTimes.Add(onframe);
									onframe = null;
								}
								if (offframe != null) {
									item.OffTimes.Add(offframe);
									offframe = null;
								}
								timedImages.Add(item);
							}
							item = new TimedImageItem();
							item.Name = value;
						} else if (Framework.ExtractKey(out value, line, "file:")) {
							item.Filename = value;
						} else if (Framework.ExtractKey(out value, line, "on_frame:")) {
							if (onframe != null) item.OnTimes.Add(onframe);
							onframe = new TimeFrame();
						} else if (Framework.ExtractKey(out value, line, "on_start_date:")) {
							onframe.StartDate = DateTime.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "on_stop_date:")) {
							onframe.StopDate = DateTime.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "on_start_time:")) {
							onframe.StartTime = DateTime.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "on_stop_time:")) {
							onframe.StopTime = DateTime.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "on_recurrence:")) {
							onframe.Recurrence = (TimeFrame.RecurringType) Enum.Parse(typeof(TimeFrame.RecurringType), value);
						} else if (Framework.ExtractKey(out value, line, "on_recurrence_week:")) {
							onframe.RecurrenceWeek = (TimeFrame.RecurringWeek) Enum.Parse(typeof(TimeFrame.RecurringWeek), value);
						} else if (Framework.ExtractKey(out value, line, "on_weekdays:")) {
							onframe.WeekDays = value;
						} else if (Framework.ExtractKey(out value, line, "on_month:")) {
							onframe.Month = int.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "on_day:")) {
							onframe.Day = int.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "off_frame:")) {
							if (offframe != null) item.OnTimes.Add(offframe);
							offframe = new TimeFrame();
						} else if (Framework.ExtractKey(out value, line, "off_start_date:")) {
							offframe.StartDate = DateTime.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "off_stop_date:")) {
							offframe.StopDate = DateTime.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "off_start_time:")) {
							offframe.StartTime = DateTime.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "off_stop_time:")) {
							offframe.StopTime = DateTime.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "off_recurrence:")) {
							offframe.Recurrence = (TimeFrame.RecurringType) Enum.Parse(typeof(TimeFrame.RecurringType), value);
						} else if (Framework.ExtractKey(out value, line, "off_recurrence_week:")) {
							offframe.RecurrenceWeek = (TimeFrame.RecurringWeek) Enum.Parse(typeof(TimeFrame.RecurringWeek), value);
						} else if (Framework.ExtractKey(out value, line, "off_weekdays:")) {
							offframe.WeekDays = value;
						} else if (Framework.ExtractKey(out value, line, "off_month:")) {
							offframe.Month = int.Parse(value);
						} else if (Framework.ExtractKey(out value, line, "off_day:")) {
							offframe.Day = int.Parse(value);
						}
					}
					line = reader.ReadLine();
				}
				reader.Close();
				reader.Dispose();
				file.Close();
				file.Dispose();
				if (item != null) {
					if (onframe != null) {
						item.OnTimes.Add(onframe);
					}
					if (offframe != null) {
						item.OffTimes.Add(offframe);
					}
					timedImages.Add(item);
				}
			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}
		}

		private string GetImage(DateTime time) {
			time = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, 0);
			if (time != lastChecked) {
				lastChecked = time;
				List<TimedImageItem> items = FindActive(DateTime.Now);
				if (items.Count > 0) {
					if (activeItem != items[0]) {
						activeItem = items[0];
						images.Clear();
						string pathname = "pictures/" + items[0].Filename;
						Console.WriteLine("Loading images: " + pathname);
						if (File.GetAttributes(pathname).HasFlag(FileAttributes.Directory)) {
							Console.WriteLine("path is a directory: " + pathname);
							foreach (string name in Directory.GetFiles(pathname)) {
								Console.WriteLine("Add directory image: " + name);
								images.Add(name);
							}
						} else {
							Console.WriteLine("Add file image: " + pathname);
							images.Add(pathname);
						}
					}
				}
			}
			string result = "";
			if (images.Count > 0) {
				if (time.Subtract(lastImageTime).TotalMinutes > minutesPerImage) {
					lastImageTime = time;
					imageIndex++;
				}
				if (imageIndex >= images.Count) imageIndex = 0;
				result = images[imageIndex];
			}
			return result;
		}

		protected void ChangeDisplayedImage(object sender) {

			Gtk.Application.Invoke(delegate {
				try {
					if (DateTime.Now > DateTime.Parse("08/01/2015")) {
						activeImage = GetImage(DateTime.Now);
					}
				} catch (Exception ex) {
					Console.WriteLine(ex.Source);
					Console.WriteLine(ex.StackTrace);
				}
			});
		}

		protected void ChangeImageThread() {

			PixbufAnimation animation = null;
			PixbufAnimationIter animationIter = null;
			int imageWidth = 0;
			int imageHeight = 0;
			bool isResizeNeeded = false;
			string currentImage = "";

			while (!isShutdown) {

				try {

					DateTime start;
					
					if (currentImage != activeImage) {
						currentImage = activeImage;

						if (animationIter != null) {
							animationIter.Dispose();
							animationIter = null;
						}
						if (animation != null) {
							animation.Dispose();
							animation = null;
						}

						Pixbuf firstImage = null;

						if (currentImage != null && currentImage != "") {

							animation = new PixbufAnimation(currentImage);
							if (animation.IsStaticImage) {
								firstImage = animation.StaticImage.Copy();
							} else {
								animationIter = animation.GetIter(IntPtr.Zero);
								firstImage = animationIter.Pixbuf.Copy();
							}

							imageWidth = firstImage.Width;
							imageHeight = firstImage.Height;
							isResizeNeeded = false;
							if (imageWidth > imageTimed.Allocation.Width) {
								imageHeight = imageHeight * imageTimed.Allocation.Width / imageWidth;
								imageWidth = imageTimed.Allocation.Width;
								isResizeNeeded = true;
							}
							if (imageHeight > imageTimed.Allocation.Height) {
								imageWidth = imageWidth * imageTimed.Allocation.Height / imageHeight;
								imageHeight = imageTimed.Allocation.Height;
								isResizeNeeded = true;
							}

							if (isResizeNeeded) {
								Pixbuf resized = firstImage.ScaleSimple(imageWidth, imageHeight, InterpType.Bilinear);
								firstImage.Dispose();
								firstImage = resized;
							}

						}

						Gtk.Application.Invoke(delegate {
							try {
								Pixbuf oldImage = imageTimed.Pixbuf;
								imageTimed.Pixbuf = firstImage;
								imageTimed.QueueDraw();
								if (oldImage != null) {
									oldImage.Dispose();
								}
							} catch (Exception ex) {
								Console.WriteLine(ex.Source);
								Console.WriteLine(ex.StackTrace);
							}
						});

						start = DateTime.Now;
					} else {
						start = DateTime.Now;
						if (animationIter != null) {
							animationIter.Advance(IntPtr.Zero);

							Pixbuf pix = animationIter.Pixbuf.Copy();
							if (isResizeNeeded) {
								Pixbuf resized = pix.ScaleSimple(imageWidth, imageHeight, InterpType.Bilinear);
								pix.Dispose();
								pix = resized;
							}

							Gtk.Application.Invoke(delegate {
								Pixbuf oldImage = imageTimed.Pixbuf;
								imageTimed.Pixbuf = pix;
								imageTimed.QueueDraw();
								if (oldImage != null) {
									oldImage.Dispose();
								}
							});
						}
					}

					if (animationIter != null && animationIter.DelayTime != -1) {
						int delay = (int)start.AddMilliseconds(animationIter.DelayTime).Subtract(DateTime.Now).TotalMilliseconds;
						if (delay > 0) Thread.Sleep(delay);
					} else {
						Thread.Sleep(100);
					}
				} catch (Exception ex) {
					Console.WriteLine(ex.Source);
					Console.WriteLine(ex.StackTrace);
				}
			}

			if (animationIter != null) {
				animationIter.Dispose();
				animationIter = null;
			}
			if (animation != null) {
				animation.Dispose();
				animation = null;
			}

		}
	}
}

