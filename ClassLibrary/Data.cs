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
        public int FiberDensityS1 { get; set; }
        public int FiberDensityS2 { get; set; }
        public int FiberDensityS3 { get; set; }
        public int FiberDensityS4 { get; set; }
        public int FiberDensityS5 { get; set; }
        public int FiberDensityS6 { get; set; }
        public int FiberDensityS7 { get; set; }
        public int FiberDensityS8 { get; set; }
        public int FiberDensityS9 { get; set; }
        public int FiberDensityS10 { get; set; }
        //Total FiberDensity of Image: totalSegCount/(imageheight*imagewidth)
        public int TotalFiberDensity { get; set; }


    }

    public class ImgProcessData
    {
        //public byte[] Segment1 { get; set; }
        //public byte[] Segment2 { get; set; }
        //public byte[] Segment3 { get; set; }
        //public byte[] Segment4 { get; set; }
        //public byte[] Segment5 { get; set; }
        //public byte[] Segment6 { get; set; }
        //public byte[] Segment7 { get; set; }
        //public byte[] Segment8 { get; set; }
        //public byte[] Segment9 { get; set; }
        //public byte[] Segment10 { get; set; }

        public Bitmap bmp { get; set; }
        public Bitmap[] Segments { get; set; }
        public int[] SegmentCounts { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Countfinal { get; set; }
        public int FiberDensityTotal { get; set; }

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