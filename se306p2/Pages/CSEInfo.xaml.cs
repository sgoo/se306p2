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

        public CSEInfo()
        {

            InitializeComponent();

        }

        #region OnInitialized
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            DataContext = this;
            _mediaElement.Pause();

            // Create an ObservableCollection, and add the items.
            // LibraryBar.ItemsSource should implement INotifyCollectionChanged
            // in order for the built-in drag-and-drop capability to work properly.
            ObservableCollection<DataThumbnail> items = new ObservableCollection<DataThumbnail>();

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

        private void _mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            _mediaElement.Stop();
        }
        private void _mediaElement_PreviewTouchDown(object sender, InputEventArgs e)
        {
            if (_mediaElement.Tag.ToString().Equals("pause"))
            {
                _mediaElement.Tag = "play";
                _mediaElement.Play();
                playButton.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                _mediaElement.Pause();
                _mediaElement.Tag = "pause";
                playButton.Visibility = System.Windows.Visibility.Visible;
            }
        }


        private void MainLibraryBar_DragOver(object sender, Microsoft.Surface.Presentation.SurfaceDragDropEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
        }
    }
}

