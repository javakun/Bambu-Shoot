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
using ClassLibrary;
using Android.Graphics;
using Plugin.ShareFile;
using System.IO;
using Plugin.Media.Abstractions;
using static Android.Graphics.BitmapFactory;

namespace BambuShootProject.Droid
{
    public class ReportListAdapter : BaseAdapter<ReportsLib>
    {
        Activity activity;
        int layoutResourceId;
        List<ReportsLib> reports;
        Bitmap smalloriginalbmp;
        int positioncurrent;

        public ReportListAdapter(Activity activity, int layoutResourceId, List<ReportsLib> listreports)
        {
            this.activity = activity;
            this.layoutResourceId = layoutResourceId;
            reports = listreports;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var row = convertView;
            var currentItem = this[position];

            if (row == null)
            {
                var inflater = activity.LayoutInflater;
                row = inflater.Inflate(layoutResourceId, parent, false);
            }

            positioncurrent = position;
            ImageView Loadedimage = row.FindViewById<ImageView>(Resource.Id.loadedimgshow);
            //Resize Image
            //  smalloriginalbmp = Bitmap.CreateScaledBitmap(BitmapFactory.DecodeFile(reports[position].originalimagepath), 150, 150, false);

            Options bitmapOptions = new Options();
            var inSampleSize = 2;
            bitmapOptions.InSampleSize = 4;
            bitmapOptions.InDensity = 1;
            bitmapOptions.InTargetDensity = 1;
            while (bitmapOptions.InSampleSize * 2 <= inSampleSize)
                bitmapOptions.InSampleSize *= 2;

            smalloriginalbmp = BitmapFactory.DecodeFile(reports[position].originalimagepath, bitmapOptions);
            smalloriginalbmp.Density = Bitmap.DensityNone;
            Loadedimage.SetImageBitmap(smalloriginalbmp);


            TextView Imagetitle = row.FindViewById<TextView>(Resource.Id.imagetitlelistviewtxt);
            Imagetitle.Text = reports[position].imagetitle;


            Button Open = row.FindViewById<Button>(Resource.Id.openpdf);
            Open.Click += Open_Click;
            Button Share = row.FindViewById<Button>(Resource.Id.sharepdf);
            Share.Click += Share_Click;

            Button Delete = row.FindViewById<Button>(Resource.Id.deletereport);
            Delete.Click += Delete_Click;

            return row;

        }

        private void Open_Click(object sender, EventArgs e)
        {
            OpenFile(reports[positioncurrent].pdfpath);
        }
        public void OpenFile(string filePath)
        {

            var bytes = File.ReadAllBytes(filePath);

            //Copy the private file's data to the EXTERNAL PUBLIC location
            string externalStorageState = global::Android.OS.Environment.ExternalStorageState;
            string application = "";

            string extension = System.IO.Path.GetExtension(filePath);

            switch (extension.ToLower())
            {
                case ".doc":
                case ".docx":
                    application = "application/msword";
                    break;
                case ".pdf":
                    application = "application/pdf";
                    break;
                case ".xls":
                case ".xlsx":
                    application = "application/vnd.ms-excel";
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                    application = "image/jpeg";
                    break;
                default:
                    application = "*/*";
                    break;
            }
            var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/report" + extension;
            File.WriteAllBytes(externalPath, bytes);

            Java.IO.File file = new Java.IO.File(externalPath);
            file.SetReadable(true);
            //Android.Net.Uri uri = Android.Net.Uri.Parse("file://" + filePath);
            Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, application);
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

            try
            {
                this.activity.StartActivity(intent);
            }
            catch (Exception)
            {
                Toast.MakeText(this.activity, "No Application Available to View PDF", ToastLength.Short).Show();
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {

            string line = null;
            string filename1 = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot/LibraryListImages.txt";
            string temp1 = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot/LibraryListImagestemp.txt";
            System.IO.File.Copy(filename1, temp1, true);
            var oStream = new FileStream(temp1, FileMode.Create, FileAccess.Write, FileShare.Read);
            var iStream = new FileStream(filename1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (StreamReader reader = new StreamReader(iStream))
            {
                using (StreamWriter writer = new StreamWriter(oStream))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!line.Contains(reports[positioncurrent].originalimagepath))
                        {
                            writer.WriteLine(line);
                        }

                    }
                }
            }
            System.IO.File.Copy(temp1, filename1, true);

            string filename2 = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot/LibraryListPDFs.txt";
            string temp2 = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot/LibraryListPDFstemp.txt";
            System.IO.File.Copy(filename2, temp2, true);
            var oStream2 = new FileStream(temp2, FileMode.Create, FileAccess.Write, FileShare.Read);
            var iStream2 = new FileStream(filename2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (StreamReader reader = new StreamReader(iStream2))
            {
                using (StreamWriter writer = new StreamWriter(oStream2))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!line.Contains(reports[positioncurrent].pdfpath))
                        {
                            writer.WriteLine(line);
                        }
                    }
                }
            }
            System.IO.File.Copy(temp2, filename2, true);

            string filename3 = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot/LibraryListImagetitles.txt";
            string temp3 = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot/LibraryListImagetitlestemp.txt";
            var oStream3 = new FileStream(temp3, FileMode.Create, FileAccess.Write, FileShare.Read);
            var iStream3 = new FileStream(filename3, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (StreamReader reader = new StreamReader(iStream3))
            {
                using (StreamWriter writer = new StreamWriter(oStream3))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!line.Contains(reports[positioncurrent].imagetitle))
                        {
                            writer.WriteLine(line);
                        }
                    }
                }
            }
            System.IO.File.Copy(temp3, filename3, true);
            Directory.Delete(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot/" + reports[positioncurrent].imagetitle, true);
            this.activity.Recreate();
        }

        private void Share_Click(object sender, EventArgs e)
        {
            CrossShareFile.Current.ShareLocalFile(reports[positioncurrent].pdfpath);
        }

        public override ReportsLib this[int position]
        {
            get
            {
                return reports[position];
            }
        }

        public override int Count
        {
            get { return reports.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

    }

}




