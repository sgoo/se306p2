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
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;
using System.Collections.ObjectModel;


namespace se306p2
{
    /// <summary>
	/// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : SurfaceWindow
    {

         private ObservableCollection<DataItem> leftItems;
         private ObservableCollection<DataItem> rightItems;
        private ObservableCollection<DataItem> targetItems;

        /// <summary>
        /// Items that bind with the drag source list box.
        /// </summary>
        public ObservableCollection<DataItem> LeftItems
        {
            get
            {
                if (leftItems == null)
                {
                    leftItems = new ObservableCollection<DataItem>();
                }

                return leftItems;
            }
        }

        /// <summary>
        /// Items that bind with the drag source list box.
        /// </summary>
        public ObservableCollection<DataItem> RightItems
        {
            get
            {
                if (rightItems == null)
                {
                    rightItems = new ObservableCollection<DataItem>();
                }

                return rightItems;
            }
        }




        /// <summary>
        /// Items that bind with the drop target list box.
        /// </summary>
        public ObservableCollection<DataItem> TargetItems
        {
            get
            {
                if (targetItems == null)
                {
                    targetItems = new ObservableCollection<DataItem>();
                }

                return targetItems;
            }
        }



        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            DataContext = this;
            LeftItems.Add(new DataItem("HOD's Welcome", false));
            LeftItems.Add(new DataItem("General", true));
            LeftItems.Add(new DataItem("Location", true));
            LeftItems.Add(new DataItem("blah", false));
            
            RightItems.Add(new DataItem("SE 1", true));
            RightItems.Add(new DataItem("SE 2", true));
            RightItems.Add(new DataItem("CSE info", false));
            RightItems.Add(new DataItem("CSE course", true));
            RightItems.Add(new DataItem("EEE", true));
            RightItems.Add(new DataItem("EEE 2", true));
        }


        /// <summary>
        /// Default constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

		private void DefaultPanel_Drop(object sender, SurfaceDragDropEventArgs e) {
            MessageBox.Show("hi");
		}

        private void OnDragSourcePreviewTouchDown(object sender, TouchEventArgs e)
        {
            FrameworkElement findSource = e.OriginalSource as FrameworkElement;
            SurfaceListBoxItem draggedElement = null;

            // Find the touched SurfaceListBoxItem object.
            while (draggedElement == null && findSource != null)
            {
                if ((draggedElement = findSource as SurfaceListBoxItem) == null)
                {
                    findSource = VisualTreeHelper.GetParent(findSource) as FrameworkElement;
                }
            }

            if (draggedElement == null)
            {
                return;
            }

            // Create the cursor visual.
            ContentControl cursorVisual = new ContentControl()
            {
                Content = draggedElement.DataContext,
                Style = FindResource("CursorStyle") as Style
            };

            // Add a handler. This will enable the application to change the visual cues.
            SurfaceDragDrop.AddTargetChangedHandler(cursorVisual, OnTargetChanged);

            // Create a list of input devices. Add the touches that
            // are currently captured within the dragged element and
            // the current touch (if it isn't already in the list).
            List<InputDevice> devices = new List<InputDevice>();
            devices.Add(e.TouchDevice);
            foreach (TouchDevice touch in draggedElement.TouchesCapturedWithin)
            {
                if (touch != e.TouchDevice)
                {
                    devices.Add(touch);
                }
            }

            // Get the drag source object
            ItemsControl dragSource = ItemsControl.ItemsControlFromItemContainer(draggedElement);

            SurfaceDragCursor startDragOkay =
                SurfaceDragDrop.BeginDragDrop(
                  dragSource,                 // The SurfaceListBox object that the cursor is dragged out from.
                  draggedElement,             // The SurfaceListBoxItem object that is dragged from the drag source.
                  cursorVisual,               // The visual element of the cursor.
                  draggedElement.DataContext, // The data associated with the cursor.
                  devices,                    // The input devices that start dragging the cursor.
                  DragDropEffects.Move);      // The allowed drag-and-drop effects of the operation.

            // If the drag began successfully, set e.Handled to true. 
            // Otherwise SurfaceListBoxItem captures the touch 
            // and causes the drag operation to fail.
            e.Handled = (startDragOkay != null);
        }



        private void OnDropTargetDragEnter(object sender, SurfaceDragDropEventArgs e)
        {
            DataItem data = e.Cursor.Data as DataItem;

            if (!data.CanDrop)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void OnDropTargetDragLeave(object sender, SurfaceDragDropEventArgs e)
        {
            // Reset the effects.
            e.Effects = e.Cursor.AllowedEffects;
        }


        private void OnTargetChanged(object sender, TargetChangedEventArgs e)
        {
            if (e.Cursor.CurrentTarget != null)
            {
                DataItem data = e.Cursor.Data as DataItem;
                e.Cursor.Visual.Tag = (data.CanDrop) ? "CanDrop" : "CannotDrop";
            }
            else
            {
                e.Cursor.Visual.Tag = null;
            }
        }

        private void OnDropTargetDrop(object sender, SurfaceDragDropEventArgs e)
        {
           // MessageBox.Show("works");
            TargetItems.Add(e.Cursor.Data as DataItem);
            //DefaultPanel.Items.Add(e.Cursor.Data as DataItem);

            //DropTarget.Items.Add(e.Cursor.Data);
            
            ScatterViewItem svi = DefaultPanel.ItemContainerGenerator.ContainerFromItem(e.Cursor.Data) as ScatterViewItem;
           // svi.Orientation = e.Cursor.GetOrientation(this);
            svi.Orientation = 0.0;
           svi.Center = e.Cursor.GetPosition(this.DefaultPanel);
            //svi.Center = new Point(300,0);
            //svi.Width = 1320;
            //svi.Height = 1050;
            svi.Width = 820;
            svi.Height = 650;
        }

       
            
        

        private void OnDragCompleted(object sender, SurfaceDragCompletedEventArgs e)
        {
            // If the operation is Move, remove the data from drag source.
           // if (e.Cursor.Effects == DragDropEffects.Move)
           // {
             //   LeftItems.Remove(e.Cursor.Data as DataItem);
            //}
        }



    }

    public class DataItem
    {
        private string name;
        private bool canDrop;

        public string Name
        {
            get { return name; }
        }

        public bool CanDrop
        {
            get { return canDrop; }
        }

        public DataItem(string name, bool canDrop)
        {
            this.name = name;
            this.canDrop = canDrop;
        }
    }

    public class ScatterViewDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (element != null && item != null)
            {
                DataItem dItem = (DataItem)item;
                if (dItem.Name == "General")
                    return element.FindResource("ScatterViewItemDataTemplate2") as DataTemplate;
                else if (dItem.Name == "Location")
                    return element.FindResource("ScatterViewItemDataTemplate") as DataTemplate;
            }
            return null;
        }
    }
}
