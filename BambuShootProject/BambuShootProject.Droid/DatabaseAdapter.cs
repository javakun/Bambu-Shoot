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
using Java.Lang;

namespace com.BambuShoot.droid
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
     
            if (row == null)
            {
                var inflater = activity.LayoutInflater;
                row = inflater.Inflate(layoutResourceId, parent, false);
            }
            
            row.SetBackgroundColor(GetColorFromInteger(gAlternatingColors[position % gAlternatingColors.Length]));

           int userreport = int.Parse(reports[position].Userid)-1 ;

            TextView username = row.FindViewById<TextView>(Resource.Id.txtUsername);
            TextView imagetitle = row.FindViewById<TextView>(Resource.Id.txtImageTitle);
            TextView location = row.FindViewById<TextView>(Resource.Id.txtLocation);
            TextView nameofspecies = row.FindViewById<TextView>(Resource.Id.txtNameofspecies);
            TextView dateofharvest = row.FindViewById<TextView>(Resource.Id.txtDateofHarvest);
            TextView datetime = row.FindViewById<TextView>(Resource.Id.txtDatetime);
            TextView totalcountfiber = row.FindViewById<TextView>(Resource.Id.txtTotalCountFiber);
            TextView totalfiberdensity = row.FindViewById<TextView>(Resource.Id.txtTotalDensityFiber);
            username.Text = users[userreport].Username; 
            imagetitle.Text = reports[position].Imagetitle;
            location.Text = reports[position].Location;
            nameofspecies.Text = reports[position].Nameofspecies;
            dateofharvest.Text = reports[position].Dateofharvest;
            datetime.Text = reports[position].DateUtc.ToString();
            totalcountfiber.Text = reports[position].TotalSegCount.ToString();
            totalfiberdensity.Text = (System.Math.Round(reports[position].TotalFiberDensity,6)*100).ToString()+"%";

            if ((position % 2) == 1)
            {
                //Green background, set text white
                username.SetTextColor(Color.White);
                imagetitle.SetTextColor(Color.White);
                location.SetTextColor(Color.White);
                nameofspecies.SetTextColor(Color.White);
                dateofharvest.SetTextColor(Color.White);
                datetime.SetTextColor(Color.White);
                totalcountfiber.SetTextColor(Color.White);
                totalfiberdensity.SetTextColor(Color.White);
            }

            else
            {
                //White background, set text black
                username.SetTextColor(Color.Black);
                imagetitle.SetTextColor(Color.Black);
                location.SetTextColor(Color.Black);
                nameofspecies.SetTextColor(Color.Black);
                dateofharvest.SetTextColor(Color.Black);
                datetime.SetTextColor(Color.Black);
                totalcountfiber.SetTextColor(Color.Black);
                totalfiberdensity.SetTextColor(Color.Black);
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