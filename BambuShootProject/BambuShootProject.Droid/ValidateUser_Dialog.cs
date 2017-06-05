
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
    public class OnValidateEventArgs : EventArgs
    {
        public string id { get; set; }
        public string username { get; set; }
        public string password { get; set; }

        public OnValidateEventArgs(string Id, string Username, string Password) : base()
        {
            id = Id;
            username  = Username;
            password = Password;
        }
    }
    class ValidateUser_Dialog:DialogFragment
    {
        Button gValidateBtn;
        EditText gUsername;
        EditText gPassword;
        Users Userinformation;
        List<Users> UserList;
        // URL of the mobile app backend.
        const string applicationURL = @"https://bambushoot.azurewebsites.net";

        // Create the client instance, using the mobile app backend URL.
        private MobileServiceClient client;
        private MobileServiceCollection<Users, Users> UserTableItems;
        private IMobileServiceTable<Users> UserTable;
        

        public event EventHandler<OnValidateEventArgs> ValidateEvent;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.ValidateUser, container, false);  // creates the dialog layout in the view 

            client = new MobileServiceClient(applicationURL);
            UserTable = client.GetTable<Users>();

            gUsername = view.FindViewById<EditText>(Resource.Id.usernameEdt);
            gPassword = view.FindViewById<EditText>(Resource.Id.passwordEdt);
            gValidateBtn = view.FindViewById<Button>(Resource.Id.ValidateBtn);

            gValidateBtn.Click += GValidateBtn_Click;

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

            Userinformation = new Users();
            Userinformation.Username = gUsername.Text;
            Userinformation.Password = gPassword.Text;

            for (int i = 0; i< UserList.Count; i++)
            {
                if(UserList.ElementAt<Users>(i).Username.Equals(Userinformation.Username) && UserList.ElementAt<Users>(i).Password.Equals(Userinformation.Password))
                {
                    Userinformation.Id = UserList.ElementAt<Users>(i).Id;
                    somethingempty = false;
                    break;
                }
                else
                {
                    Userinformation.Id = "0";
                    somethingempty = true;
                }
            
            }
            

            return somethingempty;

        }
        private async void GValidateBtn_Click(object sender, EventArgs e)
        {
           
            await RefreshUserItems();
            UserList = await UserTable.ToListAsync();
            if ( verifyData(gUsername.Text, gPassword.Text) == false) {

             ValidateEvent.Invoke(this, new OnValidateEventArgs(Userinformation.Id, Userinformation.Username, Userinformation.Password));
             this.Dismiss();

            }
            else
            {
                ValidateEvent.Invoke(this, new OnValidateEventArgs(Userinformation.Id, "empty", "empty"));

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