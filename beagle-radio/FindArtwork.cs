using System;
using Gdk;
using System.Net;

namespace beagleradio {

	public class FindArtwork {

		private WebClient client;
		private String artist;
		private String song;

		public Pixbuf Artwork { get; set; }
		public String Status { get; set; }

		public FindArtwork() {
			Status = "";
			client = new WebClient();
			client.DownloadStringCompleted += Client_DownloadStringCompleted;
			client.DownloadDataCompleted += Client_DownloadDataCompleted;
		}

		private void ReleaseImage() {
			if (Artwork != null) {
				Artwork.Dispose();
				Artwork = null;
			}
		}

		void Client_DownloadDataCompleted (object sender, DownloadDataCompletedEventArgs e)
		{
			try {
				byte[] data = e.Result;
				Pixbuf temp = new Pixbuf(data);
				if (temp.Width == 100 && temp.Height == 100) {
					ReleaseImage();
					this.Artwork = temp.ScaleSimple(150,150,InterpType.Bilinear);
				}
				temp.Dispose();
			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
				ReleaseImage();
			}
		}

		private int findArtistSong(String data) {
			int index = data.IndexOf("\"artistname\":");
			while (index >= 0) {
				index += 13;
				if (Framework.Extract(data, index, "\"", "\"") == artist) {
					int trackIndex = data.IndexOf("\"trackname\":") + 12;
					if (Framework.Extract(data, trackIndex, "\"", "\"") == song) {
						break;
					}
				}
				index = data.IndexOf("\"artistname\":");
			}
			if (index < 0) {
				index = data.IndexOf("\"artistname\":");
				while (index >= 0) {
					index += 13;
					if (Framework.Extract(data, index, "\"", "\"").Contains(artist)) {
						int trackIndex = data.IndexOf("\"trackname\":") + 12;
						if (Framework.Extract(data, trackIndex, "\"", "\"") == song) {
							break;
						}
					}
					index = data.IndexOf("\"artistname\":");
				}
			}
			if (index < 0) {
				index = data.IndexOf("\"artistname\":");
				while (index >= 0) {
					index += 13;
					if (Framework.Extract(data, index, "\"", "\"").Contains(artist)) {
						int trackIndex = data.IndexOf("\"trackname\":") + 12;
						if (Framework.Extract(data, trackIndex, "\"", "\"").Contains(song)) {
							break;
						}
					}
					index = data.IndexOf("\"artistname\":");
				}
			}
			if (index < 0) {
				index = data.IndexOf("\"artistname\":");
				while (index >= 0) {
					index += 13;
					if (Framework.Extract(data, index, "\"", "\"") == artist) {
						break;
					}
					index = data.IndexOf("\"artistname\":");
				}
			}
			if (index < 0) {
				index = 0;
			}
			return index;
		}

		void Client_DownloadStringCompleted (object sender, DownloadStringCompletedEventArgs e)
		{
			try {
				String reply = e.Result;
				int index = findArtistSong(reply.ToLower());
				index = reply.IndexOf("\"artworkUrl100\"", index);
				if (index > 0) {
					index = reply.IndexOf(":", index);
					String url = Framework.Extract(reply, index, "\"", "\"");
					Uri uri = new Uri(url);
					client.DownloadDataAsync(uri);
				} else {
					ReleaseImage();
				}
			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
				ReleaseImage();
			}
		}

		public void Find(String artist, String song) {
			try {
				this.artist = artist.ToLower().Trim();
				this.song = song.ToLower().Trim();
				String term = artist.Trim() + " " + song.Trim();
				term = term.Replace(" ","+");
				Uri uri = new Uri("https://itunes.apple.com/search?term=" + term + "&country=US&media=music");
				client.DownloadStringAsync(uri);
			} catch (Exception ex) {
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
				ReleaseImage();
			}
		}
	}
}

