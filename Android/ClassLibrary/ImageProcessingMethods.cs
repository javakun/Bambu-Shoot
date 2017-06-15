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

namespace ClassLibrary
{
    public class ImageProcessingMethods
    {
        Bitmap resultingfilter;
        Bitmap[] segmentos;

        int[] segmentcounts;
        double[] densitysegs;
        double totaldensity;

        // Color Filtering Method
        public Bitmap BWandGrayScaleFiltering(String filepath, int threshold, bool colorfiltermode)
        {
            // Default Threshold value
            int thresholdDef;
            if (threshold == 0)
                thresholdDef = 165;
            else
                thresholdDef = threshold;

            //Read Image File
            Mat img = CvInvoke.Imread(filepath , Emgu.CV.CvEnum.ImreadModes.AnyColor);
            int width = img.Width;
            int height = img.Height;
            Mat dest = new Mat();
            Mat dest3 = new Mat();
            Mat dest2 = new Mat();

            //Decolor Image to create Grayscale
            CvInvoke.Decolor(img, dest, dest2);
            //Threshold Image to create BW
            CvInvoke.Threshold(dest, dest3, thresholdDef, 255, 0);

            Image<Gray, byte> imggray = dest.ToImage<Gray, Byte>();
            Image<Gray, byte> imgbw = dest3.ToImage<Gray, Byte>();

            //Return image by filter selected
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

        //Fiber Density Method 
        public ImgProcessData FiberDensity(String filepath, bool filter)
        {
            segmentos = new Bitmap[11];
            segmentcounts = new int[11];
            densitysegs = new double[11];
          
            Mat imgsrc = CvInvoke.Imread(filepath, Emgu.CV.CvEnum.ImreadModes.Grayscale);

            int count, count1, count2, count3, count4, count5, count6, count7, count8, count9;
            int countf;
            totaldensity = 0;
            int width = imgsrc.Width;
            int height = imgsrc.Height;
            int slice = width / 10;
         
            Mat dest = new Mat();
            Mat dest3 = new Mat();
            Mat dest2 = new Mat();

            // Create Segments
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

            if (filter)
            {
                //Grayscale image. Use AdaptiveThreshold 
                CvInvoke.AdaptiveThreshold(imgsrc, dest3, 255, Emgu.CV.CvEnum.AdaptiveThresholdType.MeanC, Emgu.CV.CvEnum.ThresholdType.Binary, 251, 1);
            }
            else
            {
                // Black and white image
                dest3 = imgsrc;
            }

            //Count White pixels and substract from the total image size  = Total Fiber Count
            countf = CvInvoke.CountNonZero(dest3);
            countf = imgsrc.Total.ToInt32() - countf;

            
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

            //Counts the # of white pixels - Segment size = Count of the segment
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

            segmentcounts[1] = imgseg.Total.ToInt32() - count;
            segmentcounts[2] = imgseg1.Total.ToInt32() - count1;
            segmentcounts[3] = imgseg2.Total.ToInt32() - count2;
            segmentcounts[4] = imgseg3.Total.ToInt32() - count3;
            segmentcounts[5] = imgseg4.Total.ToInt32() - count4;
            segmentcounts[6] = imgseg5.Total.ToInt32() - count5;
            segmentcounts[7] = imgseg6.Total.ToInt32() - count6;
            segmentcounts[8] = imgseg7.Total.ToInt32() - count7;
            segmentcounts[9] = imgseg8.Total.ToInt32() - count8;
            segmentcounts[10] = imgseg9.Total.ToInt32() - count9;

            //Calculate the density: Seg Count / Seg Size
            densitysegs[1] = (double) segmentcounts[1] / (canto.Height * canto.Width);
            densitysegs[2] = (double) segmentcounts[2] / (canto1.Height * canto1.Width);
            densitysegs[3] = (double) segmentcounts[3] / (canto2.Height * canto2.Width);
            densitysegs[4] = (double) segmentcounts[4] / (canto3.Height * canto3.Width);
            densitysegs[5] = (double) segmentcounts[5] / (canto4.Height * canto4.Width);
            densitysegs[6] = (double) segmentcounts[6] / (canto5.Height * canto5.Width);
            densitysegs[7] = (double) segmentcounts[7] / (canto6.Height * canto6.Width);
            densitysegs[8] = (double) segmentcounts[8] / (canto7.Height * canto7.Width);
            densitysegs[9] = (double) segmentcounts[9] / (canto8.Height * canto8.Width);
            densitysegs[10] = (double) segmentcounts[10] / (canto9.Height * canto9.Width);

            //Calulate total density: Total Count/ Image Size
            totaldensity = (double) countf / (imgsrc.Height * imgsrc.Width);

            //Pass Data by Model
            ImgProcessData Alldata = new ImgProcessData()
            {
                Segments = segmentos,
                SegmentCounts = segmentcounts,
                Height = height,
                Width = width,
                Countfinal = countf,
                FiberDensityTotal = totaldensity,
                FiberDensitySegs = densitysegs
            };
            return Alldata;
        }


    }

}