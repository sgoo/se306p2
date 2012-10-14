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

    public partial class SEInfo : UserControl
    {
        ObservableCollection<DataThumbnail> items = new ObservableCollection<DataThumbnail>();
        public SEInfo()
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
            

            items.Add(new DataThumbnail(@"..\Resources\Information\SE10.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\SE12.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\SE14.gif"));
            items.Add(new DataThumbnail(@"..\Resources\Information\SE15.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\SE16.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\SE17.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\SE2.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\SE3.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\SE4.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\SE5.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\SE7.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\SE8.jpg"));


            MainSurfaceListBox.ItemsSource = items;
            // Get the default view and establish grouping.
        }
        #endregion

        private void MainSurfaceListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainSurfaceListBox.SelectedItem != null)
            {
                string filepath = items.ElementAt(MainSurfaceListBox.SelectedIndex).fileName;
                BitmapImage bmp = new BitmapImage(new Uri(filepath, UriKind.Relative));
                SEImage.Source = bmp;


            }

        }
       
    }
}

