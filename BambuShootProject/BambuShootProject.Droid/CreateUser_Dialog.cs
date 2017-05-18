
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
using Microsoft.WindowsAzure.MobileServices;
using ClassLibrary;
using System.Threading.Tasks;

namespace BambuShootProject.Droid
{
    class CreateUser_Dialog:DialogFragment
    {
        Button gCreateBtn;
        EditText gUsername;
        EditText gPassword;
        // URL of the mobile app backend.
        const string applicationURL = @"https://bambushoot.azurewebsites.net";

        // Create the client instance, using the mobile app backend URL.
        private MobileServiceClient client;
        private MobileServiceCollection<Users, Users> UserTableItems;
        private IMobileServiceTable<Users> UserTable;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.CreateUser, container, false);  // creates the dialog layout in the view 

            client = new MobileServiceClient(applicationURL);
            UserTable = client.GetTable<Users>();

            gUsername = view.FindViewById<EditText>(Resource.Id.UsernameEdt);
            gPassword = view.FindViewById<EditText>(Resource.Id.PasswordEdt);
            gCreateBtn = view.FindViewById<Button>(Resource.Id.CreateBtn);

            gCreateBtn.Click += GCreateBtn_Click;

            return view;

        }

        private bool verifyData(String user, String password)
        {
            bool somethingempty = false;
            //Check for empty edit text
            if (user.Length == 0)
            {
                gUsername.FindFocus();
                gUsername.Error = "Empty Username";
                somethingempty = true;
            }
            if (password.Length == 0)
            {
                gPassword.FindFocus();
                gPassword.Error = "Empty Password";
                somethingempty = true;
            }
            //Add here Validation of Username and Password
         

            return somethingempty;

        }
        private async void GCreateBtn_Click(object sender, EventArgs e)
        {
           
            await RefreshUserItems();
            
            if ( verifyData(gUsername.Text, gPassword.Text) == false) { 

            var UserList = await UserTable.ToListAsync();
            int count = UserList.Count + 1;

            Users Userinfo = new Users();
            Userinfo.id = count.ToString();
            Userinfo.username = gUsername.Text;
            Userinfo.password = gPassword.Text;

            await UserTable.InsertAsync(Userinfo);
            UserTableItems.Add(Userinfo);

            this.Dismiss();
          }
     
        }

        private async Task RefreshUserItems()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                UserTableItems = await UserTable.ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle); //Set title bar to invisible 
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation; // Set the animation
        
        }
    }
}