using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace BambuShootProject.Droid
{
	[Activity (Label = "Bambú Shoot App", MainLauncher = true, Icon = "@drawable/BambuShootIcon")]
	public class MainActivity : Activity
	{

        private Button gBtnInstructions;            // Initialize Button as Global Variable

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            gBtnInstructions = FindViewById<Button>(Resource.Id.instructionsbtn);   //Link Button variable with Axml button
            gBtnInstructions.Click += gBtnInstructions_Click;

        }

        private void gBtnInstructions_Click(object sender, EventArgs e)
        {
            //Pull the Instruction Dialog
            FragmentTransaction transaction = FragmentManager.BeginTransaction();  // Pull up the dialog from the activity
            Instructions_Dialog InstrucDialog = new Instructions_Dialog();
            InstrucDialog.Show(transaction,"dialog fragment");
        }


    }
}


