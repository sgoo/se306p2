using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		int ctr = 0;
		public bool label1 = false;
		public bool label2 = false;
		public bool label3 = false;
		public bool label4 = false;
		public bool label5 = false;
		public bool label6 = false;
		public BaseInfo() {
			InitializeComponent();

			// load video
			Util.LoadVideo("NandaanParindey.mp4", _mediaElement);

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

		private void PlaySlideShow(int ctr) {
			BitmapImage image = new BitmapImage();
			image.BeginInit();
			string filename = "/Resources/Ece" + ctr + ".jpg";
			image.UriSource = new Uri(filename, UriKind.Relative);
			image.EndInit();
			myImage.Source = image;
			myImage.Stretch = Stretch.Uniform;
			label1 = false;
			label2 = false;
			label3 = false;
			label4 = false;
			label5 = false;
			label6 = false;

		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			ctr = 4;
			PlaySlideShow(ctr);
		}

		private void Label_MouseMove(object sender, MouseEventArgs e) {
			label1 = true;
			label4 = true;
			checkFlickLeft();

		}

		private void Label2_MouseMove(object sender, MouseEventArgs e) {
			label2 = true;
			label5 = true;
		}

		private void Label3_MouseMove(object sender, MouseEventArgs e) {
			label3 = true;
			label6 = true;
			checkFlickRight();
		}

		private void checkFlickRight() {

			if (label1 && label2 && label3) {
				if (ctr >= 5 || ctr < 0) {
					ctr = 0;
				} else {
					ctr++;
				}

				PlaySlideShow(ctr);

			}
			label1 = false;
			label2 = false;
			label3 = false;


		}

		private void checkFlickLeft() {

			if (label4 && label5 && label6) {
				if (ctr > 5 || ctr <= 0) {
					ctr = 5;
				} else {
					ctr--;
				}

				PlaySlideShow(ctr);

			}

			label4 = false;
			label5 = false;
			label6 = false;

		}
	}

}
