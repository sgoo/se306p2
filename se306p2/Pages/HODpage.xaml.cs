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
using se306p2.Pages;
using System.Collections.ObjectModel;

namespace se306p2 {
	/// <summary>
	/// Interaction logic for HODpage.xaml
	/// </summary>
	public partial class HODpage : UserControl {
		public HODpage() {
			InitializeComponent();
		}
        ObservableCollection<DataThumbnail> items = new ObservableCollection<DataThumbnail>();

        #region OnInitialized
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            DataContext = this;

            // Create an ObservableCollection, and add the items.
            // LibraryBar.ItemsSource should implement INotifyCollectionChanged
            // in order for the built-in drag-and-drop capability to work properly.
            

            items.Add(new DataThumbnail(@"..\Resources\Zoran\ZORAN0.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Zoran\ZORAN2.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Zoran\ZORAN3.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Zoran\ZORAN4.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Zoran\ZORAN5.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Zoran\ZORAN6.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Zoran\ZORAN7.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Zoran\ZORAN8.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Zoran\ZORAN9.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Zoran\ZORAN10.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Zoran\ZORAN11.png"));

            MainSurfaceListBox.ItemsSource = items;
            // Get the default view and establish grouping.
        }
        #endregion

		private void tbMultiLine_TextChanged(object sender, TextChangedEventArgs e) {

		}
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                MainWindow.window.BackgroundImage = "/Resources/themes/general.jpg";
            }
        }

        private void MainSurfaceListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainSurfaceListBox.SelectedItem != null)
            {
                string filepath = items.ElementAt(MainSurfaceListBox.SelectedIndex).fileName;
                BitmapImage bmp = new BitmapImage(new Uri(filepath, UriKind.Relative));
                zoran.Source = bmp;


            }
            
        }

    

        
	}

}
