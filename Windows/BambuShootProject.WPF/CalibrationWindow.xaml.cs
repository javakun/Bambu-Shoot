using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class CalibrationWindow : Window
    {
        double rotateAngle;
        
       
        private bool isDragging = false;
        private Point anchorPoint = new Point();
        private ParameterWindow parameterWindow;
         
        //Window Constuctor that initializes its components and disables the next and crop button to avoid errors
        public CalibrationWindow(ParameterWindow parameterWindow)
        {
            InitializeComponent();
            
            this.parameterWindow = parameterWindow;
            Gridimage1.MouseLeftButtonDown += new MouseButtonEventHandler(image1_MouseLeftButtonDown);
            Gridimage1.MouseMove += new MouseEventHandler(image1_MouseMove);
            Gridimage1.MouseLeftButtonUp += new MouseButtonEventHandler(image1_MouseLeftButtonUp);
            Go.IsEnabled = false;
            Next_Button.IsEnabled = false;
                 

        }
        

        //Crop button event where the box made by the mouse is turned into a rectangle and with that rectangle you crop the image
        private void Go_Click(object sender, RoutedEventArgs e)
        {
            image1.Source = new BitmapImage(new Uri("C:/Users/Public/Pictures/BambuShoot/" + imageTitle.Text + "/RotImage.bmp"));
            if (image1.Source != null)
            {

                Rect rect1 = new Rect(Canvas.GetLeft(selectionRectangle), Canvas.GetTop(selectionRectangle), selectionRectangle.Width, selectionRectangle.Height);
                System.Windows.Int32Rect rcFrom = new System.Windows.Int32Rect();
                rcFrom.X = (int)((rect1.X) * (image1.Source.Width) / (image1.Width));
                rcFrom.Y = (int)((rect1.Y) * (image1.Source.Height) / (image1.Height));
                rcFrom.Width = (int)((rect1.Width) * (image1.Source.Width) / (image1.Width));
                rcFrom.Height = (int)((rect1.Height) * (image1.Source.Height) / (image1.Height));
                BitmapSource bs = new CroppedBitmap(image1.Source as BitmapSource, rcFrom);

                BitmapSource imageSource = bs;


        using (FileStream stream = new FileStream("C:/Users/Public/Pictures/BambuShoot/"+imageTitle.Text+"/edited.bmp", FileMode.Create))
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Interlace = PngInterlaceOption.On;
                    encoder.Frames.Add(BitmapFrame.Create(imageSource));
                    encoder.Save(stream);
                }
       



                image2.Source = bs;
            }
            Next_Button.IsEnabled = true;
            Rotate_Button.IsEnabled = false;
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

        private System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            System.Drawing.Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }
            return bitmap;
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


        //Next button event where the next window is created and the parameters are passed to the next window
        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ImageProcessWindow win1 = new ImageProcessWindow(this);
             win1.image.Source = image2.Source;
            win1.imageTitle.Text = imageTitle.Text;
            win1.nameOfSpecies.Text = nameOfSpecies.Text;
            win1.location.Text = location.Text;
            win1.dateOfHarvest.Text = dateOfHarvest.Text;
             win1.Show();
        }

        //Rotates picture button event by using the render transform of the UI and save the resulting image
        private void Rotate_Button_Click(object sender, RoutedEventArgs e)
        {
            rotateAngle += 90;
            RotateTransform rotateTransform = new RotateTransform(rotateAngle, 175, 165);
            image1.RenderTransform = rotateTransform;

            image1.UpdateLayout();

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)image1.ActualWidth, (int)image1.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            Rect bounds = bounds = VisualTreeHelper.GetDescendantBounds(image1);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext ctx = dv.RenderOpen())
            {
                VisualBrush vb = vb = new VisualBrush(image1);
                ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }
            rtb.Render(dv);

            PngBitmapEncoder png = new PngBitmapEncoder();
            png.Frames.Add(BitmapFrame.Create(rtb));
         
            using (FileStream stream = new FileStream("C:/Users/Public/Pictures/BambuShoot/" + imageTitle.Text + "/RotImage.bmp", FileMode.Create))
            {
                png.Save(stream);
            }
          //  image1.Source = new BitmapImage(new Uri("C:/Users/Public/Pictures/BambuShoot/" + imageTitle.Text + "/RotImage.bmp"));
        }
    }
}
