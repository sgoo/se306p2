﻿using System;
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

namespace se306p2.Pages {
	/// <summary>
	/// Interaction logic for HomePage.xaml
	/// </summary>
	public partial class HomePage : UserControl {

		private DispatcherTimer timer;
		private int imageIndex = 1;

		public HomePage() {
			InitializeComponent();
			timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0, 0, 4);
			timer.IsEnabled = true;
			timer.Tick += new EventHandler(TimerLoop);
		}

		private void TimerLoop(object sender, EventArgs e) {
			imageIndex++;
			if (imageIndex > 3) {
				imageIndex = 1;
			}
			SlideShow(imageIndex);
		}

		private void SlideShow(int imageIndex) {

			BitmapImage image = new BitmapImage();
			image.BeginInit();
			string filename = "/Resources/HomePage/Slideshow/0" + imageIndex + ".jpg";
			image.UriSource = new Uri(filename, UriKind.Relative);
			slideImage.Source = image;
			image.EndInit();
		}

		private void LeftSlideClick(object sender, InputEventArgs e) {
			imageIndex--;
			if (imageIndex < 1) {
				imageIndex = 8;
			}
			SlideShow(imageIndex);
			timer.Interval = new TimeSpan(0, 0, 4);
		}

		private void RightSlideClick(object sender, InputEventArgs e) {
			imageIndex++;
			if (imageIndex > 8) {
				imageIndex = 1;
			}
			SlideShow(imageIndex);
			timer.Interval = new TimeSpan(0, 0, 4);
		}

		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
			if (IsVisible) {
				MainWindow.window.BackgroundImage = "/Resources/themes/general.jpg";
			}
		}

		private void Image_PreviewMouseDown(object sender, InputEventArgs e) {
			MainWindow.window.SelectPage(MainWindow.window.LeftItems[1], sender);
		}

		private void FindOutMore(object sender, InputEventArgs e) {
			MainWindow.window.SelectPage(1);
		}
	}
}