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
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.IO;

namespace BambuShootProject.WPF
{
    /// <summary>
    /// Interaction logic for ImageProcessWindow.xaml
    /// In this window the user choses and gets a preview of the image processing method to be used in the Report
    /// </summary>
    public partial class ImageProcessWindow : Window
    {
        private CalibrationWindow calibrationWindow;

      
        // Window constructor 
        public ImageProcessWindow(CalibrationWindow calibrationWindow)
        {
            InitializeComponent();
            this.calibrationWindow = calibrationWindow;
        }
     
            
     
      //Preview button event  where it shows you the image with the filters applied  so the user can see before processing
        private void button_Click(object sender, RoutedEventArgs e)
        {
            Image<Bgr, byte> imgsrc = new Image<Bgr, byte>("C:/Users/Public/Pictures/BambuShoot/" + imageTitle.Text + "/edited.bmp");
            Mat mat = imgsrc.Mat;
            Mat bw = new Mat();
            Mat gray = new Mat();
            Mat gray2 = new Mat();
            Mat temp = new Mat();

            if(radioButton_TH.IsChecked == true)
            {
                CvInvoke.Decolor(mat, gray, temp);
                CvInvoke.Threshold(mat, bw, int.Parse(textBox.Text), 255, Emgu.CV.CvEnum.ThresholdType.Binary);
                Image<Gray, byte> imgbw = bw.ToImage<Gray, Byte>();
                image1.Source = BitmapSourceConvert.ToBitmapSource(imgbw);
            }
            else
            {
                CvInvoke.Decolor(mat, gray2, temp);
               
                Image<Gray, byte> imggray = gray2.ToImage<Gray, Byte>();
                image1.Source = BitmapSourceConvert.ToBitmapSource(imggray);
            }



        }

        //Creates the Report Window and passes all the current windows parameter to the next one
        private void process_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ReportWindow rw = new ReportWindow(this);
            rw.pimageTitle.Text = imageTitle.Text;
            rw.pNameOfSpecies.Text = "Name of Species: "+ nameOfSpecies.Text;
            rw.pLocation.Text = "Location: " + location.Text;
            rw.pDateOfHarvest.Text = "Date of Harvest: " + dateOfHarvest.Text;
         
            rw.pThreshold.Text = "Threshold: " + textBox.Text;
            if (radioButton_TH.IsChecked == true)
            {
                rw.pColorFilter.Text ="Color Filter: Black and White Threshold";
            }
            else { rw.pColorFilter.Text = "Color Filter: Grayscale Adaptive Threshold"; }
            rw.Show();
        }

        //Method to show the threshold textbox input when Black and White Threshold is chosen
        private void radioButton_TH_Click(object sender, RoutedEventArgs e)
        {
                textBlock.Visibility = Visibility.Visible;
            textBox.Visibility = Visibility.Visible;
        }

        //Method to hide the threshold textbox input when Grayscale Adaptive Threshold is chosen
        private void radioButton_ATH_Click(object sender, RoutedEventArgs e)
        {
                 textBlock.Visibility = Visibility.Hidden;
            textBox.Visibility = Visibility.Hidden;
        }
    }
}
