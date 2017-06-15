using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using System.Drawing;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace BambuShootProject.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Button Event to start to process a New image
        //creates the App folder and the report list text file inside the folder
        private void NewImageButton_Click(object sender, RoutedEventArgs e)
        {
            string temp = "C:/Users/Public/Pictures/BambuShoot";
            if (!Directory.Exists(temp))    Directory.CreateDirectory(temp);
            if(!File.Exists("C:/Users/Public/Pictures/BambuShoot/RepList.txt")) File.WriteAllText("C:/Users/Public/Pictures/BambuShoot/RepList.txt","");
            this.Hide();
            ParameterWindow par = new ParameterWindow(this);
            par.Show();
        }

        //Pops up the Add User window where youc an insert a new User to de DB
        private void CreateUserButton_Click(object sender, RoutedEventArgs e)
        {
            AddUser popup = new AddUser();
            popup.ShowDialog();
        }

        //Show you the reports that are inside the DB
        private void DataBaseButton_Click(object sender, RoutedEventArgs e)
        {
            
            DBWindow db = new DBWindow();
            db.ShowDialog();
        }


        //Shows you the Report List of the reports saved in the local device
        private void DataReportButton_Click(object sender, RoutedEventArgs e)
        {
            ReportListWindow rep = new ReportListWindow();
            rep.ShowDialog();
        }

        private void InstrucButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(" Main Window: \n View Reports on Database \t View Local Reports \t Process a New Image \t Create User \n Parameter Window: \n ");
        }
    }
}
