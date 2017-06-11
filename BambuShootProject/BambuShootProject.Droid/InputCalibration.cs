using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Newtonsoft.Json;
using Android.Content;
using Java.IO;
using System.IO;
using ClassLibrary;

namespace BambuShootProject.Droid
{
    [Activity(Label = "Input Calibration", ConfigurationChanges = ConfigChanges.Locale | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class InputCalibration : Activity
    {
        ImageView LoadedImage;
        ImageView EditedImage;
        EditText Threshold;
        Bitmap loadedbmp;
        Bitmap editedbmp;
        Button Crop;
        Button Rotate;
        Button Preview;
        Button ProcessImage;
        int thresholdinput;
        Reports previousimageinfo;
        ImageProcessingMethods Methods;
        RadioGroup ColorFilter;
        String originalFilepath, editedFilepath;

        protected override void OnCreate(Bundle bundle)
        {


            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.InputCalibration);

            previousimageinfo = JsonConvert.DeserializeObject<ClassLibrary.Reports>(Intent.GetStringExtra("Imageinfo"));
            LoadedImage = FindViewById<ImageView>(Resource.Id.loadedimageview);
            EditedImage = FindViewById<ImageView>(Resource.Id.editedimageview);
            Threshold = FindViewById<EditText>(Resource.Id.editText_threshold);
            Crop = FindViewById<Button>(Resource.Id.cropBtn);
            Rotate = FindViewById<Button>(Resource.Id.rotateBtn);
            Preview = FindViewById<Button>(Resource.Id.previewBtn);
            ProcessImage = FindViewById<Button>(Resource.Id.processimageBtn);
            ColorFilter = FindViewById<RadioGroup>(Resource.Id.colorfilterRadioGroup);

            originalFilepath = previousimageinfo.Originalimagefilepath;
            editedFilepath = previousimageinfo.Editedimagefilepath;

            loadedbmp = BitmapFactory.DecodeFile(originalFilepath);
            LoadedImage.SetImageBitmap(loadedbmp);
            Methods = new ImageProcessingMethods();
            EditedImage.Visibility = ViewStates.Gone;
            Threshold.Visibility = ViewStates.Invisible;




            Preview.Click += Preview_Click;
            Crop.Click += Crop_Click;
            ProcessImage.Click += ProcessImage_Click;
            ColorFilter.CheckedChange += ColorFilter_CheckedChange;

        }

        protected override void OnDestroy()
        {
            LoadedImage.Dispose();
            if (EditedImage != null)
            {
                EditedImage.Dispose();
            }
            loadedbmp.Dispose();
            if (editedbmp != null)
            {
                editedbmp.Dispose();
            }

            LoadedImage = null;
            EditedImage = null;
            loadedbmp = null;
            editedbmp = null;

        }

        private void ColorFilter_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {

            if (ColorFilter.CheckedRadioButtonId == Resource.Id.BWRadioBtn)
            {
                Threshold.Visibility = ViewStates.Visible;
            }
            else
            {
                Threshold.Visibility = ViewStates.Invisible;
            }
        }

        private void Preview_Click(object sender, EventArgs e)
        {
           
            if (Threshold.Text.Length == 0)
                thresholdinput = 165;
            else
                thresholdinput = int.Parse(Threshold.Text);

            if (ColorFilter.CheckedRadioButtonId == Resource.Id.GSRadioBtn )
            {
                editedbmp = Methods.BWandGrayScaleFiltering(editedFilepath, thresholdinput, true);
                previousimageinfo.Threshold = thresholdinput;
                previousimageinfo.Filter = "GrayScale";
            }
            else
            {
                editedbmp = Methods.BWandGrayScaleFiltering(editedFilepath, thresholdinput, false);
                previousimageinfo.Threshold = thresholdinput;
                previousimageinfo.Filter = "Black & White";
            }
            //Color image Filtering
            EditedImage.SetImageBitmap(editedbmp);
            EditedImage.Visibility = ViewStates.Visible;
        }

        private void ProcessImage_Click(object sender, EventArgs e)
        {
            //Save the Colored filtered Image
            try
            {
                Preview_Click(sender, e);
                FileStream Imagesave = new FileStream(editedFilepath, FileMode.Open);
                editedbmp.Compress(Bitmap.CompressFormat.Png, 100, Imagesave);
                Imagesave.Flush();
                Imagesave.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            Intent intent = new Intent(this, typeof(ReportPage));
            intent.PutExtra("reportdata", JsonConvert.SerializeObject(previousimageinfo));
            this.StartActivity(intent);

        }

        private void Crop_Click(object sender, EventArgs e)
        {

        }

    }
}