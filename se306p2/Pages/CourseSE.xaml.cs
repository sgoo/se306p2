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
using System.Windows.Media.Animation;


namespace se306p2
{
    /// <summary>
    /// Interaction logic for CourseSE.xaml
    /// </summary>
    public partial class CourseSE : UserControl
    {
        private ObservableCollection<Grid> pages = new ObservableCollection<Grid>();
        private ObservableCollection<Grid> subs = new ObservableCollection<Grid>();
        private int indicator = 2; //changed from 0 to 2 by Sam for demo

        public CourseSE()
        {
            InitializeComponent();

        }

        private void tbMultiLine_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            DataContext = this;

            //changed Add order. 2 by Sam for demo
            pages.Add(_page3);
            pages.Add(_page2);
            pages.Add(_page1);

            //changed Add order. 2 by Sam for demo
            subs.Add(_sub3);
            subs.Add(_sub2);
            subs.Add(_sub1);
            
            


        }

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{

		}



        private void PreviousButton(object sender, RoutedEventArgs e)
        {
            SwitchPrevious();
        }
        private void NextButton(object sender, RoutedEventArgs e)
        {
            SwitchNext();
        }
        private void SwitchPrevious()
        {
            if (IsPreviousValid())
            {
                Grid currentpage = pages.ElementAt(indicator);
                Grid currentSub = subs.ElementAt(indicator);
                indicator -= 1;
                Grid nextSub = subs.ElementAt(indicator);
                Grid nextpage = pages.ElementAt(indicator);



                // animations
                Storyboard sb1 = (Storyboard)Resources["SlideOriginToLeft"];
                Storyboard sb2 = (Storyboard)Resources["SlideRightToOrigin"];
                sb1.Begin(currentSub);
                sb2.Begin(nextSub);

                Storyboard sb3 = (Storyboard)Resources["FadeOut"];
                Storyboard sb4 = (Storyboard)Resources["FadeIn"];

                sb4.SeekAlignedToLastTick(new TimeSpan(0, 0, 10));
                sb3.SeekAlignedToLastTick(new TimeSpan(0, 0, 10));

                sb3.Begin(currentpage);
                sb4.Begin(nextpage);

          

                //currentpage.Visibility = System.Windows.Visibility.Collapsed;
                nextpage.Visibility = System.Windows.Visibility.Visible;
                //currentSub.Visibility = System.Windows.Visibility.Collapsed;
                nextSub.Visibility = System.Windows.Visibility.Visible;

				setCourseDescription("");
            }
        }
        private void SwitchNext()
        {
            if (IsNextValid())
            {
                Grid currentpage = pages.ElementAt(indicator);
                Grid currentSub = subs.ElementAt(indicator);
                indicator += 1;
                Grid nextpage = pages.ElementAt(indicator);
                Grid nextSub = subs.ElementAt(indicator);


                // animations
                Storyboard sb1 = (Storyboard)Resources["SlideOriginToRight"];
                Storyboard sb2 = (Storyboard)Resources["SlideLeftToOrigin"];
                sb1.Begin(currentSub);
                sb2.Begin(nextSub);

                Storyboard sb3 = (Storyboard)Resources["FadeOut"];
                Storyboard sb4 = (Storyboard)Resources["FadeIn"];
                sb3.Begin(currentpage);
                sb4.Begin(nextpage);


                //currentpage.Visibility = System.Windows.Visibility.Collapsed;
                nextpage.Visibility = System.Windows.Visibility.Visible;
                //currentSub.Visibility = System.Windows.Visibility.Collapsed;
                nextSub.Visibility = System.Windows.Visibility.Visible;

				setCourseDescription("");
            }
        }
        private bool IsPreviousValid()
        {
            if (indicator > 0)
                return true;
            else
                return false;
        }
        private bool IsNextValid()
        {
            if (indicator < pages.Count - 1)
                return true;
            else
                return false;
        }



        private Dictionary<MouseDevice, Point> currentTouchDevices = new Dictionary<MouseDevice, Point>();
        private void _touchGrid_TouchDown(object sender, MouseButtonEventArgs e)
        {
            currentTouchDevices.Add(e.MouseDevice, e.MouseDevice.GetPosition(this));
        }
        private void _touchGrid_TouchUp(object sender, MouseEventArgs e)
        {
            currentTouchDevices.Remove(e.MouseDevice);
        }
        private void _touchGrid_TouchMove(object sender, MouseEventArgs e)
        {
            if (currentTouchDevices.Count == 1)
            {
                int isLeft = 0;
                int isRight = 0;
                foreach (KeyValuePair<MouseDevice, Point> td in currentTouchDevices)
                {
                    if (td.Key != null && e.MouseDevice.GetPosition(this).X - td.Value.X > 100)
                        isRight++;
                    else if (td.Key != null && td.Value.X - e.MouseDevice.GetPosition(this).X > 100)
                        isLeft++;
                    else
                        return;
                }
                if (isLeft == 1 && isRight == 0)
                {
                    SwitchPrevious();
                    currentTouchDevices.Clear();
                    return;
                }
                else if (isRight == 1 && isLeft == 0)
                {
                    SwitchNext();
                    currentTouchDevices.Clear();
                    return;
                }
            }
        }

		private void CourseButton_TouchUp(object sender, MouseButtonEventArgs e)
		{
			Grid s = (Grid)sender;
			String courseDescription = s.Tag.ToString();

			String courseCode = ((TextBlock)s.Children[0]).Text;
			String courseName = ((TextBlock)s.Children[1]).Text;

			setCourseDescription(courseCode + " - " + courseName + Environment.NewLine + courseDescription);
		}



		private void setCourseDescription(string text)
		{
			CourseDescription1.Text = text;
			CourseDescription2.Text = text;
			CourseDescription3.Text = text;
		}

		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
			if (IsVisible) {
				MainWindow.window.BackgroundImage = "/Resources/background_se.jpg";
			}
		}

    }



}


