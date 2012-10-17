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
using System.ComponentModel;
using System.Collections.ObjectModel;
using se306p2.Pages;

namespace se306p2
{
    /// <summary>
    /// Interaction logic for InfoOnEEE.xaml
    /// </summary>
    /// 

    public partial class BaseInfo : UserControl
    {
        ObservableCollection<DataThumbnail> items = new ObservableCollection<DataThumbnail>();

        public BaseInfo()
        {

            InitializeComponent();

        }

        #region OnInitialized
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            DataContext = this;
          

            // Create an ObservableCollection, and add the items.
            // LibraryBar.ItemsSource should implement INotifyCollectionChanged
            // in order for the built-in drag-and-drop capability to work properly.
            

            items.Add(new DataThumbnail(@"..\Resources\Information\ECE0.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\ECE1.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\ECE2.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\ECE3.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\ECE4.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\ECE5.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\ECE6.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\ECE7.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\ECE8.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\ECE9.jpg"));
                  
            MainSurfaceListBox.ItemsSource = items;
            // Get the default view and establish grouping.
        }
        #endregion

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void MainSurfaceListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainSurfaceListBox.SelectedItem != null)
            {
                string filepath = items.ElementAt(MainSurfaceListBox.SelectedIndex).fileName;
                BitmapImage bmp = new BitmapImage(new Uri(filepath, UriKind.Relative));
                EceImage.Source = bmp;


            }

        }


    }
}
