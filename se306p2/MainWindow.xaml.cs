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

			LeftItems.Add(new DataItem("Home", true, new HomePage()));
           	LeftItems.Add(new DataItem("Intro to ECE", true, new BaseInfo()));
			LeftItems.Add(new DataItem("HOD's Welcome", true, new HODpage()));
			LeftItems.Add(new DataItem("Course Advisors", true, new ECE_Advisors()));
			LeftItems.Add(new DataItem("Contact/Location", true, new ContactPage()));

            // Felix:  I use these titles in Courses, so I put them in a common place.
			Course EeeCourse = new Course() {
                ProgramTitle = (string) Application.Current.FindResource("EEE_Courses_Title"),
			};
			EeeCourse.readJSON(new Uri("pack://application:,,,/Resources/eeeCourseInfo.json"));
			
			Course CseCourse = new Course() {
                ProgramTitle = (string)Application.Current.FindResource("CSE_Courses_Title"),
			};
			CseCourse.readJSON(new Uri("pack://application:,,,/Resources/cseCourseInfo.json"));
			
			Course SeCourse = new Course() {
                ProgramTitle = (string)Application.Current.FindResource("SE_Courses_Title"),
			};
			SeCourse.readJSON(new Uri("pack://application:,,,/Resources/seCourseInfo.json"));

            // Needed the colours in the Courses page, so put them in a common file (Resources>Colours.xaml).
            string cseColour = (string)Application.Current.FindResource("Colour_CSE_Str");
			RightItems.Add(new DataItem("CSE Info", true, new CSEInfo(), cseColour));
			RightItems.Add(new DataItem("CSE Courses", true, CseCourse, cseColour));
            string eeeColour = (string)Application.Current.FindResource("Colour_EEE_Str");
			RightItems.Add(new DataItem("EEE Info", true, new EEEInfo(), eeeColour));
			RightItems.Add(new DataItem("EEE Courses", true, EeeCourse, eeeColour));
            string seColour = (string)Application.Current.FindResource("Colour_SE_Str");
			RightItems.Add(new DataItem("SE Info", true, new SEInfo(), seColour));
            RightItems.Add(new DataItem("SE Courses", true, SeCourse, seColour));
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


        private void OnDragSourcePreviewTouchUp(object sender, InputEventArgs e)
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

            Storyboard sbrd = (Storyboard)Resources["MyRelease"];
            sbrd.Begin(draggedElement);

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

            TransformGroup tGroup = new TransformGroup();
            ScaleTransform sTrans = new ScaleTransform();
            TranslateTransform tTrans = new TranslateTransform();

            sTrans.ScaleX = 1;
            sTrans.ScaleY = 1;

            tTrans.X = 0;
            tTrans.Y = 0;

            tGroup.Children.Add(sTrans);
            tGroup.Children.Add(tTrans);
            draggedElement.RenderTransform = tGroup;

            Storyboard sb = (Storyboard)Resources["MyPress"];
            sb.Begin(draggedElement);
            SelectPage(draggedElement.DataContext as DataItem,sender);
     
            e.Handled = true;
		}



		public void SelectPage(DataItem d,object sender) {

            Storyboard sBrd; 
            
            if (d == null)
				return;

            if (sender == null)
            {
                return;
            }
            else
            {
                SurfaceListBox lb = sender as SurfaceListBox;

                if (lb.Name == "LeftScatterBar")
                {
                   sBrd = (Storyboard)Resources["SlideLeftToOrigin"];
                }
                else
                {
                    sBrd = (Storyboard)Resources["SlideRightToOrigin"];
                }
                
            }
            

            if (!DefaultPanel.Children.Contains(d.PageControl)) {
               
           
                DefaultPanel.Children.Clear();

                UserControl ctrl = d.PageControl as UserControl;
                Grid newGrid = new Grid();

                try
                {
                    Grid c = VisualTreeHelper.GetParent(ctrl) as Grid;
                    c.Children.Clear();
                }
                catch(Exception e)
                {
                    //no need to do anything here
                }
                
                newGrid.Children.Add(ctrl);

                
                TranslateTransform tr = new TranslateTransform();             

                TransformGroup myTransformGroup = new TransformGroup();
                
               
                myTransformGroup.Children.Add(tr);

                newGrid.RenderTransform = myTransformGroup;

                sBrd.Begin(newGrid);

                Grid.SetColumn(newGrid, 0);
                Grid.SetColumnSpan(newGrid, 2);
                Grid.SetRow(newGrid, 0);
                Grid.SetRowSpan(newGrid, 2);
                
                DefaultPanel.Children.Add(newGrid);
                   
            }

            
		}


        private void ScrollView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (SurfaceWindow.Height != 1080 || SurfaceWindow.Width != 1920)
            {
                ScrollView.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                ScrollView.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
            
            //comment out the following lines to get the scrollBars

            if (SurfaceWindow.Height != 1080 || SurfaceWindow.Width != 1920)
            {
                ScrollView.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                ScrollView.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
            }
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
