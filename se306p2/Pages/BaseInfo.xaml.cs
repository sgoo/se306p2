using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace se306p2 {
	/// <summary>
	/// Interaction logic for InfoOnEEE.xaml
	/// </summary>
	/// 

	public partial class BaseInfo : UserControl {
		DispatcherTimer timer;
		int ctr = 0;
		public BaseInfo() {
			InitializeComponent();
			
			// load video
			Util.LoadVideo("NandaanParindey.mp4",_mediaElement);

			timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0, 0, 2);
			timer.Tick += new EventHandler(timer_Tick);

		}

		private void _mediaElement_MediaEnded(object sender, RoutedEventArgs e) {
			_mediaElement.Position = TimeSpan.Zero;
			_mediaElement.Play();
		}
		private void _mediaElement_PreviewTouchDown(object sender, InputEventArgs e) {
			if (_mediaElement.Tag.ToString() == "pause") {
				_mediaElement.Tag = "play";
				_mediaElement.Play();

			} else {
				_mediaElement.Tag = "pause";
				_mediaElement.Pause();

			}
		}

		protected override void OnInitialized(EventArgs e) {
			base.OnInitialized(e);
			DataContext = this;
			_mediaElement.Play();
		}

		void timer_Tick(object sender, EventArgs e) {
			ctr++;
			if (ctr > 5) {
				ctr = 1;
			}
			PlaySlideShow(ctr);
		}
		private void Window_Loaded(object sender, RoutedEventArgs e) {
			ctr = 1;
			PlaySlideShow(ctr);
		}
		private void PlaySlideShow(int ctr) {
			BitmapImage image = new BitmapImage();
			image.BeginInit();
			string filename = "Resources/Ece" + ctr + ".jpg";
			image.UriSource = new Uri(filename, UriKind.Relative);
			image.EndInit();
			myImage.Source = image;
			myImage.Stretch = Stretch.Uniform;
			timer.IsEnabled = true;
		}


	}
}
