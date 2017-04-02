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
using Newtonsoft.Json;

namespace BambuShootProject.Droid
{
    [Activity(Label = "InputCalibration")]
    public class InputCalibration : Activity
    {
        ImageView LoadedImage;
        TextView test;
        Bitmap loadedbmp;
        ClassLibrary.ImageInfoandCalibration previousimageinfo;
        String filepath;

        protected override void OnCreate(Bundle bundle)
        {
           

            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.InputCalibration);

            previousimageinfo = JsonConvert.DeserializeObject<ClassLibrary.ImageInfoandCalibration>(Intent.GetStringExtra("imageinfo"));
            LoadedImage = FindViewById<ImageView>(Resource.Id.loadedimageview);
            test = FindViewById<TextView>(Resource.Id.testingdata);
            
            filepath = Intent.GetStringExtra("imagefile");

            loadedbmp = BitmapFactory.DecodeFile(filepath);
            LoadedImage.SetImageBitmap(loadedbmp);
            test.Text = filepath;


        }
    }
}