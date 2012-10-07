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
    /// Interaction logic for CourseCSE.xaml
    /// </summary>
    public partial class CourseCSE : UserControl
    {
        private ObservableCollection<Grid> pages = new ObservableCollection<Grid>();
        private ObservableCollection<Grid> subs = new ObservableCollection<Grid>();
        private int indicator = 0;

        public CourseCSE()
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
            
            pages.Add(_page1);
            pages.Add(_page2);
            pages.Add(_page3);

            subs.Add(_sub1);
            subs.Add(_sub2);
            subs.Add(_sub3);


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



    }



}


