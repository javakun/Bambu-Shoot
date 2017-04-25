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

namespace ClassLibrary
{
    public class ImageProcessingMethods
    {
        Bitmap blackandwhiteimage;
        Bitmap grayscaleimage;
        Bitmap BmpImage;

        public Bitmap BWandGrayScaleFiltering(String filepath)
        {

            //BmpImage = BitmapFactory.DecodeFile(filepath);
           
            //Image<Bgr, byte> imgsrc = new Image<Bgr, byte>(BmpImage);
            Mat img = CvInvoke.Imread(filepath , Emgu.CV.CvEnum.ImreadModes.AnyColor);
            int width = img.Width;
            int height = img.Height;
            Mat dest = new Mat();
            Mat dest3 = new Mat();
            Mat dest2 = new Mat();
            System.Drawing.Rectangle seg1 = new System.Drawing.Rectangle(0, 0, width / 10, height);


            CvInvoke.Decolor(img, dest, dest2);
            CvInvoke.Threshold(dest, dest3, 165, 255, 0);
            Mat dest4 = new Mat(dest3, seg1);


            Image<Gray, byte> imggray = dest2.ToImage<Gray, Byte>();
            Image<Gray, byte> imgbw = dest4.ToImage<Gray, Byte>();
            // myGreyImage.Source = Emgu.CV.WPF.BitmapSourceConvert.ToBitmapSource(imggray);
            // mybWImage.Source = Emgu.CV.WPF.BitmapSourceConvert.ToBitmapSource(imgbw);

            grayscaleimage = imggray.ToBitmap();
            


            return grayscaleimage;
        }

    }
}