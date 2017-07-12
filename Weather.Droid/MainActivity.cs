using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;

using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Views;
using Android.Content.Res;
using System.Collections.Generic;
using Android.Content.PM;
using Android.Graphics;
using WA = Weather.Api;
using System;
using System.Json;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using Weather.Api;
using Android.Content;
using System.Threading;
using SQLite;

namespace Weather.Droid
{
    [Activity(Icon = "@drawable/icon", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : ActionBarActivity
    {
        /*//location
        TextView _addressText;
        TextView _locationText;
        //location ends here


        //local variables for coordinates
        public static string _x, _y;*/

        private SupportToolbar mToolbar;

        private MYActionBarDrawerToggle mDrawerToggle;
        private DrawerLayout mDrawerLayout;
        private ListView mLeftDrawer;
        private ArrayAdapter mLeftAdapter;
        private List<string> mLeftDataSet;
        //variables for current datatime
        private string monthStr, dayStr;

        private DB.myDBRepo dbr = new DB.myDBRepo();
        private string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");

        ISharedPreferences pref = Application.Context.GetSharedPreferences("WeatherData", FileCreationMode.Private);

        //LISTVIEW WITH IMAGE
        List<clases.cls_Libro> libros = new List<clases.cls_Libro>();
        /*
        protected override void OnResume()
        {
            base.OnResume();

            Toast.MakeText(this, "OnResume", ToastLength.Short).Show();
        } 
        */
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            var db = new SQLiteConnection(dbPath);

            //fonts
            var textview1 = FindViewById<TextView>(Resource.Id.location_text);
            var textview2 = FindViewById<TextView>(Resource.Id.address_text);
            var textview3 = FindViewById<TextView>(Resource.Id.city);
            var textview4 = FindViewById<TextView>(Resource.Id.temperature);
            var textview5 = FindViewById<TextView>(Resource.Id.dataTextView);
            var textview6 = FindViewById<TextView>(Resource.Id.weatherDescriptionTextView);
            var textview7 = FindViewById<TextView>(Resource.Id.windSpeedTextView);
            Typeface tf = Typeface.CreateFromAsset(Assets, "dosis.book.ttf");
            textview1.SetTypeface(tf, TypefaceStyle.Bold);
            textview2.SetTypeface(tf, TypefaceStyle.Bold);
            textview3.SetTypeface(tf, TypefaceStyle.Bold);
            textview4.SetTypeface(tf, TypefaceStyle.Bold);
            textview5.SetTypeface(tf, TypefaceStyle.Bold);
            textview6.SetTypeface(tf, TypefaceStyle.Bold);
            textview7.SetTypeface(tf, TypefaceStyle.Bold);
            //fonts ended
            textview6.Text = pref.GetString("weatherDescGlob", String.Empty);
            //current data
            getCurrentData();
            //current data ended

            MAIN_Activity.firstAppLaunchGlob = false;

            //mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            mLeftDrawer = FindViewById<ListView>(Resource.Id.left_drawer);

            //LISTVIEW WITH IMAGE
            libros.Add(new clases.cls_Libro(1, "Home", "PATRICIA BRIGGS1", 350, 100));
            libros.Add(new clases.cls_Libro(2, "Edit", "PATRICIA BRIGGS2", 430, 70));
            libros.Add(new clases.cls_Libro(3, "Settings", "PATRICIA BRIGGS3", 350, 100));
            libros.Add(new clases.cls_Libro(4, "Report", "PATRICIA BRIGGS4", 350, 100));

            ListView lwLibros = FindViewById<ListView>(Resource.Id.left_drawer);
            lwLibros.Adapter = new adapter.adapter_listview(this, libros);
            //LISTVIEW WITH IMAGE ENDS HERE
            //mToolbar.SetBackgroundResource(Resource.Drawable.toolbar);
            //SetSupportActionBar(mToolbar);

            mDrawerToggle = new MYActionBarDrawerToggle(this, mDrawerLayout, Resource.String.openDrawer, Resource.String.closeDrawer);

            //mDrawerLayout.SetDrawerListener(mDrawerToggle);
            //SupportActionBar.SetHomeButtonEnabled(true);
            //IMPORTANT COMMENT!!!!!!!!!!
            /*
             John Mckay3 месяца назад (изменено)
             Great work as always, but as mentioned in another comment,
             I had to call SupportActionBar.SetDisplayHomeAsUpEnabled(true); instead of SupportActionBar.SetDisplayShowTitleEnabled(true); to get the icon to show 
             */
            //SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //mDrawerToggle.SyncState();

            mLeftDrawer.ItemClick += MLeftDrawer_ItemClick;

            city_or_coord();

            var table = db.Table<DB.city>();

            foreach (var i in table)
            {
                if (pref.GetString("City", String.Empty) == i.City)
                {
                    dbr.RemoveCity(i.Id);
                }
            }

            dbr.InsertCity((pref.GetString("City", String.Empty)));

            if (MAIN_Activity.cityButtonPressedIndicGlob == false)
            {
                FindViewById<TextView>(Resource.Id.address_text).Text = pref.GetString("addressTextGlobalVariable", String.Empty);
                FindViewById<TextView>(Resource.Id.location_text).Text = pref.GetString("locationTextGlobalVariable", String.Empty);
            }
            else
            {
                FindViewById<TextView>(Resource.Id.address_text).Text = "";
                FindViewById<TextView>(Resource.Id.location_text).Text = "";
            }

            if (dbr.GetAllRecordsC_F() == "celsius")
            {
                FindViewById<TextView>(Resource.Id.temperature).Text = pref.GetString("tempGlobal", String.Empty) + "° C";
                textview7.Text = "wind speed: " + pref.GetString("windSpeedGlob", String.Empty) + " m/s";
            }
            else
            {
                FindViewById<TextView>(Resource.Id.temperature).Text = pref.GetString("tempGlobal", String.Empty) + "° F";
                textview7.Text = "wind speed: " + pref.GetString("windSpeedGlob", String.Empty) + " mph";
            }

            FindViewById<TextView>(Resource.Id.city).Text = pref.GetString("City", String.Empty);

            imageMeth();
            FindViewById<Button>(Resource.Id.closeBN).Click += MainActivity_Click;
            //button to open Left Drawer
            FindViewById<Button>(Resource.Id.leftDrawerBN).Click += delegate
            {
                if (mDrawerLayout.IsDrawerOpen(mLeftDrawer))
                {
                    mDrawerLayout.CloseDrawer(mLeftDrawer);
                }
                else
                {
                    mDrawerLayout.OpenDrawer(mLeftDrawer);
                }
            };
        }

        private void MainActivity_Click(object sender, EventArgs e)
        {
            FinishAffinity();
        }

        private string imageMeth()
        {
            int hour = DateTime.Now.ToLocalTime().Hour;
            if (pref.GetString("weatherGlob", String.Empty) == "Thunderstorm")
            {
                if (hour > 4 && hour < 22)
                {
                    FindViewById<ImageView>(Resource.Id.imageViewWeather).SetBackgroundResource(Resource.Drawable.rainthunder);
                }
                else
                {
                    FindViewById<ImageView>(Resource.Id.imageViewWeather).SetBackgroundResource(Resource.Drawable.nightThunder);
                }
            }
            if (pref.GetString("weatherGlob", String.Empty) == "Snow")
            {
                if (hour > 4 && hour < 22)
                {
                    FindViewById<ImageView>(Resource.Id.imageViewWeather).SetBackgroundResource(Resource.Drawable.snow);
                }
                else
                {
                    FindViewById<ImageView>(Resource.Id.imageViewWeather).SetBackgroundResource(Resource.Drawable.nightSnow);
                }
            }
            if (pref.GetString("weatherGlob", String.Empty) == "Clouds")
            {
                if (hour > 4 && hour < 22)
                {
                    FindViewById<ImageView>(Resource.Id.imageViewWeather).SetBackgroundResource(Resource.Drawable.cloud_);
                }
                else
                {
                    FindViewById<ImageView>(Resource.Id.imageViewWeather).SetBackgroundResource(Resource.Drawable.nightCloud);
                }
            }
            if (pref.GetString("weatherGlob", String.Empty) == "Drizzle" || pref.GetString("weatherGlob", String.Empty) == "Rain")
            {
                if (hour > 4 && hour < 22)
                {
                    FindViewById<ImageView>(Resource.Id.imageViewWeather).SetBackgroundResource(Resource.Drawable.rain);
                }
                else
                {
                    FindViewById<ImageView>(Resource.Id.imageViewWeather).SetBackgroundResource(Resource.Drawable.nightRain);
                }
            }

            if (pref.GetString("weatherGlob", String.Empty) == "Atmosphere"
                || pref.GetString("weatherGlob", String.Empty) == "Mist" || pref.GetString("weatherGlob", String.Empty) == "Smoke" || pref.GetString("weatherGlob", String.Empty) == "Haze"
                || pref.GetString("weatherGlob", String.Empty).Contains("Sand") || pref.GetString("weatherGlob", String.Empty) == "Fog" || pref.GetString("weatherGlob", String.Empty) == "Dust"
                || pref.GetString("weatherGlob", String.Empty).Contains("Volcanic") || pref.GetString("weatherGlob", String.Empty) == "Squalls" || pref.GetString("weatherGlob", String.Empty) == "Tornado"
                )
            {
                FindViewById<ImageView>(Resource.Id.imageViewWeather).SetBackgroundResource(Resource.Drawable.fog);
            }

            if (pref.GetString("weatherGlob", String.Empty) == "Clear" || pref.GetString("weatherGlob", String.Empty) == "Clear sky")
            {
                if (hour > 4 && hour < 22)
                {
                    FindViewById<ImageView>(Resource.Id.imageViewWeather).SetBackgroundResource(Resource.Drawable.sun);
                }
                else
                {
                    FindViewById<ImageView>(Resource.Id.imageViewWeather).SetBackgroundResource(Resource.Drawable.moon);
                }
            }
            if (pref.GetString("weatherGlob", String.Empty) == "Extreme" || pref.GetString("weatherGlob", String.Empty) == "Tropical storm" || pref.GetString("weatherGlob", String.Empty).Contains("Cold") || pref.GetString("weatherGlob", String.Empty) == "Hot"
                || pref.GetString("weatherGlob", String.Empty) == "Windy" || pref.GetString("weatherGlob", String.Empty).Contains("Hail") || pref.GetString("weatherGlob", String.Empty) == "Additional" || pref.GetString("weatherGlob", String.Empty) == "Calm"
                || pref.GetString("weatherGlob", String.Empty) == "Light breeze" || pref.GetString("weatherGlob", String.Empty) == "Gentle breeze" || pref.GetString("weatherGlob", String.Empty) == "Moderate breeze" || pref.GetString("weatherGlob", String.Empty) == "Fresh breeze"
                || pref.GetString("weatherGlob", String.Empty) == "Strong breeze" || pref.GetString("weatherGlob", String.Empty) == "High wind, near gale" || pref.GetString("weatherGlob", String.Empty) == "Gale" || pref.GetString("weatherGlob", String.Empty) == "Severe gale"
                || pref.GetString("weatherGlob", String.Empty) == "Storm" || pref.GetString("weatherGlob", String.Empty) == "Violent storm" || pref.GetString("weatherGlob", String.Empty) == "Hurricane"
                )
            {
                FindViewById<ImageView>(Resource.Id.imageViewWeather).SetBackgroundResource(Resource.Drawable.wind);
            }

            return pref.GetString("weatherGlob", String.Empty);
        }

        //getting current data method
        private void getCurrentData()
        {
            int month = DateTime.Now.ToLocalTime().Month;
            int day = DateTime.Now.ToLocalTime().Day;
            string dayOfWeek = Convert.ToString(DateTime.Now.ToLocalTime().DayOfWeek);

            if (month == 1) { monthStr = "January"; }
            if (month == 2) { monthStr = "February"; }
            if (month == 3) { monthStr = "March"; }
            if (month == 4) { monthStr = "April"; }
            if (month == 5) { monthStr = "May"; }
            if (month == 6) { monthStr = "June"; }
            if (month == 7) { monthStr = "July"; }
            if (month == 8) { monthStr = "August"; }
            if (month == 9) { monthStr = "September"; }
            if (month == 10) { monthStr = "October"; }
            if (month == 11) { monthStr = "Nowember"; }
            if (month == 12) { monthStr = "December"; }

            if (month == 12 || month == 1 || month == 2)
            {
                FindViewById<RelativeLayout>(Resource.Id.RelativeLayout).SetBackgroundResource(Resource.Drawable.wintermaterial);
            }
            if (month == 3 || month == 4 || month == 5)
            {
                FindViewById<RelativeLayout>(Resource.Id.RelativeLayout).SetBackgroundResource(Resource.Drawable.springmaterial);
            }
            if (month == 6 || month == 7 || month == 8)
            {
                FindViewById<RelativeLayout>(Resource.Id.RelativeLayout).SetBackgroundResource(Resource.Drawable.summermaterial);
            }
            if (month == 9 || month == 10 || month == 11)
            {
                FindViewById<RelativeLayout>(Resource.Id.RelativeLayout).SetBackgroundResource(Resource.Drawable.autumnmaterial);
            }

            var textview5 = FindViewById<TextView>(Resource.Id.dataTextView);
            textview5.Text = Convert.ToString(dayOfWeek) + ", " + monthStr + " " + day;
        }
        //getting current data method ended

        private void MLeftDrawer_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (e.Position == 0)
            {
                StartActivity(new Intent(this, typeof(MAIN_Activity)));
            }
            if (e.Position == 1)
            {
                StartActivity(new Intent(this, typeof(EditActivity)));
            }
            if (e.Position == 2)
            {
                StartActivity(new Intent(this, typeof(SettingsActivity)));
            }
            if (e.Position == 3)
            {
                StartActivity(new Intent(this, typeof(ReportActivity)));
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            mDrawerToggle.OnOptionsItemSelected(item);
            return base.OnOptionsItemSelected(item);
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            mDrawerToggle.OnConfigurationChanged(newConfig);
        }

        public void city_or_coord()
        {
            //string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
            var db = new SQLiteConnection(dbPath);
            var table = db.Table<DB.cityOrCoordIndicator>();

            foreach (var i in table)
            {
                if (i.CityOrCoord == "city")
                {
                    MAIN_Activity.cityButtonPressedIndicGlob = true;
                }
                if (i.CityOrCoord == "coord")
                {
                    MAIN_Activity.cityButtonPressedIndicGlob = false;
                }
            }
        }
    }
}

