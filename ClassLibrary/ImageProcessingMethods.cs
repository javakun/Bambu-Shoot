using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Android.Media;
using System.IO;
using Java.IO;

namespace ClassLibrary
{
    public class ImageProcessingMethods
    {
        Bitmap resultingfilter;
        Bitmap[] segmentos;

        int[] segmentcounts;

        public Bitmap BWandGrayScaleFiltering(String filepath, int threshold, bool colorfiltermode)
        {
            int thresholdDef;
            //BmpImage = BitmapFactory.DecodeFile(filepath);
            if (threshold == 0)
                thresholdDef = 165;
            else
                thresholdDef = threshold;

            //Image<Bgr, byte> imgsrc = new Image<Bgr, byte>(BmpImage);
            Mat img = CvInvoke.Imread(filepath , Emgu.CV.CvEnum.ImreadModes.AnyColor);
            int width = img.Width;
            int height = img.Height;
            Mat dest = new Mat();
            Mat dest3 = new Mat();
            Mat dest2 = new Mat();

            CvInvoke.Decolor(img, dest, dest2);
            CvInvoke.Threshold(dest, dest3, thresholdDef, 255, 0);

            Image<Gray, byte> imggray = dest2.ToImage<Gray, Byte>();
            Image<Gray, byte> imgbw = dest3.ToImage<Gray, Byte>();


            if (colorfiltermode == true)
            {
                
                resultingfilter = imggray.ToBitmap();
            }
            else
            {
             
                resultingfilter = imgbw.ToBitmap();
            }
           
            return resultingfilter;
        }

        public ImgProcessData FiberDensity(String filepath)
        {
            segmentos = new Bitmap[11];
            segmentcounts = new int[11];
          
            Mat imgsrc = CvInvoke.Imread(filepath, Emgu.CV.CvEnum.ImreadModes.Grayscale);

            int countf, count, count1, count2, count3, count4, count5, count6, count7, count8, count9;
            int width = imgsrc.Width;
            int height = imgsrc.Height;
            int slice = width / 10;
         
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


           // CvInvoke.Decolor(imgsrc, dest, dest2);
            CvInvoke.AdaptiveThreshold(imgsrc, dest3, 255, Emgu.CV.CvEnum.AdaptiveThresholdType.MeanC, Emgu.CV.CvEnum.ThresholdType.Binary, 251, 1);

            countf = CvInvoke.CountNonZero(dest3);
         //   Image<Gray, byte> imgbw = dest2.ToImage<Gray, Byte>();

           // blackandwhiteimage = imgbw.ToBitmap();
            // return countf

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


            //Creates the ten Segment images
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


            segmentos[1] = canto.ToBitmap();
            segmentos[2] = canto1.ToBitmap();
            segmentos[3] = canto2.ToBitmap();
            segmentos[4] = canto3.ToBitmap();
            segmentos[5] = canto4.ToBitmap();
            segmentos[6] = canto5.ToBitmap();
            segmentos[7] = canto6.ToBitmap();
            segmentos[8] = canto7.ToBitmap();
            segmentos[9] = canto8.ToBitmap();
            segmentos[10] = canto9.ToBitmap();

            //Counts the % of Fibers
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

            segmentcounts[1] = count;
            segmentcounts[2] = count1;
            segmentcounts[3] = count2;
            segmentcounts[4] = count3;
            segmentcounts[5] = count4;
            segmentcounts[6] = count5;
            segmentcounts[7] = count6;
            segmentcounts[8] = count7;
            segmentcounts[9] = count8;
            segmentcounts[10] = count9;

            ImgProcessData Alldata = new ImgProcessData()
            {
                Segments = segmentos,
                SegmentCounts = segmentcounts,
                Height = height,
                Width = width,
                Countfinal = countf,
                FiberDensityTotal = (countf / (height * width))
            };

            return Alldata;
        }


    }

}