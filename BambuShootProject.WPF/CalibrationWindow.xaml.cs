using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BambuShootProject.WPF
{
    /// <summary>
    /// Interaction logic for CalibrationWindow.xaml
    /// </summary>
    public partial class CalibrationWindow : Window
    {

        private bool isDragging = false;
        private Point anchorPoint = new Point();

        public CalibrationWindow()
        {
            InitializeComponent();
            Gridimage1.MouseLeftButtonDown += new MouseButtonEventHandler(image1_MouseLeftButtonDown);
            Gridimage1.MouseMove += new MouseEventHandler(image1_MouseMove);
            Gridimage1.MouseLeftButtonUp += new MouseButtonEventHandler(image1_MouseLeftButtonUp);
            Go.IsEnabled = false;
        }
        private void Go_Click(object sender, RoutedEventArgs e)
        {
            if (image1.Source != null)
            {
                Rect rect1 = new Rect(Canvas.GetLeft(selectionRectangle), Canvas.GetTop(selectionRectangle), selectionRectangle.Width, selectionRectangle.Height);
                System.Windows.Int32Rect rcFrom = new System.Windows.Int32Rect();
                rcFrom.X = (int)((rect1.X) * (image1.Source.Width) / (image1.Width));
                rcFrom.Y = (int)((rect1.Y) * (image1.Source.Height) / (image1.Height));
                rcFrom.Width = (int)((rect1.Width) * (image1.Source.Width) / (image1.Width));
                rcFrom.Height = (int)((rect1.Height) * (image1.Source.Height) / (image1.Height));
                BitmapSource bs = new CroppedBitmap(image1.Source as BitmapSource, rcFrom);
                image2.Source = bs;
            }
        }
        #region "Mouse events"
        private void image1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isDragging == false)
            {
                anchorPoint.X = e.GetPosition(BackPanel).X;
                anchorPoint.Y = e.GetPosition(BackPanel).Y;
                isDragging = true;
            }

        }

        private void image1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                double x = e.GetPosition(BackPanel).X;
                double y = e.GetPosition(BackPanel).Y;
                selectionRectangle.SetValue(Canvas.LeftProperty, Math.Min(x, anchorPoint.X));
                selectionRectangle.SetValue(Canvas.TopProperty, Math.Min(y, anchorPoint.Y));
                selectionRectangle.Width = Math.Abs(x - anchorPoint.X);
                selectionRectangle.Height = Math.Abs(y - anchorPoint.Y);

                if (selectionRectangle.Visibility != Visibility.Visible)
                    selectionRectangle.Visibility = Visibility.Visible;
            }
        }

        private void image1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                if (selectionRectangle.Width > 0)
                {
                    Go.Visibility = System.Windows.Visibility.Visible;
                    Go.IsEnabled = true;
                }
                if (selectionRectangle.Visibility != Visibility.Visible)
                    selectionRectangle.Visibility = Visibility.Visible;
            }
        }

        private void RestRect()
        {
            selectionRectangle.Visibility = Visibility.Collapsed;
            isDragging = false;
        }

        #endregion



        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
           // Window1 win1 = new Window1(this);
           // win1.image.Source = image2.Source;
           // win1.Show();
        }




    }
}
