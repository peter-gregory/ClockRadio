
// This file has been generated by the GUI designer. Do not modify.
namespace beagleradio
{
	public partial class TimedImage
	{
		private global::Gtk.Image imageTimed;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget beagleradio.TimedImage
			global::Stetic.BinContainer.Attach (this);
			this.Name = "beagleradio.TimedImage";
			// Container child beagleradio.TimedImage.Gtk.Container+ContainerChild
			this.imageTimed = new global::Gtk.Image ();
			this.imageTimed.Name = "imageTimed";
			this.Add (this.imageTimed);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
		}
	}
}
