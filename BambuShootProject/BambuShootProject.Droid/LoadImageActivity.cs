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


namespace BambuShootProject.Droid
{
    [Activity(Label = "LoadImageActivity")]
    public class LoadImageActivity : Activity
    {
        Button addImageBtn;
        ImageView originalImage;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.LoadImage);
            
            addImageBtn = FindViewById<Button>(Resource.Id.addImageBtn);
            originalImage = FindViewById<ImageView>(Resource.Id.imageView1);
            // var switchSize = FindViewById<Switch>(Resource.Id.switch_size);

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
                    var bitmap = BitmapFactory.DecodeFile(file.Path);
                    originalImage.SetImageBitmap(bitmap);
                    file.Dispose();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }

            };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}