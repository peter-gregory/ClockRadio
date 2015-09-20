using System;
using Gtk;
using Gdk;

namespace beagleradio {

	public class GestureDetector {

		private bool swiping = false;
		private Point swipeStart = new Point(0,0);
		private Point swipeTest = new Point(0,0);

		private const int maxError = 48;
		private const int minSwipeLength = 48;

		public enum SwipeDirection {
			Left,
			Right,
			Up,
			Down
		}

		public event EventHandler<SwipeDirection> SwipeDetected;


		public GestureDetector() {
		}

		public void ProcessPressEvent(ButtonPressEventArgs args) {
			if (args.Event.Button == 1) {
				swiping = true;
				swipeStart = new Point((int) args.Event.X, (int) args.Event.Y);
				swipeTest = swipeStart;
			}
		}

		public void ProcessMotionEvent(MotionNotifyEventArgs args) {
			if (swiping) {
				swipeTest = new Point((int) args.Event.X, (int) args.Event.Y);
				// Swipe must be horizontal or vertical
				if ((swipeTest.X - swipeStart.X > maxError || swipeTest.X - swipeStart.X < -maxError) && (swipeTest.Y - swipeStart.Y > maxError || swipeTest.Y - swipeStart.Y < -maxError)) {
					swiping = false;
				}
			}
		}

		public void ProcessReleaseEvent(ButtonReleaseEventArgs args) {
			if (swiping) {
				swiping = false;
				swipeTest = new Point((int) args.Event.X, (int) args.Event.Y);
				if (((swipeTest.X - swipeStart.X) < maxError) && ((swipeTest.X - swipeStart.X) > -maxError)) {
					if (swipeTest.Y - swipeStart.Y > minSwipeLength) { // swipe down
						if (SwipeDetected != null) {
							SwipeDetected(this, SwipeDirection.Down);
						}
					} else if (swipeTest.Y - swipeStart.Y < -minSwipeLength) { // swipe up
						if (SwipeDetected != null) {
							SwipeDetected(this, SwipeDirection.Up);
						}
					}
				} else if (((swipeTest.Y - swipeStart.Y) < maxError) && ((swipeTest.Y - swipeStart.Y) > -maxError)) {
					if (swipeTest.X - swipeStart.X > minSwipeLength) { // swipe right
						if (SwipeDetected != null) {
							SwipeDetected(this, SwipeDirection.Right);
						}
					} else if (swipeTest.X - swipeStart.X < -minSwipeLength) { // swipe left
						if (SwipeDetected != null) {
							SwipeDetected(this, SwipeDirection.Left);
						}
					}
				}
			}
		}
	}
}

