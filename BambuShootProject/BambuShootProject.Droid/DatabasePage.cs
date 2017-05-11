
//#define OFFLINE_SYNC_ENABLED

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

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
#endif

namespace BambuShootProject.Droid
{
    [Activity(Label = "DatabasePage", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class DatabasePage : Activity
    {

        DatabaseAdapter adapter;
        EditText SearchTxt;
        Button SearchBtn;
        ListView ReportView;

        // URL of the mobile app backend.
        const string applicationURL = @"https://bambushoot.azurewebsites.net";

        // Create the client instance, using the mobile app backend URL.
        private MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        private IMobileServiceSyncTable<Users> UserTable;
        private IMobileServiceSyncTable<Reports> ReportsTable;

        const string localDbFilename = "localstore.db";

#else
        private IMobileServiceTable<ClassLibrary.Users> UserTable;
        private IMobileServiceTable<ClassLibrary.Reports> ReportsTable;
#endif

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.DatabaseLibrary);
            CurrentPlatform.Init();

            client = new MobileServiceClient(applicationURL);

#if OFFLINE_SYNC_ENABLED
            await InitLocalStoreAsync();
            // Get the sync table instance to use to store Report rows.
            ReportsTable = client.GetSyncTable<Reports>();
            UserTable = client.GetSyncTable<Users>();
#else
            ReportsTable = client.GetTable<ClassLibrary.Reports>();
            UserTable = client.GetTable<ClassLibrary.Users>();
#endif

            OnRefreshItemsSelected();
            SearchTxt = FindViewById<EditText>(Resource.Id.SearchEdttxt);
            SearchBtn = FindViewById<Button>(Resource.Id.SearchBtn);
            adapter = new DatabaseAdapter(this,Resource.Layout.Row_list_Report_DB);
            ReportView = FindViewById<ListView>(Resource.Id.listViewReports);
            ReportView.Adapter = adapter;

        }

#if OFFLINE_SYNC_ENABLED
        private async Task InitLocalStoreAsync()
        {
            var store = new MobileServiceSQLiteStore(localDbFilename);
            store.DefineTable<Reports>();

            await client.SyncContext.InitializeAsync(store);
        }

        private async Task SyncAsync(bool pullData = false)
        {
            try
            {
                await client.SyncContext.PushAsync();

                if (pullData)
                {
                    await ReportsTable.PullAsync("allReports", ReportsTable.CreateQuery()); // query ID is used for incremental sync
                   // await UserTable.PullAsync("allUsers", UserTable.CreateQuery());
                }
            }
            catch (Java.Net.MalformedURLException)
            {
                CreateAndShowDialog(new Exception("There was an error creating the Mobile Service. Verify the URL"), "Error");
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Error");
            }
        }
#endif

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
#if OFFLINE_SYNC_ENABLED
			// Get changes from the mobile app backend.
            await SyncAsync(pullData: true);
#endif
            // refresh view using local store.
            await RefreshItemsFromTableAsync();
        }

        //Refresh the list with the items in the local store.
        private async Task RefreshItemsFromTableAsync()
        {
            try
            {
                // Get the reports from table
                var Reportlist = await ReportsTable.ToListAsync();
              //  var Userlist = await UserTable.ToListAsync();

                adapter.Clear();

                foreach (ClassLibrary.Reports current in Reportlist)
                    adapter.AddReports(current);
                
               // foreach (ClassLibrary.Users current in Userlist)
              //      adapter.AddUsers(current);

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