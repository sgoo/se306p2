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

namespace se306p2 {
	/// <summary>
	/// Interaction logic for InfoOnEEE.xaml
	/// </summary>
	/// 

	public partial class BaseInfo : UserControl {

		public BaseInfo() {
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

            items.Add(new DataThumbnail(@"..\Resources\blue0.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\blue1.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\blue2.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\blue4.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\green0.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\green1.jpg"));
            items.Add(new DataThumbnail(@"..\Resources\green2.jpg"));
            MainLibraryBar.ItemsSource = items;
            // Get the default view and establish grouping.
            ICollectionView defaultView = CollectionViewSource.GetDefaultView(items);
            defaultView.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
        }
        #endregion

        private void _mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            _mediaElement.Stop();
        }
        private void _mediaElement_PreviewTouchDown(object sender, InputEventArgs e)
        {
            if (_mediaElement.Tag.ToString() == "pause")
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
	}

    #region DataThumbnailClass
    /// <summary>
    /// Represents an image thumbnail with an associated label and a group name.
    /// </summary>
    public class DataThumbnail
    {
       
        private string fileName;
        private BitmapImage bitmap;

        /// <summary>
        /// Creates a new instance of the DataThumbnail class.
        /// </summary>
        /// <param name="fileName">The file name of the image.</param>
        /// <param name="label">The text associated with the image.</param>
        /// <param name="groupName">The group name for the image.</param>
        public DataThumbnail(string fileName)
        {
            // For simplicity, not checking for null parameters.
            this.fileName = fileName;
            this.bitmap = new BitmapImage(new Uri(fileName, UriKind.Relative));
        }

        /// <summary>
        /// Gets the label of this instance.
        /// </summary>
       

        /// <summary>
        /// Gets the group name of this instance.
        /// </summary>
       

        /// <summary>
        /// Gets the image used by this instance.
        /// </summary>
        public BitmapSource Bitmap
        {
            get { return bitmap; }
        }


    }
    #endregion

}
