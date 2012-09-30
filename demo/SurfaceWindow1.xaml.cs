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

namespace demo
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow
    {

        private ObservableCollection<GifType> gifTypes;
        public ObservableCollection<GifType> GifTypes
        {
            get
            {
                if (gifTypes == null)
                {
                    gifTypes = new ObservableCollection<GifType>();
                }
                return gifTypes;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1()
        {

            InitializeComponent();

            DataContext = this;
            GifType catGifs = new GifType("Cat GIFs", "Often short clips of cats doing crazy and bizarre stuff");
            catGifs.GIFs.Add("Resources/cat1.gif");
            catGifs.GIFs.Add("Resources/cat2.gif");
            GifType dogGifs = new GifType("Dog GIFs", "Dog gifs often contain dogs trying to prove they are better and cuter than cats, however they almost always fail to capture the awesomeness that cats bring");
            dogGifs.GIFs.Add("Resources/dog1.gif");
            dogGifs.GIFs.Add("Resources/dog2.gif");
            GifType winGifs = new GifType("Win GIFs", "Short clips of people who have mastered life in every way, they are the supreme beings among us");
            winGifs.GIFs.Add("Resources/win1.gif");
            winGifs.GIFs.Add("Resources/win2.gif");
            GifType failGifs = new GifType("Fail GIFs", "These clips feature people who are very much NOT the supreme beings among us");
            failGifs.GIFs.Add("Resources/fail1.gif");
            failGifs.GIFs.Add("Resources/fail2.gif");
            GifType coolGifs = new GifType("Cool GIFs", "These are clips of things, be they real or fake, but are all rather cool");
            coolGifs.GIFs.Add("Resources/cool1.gif");
            coolGifs.GIFs.Add("Resources/cool2.gif");
            GifTypes.Add(catGifs);
            GifTypes.Add(dogGifs);
            GifTypes.Add(winGifs);
            GifTypes.Add(failGifs);
            GifTypes.Add(coolGifs);

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
        private double drop1Height = 200;
        private void drop1_Drop(object sender, SurfaceDragDropEventArgs e)
        {
            drop1.Items.Clear();
            drop1.Items.Add(e.Cursor.Data);
            drop1Height = e.Cursor.Visual.Height;
            fixRowHeight();
            listDisplay1.Items.Clear();
            listDisplay1.Items.Add(e.Cursor.Data);
        }

        private double drop2Height = 200;
        private void drop2_Drop(object sender, SurfaceDragDropEventArgs e)
        {
            drop2.Items.Clear();
            drop2.Items.Add(e.Cursor.Data);
            drop2Height = e.Cursor.Visual.Height;
            fixRowHeight();
            listDisplay2.Items.Clear();
            listDisplay2.Items.Add(e.Cursor.Data);

        }
        private void fixRowHeight()
        {
            double height = 200;
            if (drop1Height > height)
            {
                height = drop1Height;
            }
            if (drop2Height > height)
            {
                height = drop2Height;
            }
            topRow.Height = new GridLength(height);
        }


        private void OnMainDrag(object sender, InputEventArgs e)
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
            devices.Add(e.Device);
            foreach (TouchDevice touch in draggedElement.TouchesCapturedWithin)
            {
                if (touch != e.Device)
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

        private void OnTargetChanged(object sender, TargetChangedEventArgs e)
        {
            Console.WriteLine(sender);
            if (e.Cursor.CurrentTarget != null)
            {
                e.Cursor.Visual.Tag = "CanDrop";
            }
            else
            {
                e.Cursor.Visual.Tag = null;
            }
        }

        private void drop1_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }


    }
}