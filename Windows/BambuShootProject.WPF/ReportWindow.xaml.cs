using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

using OxyPlot;
using System.IO;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.Net.Mail;
using Microsoft.WindowsAzure.MobileServices;

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;  // offline sync
using Microsoft.WindowsAzure.MobileServices.Sync;         // offline sync
#endif
namespace BambuShootProject.WPF
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    /// 
    
    public partial class ReportWindow : Window
    {
        private MobileServiceCollection<Reports, Reports> ReportsTableItems;
#if OFFLINE_SYNC_ENABLED
           private IMobileServiceSyncTable<Reports> ReportsTable = App.MobileService.GetSyncTable<Reports>(); // offline sync
           private IMobileServiceSyncTable<Users> UserTable = App.MobileService.GetSyncTable<Users>(); // offline sync
#else
        private IMobileServiceTable<Reports> ReportTable = App.MobileService.GetTable<Reports>();
#endif

        private ImageProcessWindow imageProcessWindow;
        public static int[] segmentpcounts = new int[10];
        public static int[] segmentcounts = new int[10];
        public static double[] densitysegs = new double[10];
        public static double[] totdensitysegs  = new double[10];
        public string oripath, edipath,repimgpath;
        public int imhei, imwid, imthr, countf;



            double totaldensity;




        public ReportWindow(ImageProcessWindow imageProcessWindow)
        {
            Segment s1 = new Segment();
            Segment s2 = new Segment();
            Segment s3 = new Segment();
            Segment s4 = new Segment();
            Segment s5 = new Segment();
            Segment s6 = new Segment();
            Segment s7 = new Segment();
            Segment s8 = new Segment();
            Segment s9 = new Segment();
            Segment s10 = new Segment();
            s1.name = "segmento1";
            s2.name = "segmento2";
            s3.name = "segmento3";
            s4.name = "segmento4";
            s5.name = "segmento5";
            s6.name = "segmento6";
            s7.name = "segmento7";
            s8.name = "segmento8";
            s9.name = "segmento9";
            s10.name = "segmento10";


       

            this.imageProcessWindow = imageProcessWindow;
            InitializeComponent();
            string temp2 = imageProcessWindow.imageTitle.Text;
            string temp = "C:/Users/Public/Pictures/BambuShoot/" + temp2 + "/" + temp2 + ".png";
            oripath = temp;
            edipath = "C:/Users/Public/Pictures/BambuShoot/" + temp2 + "/edited.bmp";
            //Bring Original Image from folder
            Image<Bgr, byte> imgsrc = new Image<Bgr, byte>(temp);
            originalImage.Source = BitmapSourceConvert.ToBitmapSource(imgsrc);
            pImageHeight.Text = "Image Height:" + imgsrc.Height.ToString();
            pImageWidth.Text = "Image Width:" + imgsrc.Width.ToString();
            imhei = imgsrc.Height;
            imwid = imgsrc.Width;
            //Bring Edited Image from folder
            Image<Bgr, byte> editimg = new Image<Bgr, byte>("C:/Users/Public/Pictures/BambuShoot/" + temp2 + "/edited.bmp");
            Mat edimg = editimg.Mat;
            Mat gray = new Mat();
            Mat bw = new Mat();
            Mat dummy = new Mat();
            CvInvoke.Decolor(edimg, gray, dummy);

            //Create Slices of picture
            int count, count1, count2, count3, count4, count5, count6, count7, count8, count9;
            int width = editimg.Width;
            int height = editimg.Height;
            int slice = width / 10;
            System.Drawing.Rectangle seg = new System.Drawing.Rectangle(0, 0, slice, height);
            System.Drawing.Rectangle seg1 = new System.Drawing.Rectangle(slice, 0, slice, height);
            System.Drawing.Rectangle seg2 = new System.Drawing.Rectangle(slice * 2, 0, slice, height);
            System.Drawing.Rectangle seg3 = new System.Drawing.Rectangle(slice * 3, 0, slice, height);
            System.Drawing.Rectangle seg4 = new System.Drawing.Rectangle(slice * 4, 0, slice, height);
            System.Drawing.Rectangle seg5 = new System.Drawing.Rectangle(slice * 5, 0, slice, height);
            System.Drawing.Rectangle seg6 = new System.Drawing.Rectangle(slice * 6, 0, slice, height);
            System.Drawing.Rectangle seg7 = new System.Drawing.Rectangle(slice * 7, 0, slice, height);
            System.Drawing.Rectangle seg8 = new System.Drawing.Rectangle(slice * 8, 0, slice, height);
            System.Drawing.Rectangle seg9 = new System.Drawing.Rectangle(slice * 9, 0, slice, height);

            if (imageProcessWindow.radioButton_TH.IsChecked == true)
            {
                CvInvoke.Threshold(gray, bw, int.Parse(imageProcessWindow.textBox.Text), 255, Emgu.CV.CvEnum.ThresholdType.Binary);
                Image<Gray, byte> picedt = bw.ToImage<Gray, Byte>();
                editedImage.Source = BitmapSourceConvert.ToBitmapSource(picedt);
            }
            else 
            {
                CvInvoke.AdaptiveThreshold(gray, bw, 255, Emgu.CV.CvEnum.AdaptiveThresholdType.MeanC, Emgu.CV.CvEnum.ThresholdType.Binary, 251, 1);
                Image<Gray, byte> picedt = bw.ToImage<Gray, Byte>();
                editedImage.Source = BitmapSourceConvert.ToBitmapSource(picedt);
            }
            countf = CvInvoke.CountNonZero(bw);
            countf = edimg.Total.ToInt32() - countf;
            //sets the segments to Mats
            Mat imgseg = new Mat(bw, seg);
            Mat imgseg1 = new Mat(bw, seg1);
            Mat imgseg2 = new Mat(bw, seg2);
            Mat imgseg3 = new Mat(bw, seg3);
            Mat imgseg4 = new Mat(bw, seg4);
            Mat imgseg5 = new Mat(bw, seg5);
            Mat imgseg6 = new Mat(bw, seg6);
            Mat imgseg7 = new Mat(bw, seg7);
            Mat imgseg8 = new Mat(bw, seg8);
            Mat imgseg9 = new Mat(bw, seg9);
            //Creates the ten Segment images
            Image<Gray, byte> canto = imgseg.ToImage<Gray, Byte>();
            Image<Gray, byte> canto1 = imgseg1.ToImage<Gray, Byte>();
            Image<Gray, byte> canto2 = imgseg2.ToImage<Gray, Byte>();
            Image<Gray, byte> canto3 = imgseg3.ToImage<Gray, Byte>();
            Image<Gray, byte> canto4 = imgseg4.ToImage<Gray, Byte>();
            Image<Gray, byte> canto5 = imgseg5.ToImage<Gray, Byte>();
            Image<Gray, byte> canto6 = imgseg6.ToImage<Gray, Byte>();
            Image<Gray, byte> canto7 = imgseg7.ToImage<Gray, Byte>();
            Image<Gray, byte> canto8 = imgseg8.ToImage<Gray, Byte>();
            Image<Gray, byte> canto9 = imgseg9.ToImage<Gray, Byte>();
            //Set slices to reflect UI
            segm1.Source = BitmapSourceConvert.ToBitmapSource(canto);
            segm2.Source = BitmapSourceConvert.ToBitmapSource(canto1);
            segm3.Source = BitmapSourceConvert.ToBitmapSource(canto2);
            segm4.Source = BitmapSourceConvert.ToBitmapSource(canto3);
            segm5.Source = BitmapSourceConvert.ToBitmapSource(canto4);
            segm6.Source = BitmapSourceConvert.ToBitmapSource(canto5);
            segm7.Source = BitmapSourceConvert.ToBitmapSource(canto6);
            segm8.Source = BitmapSourceConvert.ToBitmapSource(canto7);
            segm9.Source = BitmapSourceConvert.ToBitmapSource(canto8);
            segm10.Source = BitmapSourceConvert.ToBitmapSource(canto9);
            //Counts the # of Matrix
            count = CvInvoke.CountNonZero(imgseg);
            count1 = CvInvoke.CountNonZero(imgseg1);
            count2 = CvInvoke.CountNonZero(imgseg2);
            count3 = CvInvoke.CountNonZero(imgseg3);
            count4 = CvInvoke.CountNonZero(imgseg4);
            count5 = CvInvoke.CountNonZero(imgseg5);
            count6 = CvInvoke.CountNonZero(imgseg6);
            count7 = CvInvoke.CountNonZero(imgseg7);
            count8 = CvInvoke.CountNonZero(imgseg8);
            count9 = CvInvoke.CountNonZero(imgseg9);

            segmentcounts[0] = imgseg.Total.ToInt32() - count;
            segmentcounts[1] = imgseg1.Total.ToInt32() - count1;
            segmentcounts[2] = imgseg2.Total.ToInt32() - count2;
            segmentcounts[3] = imgseg3.Total.ToInt32() - count3;
            segmentcounts[4] = imgseg4.Total.ToInt32() - count4;
            segmentcounts[5] = imgseg5.Total.ToInt32() - count5;
            segmentcounts[6] = imgseg6.Total.ToInt32() - count6;
            segmentcounts[7] = imgseg7.Total.ToInt32() - count7;
            segmentcounts[8] = imgseg8.Total.ToInt32() - count8;
            segmentcounts[9] = imgseg9.Total.ToInt32() - count9;
           
            s1.fibercount = segmentcounts[0];
            s2.fibercount = segmentcounts[1];
            s3.fibercount = segmentcounts[2];
            s4.fibercount = segmentcounts[3];
            s5.fibercount = segmentcounts[4];
            s6.fibercount = segmentcounts[5];
            s7.fibercount = segmentcounts[6];
            s8.fibercount = segmentcounts[7];
            s9.fibercount = segmentcounts[8];
            s10.fibercount = segmentcounts[9];

            densitysegs[0] = (double)segmentcounts[0] / (canto.Height * canto.Width);
            densitysegs[1] = (double)segmentcounts[1] / (canto1.Height * canto1.Width);
            densitysegs[2] = (double)segmentcounts[2] / (canto2.Height * canto2.Width);
            densitysegs[3] = (double)segmentcounts[3] / (canto3.Height * canto3.Width);
            densitysegs[4] = (double)segmentcounts[4] / (canto4.Height * canto4.Width);
            densitysegs[5] = (double)segmentcounts[5] / (canto5.Height * canto5.Width);
            densitysegs[6] = (double)segmentcounts[6] / (canto6.Height * canto6.Width);
            densitysegs[7] = (double)segmentcounts[7] / (canto7.Height * canto7.Width);
            densitysegs[8] = (double)segmentcounts[8] / (canto8.Height * canto8.Width);
            densitysegs[9] = (double)segmentcounts[9] / (canto9.Height * canto9.Width);
            s1.segdensity = densitysegs[0] * 100;
            s2.segdensity = densitysegs[1] * 100;
            s3.segdensity = densitysegs[2] * 100;
            s4.segdensity = densitysegs[3] * 100;
            s5.segdensity = densitysegs[4] * 100;
            s6.segdensity = densitysegs[5] * 100;
            s7.segdensity = densitysegs[6] * 100;
            s8.segdensity = densitysegs[7] * 100;
            s9.segdensity = densitysegs[8] * 100;
            s10.segdensity = densitysegs[9] * 100;



            totaldensity = (double)countf / (height * width);

            totdensitysegs[0] = (double)segmentcounts[0] / (height * width);
            totdensitysegs[1] = (double)segmentcounts[1] / (height * width);
            totdensitysegs[2] = (double)segmentcounts[2] / (height * width);
            totdensitysegs[3] = (double)segmentcounts[3] / (height * width);
            totdensitysegs[4] = (double)segmentcounts[4] / (height * width);
            totdensitysegs[5] = (double)segmentcounts[5] / (height * width);
            totdensitysegs[6] = (double)segmentcounts[6] / (height * width);
            totdensitysegs[7] = (double)segmentcounts[7] / (height * width);
            totdensitysegs[8] = (double)segmentcounts[8] / (height * width);
            totdensitysegs[9] = (double)segmentcounts[9] / (height * width);

            s1.segtotdensity = totdensitysegs[0] * 100;
            s2.segtotdensity = totdensitysegs[1] * 100;
            s3.segtotdensity = totdensitysegs[2] * 100;
            s4.segtotdensity = totdensitysegs[3] * 100;
            s5.segtotdensity = totdensitysegs[4] * 100;
            s6.segtotdensity = totdensitysegs[5] * 100;
            s7.segtotdensity = totdensitysegs[6] * 100;
            s8.segtotdensity = totdensitysegs[7] * 100;
            s9.segtotdensity = totdensitysegs[8] * 100;
            s10.segtotdensity = totdensitysegs[9] * 100;

        
            dataGrid.Items.Add(s1);
            dataGrid.Items.Add(s2);
            dataGrid.Items.Add(s3);
            dataGrid.Items.Add(s4);
            dataGrid.Items.Add(s5);
            dataGrid.Items.Add(s6);
            dataGrid.Items.Add(s7);
            dataGrid.Items.Add(s8);
            dataGrid.Items.Add(s9);
            dataGrid.Items.Add(s10);

           


        }


        public class Segment
        {
            public string name { get; set; }
            public int fibercount { get; set; }
            public double segdensity { get; set; }
            public double segtotdensity { get; set; }
        }

        
        public static void SaveWindow(Window window, int dpi, string filename)
        {

            var rtb = new RenderTargetBitmap(
                (int)window.Width, //width 
                (int)window.Width, //height 
                dpi, //dpi x 
                dpi, //dpi y 
                PixelFormats.Pbgra32 // pixelformat 
                );
            rtb.Render(window);

            SaveRTBAsPNG(rtb, filename);

        }


        private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
        {
            var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
            enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

            using (var stm = System.IO.File.Create(filename))
            {
                enc.Save(stm);
            }
        }
       

        public static void createPdfFromImage(string imageFile, string pdfFile)
        {
            using (var ms = new MemoryStream())
            {
                var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER.Rotate(), 0, 0, 0, 0);
                PdfWriter.GetInstance(document, new FileStream(pdfFile, FileMode.Create));
                iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms).SetFullCompression();
                document.Open();

                FileStream fs = new FileStream(imageFile, FileMode.Open);
                var image = iTextSharp.text.Image.GetInstance(fs);
                image.ScaleToFit(document.PageSize.Width, document.PageSize.Height);
                document.Add(image);
                document.Close();

          
            }
        }


        private void sToPdfButton_Click(object sender, RoutedEventArgs e)
        {
            saveReport();

            try
            {

                string tempfile = System.IO.Path.GetTempFileName();
                string filename = "C:/Users/Public/Pictures/BambuShoot/RepList.txt";
                using (var writer = new StreamWriter(tempfile))
                using (var reader = new StreamReader(filename))
                {
                    writer.WriteLine(pimageTitle.Text + "+" + oripath + "+" + repimgpath);
                    while (!reader.EndOfStream)
                        writer.WriteLine(reader.ReadLine());
                }
                File.Copy(tempfile, filename, true);
                //Read the first line of text
                
              
                //Continue to read until you reach end of file

               
            }
            catch (Exception re)
            {
                Console.WriteLine("Exception: " + re.Message);
            }

        }


        private void saveReport()
        {

            string sPDFFileName = "C:/Users/Public/Pictures/BambuShoot/" + pimageTitle.Text + "/Report.pdf";
            string sImagePath = "C:/Users/Public/Pictures/BambuShoot/" + pimageTitle.Text + "/window.png";
            repimgpath = sImagePath;
            SaveWindow(WindowForm, 96, sImagePath);
            createPdfFromImage(sImagePath, sPDFFileName);
        }


        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
           

            ShareWindow share = new ShareWindow(this);
           
             share.imageTitle.Text = pimageTitle.Text;
            share.Show();

        }


        private async void sToDataBadeButton_Click(object sender, RoutedEventArgs e)
        {
            await RefreshReportItems();
            var ReportList = await ReportTable.ToListAsync();
            int count = ReportList.Count + 1;
            Reports ReportItem = new Reports();

            ReportItem.Id = count.ToString();
            ReportItem.Imagetitle = imageProcessWindow.imageTitle.Text;
            ReportItem.Location = pLocation.Text;
            ReportItem.Nameofspecies = pNameOfSpecies.Text;
            ReportItem.Dateofharvest = pDateOfHarvest.Text;
            ReportItem.Originalimagefilepath = oripath;
            ReportItem.Editedimagefilepath = edipath;
            ReportItem.Imageheight = imhei;
            ReportItem.Imagewidth = imwid;
            ReportItem.Threshold = int.Parse(imageProcessWindow.textBox.Text);
            ReportItem.CountS1 = segmentcounts[0];
            ReportItem.CountS2 = segmentcounts[1];
            ReportItem.CountS3 = segmentcounts[2];
            ReportItem.CountS4 = segmentcounts[3];
            ReportItem.CountS5 = segmentcounts[4];
            ReportItem.CountS6 = segmentcounts[5];
            ReportItem.CountS7 = segmentcounts[6];
            ReportItem.CountS8 = segmentcounts[7];
            ReportItem.CountS9 = segmentcounts[8];
            ReportItem.CountS10 = segmentcounts[9];
            ReportItem.FiberDensityS1 = densitysegs[0];
            ReportItem.FiberDensityS2 = densitysegs[1];
            ReportItem.FiberDensityS3 = densitysegs[2];
            ReportItem.FiberDensityS4 = densitysegs[3];
            ReportItem.FiberDensityS5 = densitysegs[4];
            ReportItem.FiberDensityS6 = densitysegs[5];
            ReportItem.FiberDensityS7 = densitysegs[6];
            ReportItem.FiberDensityS8 = densitysegs[7];
            ReportItem.FiberDensityS9 = densitysegs[8];
            ReportItem.FiberDensityS10 = densitysegs[9];
            ReportItem.FiberDensityS1Total = totdensitysegs[0];
            ReportItem.FiberDensityS2Total = totdensitysegs[1];
            ReportItem.FiberDensityS3Total = totdensitysegs[2];
            ReportItem.FiberDensityS4Total = totdensitysegs[3];
            ReportItem.FiberDensityS5Total = totdensitysegs[4];
            ReportItem.FiberDensityS6Total = totdensitysegs[5];
            ReportItem.FiberDensityS7Total = totdensitysegs[6];
            ReportItem.FiberDensityS8Total = totdensitysegs[7];
            ReportItem.FiberDensityS9Total = totdensitysegs[8];
            ReportItem.FiberDensityS10Total = totdensitysegs[9];
            ReportItem.TotalSegCount = countf;
            ReportItem.TotalFiberDensity = totaldensity;
            ReportItem.Userid = "wot";
            if (imageProcessWindow.radioButton_TH.IsChecked == true)
            {
                ReportItem.Filter = "Black and White Threshold";
            }else { ReportItem.Filter = "Grayscale Adaptive Threshold"; }


                await InsertReportItem(ReportItem);

        }


        private async Task InsertReportItem(Reports ReportsItem)
        {
            // This code inserts a new TodoItem into the database. After the operation completes
            // and the mobile app backend has assigned an id, the item is added to the CollectionView.
            await ReportTable.InsertAsync(ReportsItem);
            ReportsTableItems.Add(ReportsItem);

#if OFFLINE_SYNC_ENABLED
            await App.MobileService.SyncContext.PushAsync(); // offline sync
#endif
        }


        private async Task RefreshReportItems()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                // This code refreshes the entries in the list view by querying the TodoItems table.
         
                ReportsTableItems = await ReportTable.ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
               
            }
            else
            {
            
            }
        }


        public class MainViewModel
        {
            public MainViewModel()
            {
                this.Title = "Fiber Count vs Bamboo Culm";
             
                this.Points = new List<DataPoint>
                              {
                                  new DataPoint(1, segmentpcounts[0]),
                                 new DataPoint(2, segmentpcounts[1]),
                                 new DataPoint(3, segmentpcounts[2]),
                                 new DataPoint(4, segmentpcounts[3]),
                                 new DataPoint(5, segmentpcounts[4]),
                                 new DataPoint(6, segmentpcounts[5]),
                                 new DataPoint(7, segmentpcounts[6]),
                                 new DataPoint(8, segmentpcounts[7]),
                                 new DataPoint(9, segmentpcounts[8]),
                                 new DataPoint(10, segmentpcounts[9])
                              };
            }

            public string Title { get; private set; }

            public IList<DataPoint> Points { get; private set; }
        }

    }
    }
  


