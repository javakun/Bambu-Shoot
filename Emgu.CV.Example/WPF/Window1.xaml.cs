//----------------------------------------------------------------------------
//  Copyright (C) 2004-2016 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using System.Drawing;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;


namespace Emgu.CV.WPF
{
   /// <summary>
   /// Interaction logic for Window1.xaml
   /// </summary>
   public partial class Window1 : Window
   {
      public Window1()
      {
         InitializeComponent();
      }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openPic = new OpenFileDialog();
            if (openPic.ShowDialog() == true)
            {
                Image<Bgr, byte> imgsrc = new Image<Bgr, byte>(openPic.FileName);
                myImage.Source = Emgu.CV.WPF.BitmapSourceConvert.ToBitmapSource(imgsrc);
                int width = imgsrc.Width;
                int height = imgsrc.Height;
                Mat mat = imgsrc.Mat;
                Mat dest = new Mat();
                Mat dest3 = new Mat();
                Mat dest2 = new Mat();
                System.Drawing.Rectangle seg1 = new System.Drawing.Rectangle(0, 0, width/10, height);
              
                
                CvInvoke.Decolor(mat, dest, dest2);
                CvInvoke.Threshold(dest, dest3, 165,255, 0);
                Mat dest4 = new Mat(dest3, seg1);


                Image<Gray, byte> imggray = dest2.ToImage<Gray, Byte>();
                Image<Gray, byte> imgbw = dest4.ToImage<Gray, Byte>();
                myGreyImage.Source = Emgu.CV.WPF.BitmapSourceConvert.ToBitmapSource(imggray);
                mybWImage.Source = Emgu.CV.WPF.BitmapSourceConvert.ToBitmapSource(imgbw);
                int count;
                count = CvInvoke.CountNonZero(dest3);
                BWCount.Text = count.ToString();
                Height.Text = height.ToString();
                Width.Text = width.ToString();
            }
        }
    }
}
