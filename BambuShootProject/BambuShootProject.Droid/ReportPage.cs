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
using Android.Preferences;

namespace com.BambuShoot.droid
{

    [Activity(Label = "Data Report", ConfigurationChanges = ConfigChanges.Locale | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
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
        ImgProcessData ImgReportdata;
        Button Save;
        Button Share;
        Button DBAdd;
        Button ReturnMM;
        string reportfilepath;
        bool isOnline;
        const string applicationURL = @"https://bambushoot.azurewebsites.net";
        Users RegisteredUser;
        EditText comments;



        // Create the client instance, using the mobile app backend URL.
        private MobileServiceClient client;

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

            comments = FindViewById<EditText>(Resource.Id.commentshow);

            Save = FindViewById<Button>(Resource.Id.saveinternal);
            Share = FindViewById<Button>(Resource.Id.share);
            DBAdd = FindViewById<Button>(Resource.Id.databaseadd);
            ReturnMM = FindViewById<Button>(Resource.Id.returntoMM);

            //Set Original Image
            loadedimgbmp = BitmapFactory.DecodeFile(FinalReport.Originalimagefilepath);
            LoadedImage.SetImageBitmap(loadedimgbmp);

            //Set Edited Image
            editedimgbmp = BitmapFactory.DecodeFile(FinalReport.Editedimagefilepath);
            EditedImage.SetImageBitmap(editedimgbmp);

            bool filterbool = false;
            if (FinalReport.Filter == "GrayScale")
            {
                filterbool = true;
            }
            else
            {
                filterbool = false;
            }

            ImgReportdata = new ImgProcessData();
            ImgReportdata = Methods.FiberDensity(FinalReport.Editedimagefilepath,filterbool);
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


            Share.Enabled = false;

            Share.Click += Share_Click;
            Save.Click += Save_Click;
            DBAdd.Click += DBAdd_Click;
            ReturnMM.Click += ReturnMM_Click;


        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            LoadedImage.SetImageBitmap(null);
            LoadedImage.Dispose();
            LoadedImage = null;

            EditedImage.SetImageBitmap(null);
            EditedImage.Dispose();
            EditedImage = null;

            loadedimgbmp.Recycle();
            loadedimgbmp.Dispose();
            loadedimgbmp = null;

            editedimgbmp.Recycle();
            editedimgbmp.Dispose();
            editedimgbmp = null;

            Segment1.SetImageBitmap(null);
            Segment1.Dispose();
            Segment1 = null;

            Segment2.SetImageBitmap(null);
            Segment2.Dispose();
            Segment2 = null;

            Segment3.SetImageBitmap(null);
            Segment3.Dispose();
            Segment3 = null;

            Segment4.SetImageBitmap(null);
            Segment4.Dispose();
            Segment4 = null;

            Segment5.SetImageBitmap(null);
            Segment5.Dispose();
            Segment5 = null;

            Segment6.SetImageBitmap(null);
            Segment6.Dispose();
            Segment6 = null;

            Segment7.SetImageBitmap(null);
            Segment7.Dispose();
            Segment7 = null;

            Segment8.SetImageBitmap(null);
            Segment8.Dispose();
            Segment8 = null;

            Segment9.SetImageBitmap(null);
            Segment9.Dispose();
            Segment9 = null;

            Segment10.SetImageBitmap(null);
            Segment10.Dispose();
            Segment10 = null;

        }
        private void ReturnMM_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            this.StartActivity(intent);
           
        }
        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(MainActivity))
                    .SetFlags(ActivityFlags.ReorderToFront);
            StartActivity(intent);
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
                FinalReport.DateUtc = DateTime.UtcNow;
                try
                {

                    await ReportsTable.InsertAsync(FinalReport);

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
            Picture OI = new Picture();
            Canvas canvas = OI.BeginRecording(loadedimgbmp.Width, loadedimgbmp.Height);
            canvas.DrawBitmap(loadedimgbmp, null, new RectF(25f, 25f, 225f, 225f),null);
            OI.EndRecording();
            page.Canvas.DrawPicture(OI);

            Picture EI = new Picture();
            Canvas canvas2 = EI.BeginRecording(editedimgbmp.Width, editedimgbmp.Height);
            canvas2.DrawBitmap(editedimgbmp, null, new RectF(370f, 25f, 570f, 225f), null);
            EI.EndRecording();
            page.Canvas.DrawPicture(EI);

            //Segments
            Picture Seg1 = new Picture();
            Canvas canvasSeg1 = Seg1.BeginRecording( ImgReportdata.Segments[1].Width, ImgReportdata.Segments[1].Height);
            canvasSeg1.DrawBitmap(ImgReportdata.Segments[1], null, new RectF(300f, 250f, 320f, 450f), null);
            Seg1.EndRecording();
            page.Canvas.DrawPicture(Seg1);

            Picture Seg2 = new Picture();
            Canvas canvasSeg2 = Seg2.BeginRecording(ImgReportdata.Segments[2].Width, ImgReportdata.Segments[2].Height);
            canvasSeg2.DrawBitmap(ImgReportdata.Segments[2], null, new RectF(330f, 250f, 350f, 450f), null);
            Seg2.EndRecording();
            page.Canvas.DrawPicture(Seg2);

            Picture Seg3 = new Picture();
            Canvas canvasSeg3 = Seg3.BeginRecording(ImgReportdata.Segments[3].Width, ImgReportdata.Segments[3].Height);
            canvasSeg3.DrawBitmap(ImgReportdata.Segments[3], null, new RectF(360f, 250f, 380f, 450f), null);
            Seg3.EndRecording();
            page.Canvas.DrawPicture(Seg3);

            Picture Seg4 = new Picture();
            Canvas canvasSeg4 = Seg4.BeginRecording(ImgReportdata.Segments[4].Width, ImgReportdata.Segments[4].Height);
            canvasSeg4.DrawBitmap(ImgReportdata.Segments[4], null, new RectF(390f, 250f, 410f, 450f), null);
            Seg4.EndRecording();
            page.Canvas.DrawPicture(Seg4);

            Picture Seg5 = new Picture();
            Canvas canvasSeg5 = Seg5.BeginRecording(ImgReportdata.Segments[5].Width, ImgReportdata.Segments[5].Height);
            canvasSeg5.DrawBitmap(ImgReportdata.Segments[5], null, new RectF(420f, 250f, 440f, 450f), null);
            Seg5.EndRecording();
            page.Canvas.DrawPicture(Seg5);

            Picture Seg6 = new Picture();
            Canvas canvasSeg6 = Seg6.BeginRecording(ImgReportdata.Segments[6].Width, ImgReportdata.Segments[6].Height);
            canvasSeg6.DrawBitmap(ImgReportdata.Segments[6], null, new RectF(450f, 250f, 470f, 450f), null);
            Seg6.EndRecording();
            page.Canvas.DrawPicture(Seg6);

            Picture Seg7 = new Picture();
            Canvas canvasSeg7 = Seg7.BeginRecording(ImgReportdata.Segments[7].Width, ImgReportdata.Segments[7].Height);
            canvasSeg7.DrawBitmap(ImgReportdata.Segments[7], null, new RectF(480f, 250f, 500f, 450f), null);
            Seg7.EndRecording();
            page.Canvas.DrawPicture(Seg7);

            Picture Seg8 = new Picture();
            Canvas canvasSeg8 = Seg8.BeginRecording(ImgReportdata.Segments[8].Width, ImgReportdata.Segments[8].Height);
            canvasSeg8.DrawBitmap(ImgReportdata.Segments[8], null, new RectF(510f, 250f, 530f, 450f), null);
            Seg8.EndRecording();
            page.Canvas.DrawPicture(Seg8);

            Picture Seg9 = new Picture();
            Canvas canvasSeg9 = Seg9.BeginRecording(ImgReportdata.Segments[9].Width, ImgReportdata.Segments[9].Height);
            canvasSeg9.DrawBitmap(ImgReportdata.Segments[9], null, new RectF(540f, 250f, 560f, 450f), null);
            Seg9.EndRecording();
            page.Canvas.DrawPicture(Seg9);

            Picture Seg10 = new Picture();
            Canvas canvasSeg10 = Seg10.BeginRecording(ImgReportdata.Segments[10].Width, ImgReportdata.Segments[10].Height);
            canvasSeg10.DrawBitmap(ImgReportdata.Segments[10], null, new RectF(570f, 250f, 590f, 450f), null);
            Seg10.EndRecording();
            page.Canvas.DrawPicture(Seg10);

            //Paremeters
            Paint paint = new Paint();
            paint.Color = Color.Black;
            paint.TextSize = 16;
            page.Canvas.DrawText("Parameters: ", 25f, 250f, paint);
            page.Canvas.DrawText("Image Title: " + FinalReport.Imagetitle, 25f, 270f, paint);
            page.Canvas.DrawText("Location: " + FinalReport.Location, 25f, 290f, paint);
            page.Canvas.DrawText("Name of Species: " + FinalReport.Nameofspecies, 25f, 310f, paint);
            page.Canvas.DrawText("Date of Harvest: " + FinalReport.Dateofharvest, 25f, 330f, paint);
            page.Canvas.DrawText("Image Width: " + FinalReport.Imagewidth, 25f, 350f, paint);
            page.Canvas.DrawText("Image Height: " + FinalReport.Imageheight, 25f, 370f, paint);
            page.Canvas.DrawText("Color Filter: " + FinalReport.Filter, 25f, 390f, paint);
            page.Canvas.DrawText("Threshold: " + FinalReport.Threshold, 25f, 410f, paint);

            Picture graph = new Picture();
            Canvas graphcanvas = graph.BeginRecording(view.Width, view.Height);
            view.Draw(graphcanvas);
            graph.EndRecording();
            page.Canvas.DrawPicture(graph,new RectF(25f, 450f, 250f, 750f));

            page.Canvas.DrawText("Data Results \t\t FC \t\t\t\t\t FD of Seg \t\t\t\t FD Seg/TP", 260f, 500f, paint);
            page.Canvas.DrawText("Segment 1 \t\t\t " + FinalReport.CountS1.ToString() + "\t\t\t" + (FinalReport.FiberDensityS1 * 100).ToString() + "% \t\t\t" + (FinalReport.FiberDensityS1Total * 100).ToString() + "%", 260f, 520f,paint);
            page.Canvas.DrawText("Segment 2 \t\t\t  " + FinalReport.CountS2.ToString() + "\t\t\t" + (FinalReport.FiberDensityS2 * 100).ToString() + "% \t\t\t" + (FinalReport.FiberDensityS2Total * 100).ToString() + "%", 260f, 540f, paint);
            page.Canvas.DrawText("Segment 3 \t\t\t " + FinalReport.CountS3.ToString() + "\t\t\t" + (FinalReport.FiberDensityS3 * 100).ToString() + "% \t\t\t" + (FinalReport.FiberDensityS3Total * 100).ToString() + "%", 260f, 560f, paint);
            page.Canvas.DrawText("Segment 4 \t\t\t " + FinalReport.CountS4.ToString() + "\t\t\t" + (FinalReport.FiberDensityS4 * 100).ToString() + "% \t\t\t" + (FinalReport.FiberDensityS4Total * 100).ToString() + "%", 260f, 580f, paint);
            page.Canvas.DrawText("Segment 5 \t\t\t " + FinalReport.CountS5.ToString() + "\t\t\t" + (FinalReport.FiberDensityS5 * 100).ToString() + "% \t\t\t" + (FinalReport.FiberDensityS5Total * 100).ToString() + "%", 260f, 600f, paint);
            page.Canvas.DrawText("Segment 6 \t\t\t " + FinalReport.CountS6.ToString() + "\t\t\t" + (FinalReport.FiberDensityS6 * 100).ToString() + "% \t\t\t" + (FinalReport.FiberDensityS6Total * 100).ToString() + "%", 260f, 620f, paint);
            page.Canvas.DrawText("Segment 7 \t\t\t " + FinalReport.CountS7.ToString() + "\t\t\t" + (FinalReport.FiberDensityS7 * 100).ToString() + "% \t\t\t" + (FinalReport.FiberDensityS7Total * 100).ToString() + "%", 260f, 640f, paint);
            page.Canvas.DrawText("Segment 8 \t\t\t " + FinalReport.CountS8.ToString() + "\t\t\t" + (FinalReport.FiberDensityS8 * 100).ToString() + "% \t\t\t" + (FinalReport.FiberDensityS8Total * 100).ToString() + "%", 260f, 660f, paint);
            page.Canvas.DrawText("Segment 9 \t\t\t " + FinalReport.CountS9.ToString() + "\t\t\t" + (FinalReport.FiberDensityS9 * 100).ToString() + "% \t\t\t" + (FinalReport.FiberDensityS9Total * 100).ToString() + "%", 260f, 680f, paint);
            page.Canvas.DrawText("Segment 10 \t\t\t" + FinalReport.CountS10.ToString() + "\t\t\t" + (FinalReport.FiberDensityS10 * 100).ToString() + "% \t\t\t" + (FinalReport.FiberDensityS10Total * 100).ToString() + "%", 260f, 700f, paint);
            page.Canvas.DrawText("Total Count\t\t\t Total Density",260f,720f,paint);
            page.Canvas.DrawText(FinalReport.TotalSegCount.ToString() + "\t\t\t" + (Math.Round(FinalReport.TotalFiberDensity,6)*100).ToString(), 260f, 740f, paint);

            page.Canvas.DrawText("Comments: " + comments.Text, 25f, 800f, paint);

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

          
            //adding to library
            string filename1 = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot/LibraryListImages.txt";
            using (var streamWriter = new StreamWriter(filename1, true))
            {
                streamWriter.WriteLine(FinalReport.Originalimagefilepath);
            }
            string filename2 = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot/LibraryListPDFs.txt";
            using (var streamWriter = new StreamWriter(filename2, true))
            {
                streamWriter.WriteLine(reportfilepath);
            }
            string filename3 = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/BambuShoot/LibraryListImagetitles.txt";
            using (var streamWriter = new StreamWriter(filename3, true))
            {
                streamWriter.WriteLine(FinalReport.Imagetitle);
            }

            Toast.MakeText(this, "Saved", ToastLength.Long).Show();
            Share.Enabled = true;

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutBoolean("key_for_my_bool_value", true);
            editor.Apply();

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
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Maximum = 100, Minimum = 1, Title = "Density %" });

            var series1 = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White
            };

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