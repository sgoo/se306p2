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

namespace se306p2.Pages {
	/// <summary>
	/// Interaction logic for Course.xaml
	/// </summary>
	public partial class Course : UserControl, INotifyPropertyChanged {
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

				PropertyChanged(this, new PropertyChangedEventArgs("CurrentCourseSet"));
			}
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

		public void addButtons(SortedList<String, CourseItem> courseList) {
			Grid fadeIn = fadePanels[currrentPanel++ % 2];
			Grid fadeOut = fadePanels[currrentPanel % 2];

			StackPanel topLine = fadeIn.Children[0] as StackPanel;
			StackPanel bottomLine = fadeIn.Children[1] as StackPanel;
			
			topLine.Children.Clear();
			bottomLine.Children.Clear();

			StackPanel[] panelLines = new StackPanel[] { topLine, bottomLine };
			
			if (courseList.Count < 4) {
				panelLines = new StackPanel[] { topLine };
			}
			int line = 0;
			foreach (KeyValuePair<String, CourseItem> kvp in courseList) {
				CourseButton button = new CourseButton() {
					CourseTitle = kvp.Value.Name,
					CourseCode = kvp.Key,
					CourseItem = kvp.Value
				};
				button.PreviewTouchDown += ClickCourse;
				button.PreviewMouseDown += ClickCourse;
				panelLines[line++ % panelLines.Count()].Children.Add(button);
			}

			fadeIn.Opacity = 0;

			Storyboard sb1 = (Storyboard)Resources["FadeOut"];
			Storyboard sb2 = (Storyboard)Resources["FadeIn"];

			sb1.Completed += FadeOut_Completed;

			sb1.SeekAlignedToLastTick(new TimeSpan(0, 0, 10));
			sb2.SeekAlignedToLastTick(new TimeSpan(0, 0, 10));

			sb1.Begin(fadeOut);
			sb2.Begin(fadeIn);
		}

		void FadeOut_Completed(object sender, EventArgs e) {
			(fadePanels[currrentPanel % 2].Children[0] as StackPanel).Children.Clear();
			(fadePanels[currrentPanel % 2].Children[1] as StackPanel).Children.Clear();
		}

		public void ClickCourse(object sender, InputEventArgs e) {
			CourseButton cb = sender as CourseButton;
			CurrentCourse = cb.CourseItem;
		}

		public void RightButtonClick(object sender, InputEventArgs e) {
			if (CourseSets.ContainsKey(currentCourseSet + 1)) {
				CurrentCourseSet++;
			}
		}

		public void LeftButtonClick(object sender, InputEventArgs e) {
			if (CourseSets.ContainsKey(currentCourseSet - 1)) {
				CurrentCourseSet--;
			}
		}


		public void readJSON(Uri location) {

			string text = new StreamReader(Application.GetResourceStream(location).Stream).ReadToEnd();
			Console.WriteLine(text);
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

		// swipe code
		private Dictionary<MouseDevice, Point> currentTouchDevices = new Dictionary<MouseDevice, Point>();
		private void Swipe_TouchDown(object sender, MouseButtonEventArgs e) {
			currentTouchDevices.Add(e.MouseDevice, e.MouseDevice.GetPosition(this));
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
