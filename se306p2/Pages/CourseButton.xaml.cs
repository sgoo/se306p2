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
using System.ComponentModel;

namespace se306p2.Pages {
	/// <summary>
	/// Interaction logic for CourseButton.xaml
	/// </summary>
	public partial class CourseButton : UserControl, INotifyPropertyChanged {
		public CourseButton() {
			InitializeComponent();
		}
		public Course.CourseItem CourseItem { get; set; }

		private string title = "";
		public string CourseTitle {
			get { return title; }
			set {
				title = value;
				PropertyChanged(this, new PropertyChangedEventArgs("CourseTitle"));
			}
		}

		private string code = "";
		public string CourseCode {
			get { return code; }
			set {
				code = value;
				PropertyChanged(this, new PropertyChangedEventArgs("CourseCode"));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
