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
using OxyPlot.Xamarin.Android;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Plugin.ShareFile;
using Android.Graphics.Pdf;
using static Android.Graphics.Pdf.PdfDocument;
using System.IO;
using Microsoft.WindowsAzure.MobileServices;
using Android.Net;

namespace BambuShootProject.Droid
{
    [Activity(Label = "ReportPage", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class ReportPage : Activity
    {
        ImageView LoadedImage;
        ImageView EditedImage;
        Bitmap loadedimgbmp;
        Bitmap editedimgbmp;

        ImageView Segment1;
        ImageView Segment2;
        ImageView Segment3;
        ImageView Segment4;
        ImageView Segment5;
        ImageView Segment6;
        ImageView Segment7;
        ImageView Segment8;
        ImageView Segment9;
        ImageView Segment10;

        Reports FinalReport;
        ImageProcessingMethods Methods;

        TextView imagetitle;
        TextView location;
        TextView nameofspecies;
        TextView dateofharvest;
        TextView imagewidth;
        TextView imageheight;
        TextView threshold;
        TextView colorfilter;

        TextView seg1fcount;
        TextView seg1density;
        TextView seg2fcount;
        TextView seg2density;
        TextView seg3fcount;
        TextView seg3density;
        TextView seg4fcount;
        TextView seg4density;
        TextView seg5fcount;
        TextView seg5density;
        TextView seg6fcount;
        TextView seg6density;
        TextView seg7fcount;
        TextView seg7density;
        TextView seg8fcount;
        TextView seg8density;
        TextView seg9fcount;
        TextView seg9density;
        TextView seg10fcount;
        TextView seg10density;

        TextView seg1densitytotal;
        TextView seg2densitytotal;
        TextView seg3densitytotal;
        TextView seg4densitytotal;
        TextView seg5densitytotal;
        TextView seg6densitytotal;
        TextView seg7densitytotal;
        TextView seg8densitytotal;
        TextView seg9densitytotal;
        TextView seg10densitytotal;
        TextView TotalCount;
        TextView TotalDensity;
        PlotView view;

        Button Save;
        Button Share;
        Button DBAdd;
        string reportfilepath;
        bool isOnline;
        const string applicationURL = @"https://bambushoot.azurewebsites.net";
        Users RegisteredUser;


        // Create the client instance, using the mobile app backend URL.
        private MobileServiceClient client;

        private MobileServiceCollection<Reports, Reports> ReportsTableItems;
        private IMobileServiceTable<Reports> ReportsTable;

        protected override void OnCreate(Bundle bundle)
        {
            Methods = new ImageProcessingMethods();
            
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.DataReportPage);

            FinalReport = JsonConvert.DeserializeObject<Reports>(Intent.GetStringExtra("reportdata"));
            LoadedImage = FindViewById<ImageView>(Resource.Id.loadedimageview);
            EditedImage = FindViewById<ImageView>(Resource.Id.editedimageview);
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

            Save = FindViewById<Button>(Resource.Id.saveinternal);
            Share = FindViewById<Button>(Resource.Id.share);
            DBAdd = FindViewById<Button>(Resource.Id.databaseadd);

            //Set Original Image
            loadedimgbmp = BitmapFactory.DecodeFile(FinalReport.Originalimagefilepath);
            LoadedImage.SetImageBitmap(loadedimgbmp);

            //Set Edited Image
            editedimgbmp = BitmapFactory.DecodeFile(FinalReport.Editedimagefilepath);
            EditedImage.SetImageBitmap(editedimgbmp);

            ImgProcessData ImgReportdata = new ImgProcessData();
            ImgReportdata = Methods.FiberDensity(FinalReport.Editedimagefilepath);
            FinalReport.Imagewidth = ImgReportdata.Width;
            FinalReport.Imageheight = ImgReportdata.Height;

            FinalReport.CountS1 = ImgReportdata.SegmentCounts[1];
            FinalReport.CountS2 = ImgReportdata.SegmentCounts[2];
            FinalReport.CountS3 = ImgReportdata.SegmentCounts[3];
            FinalReport.CountS4 = ImgReportdata.SegmentCounts[4];
            FinalReport.CountS5 = ImgReportdata.SegmentCounts[5];
            FinalReport.CountS6 = ImgReportdata.SegmentCounts[6];
            FinalReport.CountS7 = ImgReportdata.SegmentCounts[7];
            FinalReport.CountS8 = ImgReportdata.SegmentCounts[8];
            FinalReport.CountS9 = ImgReportdata.SegmentCounts[9];
            FinalReport.CountS10 = ImgReportdata.SegmentCounts[10];

            FinalReport.TotalSegCount = ImgReportdata.Countfinal;
            FinalReport.TotalFiberDensity = ImgReportdata.FiberDensityTotal;

            FinalReport.FiberDensityS1Total = Math.Round((double) FinalReport.CountS1 / (FinalReport.Imageheight*FinalReport.Imagewidth), 6);
            FinalReport.FiberDensityS2Total = Math.Round((double)FinalReport.CountS2 / (FinalReport.Imageheight * FinalReport.Imagewidth), 6);
            FinalReport.FiberDensityS3Total = Math.Round((double)FinalReport.CountS3 / (FinalReport.Imageheight * FinalReport.Imagewidth), 6);
            FinalReport.FiberDensityS4Total = Math.Round((double)FinalReport.CountS4 / (FinalReport.Imageheight * FinalReport.Imagewidth), 6);
            FinalReport.FiberDensityS5Total = Math.Round((double)FinalReport.CountS5 / (FinalReport.Imageheight * FinalReport.Imagewidth), 6);
            FinalReport.FiberDensityS6Total = Math.Round((double)FinalReport.CountS6 / (FinalReport.Imageheight * FinalReport.Imagewidth), 6);
            FinalReport.FiberDensityS7Total = Math.Round((double)FinalReport.CountS7 / (FinalReport.Imageheight * FinalReport.Imagewidth), 6);
            FinalReport.FiberDensityS8Total = Math.Round((double)FinalReport.CountS8 / (FinalReport.Imageheight * FinalReport.Imagewidth), 6);
            FinalReport.FiberDensityS9Total = Math.Round((double)FinalReport.CountS9 / (FinalReport.Imageheight * FinalReport.Imagewidth), 6);
            FinalReport.FiberDensityS10Total = Math.Round((double)FinalReport.CountS10 / (FinalReport.Imageheight * FinalReport.Imagewidth), 6);

            FinalReport.FiberDensityS1 = Math.Round((double)ImgReportdata.FiberDensitySegs[1], 6);
            FinalReport.FiberDensityS2 = Math.Round((double)ImgReportdata.FiberDensitySegs[2], 6);
            FinalReport.FiberDensityS3 = Math.Round((double)ImgReportdata.FiberDensitySegs[3], 6);
            FinalReport.FiberDensityS4 = Math.Round((double)ImgReportdata.FiberDensitySegs[4], 6);
            FinalReport.FiberDensityS5 = Math.Round((double)ImgReportdata.FiberDensitySegs[5], 6);
            FinalReport.FiberDensityS6 = Math.Round((double)ImgReportdata.FiberDensitySegs[6], 6);
            FinalReport.FiberDensityS7 = Math.Round((double)ImgReportdata.FiberDensitySegs[7], 6);
            FinalReport.FiberDensityS8 = Math.Round((double)ImgReportdata.FiberDensitySegs[8], 6);
            FinalReport.FiberDensityS9 = Math.Round((double)ImgReportdata.FiberDensitySegs[9], 6);
            FinalReport.FiberDensityS10 = Math.Round((double)ImgReportdata.FiberDensitySegs[10], 6);

            view = FindViewById<PlotView>(Resource.Id.plot_view);
            view.Model = CreatePlotModel();
            view.Controller = ControlPlotModel();

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

            //Set Information

            imagetitle = FindViewById<TextView>(Resource.Id.imagetitlebar);
            location = FindViewById<TextView>(Resource.Id.locationinfo);
            nameofspecies = FindViewById<TextView>(Resource.Id.nameofspeciesinfo);
            dateofharvest = FindViewById<TextView>(Resource.Id.dateofharvestinfo);
            imagewidth = FindViewById<TextView>(Resource.Id.imagewidthinfo);
            imageheight = FindViewById<TextView>(Resource.Id.imageheightinfo);
            threshold = FindViewById<TextView>(Resource.Id.thresholdinfo);
            colorfilter = FindViewById<TextView>(Resource.Id.colorfilterinfo);

            seg1fcount = FindViewById<TextView>(Resource.Id.fibercountseg1);
            seg1density = FindViewById<TextView>(Resource.Id.fiberdensityseg1);
            seg2fcount = FindViewById<TextView>(Resource.Id.fibercountseg2);
            seg2density = FindViewById<TextView>(Resource.Id.fiberdensityseg2);
            seg3fcount = FindViewById<TextView>(Resource.Id.fibercountseg3);
            seg3density = FindViewById<TextView>(Resource.Id.fiberdensityseg3);
            seg4fcount = FindViewById<TextView>(Resource.Id.fibercountseg4);
            seg4density = FindViewById<TextView>(Resource.Id.fiberdensityseg4);
            seg5fcount = FindViewById<TextView>(Resource.Id.fibercountseg5);
            seg5density = FindViewById<TextView>(Resource.Id.fiberdensityseg5);
            seg6fcount = FindViewById<TextView>(Resource.Id.fibercountseg6);
            seg6density = FindViewById<TextView>(Resource.Id.fiberdensityseg6);
            seg7fcount = FindViewById<TextView>(Resource.Id.fibercountseg7);
            seg7density = FindViewById<TextView>(Resource.Id.fiberdensityseg7);
            seg8fcount = FindViewById<TextView>(Resource.Id.fibercountseg8);
            seg8density = FindViewById<TextView>(Resource.Id.fiberdensityseg8);
            seg9fcount = FindViewById<TextView>(Resource.Id.fibercountseg9);
            seg9density = FindViewById<TextView>(Resource.Id.fiberdensityseg9);
            seg10fcount = FindViewById<TextView>(Resource.Id.fibercountseg10);
            seg10density = FindViewById<TextView>(Resource.Id.fiberdensityseg10);

            seg1densitytotal = FindViewById<TextView>(Resource.Id.fiberdensityseg1total);
            seg2densitytotal = FindViewById<TextView>(Resource.Id.fiberdensityseg2total);
            seg3densitytotal = FindViewById<TextView>(Resource.Id.fiberdensityseg3total);
            seg4densitytotal = FindViewById<TextView>(Resource.Id.fiberdensityseg4total);
            seg5densitytotal = FindViewById<TextView>(Resource.Id.fiberdensityseg5total);
            seg6densitytotal = FindViewById<TextView>(Resource.Id.fiberdensityseg6total);
            seg7densitytotal = FindViewById<TextView>(Resource.Id.fiberdensityseg7total);
            seg8densitytotal = FindViewById<TextView>(Resource.Id.fiberdensityseg8total);
            seg9densitytotal = FindViewById<TextView>(Resource.Id.fiberdensityseg9total);
            seg10densitytotal = FindViewById<TextView>(Resource.Id.fiberdensityseg10total);

            TotalCount = FindViewById<TextView>(Resource.Id.fibercounttotalinfo);
            TotalDensity = FindViewById<TextView>(Resource.Id.fiberdensitytotalallinfo);

            imagetitle.Text = FinalReport.Imagetitle;
            location.Text = FinalReport.Location;
            nameofspecies.Text = FinalReport.Nameofspecies;
            dateofharvest.Text = FinalReport.Dateofharvest;
            imagewidth.Text = FinalReport.Imagewidth.ToString();
            imageheight.Text = FinalReport.Imageheight.ToString();
            threshold.Text = FinalReport.Threshold.ToString();
            colorfilter.Text = FinalReport.Filter;

            seg1fcount.Text = FinalReport.CountS1.ToString();
            seg2fcount.Text = FinalReport.CountS2.ToString();
            seg3fcount.Text = FinalReport.CountS3.ToString();
            seg4fcount.Text = FinalReport.CountS4.ToString();
            seg5fcount.Text = FinalReport.CountS5.ToString();
            seg6fcount.Text = FinalReport.CountS6.ToString();
            seg7fcount.Text = FinalReport.CountS7.ToString();
            seg8fcount.Text = FinalReport.CountS8.ToString();
            seg9fcount.Text = FinalReport.CountS9.ToString();
            seg10fcount.Text = FinalReport.CountS10.ToString();

            seg1density.Text = (FinalReport.FiberDensityS1 * 100).ToString() + "%";
            seg2density.Text = (FinalReport.FiberDensityS2 * 100).ToString() + "%";
            seg3density.Text = (FinalReport.FiberDensityS3 * 100).ToString() + "%";
            seg4density.Text = (FinalReport.FiberDensityS4 * 100).ToString() + "%";
            seg5density.Text = (FinalReport.FiberDensityS5 * 100).ToString() + "%";
            seg6density.Text = (FinalReport.FiberDensityS6 * 100).ToString() + "%";
            seg7density.Text = (FinalReport.FiberDensityS7 * 100).ToString() + "%";
            seg8density.Text = (FinalReport.FiberDensityS8 * 100).ToString() + "%";
            seg9density.Text = (FinalReport.FiberDensityS9 * 100).ToString() + "%";
            seg10density.Text = (FinalReport.FiberDensityS10 * 100).ToString() + "%";

            seg1densitytotal.Text = (FinalReport.FiberDensityS1Total * 100).ToString() + "%";
            seg2densitytotal.Text = (FinalReport.FiberDensityS2Total * 100).ToString() + "%";
            seg3densitytotal.Text = (FinalReport.FiberDensityS3Total * 100).ToString() + "%";
            seg4densitytotal.Text = (FinalReport.FiberDensityS4Total * 100).ToString() + "%";
            seg5densitytotal.Text = (FinalReport.FiberDensityS5Total * 100).ToString() + "%";
            seg6densitytotal.Text = (FinalReport.FiberDensityS6Total * 100).ToString() + "%";
            seg7densitytotal.Text = (FinalReport.FiberDensityS7Total * 100).ToString() + "%";
            seg8densitytotal.Text = (FinalReport.FiberDensityS8Total * 100).ToString() + "%";
            seg9densitytotal.Text = (FinalReport.FiberDensityS9Total * 100).ToString() + "%";
            seg10densitytotal.Text = (FinalReport.FiberDensityS10Total * 100).ToString() + "%";

            TotalCount.Text = FinalReport.TotalSegCount.ToString();
            TotalDensity.Text = Math.Round((FinalReport.TotalFiberDensity * 100), 4).ToString() + "%";

            

            Share.Click += Share_Click;
            Save.Click += Save_Click;
            DBAdd.Click += DBAdd_Click;
           
        }

        private void DBAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
                NetworkInfo networkInfo = connectivityManager.ActiveNetworkInfo;
                try
                {
                    isOnline = networkInfo.IsConnected;
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }

                if (isOnline)
                {
                    RegisteredUser = new Users();
                    validateUser();
                }
                else
                    Toast.MakeText(this, "No Connection, Please Connect to Internet", ToastLength.Long).Show();

            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }

        private void validateUser()
        {

            //Pull the Validate User Dialog
            FragmentTransaction transaction = FragmentManager.BeginTransaction();  // Pull up the dialog from the activity
            ValidateUser_Dialog ValidateUser_Dialog = new ValidateUser_Dialog();
            ValidateUser_Dialog.Show(transaction, "dialog fragment");

            ValidateUser_Dialog.ValidateEvent += ValidateUser_Dialog_ValidateEvent;
        }

        private async void ValidateUser_Dialog_ValidateEvent(object sender, OnValidateEventArgs e)
        {
            RegisteredUser.Id = e.id;
            RegisteredUser.Username = e.username;
            RegisteredUser.Password = e.password;

            if (int.Parse(RegisteredUser.Id) > 0)
            {

                client = new MobileServiceClient(applicationURL);
                ReportsTable = client.GetTable<Reports>();

                var ReportsList = await ReportsTable.ToListAsync();
                int count = ReportsList.Count + 1;

                FinalReport.Id = count.ToString();
                FinalReport.Userid = RegisteredUser.Id;

                try
                {
                    await ReportsTable.InsertAsync(FinalReport);
                    ReportsTableItems.Add(FinalReport);

                    Toast.MakeText(this, "User Valid, Report Added", ToastLength.Long).Show();
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
            }
            else
            {
                Toast.MakeText(this, "User is not Registered", ToastLength.Long).Show();
            }


     
        }

        private void Save_Click(object sender, EventArgs e)
        {

            //Create PDF
            PdfDocument document = new PdfDocument();
            // create a page description
            PageInfo pageInfo = new PageInfo.Builder(595, 842, 1).Create();
            // start a page
            Page page = document.StartPage(pageInfo);

            // draw something on the page

            // matrix
            //Matrix matrix = new Matrix();
            //matrix.PreTranslate(500 , 500);
            //matrix.PreScale(2, 2);
            //page.Canvas.DrawBitmap(loadedimgbmp, matrix, null);
            Picture picture = new Picture();
            Canvas canvas = picture.BeginRecording(loadedimgbmp.Width, loadedimgbmp.Height);
            canvas.DrawBitmap(loadedimgbmp, null, new RectF(0f, 0f, 300, 300),null);
            picture.EndRecording();
            page.Canvas.DrawPicture(picture);
            Paint paint = new Paint();
            paint.Color = Color.Black;
            paint.TextSize = 24;
            page.Canvas.DrawText("HelloWorld", 0f, 100f, paint);
            

            // finish the page
            document.FinishPage(page);

            // save PDF
            reportfilepath = FinalReport.Originalimagefilepath + ".pdf";
            FileStream fileStream = new FileStream(reportfilepath, FileMode.Create);
            // write the document content
            document.WriteTo(fileStream);
            fileStream.Flush();
            fileStream.Close();

            // close the document
            document.Close();

            Toast.MakeText(this, "Saved", ToastLength.Long).Show();
        }

        private void Share_Click(object sender, EventArgs e)
        {
           CrossShareFile.Current.ShareLocalFile(reportfilepath);
        }

        private PlotController ControlPlotModel()
        {
            var myController = new PlotController();

            myController.UnbindAll();

            return myController;
        }
        private PlotModel CreatePlotModel()
        {
            var plotModel = new PlotModel { Title = " Fiber Density Chart" };
            
            

            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Segments" });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Maximum = 100, Minimum = 0, Title = "Density %" });

            var series1 = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White
            };

            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(1, Math.Round(FinalReport.FiberDensityS1 * 100,3)));
            series1.Points.Add(new DataPoint(2, Math.Round(FinalReport.FiberDensityS2 * 100, 3)));
            series1.Points.Add(new DataPoint(3, Math.Round(FinalReport.FiberDensityS3 * 100, 3)));
            series1.Points.Add(new DataPoint(4, Math.Round(FinalReport.FiberDensityS4 * 100, 3)));
            series1.Points.Add(new DataPoint(5, Math.Round(FinalReport.FiberDensityS5 * 100, 3)));
            series1.Points.Add(new DataPoint(6, Math.Round(FinalReport.FiberDensityS6 * 100, 3)));
            series1.Points.Add(new DataPoint(7, Math.Round(FinalReport.FiberDensityS7 * 100, 3)));
            series1.Points.Add(new DataPoint(8, Math.Round(FinalReport.FiberDensityS8 * 100, 3)));
            series1.Points.Add(new DataPoint(9, Math.Round(FinalReport.FiberDensityS9 * 100, 3)));
            series1.Points.Add(new DataPoint(10, Math.Round(FinalReport.FiberDensityS10 * 100, 3)));


            plotModel.Series.Add(series1);

            return plotModel;
        }
    }
}