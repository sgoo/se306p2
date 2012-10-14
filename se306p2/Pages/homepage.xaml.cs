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

namespace se306p2.Pages {
	/// <summary>
	/// Interaction logic for HomePage.xaml
	/// </summary>
	public partial class HomePage : UserControl {
		public HomePage() {
			InitializeComponent();
		}

		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
			if (IsVisible) {
				MainWindow.window.BackgroundImage = "/Resources/themes/general.jpg";
			}
		}

		private void Image_PreviewMouseDown(object sender, InputEventArgs e) {
			MainWindow.window.SelectPage(MainWindow.window.LeftItems[1],sender);
		}
	}
}