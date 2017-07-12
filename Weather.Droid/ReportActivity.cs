using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Content.PM;
using Android.Support.V4.Widget;
using Android.Graphics;

namespace Weather.Droid
{
	[Activity(Label = "Tell us about troubles in app", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ReportActivity : ActionBarActivity
    {
        private SupportToolbar mToolbar;

        private MYActionBarDrawerToggle mDrawerToggle;
        private DrawerLayout mDrawerLayout;
        private ListView mLeftDrawer;
        private ArrayAdapter mLeftAdapter;
        private List<string> mLeftDataSet;

        //LISTVIEW WITH IMAGE
        List<clases.cls_Libro> libros = new List<clases.cls_Libro>();

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.report);

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
           // mToolbar.SetBackgroundResource(Resource.Drawable.toolbar);
            //SetSupportActionBar(mToolbar);

            mDrawerToggle = new MYActionBarDrawerToggle(this, mDrawerLayout, Resource.String.openDrawer, Resource.String.closeDrawer);

           // SupportActionBar.SetHomeButtonEnabled(true);

            //SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            //mDrawerToggle.SyncState();

            mLeftDrawer.ItemClick += MLeftDrawer_ItemClick;

            //EMAIL SENDIND
            var reportET = FindViewById<EditText>(Resource.Id.reportET);
            var sendBN = FindViewById<Button>(Resource.Id.sendBN);

            sendBN.Click += (s, e) =>
              {
                  Intent email = new Intent(Intent.ActionSend);
                  email.PutExtra(Intent.ExtraEmail,new string[] { "koshelnick1994@mail.ru" });
                  email.PutExtra(Intent.ExtraSubject, "Problems with weather app");
                  email.PutExtra(Intent.ExtraText, reportET.Text.ToString());
                  email.SetType("message/rfc822");
                  StartActivity(Intent.CreateChooser(email, "Send Email Via"));
              };
            //EMAIL SENDIND ENDS HERE

            //fonts
            var textView2 = FindViewById<TextView>(Resource.Id.textView2);
            Typeface tf = Typeface.CreateFromAsset(Assets, "dosis.book.ttf");
            reportET.SetTypeface(tf, TypefaceStyle.Bold);
            sendBN.SetTypeface(tf, TypefaceStyle.Bold);
            textView2.SetTypeface(tf, TypefaceStyle.Bold);
            //fonts ended
            FindViewById<Button>(Resource.Id.closeBN).Click += ReportActivity_Click;
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

        private void ReportActivity_Click(object sender, EventArgs e)
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
            if (e.Position == 2)
            {
                StartActivity(new Intent(this, typeof(SettingsActivity)));
            }
        }

        //FOR OPENING LEFT DRAWER WITH CLICKING A BUTTON ON TOOLBAR
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
        //FOR OPENING LEFT DRAWER WITH CLICKING A BUTTON ON TOOLBAR. ENDED
    }
}