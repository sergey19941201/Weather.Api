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
using SQLite;
using System.Linq;

namespace Weather.Droid
{
    [Activity(Label = "Edit", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class EditActivity : ActionBarActivity
    {
        private SupportToolbar mToolbar;

        private MYActionBarDrawerToggle mDrawerToggle;
        private DrawerLayout mDrawerLayout;
        private ListView mLeftDrawer;

        private ListView lvCities;

        //LISTVIEW WITH IMAGE
        List<clases.cls_Libro> libros = new List<clases.cls_Libro>();
        List<clases.citiesClass> citiesList = new List<clases.citiesClass>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.edit);

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

            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
            var db = new SQLiteConnection(dbPath);
            var table = db.Table<DB.city>();

            //this shitcode is to tell the app how items of listview must it create
            //!!!IMPORTANT  .AsEnumerable().Reverse() is for sorting cities at the end of the list
            foreach (var Item in table.AsEnumerable().Reverse())
            {
                //LISTVIEW WITH IMAGE
                if (Item.City == null || Item.City == "" || Item.City == " ")
                {
                    //break;
                }
                else
                {
                    // citiesList.Add(new clases.citiesClass(1, Convert.ToString(Item.Id)));
                    citiesList.Add(new clases.citiesClass(Convert.ToString(Item.Id)));
                }
            }
            //this shitcode is to tell the app how items of listview must it create ENDED

            /*citiesList.Add(new clases.citiesClass(2, "город2"));
            citiesList.Add(new clases.citiesClass(3, "город3"));
            citiesList.Add(new clases.citiesClass(4, "город4------"));*/
            lvCities = FindViewById<ListView>(Resource.Id.LVcities);
            lvCities.Adapter = new adapter.cityListViewAdapter(this, citiesList);

            lvCities.ItemClick += LvCities_ItemClick;
            //LISTVIEW WITH IMAGE ENDS HERE
            /*mToolbar.SetBackgroundResource(Resource.Drawable.toolbar);
            SetSupportActionBar(mToolbar);
            */
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

            //fonts
            var textView2 = FindViewById<TextView>(Resource.Id.textView2);
            Typeface tf = Typeface.CreateFromAsset(Assets, "dosis.book.ttf");
            textView2.SetTypeface(tf, TypefaceStyle.Bold);
            //fonts ENDED
            FindViewById<Button>(Resource.Id.closeBN).Click += EditActivity_Click;
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

        private void EditActivity_Click(object sender, EventArgs e)
        {
            FinishAffinity();
        }

        private void LvCities_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, Convert.ToString(citiesList[e.Position].Nombre), ToastLength.Short).Show();
        }

        private void MLeftDrawer_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (e.Position == 0)
            {
                StartActivity(new Intent(this, typeof(MAIN_Activity)));
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
    }
}