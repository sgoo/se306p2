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

namespace se306p2.Pages {
	/// <summary>
	/// Interaction logic for Course.xaml
	/// </summary>
	public partial class Course : UserControl, INotifyPropertyChanged {
		public Course() {
			InitializeComponent();
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private int currentCourseSet = 0;
		public int CurrentCourseSet {
			get { return currentCourseSet; }
			set {
				currentCourseSet = value;
				CourseSet cs = CourseSets[value];
				CourseSetTitle = cs.Title;
				addButtons(cs.CourseList);
				CurrentCourseDesc = "";
				CurrentCourseTitle = "";
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

		private string courseTitle = "";
		public string CourseSetTitle {
			get { return courseTitle; }
			set {
				courseTitle = value;
				PropertyChanged(this, new PropertyChangedEventArgs("CourseSetTitle"));
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


		public void addButtons(SortedList<String, CourseItem> courseList) {
			StackPanel.Children.Clear();

			foreach (KeyValuePair<String, CourseItem> kvp in courseList) {
				CourseButton button = new CourseButton() {
					CourseTitle = kvp.Value.Name,
					CourseCode = kvp.Key,
					CourseItem = kvp.Value
				};
				button.PreviewTouchDown += ClickCourse;
				button.PreviewMouseDown += ClickCourse;
				StackPanel.Children.Add(button);
			}
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
