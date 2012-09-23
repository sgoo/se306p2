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

namespace demo {
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    public partial class SurfaceWindow1 : SurfaceWindow {

        private ObservableCollection<GifType> gifTypes;
        public ObservableCollection<GifType> GifTypes {
            get {
                if (gifTypes == null) {
                    gifTypes = new ObservableCollection<GifType>();
                }
                return gifTypes;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SurfaceWindow1() {

            InitializeComponent();
            DataContext = this;
            GifTypes.Add(new GifType("Cat GIFs", new string[] { "Resources/cat1.gif", "Resources/cat2.gif", "Resources/cat3.gif" }));
            GifTypes.Add(new GifType("Dog GIFs", new string[] { "Resources/dog1.gif", "Resources/dog2.gif", "Resources/dog3.gif" }));
            GifTypes.Add(new GifType("Win GIFs", new string[] { "Resources/win1.gif", "Resources/win2.gif", "Resources/win3.gif" }));
            GifTypes.Add(new GifType("Fail GIFs", new string[] { "Resources/fail1.gif", "Resources/fail2.gif", "Resources/fail3.gif" }));

            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers() {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers() {
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
        private void OnWindowInteractive(object sender, EventArgs e) {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e) {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e) {
            //TODO: disable audio, animations here
        }
        private double drop1Height = 200;
        private void drop1_Drop(object sender, SurfaceDragDropEventArgs e) {
            drop1.Items.Clear();
            drop1.Items.Add(e.Cursor.Data);
            drop1Height = e.Cursor.Visual.Height;
            fixRowHeight();
            listDisplay1.Items.Clear();
            listDisplay1.Items.Add(e.Cursor.Data);
        }

        private double drop2Height = 200;
        private void drop2_Drop(object sender, SurfaceDragDropEventArgs e) {
            drop2.Items.Clear();
            drop2.Items.Add(e.Cursor.Data);
            drop2Height = e.Cursor.Visual.Height;
            fixRowHeight();
            listDisplay2.Items.Clear();
            listDisplay2.Items.Add(e.Cursor.Data);

        }
        private void fixRowHeight() {
            double height = 200;
            if (drop1Height > height) {
                height = drop1Height;
            }
            if (drop2Height > height) {
                height = drop2Height;
            }
            topRow.Height = new GridLength(height);
        }


        private void OnMainDrag(object sender, InputEventArgs e) {
            FrameworkElement findSource = e.OriginalSource as FrameworkElement;
            SurfaceListBoxItem draggedElement = null;

            // Find the touched SurfaceListBoxItem object.
            while (draggedElement == null && findSource != null) {
                if ((draggedElement = findSource as SurfaceListBoxItem) == null) {
                    findSource = VisualTreeHelper.GetParent(findSource) as FrameworkElement;
                }
            }

            if (draggedElement == null) {
                return;
            }

            // Create the cursor visual.
            ContentControl cursorVisual = new ContentControl() {
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
            foreach (TouchDevice touch in draggedElement.TouchesCapturedWithin) {
                if (touch != e.Device) {
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

        private void OnTargetChanged(object sender, TargetChangedEventArgs e) {
            Console.WriteLine(sender);
            if (e.Cursor.CurrentTarget != null) {
                e.Cursor.Visual.Tag = "CanDrop";
            }
            else {
                e.Cursor.Visual.Tag = null;
            }
        }


    }
}