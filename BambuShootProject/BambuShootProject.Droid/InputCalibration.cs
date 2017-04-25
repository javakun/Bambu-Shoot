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
        ImageView EditedImage;
        TextView Username;
        TextView Location;
        TextView NameofSpecies;
        TextView DateofHavest;
        TextView imagetitle;
        Bitmap loadedbmp;
        Bitmap editedbmp;
        Button Crop;
        Button Rotate;

        ClassLibrary.ImageInfoandCalibration previousimageinfo;
        
        String originalFilepath,editedFilepath;

        protected override void OnCreate(Bundle bundle)
        {
           

            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.InputCalibration);

            previousimageinfo = JsonConvert.DeserializeObject<ClassLibrary.ImageInfoandCalibration>(Intent.GetStringExtra("Imageinfo"));
            LoadedImage = FindViewById<ImageView>(Resource.Id.loadedimageview);
            EditedImage = FindViewById<ImageView>(Resource.Id.editedimageview);
            imagetitle = FindViewById<TextView>(Resource.Id.imagetitledata);
            Username = FindViewById<TextView>(Resource.Id.testingdata1);
            Location = FindViewById<TextView>(Resource.Id.testingdata2);
            NameofSpecies = FindViewById<TextView>(Resource.Id.testingdata3);
            DateofHavest = FindViewById<TextView>(Resource.Id.testingdata4);

            Crop = FindViewById<Button>(Resource.Id.cropBtn);
            Rotate = FindViewById<Button>(Resource.Id.rotateBtn);

            originalFilepath = previousimageinfo.originalimagefilepath;
            editedFilepath = previousimageinfo.editedimagefilepath;


            loadedbmp = BitmapFactory.DecodeFile(originalFilepath);
            LoadedImage.SetImageBitmap(loadedbmp);
            imagetitle.Text = previousimageinfo.imagetitle;
            Username.Text = previousimageinfo.username;
            Location.Text = previousimageinfo.location;
            NameofSpecies.Text = previousimageinfo.nameofspecies;
            DateofHavest.Text = previousimageinfo.dateofharvest;


            ClassLibrary.ImageProcessingMethods Methods = new ClassLibrary.ImageProcessingMethods();


            editedbmp = Methods.BWandGrayScaleFiltering(editedFilepath);
            //editedbmp = BitmapFactory.DecodeFile(editedFilepath);
            EditedImage.SetImageBitmap(editedbmp);

            Crop.Click += Crop_Click;

        }

        private void Crop_Click(object sender, EventArgs e)
        {
            
        }
    }
}