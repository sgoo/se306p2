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

namespace se306p2 {
	/// <summary>
	/// Interaction logic for HODpage.xaml
	/// </summary>
	public partial class HODpage : UserControl {
		public HODpage() {
			InitializeComponent();
		}

		private void tbMultiLine_TextChanged(object sender, TextChangedEventArgs e) {

		}
		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
			if (IsVisible) {
				MainWindow.window.BackgroundImage = "/Resources/themes/general.jpg";
			}
		}
	}
}
