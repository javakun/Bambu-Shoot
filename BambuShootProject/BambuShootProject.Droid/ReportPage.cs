using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ClassLibrary;
using Newtonsoft.Json;
using Android.Graphics;

namespace BambuShootProject.Droid
{
    [Activity(Label = "ReportPage", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class ReportPage:Activity
    {
        ImageView LoadedImage;
        Bitmap loadedimgbmp;

        ImageView Segment1;
        Bitmap Seg1;

        ImageView Segment2;
        Bitmap Seg2;
        ImageView Segment3;
        Bitmap Seg3;
        ImageView Segment4;
        Bitmap Seg4;
        ImageView Segment5;
        Bitmap Seg5;
        ImageView Segment6;
        Bitmap Seg6;
        ImageView Segment7;
        Bitmap Seg7;
        ImageView Segment8;
        Bitmap Seg8;
        ImageView Segment9;
        Bitmap Seg9;
        ImageView Segment10;
        Bitmap Seg10;

        Reports FinalReport;
//        ImgProcessData Segmentdata;
        ImageProcessingMethods Methods;

        protected override void OnCreate(Bundle bundle)
        {
            Methods = new ImageProcessingMethods();

            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.DataReportPage);
            FinalReport = JsonConvert.DeserializeObject<Reports>(Intent.GetStringExtra("reportdata"));
            LoadedImage = FindViewById<ImageView>(Resource.Id.loadedimageview);
            Segment1 = FindViewById<ImageView>(Resource.Id.segment1);
            Segment2 = FindViewById<ImageView>(Resource.Id.segment2);
            Segment3 = FindViewById<ImageView>(Resource.Id.segment3);
            Segment4 = FindViewById<ImageView>(Resource.Id.segment4);
            Segment5 = FindViewById<ImageView>(Resource.Id.segment5);
            Segment6 = FindViewById<ImageView>(Resource.Id.segment6);
            Segment7 = FindViewById<ImageView>(Resource.Id.segment7);
            Segment8 = FindViewById<ImageView>(Resource.Id.segment8);
            Segment9 = FindViewById<ImageView>(Resource.Id.segment9);
            Segment10 = FindViewById<ImageView>(Resource.Id.segment10);

            //Set Original Image
            loadedimgbmp = BitmapFactory.DecodeFile(FinalReport.originalimagefilepath);
            LoadedImage.SetImageBitmap(loadedimgbmp);

            ImgProcessData ImgReportdata = new ImgProcessData();
            ImgReportdata = Methods.FiberDensity(FinalReport.editedimagefilepath);

            //Set Segment Images
            Segment1.SetImageBitmap(ImgReportdata.Segments[1]);
            Segment2.SetImageBitmap(ImgReportdata.Segments[2]);
            Segment3.SetImageBitmap(ImgReportdata.Segments[3]);
            Segment4.SetImageBitmap(ImgReportdata.Segments[4]);
            Segment5.SetImageBitmap(ImgReportdata.Segments[5]);
            Segment6.SetImageBitmap(ImgReportdata.Segments[6]);
            Segment7.SetImageBitmap(ImgReportdata.Segments[7]);
            Segment8.SetImageBitmap(ImgReportdata.Segments[8]);
            Segment9.SetImageBitmap(ImgReportdata.Segments[9]);
            Segment10.SetImageBitmap(ImgReportdata.Segments[10]);


        }

    }
}