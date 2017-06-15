using System;
using System.Collections.Generic;
using System.IO;
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

namespace BambuShootProject.WPF
{
    /// <summary>
    /// Interaction logic for ReportListWindow.xaml
    /// </summary>
    public partial class ReportListWindow : Window
    {
      
       


        //Window constructor that initializes the UI elements and runs the populate method
        public ReportListWindow()
        {
            InitializeComponent();
            populate();
          
        }
        
        // Item to populate Report list 
        public class ReportListItem
        {
           
            public string imgpath { get; set; }
            public string imageTitle { get; set; }
            public string rptpath { get; set; }

        }

        //Back button event that returns you t the Main window
        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        //Button inside of the listbox item that lets you view the report associated with the item in another window
        private void view_button_Click(object sender, RoutedEventArgs e)
        {
            ReportViewWindow rpv = new ReportViewWindow(this);
            Button b = (Button)sender;
            ReportListItem p = (ReportListItem)b.Tag;
            
           
     
            image_topass.Source = new BitmapImage(new Uri(p.rptpath));
            rpv.imagep.Source = image_topass.Source;
         
            rpv.ShowDialog();

        }

        //With the textfie that has all the report references it saves all the lines in the file in an array
        //then each line it separetes the parameters in a temporary array, sets parameters of the ReportListItems 
        //and populates the list box with each one
        private void populate()
        {

         


            try
            {
            


                //Read the first line of text
       
                string[] lines = System.IO.File.ReadAllLines("C:/Users/Public/Pictures/BambuShoot/RepList.txt");
                //Continue to read until you reach end of file
                for (int i = 0; i <lines.Length; i++)
                {
                    ReportListItem temp = new ReportListItem();
                    string[] words = lines[i].Split('+');
                    temp.imageTitle = words[0];
                    temp.imgpath = words[1];
                    temp.rptpath = words[2];
                  
                    listBox.Items.Add(temp);
                  // repitems.Add(temp);

                }
             
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        //Deletes item from Listbox UI
        private void delete_button_Click(object sender, RoutedEventArgs e)
        {

            Button b = (Button)sender;
            ReportListItem p = (ReportListItem)b.Tag;
            listBox.Items.Remove(p);
        }
    }
}
