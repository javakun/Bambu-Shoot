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
     

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
           


        }
    }
}


