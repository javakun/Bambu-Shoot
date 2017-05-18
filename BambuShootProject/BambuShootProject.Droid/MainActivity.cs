using System;
using Android.Graphics;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace BambuShootProject.Droid
{
	[Activity (Label = "Bambú Shoot", MainLauncher = true, Icon = "@drawable/BambuShootIcon", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation , ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainActivity : Activity
	{

        public Button gBtnInstructions;            // Initialize Button as Global Variable
        public Button gBtnLoadImage;
        public Button gBtnDatabase;
        public Button gBtnDataReportLib;
        public Button gBtnCreateUser;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
            gBtnLoadImage = FindViewById<Button>(Resource.Id.processnewimagebtn);
            gBtnDatabase = FindViewById<Button>(Resource.Id.databasebtn);
            gBtnDataReportLib = FindViewById<Button>(Resource.Id.mydatareportlibrarybtn);
            gBtnInstructions = FindViewById<Button>(Resource.Id.instructionsbtn);   //Link Button variable with Axml button
            gBtnCreateUser = FindViewById<Button>(Resource.Id.CreateUserBtn);

            gBtnCreateUser.Click += GBtnCreateUser_Click;
            gBtnLoadImage.Click += GBtnLoadImage_Click;
            gBtnInstructions.Click += GBtnInstructions_Click;
            gBtnDatabase.Click += GBtnDatabase_Click;
            gBtnDataReportLib.Click += GBtnDataReportLib_Click;

        }

        private void GBtnCreateUser_Click(object sender, EventArgs e)
        {
            //Pull the Create User Dialog
            FragmentTransaction transaction = FragmentManager.BeginTransaction();  // Pull up the dialog from the activity
            CreateUser_Dialog User_Dialog = new CreateUser_Dialog();
            User_Dialog.Show(transaction, "dialog fragment");
        }

        private void GBtnDataReportLib_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(DataReportLibPage));
            this.StartActivity(intent);
        }

        private void GBtnDatabase_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(DatabasePage));
            this.StartActivity(intent);
        }

        private void GBtnLoadImage_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(LoadImageActivity));
            this.StartActivity(intent);

        }

        private void GBtnInstructions_Click(object sender, EventArgs e)
        {
            //Pull the Instruction Dialog
            FragmentTransaction transaction = FragmentManager.BeginTransaction();  // Pull up the dialog from the activity
            Instructions_Dialog InstrucDialog = new Instructions_Dialog();
            InstrucDialog.Show(transaction,"dialog fragment");
        }


    }

}


