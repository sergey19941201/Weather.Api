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
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net;
using Weather.Api;
using Android.Content;
using System.Threading;

namespace Weather.Droid
{
    [Activity(Label = "Settings", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SettingsActivity : ActionBarActivity
    {
        private SupportToolbar mToolbar;

        private MYActionBarDrawerToggle mDrawerToggle;
        private DrawerLayout mDrawerLayout;
        private ListView mLeftDrawer;
        private ArrayAdapter mLeftAdapter;
        private List<string> mLeftDataSet;
        public static string c_f_Global;

        //LISTVIEW WITH IMAGE
        List<clases.cls_Libro> libros = new List<clases.cls_Libro>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.settings);

            //fonts
            var textview1 = FindViewById<TextView>(Resource.Id.textView1);
            var radiobutton1 = FindViewById<RadioButton>(Resource.Id.celsiusRB);
            var radiobutton2 = FindViewById<RadioButton>(Resource.Id.FahrenheitRB);
            var textView2 = FindViewById<TextView>(Resource.Id.textView2);
            Typeface tf = Typeface.CreateFromAsset(Assets, "dosis.book.ttf");
            textview1.SetTypeface(tf, TypefaceStyle.Bold);
            radiobutton1.SetTypeface(tf, TypefaceStyle.Bold);
            radiobutton2.SetTypeface(tf, TypefaceStyle.Bold);
            textView2.SetTypeface(tf, TypefaceStyle.Bold);
            //fonts ended

            DB.myDBRepo dbr = new DB.myDBRepo();

            // mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
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
             John Mckay3 мес€ца назад (изменено)
             Great work as always, but as mentioned in another comment,
             I had to call SupportActionBar.SetDisplayHomeAsUpEnabled(true); instead of SupportActionBar.SetDisplayShowTitleEnabled(true); to get the icon to show 
             */
            //SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //mDrawerToggle.SyncState();

            mLeftDrawer.ItemClick += MLeftDrawer_ItemClick;

            //Radiogroup
            RadioGroup radioGroup = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
            FindViewById<RadioButton>(Resource.Id.celsiusRB).Click += delegate
            {
                c_f_Global = "celsius";
                dbr.InsertCelsius_Fahrenheit("celsius");
            };
            FindViewById<RadioButton>(Resource.Id.FahrenheitRB).Click += delegate
            {
                c_f_Global = "fahrenheit";
                dbr.InsertCelsius_Fahrenheit("fahrenheit");
            };
            //this snippet sets radiobutton by default
            if (dbr.GetAllRecordsC_F() == "celsius")
            {
                FindViewById<RadioButton>(Resource.Id.celsiusRB).Checked = true;
            }
            else
            {
                FindViewById<RadioButton>(Resource.Id.FahrenheitRB).Checked = true;
            }
            //this snippet sets radiobutton by default ended
            //Radiogroup ended

            /*FindViewById<Button>(Resource.Id.button1).Click += delegate
              {
                  Toast.MakeText(this, dbr.GetAllRecordsC_F(), ToastLength.Short).Show();
              };*/
            FindViewById<Button>(Resource.Id.closeBN).Click += SettingsActivity_Click;
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

        private void SettingsActivity_Click(object sender, EventArgs e)
        {
            FinishAffinity();
        }

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

        public override void OnBackPressed()
        {
            StartActivity(new Intent(this, typeof(LoadingActivity)));
        }
    }
}