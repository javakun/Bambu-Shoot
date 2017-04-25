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
        DatePicker dateofharvest;
        TextView NoImageSelected;
        bool ImagePicked = false;

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
            dateofharvest = FindViewById<DatePicker>(Resource.Id.datePicker1);

            originalImage = FindViewById<ImageView>(Resource.Id.imageView1);
            NoImageSelected = FindViewById<TextView>(Resource.Id.Noimageselected);
          
            loadImageBtn.Click += LoadImageBtn_Click;

            addImageBtn.Click += async (sender, args) =>
            {
                try
                {
                    var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync();
                    if (file == null)
                        return;
                    var path = file.Path;
                    Toast.MakeText(this, path, ToastLength.Long).Show();
                    System.Diagnostics.Debug.WriteLine(path);
                    bitmap = BitmapFactory.DecodeFile(file.Path);
                    originalImage.SetImageBitmap(bitmap);
                    filepath = file.Path;
                    file.Dispose();
                    ImagePicked = true;
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }

            };


        }
        private bool verifyData(ClassLibrary.ImageInfoandCalibration data)
        {
            
            bool somethingempty = false;
            //Check for empty edit text
            if(data.username.Length == 0 )
            {
                username.FindFocus();
                username.Error = "Empty Username";
                somethingempty = true;
            }
            if (data.imagetitle.Length == 0 )
            {
                imagetitle.FindFocus();
                imagetitle.Error = "Empty Image Title";
                somethingempty = true;
            }
            if (data.location.Length == 0 )
            {
                location.FindFocus();
                location.Error = "Empty Location";
                somethingempty = true;
            }
            if (data.nameofspecies.Length == 0)
            {
                nameofspecies.FindFocus();
                nameofspecies.Error = "Empty Name of Species";
                somethingempty = true;
            }
            if(!ImagePicked)
            {
                NoImageSelected.Visibility = ViewStates.Visible ;
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

            ClassLibrary.ImageInfoandCalibration imageinfo = new ClassLibrary.ImageInfoandCalibration()
            {
                username = username.Text,
                imagetitle = imagetitle.Text,
                location = location.Text,
                nameofspecies = nameofspecies.Text,
                dateofharvest = dateofharvest.DayOfMonth.ToString() + "-" + dateofharvest.Month.ToString() + "-" + dateofharvest.Year.ToString(),
                originalimagefilepath = dest + "/" + imagetitle.Text + "_original.bmp" ,
                editedimagefilepath = dest + "/" + imagetitle.Text + "_edited.bmp"

            };
            if (verifyData(imageinfo) == false && ImagePicked)
            { 
                System.IO.File.Copy(filepath, System.IO.Path.Combine(dest, imagetitle.Text + "_original.bmp"), true);
                System.IO.File.Copy(filepath, System.IO.Path.Combine(dest, imagetitle.Text + "_edited.bmp"), true);
                intent.PutExtra("Imageinfo", JsonConvert.SerializeObject(imageinfo));
                this.StartActivity(intent);
            }
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