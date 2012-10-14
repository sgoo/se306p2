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

    public partial class CSEInfo : UserControl
    {
        ObservableCollection<DataThumbnail> items = new ObservableCollection<DataThumbnail>();

        public CSEInfo()
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
            

            items.Add(new DataThumbnail(@"..\Resources\Information\CSE0.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\CSE1.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\CSE10.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\CSE11.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\CSE12.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\CSE13.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\CSE2.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\CSE3.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\CSE5.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\CSE6.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\CSE7.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\CSE8.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\Information\CSE9.gif"));


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
                CSEImage.Source = bmp;


            }

        }
        
    }
}

