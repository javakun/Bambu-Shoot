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
using Java.Nio.Channels;

namespace BambuShootProject.Droid
{
    [Activity(Label = "LoadImageActivity")]
    public class LoadImageActivity : Activity
    {
        Button addImageBtn;
        Button loadImageBtn;
        ImageView originalImage;
        Bitmap bitmap;
        String filepath;
        EditText username;
        EditText imagetitle;
        EditText location;
        EditText nameofspecies;
        EditText dateofharvest;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.LoadImage);

            addImageBtn = FindViewById<Button>(Resource.Id.addImageBtn);
            loadImageBtn = FindViewById<Button>(Resource.Id.loadimageBtn);
            imagetitle = FindViewById<EditText>(Resource.Id.editText_Imagetitle);
            username = FindViewById<EditText>(Resource.Id.editText_Username);
            location = FindViewById<EditText>(Resource.Id.editText_location);
            nameofspecies = FindViewById<EditText>(Resource.Id.editText_Nameofspecies);
            dateofharvest = FindViewById<EditText>(Resource.Id.editText_dateofharvest);

            originalImage = FindViewById<ImageView>(Resource.Id.imageView1);
            // var switchSize = FindViewById<Switch>(Resource.Id.switch_size);
            loadImageBtn.Click += LoadImageBtn_Click;

            addImageBtn.Click += async (sender, args) =>
            {
                try
                {
                    var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync();
                    //  {
                    //PhotoSize = switchSize.Checked ? Plugin.Media.Abstractions.PhotoSize.Large : Plugin.Media.Abstractions.PhotoSize.Full
                    //  });
                    if (file == null)
                        return;
                    var path = file.Path;
                    Toast.MakeText(this, path, ToastLength.Long).Show();
                    System.Diagnostics.Debug.WriteLine(path);
                    bitmap = BitmapFactory.DecodeFile(file.Path);
                    originalImage.SetImageBitmap(bitmap);
                    filepath = file.Path;
                    file.Dispose();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }

            };


        }

        private void LoadImageBtn_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(InputCalibration));
            ClassLibrary.ImageInfoandCalibration imageinfo = new ClassLibrary.ImageInfoandCalibration()
            {
                username = username.Text,
                imagetitle = imagetitle.Text,
                location = location.Text,
                nameofspecies = nameofspecies.Text,
                dateofharvest = dateofharvest.Text

            };

            var dest = MakeNewFileDestination(imagetitle.Text);
            System.IO.File.Copy(filepath, System.IO.Path.Combine(dest,imagetitle.Text+".bmp"), true);
            intent.PutExtra("imagefile", dest+ imagetitle.Text + ".bmp");
            intent.PutExtra("imageinfo", JsonConvert.SerializeObject(imageinfo));
            this.StartActivity(intent);
        }
        public String MakeNewFileDestination(String imagetitle)
        {
            String newpath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot";
            Directory.CreateDirectory(newpath);
            Directory.CreateDirectory(newpath + "/" + imagetitle);
            String destination = newpath + "/" + imagetitle;
            return destination;
        }
    
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}