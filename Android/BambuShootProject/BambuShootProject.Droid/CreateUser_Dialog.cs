
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

namespace com.BambuShoot.droid
{
    class CreateUser_Dialog:DialogFragment
    {
        Button gCreateBtn;
        EditText gUsername;
        EditText gPassword;
        Users Userinformation;
        Users Validating;
        List<Users> UserList;

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

            //initiate Client
            client = new MobileServiceClient(applicationURL);
            //Get User Table from client
            UserTable = client.GetTable<Users>();

            gUsername = view.FindViewById<EditText>(Resource.Id.usernameEdt);
            gPassword = view.FindViewById<EditText>(Resource.Id.passwordEdt);
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
            //Validation of Username and Password

            Validating = new Users();
            Validating.Username = gUsername.Text;

            for (int i = 0; i < UserList.Count; i++)
            {
                //Check on list for taken username
                if (UserList.ElementAt<Users>(i).Username.Equals(Validating.Username))
                {
                    gUsername.FindFocus();
                    gUsername.Error = "Username Taken";
                    somethingempty = true;
                    break;
                }
                else
                {
                    somethingempty = false;
                }

            }

            return somethingempty;

        }
        private async void GCreateBtn_Click(object sender, EventArgs e)
        {
           //Refresh List
            await RefreshUserItems();
            //Turn table into List
            UserList = await UserTable.ToListAsync();

            if ( verifyData(gUsername.Text, gPassword.Text) == false) { 
                
            int count = UserList.Count + 1;

            Userinformation = new Users();
            Userinformation.Id = count.ToString();
            Userinformation.Username = gUsername.Text;
            Userinformation.Password = gPassword.Text;
            Userinformation.DateUtc = DateTime.UtcNow;

                try
                {
                    await UserTable.InsertAsync(Userinformation);

                    UserTableItems.Add(Userinformation);
                }
                catch(Exception ex)
                {
                    Console.Write(ex);
                }
                Toast.MakeText(this.Activity, "User Added", ToastLength.Short).Show();
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