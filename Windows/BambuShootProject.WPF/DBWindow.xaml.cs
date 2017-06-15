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
using System.Windows.Navigation;
using Microsoft.WindowsAzure.MobileServices;
using System.Windows.Shapes;

namespace BambuShootProject.WPF
{


    /// <summary>
    /// Interaction logic for DBWindow.xaml
    /// </summary>
    public partial class DBWindow : Window
    {

     //Creates the Local Collection
        private MobileServiceCollection<Reports, Reports> ReportsTableItems;
      
#if OFFLINE_SYNC_ENABLED
      
           private IMobileServiceSyncTable<Reports> ReportsTable = App.MobileService.GetSyncTable<Reports>(); // offline sync
      
#else
     //Creates the local Table
        private IMobileServiceTable<Reports> ReportTable = App.MobileService.GetTable<Reports>();
     
#endif

        //Window constructor that initializes components in the UI
        public DBWindow()
        {
            InitializeComponent();
        }



        //Pull the database table and collection to the local table 
        private async Task RefreshReportItems()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                // This code refreshes the entries in the list view by querying the TodoItems table.
                // The query excludes completed TodoItems.
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
                ListItems.ItemsSource = ReportsTableItems;

            }
        }




        //Refreshes the local table that is linked to the XAML ListView and shows the UI items
        private async void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            ButtonRefresh.IsEnabled = false;

#if OFFLINE_SYNC_ENABLED
            await SyncAsync(); // offline sync
#endif
            await RefreshReportItems();

            ButtonRefresh.IsEnabled = true;
        }
    }
}
