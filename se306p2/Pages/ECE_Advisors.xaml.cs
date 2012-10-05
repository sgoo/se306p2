using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace se306p2
{
    /// <summary>
    /// Interaction logic for ECE_Advisors.xaml
    /// </summary>
    public partial class ECE_Advisors : UserControl
    {
        public ECE_Advisors()
        {
            InitializeComponent();
            

        }

 

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Query the registry to find out where the sample photos are stored.
            const string shellKey =
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\explorer\Shell Folders";

            string imagesPath =
                (string)Microsoft.Win32.Registry.GetValue(shellKey, "CommonPictures", null) + @"\Sample Pictures";

            imagesPath = "D:/se306p2/se306p2/Resources/Advisors";
            MessageBox.Show(imagesPath);
            try
            {
                // Get the list of files.
                string[] files = System.IO.Directory.GetFiles(imagesPath, "*.jpg");

                // Create an ObservableCollection from the file names.
                // Cannot assign string[] files to LibraryStack.ItemsSource.
                // LibraryStack.ItemsSource must implement INotifyCollectionChanged.
                ObservableCollection<string> items = new ObservableCollection<string>(files);

                // Set the ItemsSource property.
                MainLibraryStack.ItemsSource = items;
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                // Handle exception as needed.
            }
        }

        

    }
}
