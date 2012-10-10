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
using Microsoft.Surface.Presentation;

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


        ObservableCollection<Uri> items;
        Uri[] files;

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Query the registry to find out where the sample photos are stored.
            const string shellKey =
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\explorer\Shell Folders";

            string imagesPath =
                (string)Microsoft.Win32.Registry.GetValue(shellKey, "CommonPictures", null) + @"\Sample Pictures";

            //imagesPath = "D:/se306p2/se306p2/Resources/Advisors";
            imagesPath = "Resources\\Advisors";
            // MessageBox.Show(imagesPath);
            try
            {
                // Get the list of files.

                files = new Uri[4];
                files[0] = new Uri("pack://application:,,,/Resources/Advisors/TopCard.bmp");
                
                files[1] = new Uri("pack://application:,,,/Resources/Advisors/Roop.bmp"); //CSE
                files[2] = new Uri("pack://application:,,,/Resources/Advisors/card.png");
                //files[2] = new Uri("pack://application:,,,/Resources/Advisors/Berber.bmp"); //EEE                
                files[3] = new Uri("pack://application:,,,/Resources/Advisors/Watson.bmp"); //SE
                


                //Uri[] files = System.IO.Directory.GetFiles(imagesPath, "*.jpg");


                // Create an ObservableCollection from the file names.
                // Cannot assign string[] files to LibraryStack.ItemsSource.
                // LibraryStack.ItemsSource must implement INotifyCollectionChanged.
                items = new ObservableCollection<Uri>(files);
                // Set the ItemsSource property.
                MainLibraryStack.ItemsSource = items;
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                // Handle exception as needed.
            }
        }

        private void MainLibraryStack_DragOver(object sender, SurfaceDragDropEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        private void MainLibraryStack_DragCompleted(object sender, SurfaceDragCompletedEventArgs e)
        {
            Uri data = e.Cursor.Data as Uri;
            if (!items.Contains<Uri>(data))
            {
                items.Add(data);
            }
        }

        //resets the Library Stack when you leave
        private void MainLibraryStack_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            items = new ObservableCollection<Uri>(files);
            MainLibraryStack.ItemsSource = items;
        }

        private void Button_PreviewCSEDown(object sender, InputEventArgs e)
        {
            items.IndexOf(files[1]);
            MainLibraryStack.SelectedIndex = items.IndexOf(files[1]);
            //items.Remove(files[1]);
            //items.Insert(0, files[1]);
        }

        private void Button_PreviewEEEDown(object sender, InputEventArgs e)
        {
            items.IndexOf(files[2]);
            MainLibraryStack.SelectedIndex = items.IndexOf(files[2]);
            //items.Remove(files[1]);
            //items.Insert(0, files[1]);
        }

        

        private void Button_PreviewSEDown(object sender, InputEventArgs e)
        {
            items.IndexOf(files[3]);
            MainLibraryStack.SelectedIndex = items.IndexOf(files[3]);
            //items.Remove(files[1]);
            //items.Insert(0, files[1]);
        }

    }
}
