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
using se306p2.Pages;
using System.Windows.Media.Animation;


namespace se306p2 {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : SurfaceWindow {

		public static MainWindow window;

		private ObservableCollection<DataItem> leftItems;
		private ObservableCollection<DataItem> rightItems;

        //testing animations
        private ObservableCollection<UserControl> pages = new ObservableCollection<UserControl>();



		private String lastBackgroundImage = "";
        
		public String BackgroundImage {
			set {
        		if (lastBackgroundImage == value) {
					return;
				}
				// TODO: implement some fading?

				//MainWindowGrid.Background = value;
				BitmapImage img = new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), value));
				ImageBrush brush = new ImageBrush(img);
				brush.Stretch = Stretch.Uniform;
				brush.TileMode = TileMode.None;

				//ScrollView.Background = brush;


			}
		}


		/// <summary>
		/// Items that bind with the drag source list box.
		/// </summary>
		public ObservableCollection<DataItem> LeftItems {
			get {
				if (leftItems == null) {
					leftItems = new ObservableCollection<DataItem>();
				}

				return leftItems;
			}
		}

		/// <summary>
		/// Items that bind with the drag source list box.
		/// </summary>
		public ObservableCollection<DataItem> RightItems {
			get {
				if (rightItems == null) {
					rightItems = new ObservableCollection<DataItem>();
				}

				return rightItems;
			}
		}




		protected override void OnInitialized(EventArgs e) {
			base.OnInitialized(e);
			DataContext = this;

            //testing animations
            pages.Add(new HomePage());
            pages.Add(new ExamplePage());
            pages.Add(new BaseInfo());
            // and so on...add all if this approach works



			LeftItems.Add(new DataItem("Home", true, new HomePage()));
			LeftItems.Add(new DataItem("Intro to ECE", true, new BaseInfo()));
			LeftItems.Add(new DataItem("HOD's Welcome", true, new HODpage()));
			LeftItems.Add(new DataItem("Course Advisors", true, new ECE_Advisors()));
			LeftItems.Add(new DataItem("Contact/Location", true, new ContactPage()));
			Course EeeCourse = new Course() {
				ProgramTitle = "EEE Courses",
			};
			EeeCourse.readJSON(new Uri("pack://application:,,,/Resources/eeeCourseInfo.json"));
			
			Course CseCourse = new Course() {
				ProgramTitle = "CSE Courses",
			};
			CseCourse.readJSON(new Uri("pack://application:,,,/Resources/cseCourseInfo.json"));
			
			Course SeCourse = new Course() {
				ProgramTitle = "SE Courses",
			};
			SeCourse.readJSON(new Uri("pack://application:,,,/Resources/seCourseInfo.json"));


			RightItems.Add(new DataItem("CSE Info", false, "#0b9246"));
			RightItems.Add(new DataItem("CSE Courses", true, CseCourse, "#0b9246"));
			RightItems.Add(new DataItem("EEE Info", false, "#1e9ad5"));
			RightItems.Add(new DataItem("EEE Courses", true, EeeCourse, "#1e9ad5"));
			RightItems.Add(new DataItem("SE Info", false, "#f6a220"));
            RightItems.Add(new DataItem("SE Courses", true, SeCourse, "#f6a220"));
		}


		/// <summary>
		/// Default constructor.
		/// </summary>
		public MainWindow() {
			window = this;
			InitializeComponent();

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

		private void OnDragSourcePreviewTouchDown(object sender, InputEventArgs e) {


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

            
            SelectPage(draggedElement.DataContext as DataItem);

            //// Create the cursor visual.
            //ContentControl cursorVisual = new ContentControl() {
            //    Content = draggedElement.DataContext,
            //    Style = FindResource("CursorStyle") as Style
            //};


            //// Add a handler. This will enable the application to change the visual cues.
            //SurfaceDragDrop.AddTargetChangedHandler(cursorVisual, OnTargetChanged);

            //// Create a list of input devices. Add the touches that
            //// are currently captured within the dragged element and
            //// the current touch (if it isn't already in the list).
            //List<InputDevice> devices = new List<InputDevice>();
            //devices.Add(e.Device);
            //foreach (TouchDevice touch in draggedElement.TouchesCapturedWithin) {
            //    if (touch != e.Device) {
            //        devices.Add(touch);
            //    }
            //}

            //// Get the drag source object
            //ItemsControl dragSource = ItemsControl.ItemsControlFromItemContainer(draggedElement);

            //SurfaceDragCursor startDragOkay =
            //    SurfaceDragDrop.BeginDragDrop(
            //      dragSource,                 // The SurfaceListBox object that the cursor is dragged out from.
            //      draggedElement,             // The SurfaceListBoxItem object that is dragged from the drag source.
            //      cursorVisual,               // The visual element of the cursor.
            //      draggedElement.DataContext, // The data associated with the cursor.
            //      devices,                    // The input devices that start dragging the cursor.
            //      DragDropEffects.Move);      // The allowed drag-and-drop effects of the operation.

            //// If the drag began successfully, set e.Handled to true. 
            //// Otherwise SurfaceListBoxItem captures the touch 
            //// and causes the drag operation to fail.
            //e.Handled = (startDragOkay != null);
            e.Handled = true;
		}


        //dont need, also remove s:SurfaceDragDrop.Drop="ListBoxDrop" type code from xaml file from the Default Panel and the two ListBoxes
		private void OnDropTargetDragEnter(object sender, SurfaceDragDropEventArgs e) {
			DataItem data = e.Cursor.Data as DataItem;

			//added to stop advisors drag from crashing program
			if (data == null)
				return;

			if (!data.CanDrop) {
				e.Effects = DragDropEffects.None;
			}
		}

		//dont need
        private void OnDropTargetDragLeave(object sender, SurfaceDragDropEventArgs e) {
			// Reset the effects.
			e.Effects = e.Cursor.AllowedEffects;
		}


		//dont need
        private void OnTargetChanged(object sender, TargetChangedEventArgs e) {
			if (e.Cursor.CurrentTarget != null) {
				DataItem data = e.Cursor.Data as DataItem;
				e.Cursor.Visual.Tag = (data.CanDrop) ? "CanDrop" : "CannotDrop";
			} else {
				e.Cursor.Visual.Tag = null;
			}
		}
		
        //dont need
        private void OnDropTargetDrop(object sender, SurfaceDragDropEventArgs e) {


			DataItem d = e.Cursor.Data as DataItem;

			SelectPage(d);
		}
		public void SelectPage(DataItem d) {

			if (d == null)
				return;
            
            
            //testing animations - doesn't work
            Storyboard sb2 = (Storyboard)Resources["SlideLeftToOrigin"];
            sb2.Begin(pages.ElementAt(1));
            pages.ElementAt(2).Visibility = System.Windows.Visibility.Visible;
            
            //uncomment the following lines to make it work again
            
            //Grid.SetColumn(d.PageControl, 0);
            //Grid.SetColumnSpan(d.PageControl, 2);
            //Grid.SetRow(d.PageControl, 0);
            //Grid.SetRowSpan(d.PageControl, 2);

            //if (!DefaultPanel.Children.Contains(d.PageControl)) {
            //    DefaultPanel.Children.Clear();
            //    DefaultPanel.Children.Add(d.PageControl);
            //}

            //DefaultPanel.UpdateLayout();
		}

		//dont need
        public void ListBoxDrop(Object sender, SurfaceDragDropEventArgs e) {

			SelectPage(e.Cursor.Data as DataItem);

		}

        

        private void ScrollView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (SurfaceWindow.Height != 1080 || SurfaceWindow.Width != 1920)
            {
                ScrollView.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                ScrollView.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
            //if (SurfaceWindow.Height != 1080 || SurfaceWindow.Width != 1920)
            //{
            //    ScrollView.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            //    ScrollView.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            //}
        }
	}



	public class DataItem {
		private string name;
		private string color;
		private bool canDrop;
		private UIElement pageControl;

		public UIElement PageControl {
			get { return pageControl; }
		}

		public string Name {
			get { return name; }
		}

		public string Color {
			get { return color; }
		}

		public bool CanDrop {
			get { return canDrop; }
		}

		public DataItem(string name, bool canDrop, UIElement uielement, string color = "Purple") {
			this.name = name;
			this.color = color;
			this.canDrop = canDrop;
			this.pageControl = uielement;
		}

		public DataItem(string name, bool canDrop)
			: this(name, canDrop, new ExamplePage(), "Crimson") {
		}

		public DataItem(string name, bool canDrop, string color)
			: this(name, canDrop, new ExamplePage(), color) {
		}

		public override String ToString() {
			return name;
		}
	}
}
