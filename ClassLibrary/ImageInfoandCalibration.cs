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

namespace ClassLibrary
{
    public class ImageInfoandCalibration
    {
        public string username { get; set; }
        public string imagetitle { get; set; }
        public string location { get; set; }
        public string nameofspecies { get; set; }
        public string dateofharvest { get; set; }
        public string originalimagefilepath { get; set; }
        public string editedimagefilepath { get; set; }
        public int imagewidth { get; set; }
        public int imageheigth { get; set; }


    }
}