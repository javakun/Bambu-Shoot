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
using Emgu.CV.Util;
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
                //myImage.Source = Emgu.CV.WPF.BitmapSourceConvert.ToBitmapSource(imgsrc);
                int countf, count, count1, count2, count3, count4, count5, count6, count7, count8, count9;
                int width = imgsrc.Width;
                VectorOfVectorOfPoint contoursDetected = new VectorOfVectorOfPoint();
                int height = imgsrc.Height;
                int slice = width / 10;
                Mat mat = imgsrc.Mat;
                Mat dest = new Mat();
                Mat dest3 = new Mat();
                Mat dest2 = new Mat();
                System.Drawing.Rectangle seg = new System.Drawing.Rectangle(0, 0, slice, height);
                System.Drawing.Rectangle seg1 = new System.Drawing.Rectangle(slice, 0, slice, height);
                System.Drawing.Rectangle seg2 = new System.Drawing.Rectangle(slice * 2, 0, slice, height);
                System.Drawing.Rectangle seg3 = new System.Drawing.Rectangle(slice * 3, 0, slice, height);
                System.Drawing.Rectangle seg4 = new System.Drawing.Rectangle(slice * 4, 0, slice, height);
                System.Drawing.Rectangle seg5 = new System.Drawing.Rectangle(slice * 5, 0, slice, height);
                System.Drawing.Rectangle seg6 = new System.Drawing.Rectangle(slice * 6, 0, slice, height);
                System.Drawing.Rectangle seg7 = new System.Drawing.Rectangle(slice * 7, 0, slice, height);
                System.Drawing.Rectangle seg8 = new System.Drawing.Rectangle(slice * 8, 0, slice, height);
                System.Drawing.Rectangle seg9 = new System.Drawing.Rectangle(slice * 9, 0, slice, height);


                CvInvoke.Decolor(mat, dest, dest2);
               // CvInvoke.Threshold(dest, dest3, 165,255, Emgu.CV.CvEnum.ThresholdType.BinaryInv);
                CvInvoke.AdaptiveThreshold(dest, dest3, 255, Emgu.CV.CvEnum.AdaptiveThresholdType.MeanC, Emgu.CV.CvEnum.ThresholdType.Binary,251,1);
               //CvInvoke.FindContours(dest3, contoursDetected, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                int counter = contoursDetected.Size;
                Image<Gray, byte> imgbw = dest3.ToImage<Gray, Byte>();
                myImage.Source = Emgu.CV.WPF.BitmapSourceConvert.ToBitmapSource(imgbw);
                countf = CvInvoke.CountNonZero(dest3);
                BWCount.Text = counter.ToString();
                Mat imgseg = new Mat(dest3, seg);
                Mat imgseg1 = new Mat(dest3, seg1);
                Mat imgseg2 = new Mat(dest3, seg2);
                Mat imgseg3 = new Mat(dest3, seg3);
                Mat imgseg4 = new Mat(dest3, seg4);
                Mat imgseg5 = new Mat(dest3, seg5);
                Mat imgseg6 = new Mat(dest3, seg6);
                Mat imgseg7 = new Mat(dest3, seg7);
                Mat imgseg8 = new Mat(dest3, seg8);
                Mat imgseg9 = new Mat(dest3, seg9);


                //Image<Gray, byte> imggray = dest2.ToImage<Gray, Byte>();
                Image<Gray, byte> canto = imgseg.ToImage<Gray, Byte>();
                Image<Gray, byte> canto1 = imgseg1.ToImage<Gray, Byte>();
                Image<Gray, byte> canto2 = imgseg2.ToImage<Gray, Byte>();
                Image<Gray, byte> canto3 = imgseg3.ToImage<Gray, Byte>();
                Image<Gray, byte> canto4 = imgseg4.ToImage<Gray, Byte>();
                Image<Gray, byte> canto5 = imgseg5.ToImage<Gray, Byte>();
                Image<Gray, byte> canto6 = imgseg6.ToImage<Gray, Byte>();
                Image<Gray, byte> canto7 = imgseg7.ToImage<Gray, Byte>();
                Image<Gray, byte> canto8 = imgseg8.ToImage<Gray, Byte>();
                Image<Gray, byte> canto9 = imgseg9.ToImage<Gray, Byte>();
               
                image.Source  = Emgu.CV.WPF.BitmapSourceConvert.ToBitmapSource(canto);
                image1.Source = Emgu.CV.WPF.BitmapSourceConvert.ToBitmapSource(canto1);
                image2.Source = Emgu.CV.WPF.BitmapSourceConvert.ToBitmapSource(canto2);
                image3.Source = Emgu.CV.WPF.BitmapSourceConvert.ToBitmapSource(canto3);
                image4.Source = Emgu.CV.WPF.BitmapSourceConvert.ToBitmapSource(canto4);
                image5.Source = Emgu.CV.WPF.BitmapSourceConvert.ToBitmapSource(canto5);
                image6.Source = Emgu.CV.WPF.BitmapSourceConvert.ToBitmapSource(canto6);
                image7.Source = Emgu.CV.WPF.BitmapSourceConvert.ToBitmapSource(canto7);
                image8.Source = Emgu.CV.WPF.BitmapSourceConvert.ToBitmapSource(canto8);
                image9.Source = Emgu.CV.WPF.BitmapSourceConvert.ToBitmapSource(canto9);
        
                
                
                count = CvInvoke.CountNonZero(imgseg);
                count1 = CvInvoke.CountNonZero(imgseg1);
                count2 = CvInvoke.CountNonZero(imgseg2);
                count3 = CvInvoke.CountNonZero(imgseg3);
                count4 = CvInvoke.CountNonZero(imgseg4);
                count5 = CvInvoke.CountNonZero(imgseg5);
                count6 = CvInvoke.CountNonZero(imgseg6);
                count7 = CvInvoke.CountNonZero(imgseg7);
                count8 = CvInvoke.CountNonZero(imgseg8);
                count9 = CvInvoke.CountNonZero(imgseg9);

                
                textBlock.Text = count.ToString();
                textBlock1.Text = count1.ToString();
                textBlock2.Text = count2.ToString();
                textBlock3.Text = count3.ToString();
                textBlock4.Text = count4.ToString();
                textBlock5.Text = count5.ToString();
                textBlock6.Text = count6.ToString();
                textBlock7.Text = count7.ToString();
                textBlock8.Text = count8.ToString();
                textBlock9.Text = count9.ToString();
                Height.Text = height.ToString();
                Width.Text = width.ToString();
            }
        }
    }
}
