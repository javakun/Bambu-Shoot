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
        public string Id { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

    }

    public class Reports
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "userid")]
        public string Userid { get; set; }

        [JsonProperty(PropertyName = "imagetitle")]
        public string Imagetitle { get; set; }

        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "nameofspecies")]
        public string Nameofspecies { get; set; }

        [JsonProperty(PropertyName = "dateofharvest")]
        public string Dateofharvest { get; set; }

        public string Originalimagefilepath { get; set; }
        public string Editedimagefilepath { get; set; }

        [JsonProperty(PropertyName = "imagewidth")]
        public int Imagewidth { get; set; }

        [JsonProperty(PropertyName = "imageheight")]
        public int Imageheight { get; set; }

        [JsonProperty(PropertyName = "filter")]
        public string Filter { get; set; }

        //Value for image transform 0-255
        [JsonProperty(PropertyName = "threshold")]
        public int Threshold { get; set; }

        //Amount of Black Pixels(Fibers)
        [JsonProperty(PropertyName = "countS1")]
        public int CountS1 { get; set; }
        [JsonProperty(PropertyName = "countS2")]
        public int CountS2 { get; set; }
        [JsonProperty(PropertyName = "countS3")]
        public int CountS3 { get; set; }
        [JsonProperty(PropertyName = "countS4")]
        public int CountS4 { get; set; }
        [JsonProperty(PropertyName = "countS5")]
        public int CountS5 { get; set; }
        [JsonProperty(PropertyName = "countS6")]
        public int CountS6 { get; set; }
        [JsonProperty(PropertyName = "countS7")]
        public int CountS7 { get; set; }
        [JsonProperty(PropertyName = "countS8")]
        public int CountS8 { get; set; }
        [JsonProperty(PropertyName = "countS9")]
        public int CountS9 { get; set; }
        [JsonProperty(PropertyName = "countS10")]
        public int CountS10 { get; set; }
        //Sum of All Segments
        [JsonProperty(PropertyName = "totalSegCount")]
        public int TotalSegCount { get; set; }
        //Count of seg / Count de complete image
        [JsonProperty(PropertyName = "fDS1total")]
        public double FiberDensityS1Total { get; set; }
        [JsonProperty(PropertyName = "fDS2total")]
        public double FiberDensityS2Total { get; set; }
        [JsonProperty(PropertyName = "fDS3total")]
        public double FiberDensityS3Total { get; set; }
        [JsonProperty(PropertyName = "fDS4total")]
        public double FiberDensityS4Total { get; set; }
        [JsonProperty(PropertyName = "fDS5total")]
        public double FiberDensityS5Total { get; set; }
        [JsonProperty(PropertyName = "fDS6total")]
        public double FiberDensityS6Total { get; set; }
        [JsonProperty(PropertyName = "fDS7total")]
        public double FiberDensityS7Total { get; set; }
        [JsonProperty(PropertyName = "fDS8total")]
        public double FiberDensityS8Total { get; set; }
        [JsonProperty(PropertyName = "fDS9total")]
        public double FiberDensityS9Total { get; set; }
        [JsonProperty(PropertyName = "fDS10total")]
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
        [JsonProperty(PropertyName = "totalFiberDensity")]
        public double TotalFiberDensity { get; set; }


    }

    public class ReportsLib
    {
        public string imagetitle { get; set; }
        public string pdfpath { get; set; }
        public string originalimagepath { get; set; }
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
}