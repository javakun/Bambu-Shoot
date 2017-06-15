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
using Android.Content.PM;
using Java.IO;
using System.IO;
using Newtonsoft.Json;
using Plugin.ShareFile;
using Android.Preferences;
using Android.Provider;
using Android.Util;

namespace com.BambuShoot.droid
{
    [Activity(Label = "Load Image", ConfigurationChanges = ConfigChanges.Locale | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoadImageActivity : Activity
    {
        Button addImageBtn;
        Button loadImageBtn;
        ImageView originalImage;
        Bitmap bitmap;
        EditText imagetitle;
        EditText location;
        EditText nameofspecies;
        DatePicker dateofharvest;
        TextView NoImageSelected;
        Android.Net.Uri uri;

        bool ImagePicked = false;
        ClassLibrary.Reports imageinfo;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.LoadImage);

            addImageBtn = FindViewById<Button>(Resource.Id.addImageBtn);
            loadImageBtn = FindViewById<Button>(Resource.Id.loadimageBtn);
            imagetitle = FindViewById<EditText>(Resource.Id.editText_Imagetitle);
            location = FindViewById<EditText>(Resource.Id.editText_location);
            nameofspecies = FindViewById<EditText>(Resource.Id.editText_Nameofspecies);
            dateofharvest = FindViewById<DatePicker>(Resource.Id.datePicker1);
            originalImage = FindViewById<ImageView>(Resource.Id.imageView1);
            NoImageSelected = FindViewById<TextView>(Resource.Id.Noimageselected);

            loadImageBtn.Click += LoadImageBtn_Click;


            addImageBtn.Click += (sender, args) =>
           {

               Intent imagecrop = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
               imagecrop.PutExtra("crop", "true");
               imagecrop.PutExtra("return-data", true);

               String newpath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot";
               Directory.CreateDirectory(newpath);

               Java.IO.File f = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot/temporary_holder.bmp");
               try
               {
                   f.CreateNewFile();
               }
               catch (Exception ex)
               {
                   System.Console.Write(ex);
               }

               uri = Android.Net.Uri.FromFile(f);
               imagecrop.PutExtra(MediaStore.ExtraOutput, uri);
               this.StartActivityForResult(Intent.CreateChooser(imagecrop, "Selecte a Photo"),1);
              
           };


        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if(resultCode == Result.Ok && data != null && requestCode == 1)
            {
                String filePath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot/temporary_holder.bmp";
                bitmap= BitmapFactory.DecodeFile(filePath);
                originalImage.SetImageBitmap(bitmap);
                ImagePicked = true;

            }
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (originalImage != null)
            {
                originalImage.SetImageBitmap(null);
                originalImage.Dispose();
                originalImage = null;
            }
            if (bitmap != null)
            {
                bitmap.Recycle();
                bitmap.Dispose();
                bitmap = null;
            }
        }

        private bool verifyData(ClassLibrary.Reports reportsdata)
        {

            bool somethingempty = false;
            //Check for empty edit text
            if (reportsdata.Imagetitle.Length == 0)
            {
                imagetitle.FindFocus();
                imagetitle.Error = "Empty Image Title";
                somethingempty = true;
            }
            if (reportsdata.Location.Length == 0)
            {
                location.FindFocus();
                location.Error = "Empty Location";
                somethingempty = true;
            }
            if (reportsdata.Nameofspecies.Length == 0)
            {
                nameofspecies.FindFocus();
                nameofspecies.Error = "Empty Name of Species";
                somethingempty = true;
            }
            if (!ImagePicked)
            {
                NoImageSelected.Visibility = ViewStates.Visible;
                somethingempty = true;
            }
            else
            {
                NoImageSelected.Visibility = ViewStates.Gone;
            }

            return somethingempty;

        }
        private void LoadImageBtn_Click(object sender, EventArgs e)
        {

            Intent intent = new Intent(this, typeof(InputCalibration));
            var dest = MakeNewFileDestination(imagetitle.Text);

            imageinfo = new ClassLibrary.Reports()
            {
                Imagetitle = imagetitle.Text,
                Location = location.Text,
                Nameofspecies = nameofspecies.Text,
                Dateofharvest = dateofharvest.DayOfMonth.ToString() + "-" + dateofharvest.Month.ToString() + "-" + dateofharvest.Year.ToString(),
                Originalimagefilepath = dest + "/" + imagetitle.Text + "_original.bmp",
                Editedimagefilepath = dest + "/" + imagetitle.Text + "_edited.bmp"

            };
            if (verifyData(imageinfo) == false && ImagePicked)
            {
                FileStream original = new FileStream(System.IO.Path.Combine(dest, imagetitle.Text + "_original.bmp"), FileMode.Create);
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, original);
                original.Flush();
                original.Close();

                FileStream edited = new FileStream(System.IO.Path.Combine(dest, imagetitle.Text + "_edited.bmp"), FileMode.Create);
                bitmap.Compress(Bitmap.CompressFormat.Png, 100, edited);
                edited.Flush();
                edited.Close();

                intent.PutExtra("Imageinfo", JsonConvert.SerializeObject(imageinfo));
                this.StartActivity(intent);
                Finish();
             
            }
        }
        public String MakeNewFileDestination(String imagetitle)
        {
            String newpath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot";
            Directory.CreateDirectory(newpath + "/" + imagetitle);
            String destination = newpath + "/" + imagetitle;

            string filename1 = newpath + "/" + "LibraryListImages.txt";
            string filename2 = newpath + "/" + "LibraryListPDFs.txt";
            string filename3 = newpath + "/" + "LibraryListImagetitles.txt";

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            var savedfile = prefs.GetBoolean("key_for_my_bool_value", false);
            if (savedfile)
            {
                System.IO.File.AppendText(filename1);
                System.IO.File.AppendText(filename2);
                System.IO.File.AppendText(filename3);
            }
            else
            {
                System.IO.File.CreateText(filename1);
                System.IO.File.CreateText(filename2);
                System.IO.File.CreateText(filename3);
            }


            return destination;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}