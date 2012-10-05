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
using System.Collections.ObjectModel;

namespace se306p2
{
    /// <summary>
    /// Interaction logic for Advisors.xaml
    /// </summary>
    public partial class Advisors : UserControl
    {
        public Advisors()
        {
            InitializeComponent();
            


            items.Add("Resources/Advisors/sample.jpg");
            MainLibraryStack.ItemsSource = items;
        }

        ObservableCollection<String> items = new ObservableCollection<String>();


        private void libraryStack1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
