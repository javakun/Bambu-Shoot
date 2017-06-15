

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using ClassLibrary;

namespace com.BambuShoot.droid
{
    [Activity(Label = "Database Report List", ConfigurationChanges = ConfigChanges.Locale | Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class DatabasePage : Activity
    {

        DatabaseAdapter adapter;
        ListView ReportView;

        // URL of the mobile app backend.
        const string applicationURL = @"https://bambushoot.azurewebsites.net";

        // Create the client instance, using the mobile app backend URL.
        private MobileServiceClient client;

        private IMobileServiceTable<ClassLibrary.Users> UserTable;
        private IMobileServiceTable<ClassLibrary.Reports> ReportsTable;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.DatabaseLibrary);
            CurrentPlatform.Init();

            client = new MobileServiceClient(applicationURL);

            ReportsTable = client.GetTable<ClassLibrary.Reports>();
            UserTable = client.GetTable<ClassLibrary.Users>();

            OnRefreshItemsSelected();
            adapter = new DatabaseAdapter(this,Resource.Layout.Row_list_Report_DB);
            ReportView = FindViewById<ListView>(Resource.Id.listViewReports);
            ReportView.Adapter = adapter;

        }

        //Initializes the activity menu
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.activity_DB ,menu);
            return true;
        }

        //Select an option from the menu
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.menu_refresh)
            {
                item.SetEnabled(false);

                OnRefreshItemsSelected();

                item.SetEnabled(true);
            }
            return true;
        }
        // Called when the refresh menu option is selected.
        private async void OnRefreshItemsSelected()
        {
            // refresh view using local store.
            await RefreshItemsFromTableAsync();
        }

        //Refresh the list with the items in the local store.
        private async Task RefreshItemsFromTableAsync()
        {
            try
            {
                // Get the reports from table
                var Reportlist = await ReportsTable.OrderBy(Reports => Reports.DateUtc).ToListAsync();
                var Userlist = await UserTable.OrderBy(Users => Users.Id).ToListAsync();
                

                adapter.Clear();

                foreach (ClassLibrary.Reports current in Reportlist)
                    adapter.AddReports(current);
                
                foreach (ClassLibrary.Users current in Userlist)
                    adapter.AddUsers(current);

            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Error");
            }
        }

        private void CreateAndShowDialog(Exception exception, String title)
        {
            CreateAndShowDialog(exception.Message, title);
        }

        private void CreateAndShowDialog(string message, string title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }

    }
}