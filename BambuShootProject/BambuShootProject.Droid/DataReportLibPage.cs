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
using System.IO;
using Android.Content.PM;

namespace BambuShootProject.Droid
{
    [Activity(Label = "Data Report Library", ConfigurationChanges = ConfigChanges.Locale | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class DataReportLibPage : Activity
    {
        ListView ReportsListView;
        BaseAdapter<ReportsLib> gAdapter;

        List<ReportsLib> Library;
        List<String> listimagetitles;
        List<String> listpdfs;
        List<String> listimages;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DataReportLibrary);

            ReportsListView = FindViewById<ListView>(Resource.Id.librarylistview);
            listimagetitles = new List<String>();
            listpdfs = new List<String>();
            listimages = new List<String>();
            Library = new List<ReportsLib>();
            Library.Capacity = 1000;
            listimagetitles.Capacity = 1000;
            listpdfs.Capacity = 1000;
            listimages.Capacity = 1000;
            listimagetitles.Clear();
            listpdfs.Clear();
            listimages.Clear();
            Library.Clear();
            ReadList();
            if (Library[0].imagetitle != null)
            {
                gAdapter = new ReportListAdapter(this, Resource.Layout.Row_Reports, Library);
                ReportsListView.Adapter = gAdapter;
            }


        }

        public void ReadList()
        {
           
            string filename1 = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot/LibraryListImages.txt";
            using (var streamReader = new StreamReader(filename1))
            {
                for(int i = 0; i < 1000; i++)
                {
                    listimages.Add(streamReader.ReadLine());
                    if (streamReader.EndOfStream)
                    {
                        break;
                    }
                }
            }
            string filename2 = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot/LibraryListPDFs.txt";
            using (var streamReader = new StreamReader(filename2))
            {
                for (int i = 0; i < 1000; i++)
                {
                    listpdfs.Add(streamReader.ReadLine());
                    if (streamReader.EndOfStream)
                    {
                        break;
                    }
                }
            }
            string filename3 = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot/LibraryListImagetitles.txt";
            using (var streamReader = new StreamReader(filename3))
            {
                for (int i = 0; i < 1000; i++)
                {
                    listimagetitles.Add( streamReader.ReadLine());
                    if (streamReader.EndOfStream)
                    {
                        break;
                    }
                }
            }

            for(int y=0;y < listimagetitles.Count; y++)
            {
                Library.Add( new ReportsLib { imagetitle = listimagetitles.ElementAt(y) ,originalimagepath = listimages.ElementAt(y) , pdfpath = listpdfs.ElementAt(y) });
            }


        }


    }
}