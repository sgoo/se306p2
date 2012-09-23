using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;


namespace GifControl {
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:GifControl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:GifControl;assembly=GifControl"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class GifControl : System.Windows.Controls.Image {

        static GifControl() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GifControl), new FrameworkPropertyMetadata(typeof(GifControl)));
        }


        public static readonly DependencyProperty UriProperty =
        DependencyProperty.Register(
            "Uri",
            typeof(String),
            typeof(GifControl),
            new PropertyMetadata(default(String), OnUriChange));

        private static void OnUriChange(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            GifControl gif = d as GifControl;
            String u = e.NewValue.ToString();
            gif.Uri = u;
        }


        public String uri;
        [Description("Test text displayed in the textbox"), Category("Data")]
        public string Uri {
            get {
                return uri.ToString();
            }
            set {
                uri = value;
                Uri u = new Uri(Environment.CurrentDirectory + "\\" + value);
                gifImg = System.Drawing.Image.FromFile(Environment.CurrentDirectory + "\\" + value);
                dimension = new FrameDimension(gifImg.FrameDimensionsList[0]);
                anim = new Int32Animation(0, gifImg.GetFrameCount(dimension) - 1, new Duration(new TimeSpan(0, 0, 0, gifImg.GetFrameCount(dimension) / 10, (int)((gifImg.GetFrameCount(dimension) / 10.0 - gifImg.GetFrameCount(dimension) / 10) * 1000))));
                anim.RepeatBehavior = RepeatBehavior.Forever;

                Height = gifImg.Height;
                Width = gifImg.Width;


            }
        }

        public int FrameIndex {
            get { return (int)GetValue(FrameIndexProperty); }
            set { SetValue(FrameIndexProperty, value); }
        }

        private static readonly DependencyProperty FrameIndexProperty =
            DependencyProperty.Register("FrameIndex", typeof(int), typeof(GifControl), new UIPropertyMetadata(0, new PropertyChangedCallback(ChangingFrameIndex)));

        static void ChangingFrameIndex(DependencyObject obj, DependencyPropertyChangedEventArgs ev) {
            GifControl ob = obj as GifControl;


            ob.gifImg.SelectActiveFrame(ob.dimension, (int)ev.NewValue);

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            // Save to a memory stream..
            ob.gifImg.Save(ms, ImageFormat.Bmp);
            // Rewind the stream..
            ms.Seek(0, SeekOrigin.Begin);
            // Tell the WPF image to use this stream...
            bi.StreamSource = ms;
            bi.EndInit();


            ob.Source = bi;
            ob.InvalidateVisual();
        }
        private System.Drawing.Image gifImg;
        FrameDimension dimension;
        //private GifBitmapDecoder gf;
        private Int32Animation anim;
        public GifControl() {

        }


        private bool animationIsWorking = false;

        protected override void OnRender(DrawingContext dc) {
            base.OnRender(dc);
            if (!animationIsWorking) {
                BeginAnimation(FrameIndexProperty, anim);
                animationIsWorking = true;
            }
        }
    }
}
