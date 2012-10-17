using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.IO;

namespace se306p2 {
	class Util {
		public static void LoadVideo(String videoName, MediaElement media) {
			DateTime start = DateTime.Now;
			String[] drives = Environment.GetLogicalDrives();
			foreach (String drive in drives) {
				if (File.Exists(drive + videoName)) {
					media.Source = new Uri(drive + videoName);
					break;
				}
			}
			Console.WriteLine("Took {0} to load {1}", (DateTime.Now - start).TotalMilliseconds, videoName);
		}
	}
}
