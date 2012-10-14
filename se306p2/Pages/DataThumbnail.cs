using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace se306p2.Pages
{
    public class DataThumbnail
    {

        public string fileName;
        public BitmapImage bitmap;

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


        public BitmapSource Bitmap
        {
            get { return bitmap; }
        }


    }
}
