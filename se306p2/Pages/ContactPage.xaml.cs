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
using Microsoft.Maps.MapControl.WPF;

namespace se306p2.Pages {
	/// <summary>
	/// Interaction logic for ContactPage.xaml
	/// </summary>
	public partial class ContactPage : UserControl, ResettableControl {
		private Location previousFrameCenter;

		public ContactPage() {
			InitializeComponent();
			bingMap.ViewChangeOnFrame += map_ViewChangeOnFrame;
		}
		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
			if (IsVisible) {
				MainWindow.window.BackgroundImage = "/Resources/themes/general.jpg";
			}
		}

		private void map_ViewChangeOnFrame(object sender, MapEventArgs e) {
			if (bingMap.ZoomLevel < 12 && previousFrameCenter != null) {
				bingMap.SetView(previousFrameCenter, 12);

				e.Handled = true; // Do we need it???
			} else {
				previousFrameCenter = bingMap.Center;
			}

		}

		public void reset() {
			bingMap.Center = new Location(-36.8527, 174.7684);
			bingMap.ZoomLevel = 17;
			bingMap.Heading = 0;
		}

		public void recenterMap(object sender, InputEventArgs e) {
			reset();
		}

		public void repointMap(object sender, InputEventArgs e) {
			bingMap.Heading = 0;
		}


	}
}
