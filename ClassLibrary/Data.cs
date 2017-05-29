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
using Newtonsoft.Json;
using Android.Graphics;

namespace ClassLibrary
{

    public class Users
    {

        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string username { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string password { get; set; }

    }

    public class Reports
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "userid")]
        public string userid { get; set; }

        [JsonProperty(PropertyName = "imagetitle")]
        public string imagetitle { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string location { get; set; }

        [JsonProperty(PropertyName = "nameofspecies")]
        public string nameofspecies { get; set; }

        [JsonProperty(PropertyName = "dateofharvest")]
        public string dateofharvest { get; set; }

        public string originalimagefilepath { get; set; }
        public string editedimagefilepath { get; set; }

        public int imagewidth { get; set; }
        public int imageheigth { get; set; }
        public string filter { get; set; }
        //Value for image transform 0-255
        public int threshold { get; set; }
        //Amount of Black Pixels(Fibers)
        public int countS1 { get; set; }
        public int countS2 { get; set; }
        public int countS3 { get; set; }
        public int countS4 { get; set; }
        public int countS5 { get; set; }
        public int countS6 { get; set; }
        public int countS7 { get; set; }
        public int countS8 { get; set; }
        public int countS9 { get; set; }
        public int countS10 { get; set; }
        //Sum of All Segments
        public int totalSegCount { get; set; }
        //Count of seg / Count de complete image
        public double FiberDensityS1Total { get; set; }
        public double FiberDensityS2Total { get; set; }
        public double FiberDensityS3Total { get; set; }
        public double FiberDensityS4Total { get; set; }
        public double FiberDensityS5Total { get; set; }
        public double FiberDensityS6Total { get; set; }
        public double FiberDensityS7Total { get; set; }
        public double FiberDensityS8Total { get; set; }
        public double FiberDensityS9Total { get; set; }
        public double FiberDensityS10Total { get; set; }

        //Count of seg / Count de segment image
        public double FiberDensityS1 { get; set; }
        public double FiberDensityS2 { get; set; }
        public double FiberDensityS3 { get; set; }
        public double FiberDensityS4 { get; set; }
        public double FiberDensityS5 { get; set; }
        public double FiberDensityS6 { get; set; }
        public double FiberDensityS7 { get; set; }
        public double FiberDensityS8 { get; set; }
        public double FiberDensityS9 { get; set; }
        public double FiberDensityS10 { get; set; }
        //Total FiberDensity of Image: totalSegCount/(imageheight*imagewidth)
        public double TotalFiberDensity { get; set; }


    }

    public class ImgProcessData
    {

        public Bitmap[] Segments { get; set; }
        public int[] SegmentCounts { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Countfinal { get; set; }
        public double FiberDensityTotal { get; set; }
        public double[] FiberDensitySegs { get; set; }

    }

    public class UserDataWrapper : Java.Lang.Object
    {
        public UserDataWrapper (Users item)
        {
            UserDataitem = item;
        }

        public Users UserDataitem { get; private set; }
    }

    public class ReportDataWrapper : Java.Lang.Object
    {
        public ReportDataWrapper(Reports item)
        {
            ReportDataitem = item;
        }

        public Reports ReportDataitem { get; private set; }
    }
}