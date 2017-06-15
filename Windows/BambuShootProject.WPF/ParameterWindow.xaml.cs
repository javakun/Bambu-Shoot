using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
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
    /// Interaction logic for ParameterWindow.xaml
    /// </summary>
    public partial class ParameterWindow : Window
    {

        Image<Bgr, byte> image1;


        string imageTitle, location, nameOfSpecies, dateOfHarvest;
        private MainWindow mainWindow;

        //Load button event that opens a file dialog to choose picture to process and enables the next button
        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openPic = new OpenFileDialog();
            if (openPic.ShowDialog() == true)
            {
                Image<Bgr, byte> imgsrc = new Image<Bgr, byte>(openPic.FileName);
                image1 = imgsrc;
               
                image.Source = BitmapSourceConvert.ToBitmapSource(imgsrc);
                
                
            }
            button1.IsEnabled = true;
        }


        //Window Constructor 
        public ParameterWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            button1.IsEnabled = false;
        }

        //Next button event that passes all parameters entered 
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text ==null  ||  String.IsNullOrEmpty(textBox2.Text) || String.IsNullOrEmpty(textBox3.Text) || String.IsNullOrEmpty(datePicker.Text))
            {
                MessageBox.Show(" Please fill the parameters");
            }
            else
            { 
                imageTitle = textBox1.Text;
                location = textBox2.Text;
                nameOfSpecies = textBox3.Text;
                dateOfHarvest = datePicker.Text;
                string temp = "C:/Users/Public/Pictures/BambuShoot/" + imageTitle;
                if (!Directory.Exists(temp))
                    Directory.CreateDirectory(temp);
                image1.Save(temp + "/" + imageTitle + ".png");
                this.Hide();
                CalibrationWindow cal = new CalibrationWindow(this);
                cal.image1.Source = image.Source;
                cal.location.Text = location;
                cal.nameOfSpecies.Text = nameOfSpecies;
                cal.dateOfHarvest.Text = dateOfHarvest;
                cal.imageTitle.Text = imageTitle;

                cal.Show();
            }


        }




    }
}
