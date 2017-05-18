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
using ClassLibrary;
using Android.Graphics;

namespace BambuShootProject.Droid
{
    public class DatabaseAdapter : BaseAdapter<Reports>
    {
        Activity activity;
        int layoutResourceId;
        List<Reports> reports = new List<Reports>();
        List<Users> users = new List<Users>();
        int[] gAlternatingColors;

        public DatabaseAdapter( Activity activity, int layoutResourceId)
        {
            this.activity = activity;
            this.layoutResourceId = layoutResourceId;
            gAlternatingColors = new int[] { 0xF2F2F2, 0x009900 };
        }
        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            var row = convertView;
            var currentItem = this [position];

            if (row == null)
            {
                var inflater = activity.LayoutInflater;
                row = inflater.Inflate(layoutResourceId, parent, false);
            }
            
            row.SetBackgroundColor(GetColorFromInteger(gAlternatingColors[position % gAlternatingColors.Length]));

          //  int userreport = reports[position].userid;

            TextView username = row.FindViewById<TextView>(Resource.Id.txtUsername);
            //username.Text = users[userreport].username;
            username.Text = reports[position].userid;

            TextView imagetitle = row.FindViewById<TextView>(Resource.Id.txtImageTitle);
            imagetitle.Text = reports[position].imagetitle;

            TextView location = row.FindViewById<TextView>(Resource.Id.txtLocation);
            location.Text = reports[position].location;

            TextView nameofspecies = row.FindViewById<TextView>(Resource.Id.txtNameofspecies);
            nameofspecies.Text = reports[position].nameofspecies;

            TextView dateofharvest = row.FindViewById<TextView>(Resource.Id.txtDateofHarvest);
            dateofharvest.Text = reports[position].dateofharvest;

            if ((position % 2) == 1)
            {
                //Green background, set text white
                username.SetTextColor(Color.White);
                imagetitle.SetTextColor(Color.White);
                location.SetTextColor(Color.White);
                nameofspecies.SetTextColor(Color.White);
                dateofharvest.SetTextColor(Color.White);
            }

            else
            {
                //White background, set text black
                username.SetTextColor(Color.Black);
                imagetitle.SetTextColor(Color.Black);
                location.SetTextColor(Color.Black);
                nameofspecies.SetTextColor(Color.Black);
                dateofharvest.SetTextColor(Color.Black);
            }

            return row;

        }

        private Color GetColorFromInteger(int color)
        {
            return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }

        public override Reports this[int position]
        {
            get
            {
                return reports[position];
            }
        }

        public override int Count
        {
            get { return reports.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public void Clear()
        {
            reports.Clear();
            users.Clear();
            NotifyDataSetChanged();
        }
        public void AddReports(Reports item)
        {
            reports.Add(item);
            NotifyDataSetChanged();
        }
        public void AddUsers(Users item)
        {
            users.Add(item);
            NotifyDataSetChanged();
        }

    }
}