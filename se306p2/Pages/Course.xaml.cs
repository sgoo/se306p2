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
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows.Media.Animation;

// TODO:  Change colour of buttons to specialisation colours (from purple).

namespace se306p2.Pages {
	/// <summary>
	/// Interaction logic for Course.xaml
	/// </summary>
	public partial class Course : UserControl, INotifyPropertyChanged, ResettableControl {
		public Course() {
			InitializeComponent();
			fadePanels = new Grid[] { FadePanel1, FadePanel2 };

		}

		public event PropertyChangedEventHandler PropertyChanged;
		private int currentCourseSet = 0;
		public int CurrentCourseSet {
			get { return currentCourseSet; }
			set {
				Storyboard sb1;
				Storyboard sb2;
				if (currentCourseSet > value) {
					sb1 = (Storyboard)Resources["SlideOriginToRight"];
					sb2 = (Storyboard)Resources["SlideLeftToOrigin"];
				} else {
					sb1 = (Storyboard)Resources["SlideOriginToLeft"];
					sb2 = (Storyboard)Resources["SlideRightToOrigin"];
				}
				currentCourseSet = value;

				// Make "<" or ">" button appear/disappear as necessary.
				handleArrows(currentCourseSet);

				//CourseSetTitle = "";

				CourseSet cs = CourseSets[value];

				//CourseSetTitle = cs.Title;

				addButtons(cs.CourseList);
				CurrentCourseDesc = "";
				CurrentCourseTitle = "";

				SlideOutTitle = SlideInTitle;
				SlideInTitle = cs.Title;

				sb1.Begin(SlideOutTextBlock);
				sb2.Begin(SlideInTextBlock);
				CurrentCourse = cs.CourseList.First().Value;

				PropertyChanged(this, new PropertyChangedEventArgs("CurrentCourseSet"));
			}
		}

		// Make the "<" or ">" disappear if clicking them would do nothging.
		// Ie, if we are in Part II or Part IV and cannot go further left or right.
		private void handleArrows(int currentPos) {
			// 1) Make arrows disappear.
			// looked starnge with borders arround the buttons so i removed this? - scott
			/*if (currentPos == 0)
				// Make "<" disappear.
				LeftArrow.Visibility = Visibility.Hidden;
			else if (currentPos == 2)
				// Make ">" disappear.
				RightArrow.Visibility = Visibility.Hidden;

			// 2) Make arrows appear.
			if (currentPos != 0)
				// Make "<" appear.
				LeftArrow.Visibility = Visibility.Visible;
			if (currentPos != 2)
				// Make ">" appear.
				RightArrow.Visibility = Visibility.Visible;*/
		}

		private string title = "";
		public string ProgramTitle {
			get { return title; }
			set {
				title = value;
				PropertyChanged(this, new PropertyChangedEventArgs("ProgramTitle"));
			}
		}

		/*private string courseTitle = "";
		public string CourseSetTitle {
			get { return courseTitle; }
			set {
				courseTitle = value;
				PropertyChanged(this, new PropertyChangedEventArgs("CourseSetTitle"));
			}
		}*/

		private string slideOutTitle = "";
		public string SlideOutTitle {
			get { return slideOutTitle; }
			set {
				slideOutTitle = value;
				PropertyChanged(this, new PropertyChangedEventArgs("SlideOutTitle"));
			}
		}

		private string slideInTitle = "";
		public string SlideInTitle {
			get { return slideInTitle; }
			set {
				slideInTitle = value;
				PropertyChanged(this, new PropertyChangedEventArgs("SlideInTitle"));
			}
		}

		private String currentCourseTitle;
		public String CurrentCourseTitle {
			get { return currentCourseTitle; }
			set {
				currentCourseTitle = value;
				PropertyChanged(this, new PropertyChangedEventArgs("CurrentCourseTitle"));
			}
		}
		private String currentCourseDesc;
		public String CurrentCourseDesc {
			get { return currentCourseDesc; }
			set {
				currentCourseDesc = value;
				PropertyChanged(this, new PropertyChangedEventArgs("CurrentCourseDesc"));
			}
		}

		private CourseItem currentCourse;
		public CourseItem CurrentCourse {
			get { return currentCourse; }
			set {
				currentCourse = value;
				CurrentCourseTitle = value.Code + " - " + value.Name;
				CurrentCourseDesc = value.Desc;
				PropertyChanged(this, new PropertyChangedEventArgs("CurrentCourse"));
			}
		}

		public SortedList<int, CourseSet> CourseSets = new SortedList<int, CourseSet>();

		private Grid[] fadePanels;
		private int currrentPanel = 0;

		// Adds the course buttons.
		public void addButtons(SortedList<String, CourseItem> courseList) {
			// Electrical has 30 4th year courses!!!

			Grid fadeIn = fadePanels[currrentPanel++ % 2];
			Grid fadeOut = fadePanels[currrentPanel % 2];

			StackPanel[] cols = { fadeIn.Children[0] as StackPanel, fadeIn.Children[1] as StackPanel, fadeIn.Children[2] as StackPanel, fadeIn.Children[3] as StackPanel };

			// Clear old courses.
			foreach (StackPanel c in cols) {
				c.Children.Clear();
			}

			//cols = new StackPanel[]{ fadeIn.Children[0] as StackPanel, fadeIn.Children[1] as StackPanel, fadeIn.Children[2] as StackPanel, fadeIn.Children[3] as StackPanel };

			// Actually add courses.
			int currentCol = 0;
			foreach (KeyValuePair<String, CourseItem> kvp in courseList) {
				CourseButton button = new CourseButton() {
					CourseTitle = kvp.Value.Name,
					CourseCode = kvp.Key,
					CourseItem = kvp.Value
				};
				button.PreviewTouchDown += ClickCourse;
				button.PreviewMouseDown += ClickCourse;

				cols[currentCol].Children.Add(button);
				currentCol = (currentCol + 1) % cols.Length;

				// Add courses to lines alternatively (eg:  line 1, 2, 1, 2 etc).
				//panelLines[line++ % panelLines.Count()].Children.Add(button);

				// Make button the colour of that specialisation.
				setButtonColour(button);
			}

			fadeIn.Opacity = 0;

			Storyboard sb1 = (Storyboard)Resources["FadeOut"];
			Storyboard sb2 = (Storyboard)Resources["FadeIn"];

			sb1.Completed += FadeOut_Completed;

			sb1.SeekAlignedToLastTick(new TimeSpan(0, 0, 10));
			sb2.SeekAlignedToLastTick(new TimeSpan(0, 0, 10));
			
			MainScrollViewer.ScrollToTop();

			sb1.Begin(fadeOut);
			sb2.Begin(fadeIn);
		}

		private void setButtonColour(CourseButton button) {
			if (ProgramTitle.Equals((string)Application.Current.FindResource("CSE_Courses_Title")))
				button.Button.Background = (SolidColorBrush)Application.Current.FindResource("Colour_CSE");
			else if (ProgramTitle.Equals((string)Application.Current.FindResource("EEE_Courses_Title")))
				button.Button.Background = (SolidColorBrush)Application.Current.FindResource("Colour_EEE");
			else if (ProgramTitle.Equals((string)Application.Current.FindResource("SE_Courses_Title")))
				button.Button.Background = (SolidColorBrush)Application.Current.FindResource("Colour_SE");
		}

		void FadeOut_Completed(object sender, EventArgs e) {
			(fadePanels[currrentPanel % 2].Children[0] as StackPanel).Children.Clear();
			(fadePanels[currrentPanel % 2].Children[1] as StackPanel).Children.Clear();
			(fadePanels[currrentPanel % 2].Children[2] as StackPanel).Children.Clear();
			(fadePanels[currrentPanel % 2].Children[3] as StackPanel).Children.Clear();
		}

		public void ClickCourse(object sender, InputEventArgs e) {
			CourseButton cb = sender as CourseButton;
			CurrentCourse = cb.CourseItem;
		}
		
		private DateTime lastTimeRight = DateTime.Now;
		public void RightButtonClick(object sender, InputEventArgs e) {
			if ((DateTime.Now - lastTimeRight).TotalMilliseconds < 100) {
				return;
			}
			lastTimeRight = DateTime.Now;
			if (CourseSets.ContainsKey(currentCourseSet + 1)) {
				CurrentCourseSet++;
			}
		}

		private DateTime lastTimeLeft = DateTime.Now;
		public void LeftButtonClick(object sender, InputEventArgs e) {
			if ((DateTime.Now - lastTimeLeft).TotalMilliseconds < 100) {
				return;
			}
			lastTimeLeft = DateTime.Now;

			if (CourseSets.ContainsKey(currentCourseSet - 1)) {
				CurrentCourseSet--;
			}
		}

		public void readJSON(Uri location) {
			string text = new StreamReader(Application.GetResourceStream(location).Stream).ReadToEnd();
			//Console.WriteLine(text);
			//JObject json = JObject.Parse(text);

			String[] yearTitles = { "Part II", "Part III", "Part IV" };
			JArray courses = JArray.Parse(text);
			for (int i = 0; i < courses.Count; i++) {
				CourseSet cs = new CourseSet() {
					Title = yearTitles[i]
				};

				JArray yearCourses = courses[i] as JArray;

				for (int j = 0; j < yearCourses.Count; j++) {
					JObject course = yearCourses[j] as JObject;
					CourseItem ci = new CourseItem();
					ci.Name = course["name"].ToString();
					ci.Code = course["code"].ToString();
					ci.Points = course["points"].ToString();
					ci.Desc = course["text"].ToString();

					cs.CourseList.Add(ci.Code, ci);
				}
				CourseSets.Add(i, cs);
			}
			CurrentCourseSet = 0;
		}

		// === swipe code ===
		private Dictionary<MouseDevice, Point> currentTouchDevices = new Dictionary<MouseDevice, Point>();
		private void Swipe_TouchDown(object sender, MouseButtonEventArgs e) {
			if (!currentTouchDevices.ContainsKey(e.MouseDevice)) {
				currentTouchDevices.Add(e.MouseDevice, e.MouseDevice.GetPosition(this));
			}
		}
		private void Swipe_TouchUp(object sender, MouseEventArgs e) {
			currentTouchDevices.Remove(e.MouseDevice);
		}
		private void Swipe_TouchMove(object sender, MouseEventArgs e) {
			if (currentTouchDevices.Count == 1) {
				int isLeft = 0;
				int isRight = 0;
				foreach (KeyValuePair<MouseDevice, Point> td in currentTouchDevices) {
					if (td.Key != null && e.MouseDevice.GetPosition(this).X - td.Value.X > 100)
						isRight++;
					else if (td.Key != null && td.Value.X - e.MouseDevice.GetPosition(this).X > 100)
						isLeft++;
					else
						return;
				}
				if (isLeft == 1 && isRight == 0) {
					if (CourseSets.ContainsKey(currentCourseSet + 1)) {
						CurrentCourseSet++;
					}
					currentTouchDevices.Clear();
					return;
				} else if (isRight == 1 && isLeft == 0) {
					if (CourseSets.ContainsKey(currentCourseSet - 1)) {
						CurrentCourseSet--;
					}
					currentTouchDevices.Clear();
					return;
				}
			}
		}

		public void reset() {
			CurrentCourseSet = 0;
		}

		public class CourseSet {
			public String Title;
			public SortedList<String, CourseItem> CourseList = new SortedList<String, CourseItem>();
		}

		public class CourseItem {
			public String Name { get; set; }
			public String Code { get; set; }
			public String Points { get; set; }
			public String Desc { get; set; }
		}

	}
}
