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
using Android.Views.Animations;
using WA = Weather.Api;
using Plugin.VersionTracking;
using SQLite;
using Android.Content.PM;
using Android.Net;
using Android.Graphics;

using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

namespace Weather.Droid
{


    [Activity(Label = "Simple Weather", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoadingActivity : Activity
    {
        //location
        TextView _addressText;
        TextView _locationText;
        //location ends here


        //local variables for coordinates
        public static string _x, _y;
        private DB.myDBRepo dbr = new DB.myDBRepo();


        /*
        protected override void OnResume()
        {
            base.OnResume();

            Toast.MakeText(this, "OnResumeLoading", ToastLength.Short).Show();
        }
        */

        protected override void OnCreate(Bundle savedInstanceState)
        {
            MobileCenter.Start("005f8f23-ea2c-48ac-962f-b4929664c618",
                    typeof(Analytics), typeof(Crashes));

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.anim_layout);

            var restartBn = FindViewById<Button>(Resource.Id.restartBN);

            //using plugin when it is the first app launch
            CrossVersionTracking.Current.Track();
            var vt = CrossVersionTracking.Current;
            vt.OnFirstLaunchOfVersion("1.0", () => firstMethod());
            //using plugin when it is the first app launch ended

            //checking internet connection
            ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);

            NetworkInfo activeConnection = connectivityManager.ActiveNetworkInfo;
            bool isOnline = (activeConnection != null) && activeConnection.IsConnected;
            //checking internet connection ended

            if (isOnline == true)
            {
                restartBn.Visibility = ViewStates.Gone;
                /*_x = MAIN_Activity._x;
                _y = MAIN_Activity._y;*/
                //Create the DB:
                DB.myDBRepo dbr = new DB.myDBRepo();
                dbr.CreateDB();
                //Create table:
                dbr.CreateTable();
                dbr.CreateTableC_F();
                dbr.CreateIndicatorTable();
                dbr.CreateCoordsTable();

                //setting celsiusOr_fahrenheit by default
                if (dbr.GetAllRecordsC_F() == "celsius")
                {
                    SettingsActivity.c_f_Global = "celsius";
                }
                else
                {
                    SettingsActivity.c_f_Global = "fahrenheit";
                }
                //setting celsiusOr_fahrenheit by default ended

                if (MAIN_Activity.firstAppLaunchGlob == false && MAIN_Activity.ipressedGlob == false)
                {
                    dbr.GetAllRecords();
                }

                Animation myAnimation = AnimationUtils.LoadAnimation(this, Resource.Animation.MyAnimation);
                ImageView myImage = FindViewById<ImageView>(Resource.Id.imageView1);

                myImage.StartAnimation(myAnimation);
                dbr.CreateIndicatorTable();

                string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
                var db = new SQLiteConnection(dbPath);
                var cityOrCoordIndicator = db.Table<DB.cityOrCoordIndicator>();

                MainActivity ma = new MainActivity();

                ma.city_or_coord();

                if (MAIN_Activity.cityButtonPressedIndicGlob == false)
                {
                    var coordinates = db.Table<DB.coordinates>();
                    foreach (var b in coordinates)
                    {
                        _x = b._X;
                        _y = b._Y;
                    }
                }
                if (MAIN_Activity.firstAppLaunchGlob == false)
                {
                    Displaying();
                }

                ISharedPreferences pref = Application.Context.GetSharedPreferences("WeatherData",FileCreationMode.Private);
                ISharedPreferencesEditor edit = pref.Edit();
                if (MAIN_Activity._addressTextGlobalVariable != null)
                {
                    edit.PutString("addressTextGlobalVariable", MAIN_Activity._addressTextGlobalVariable);
                }
                if (MAIN_Activity._locationTextGlobalVariable != null)
                {
                    edit.PutString("locationTextGlobalVariable", MAIN_Activity._locationTextGlobalVariable);
                }
                edit.Apply();
            }
            else
            {
                if (MAIN_Activity.firstAppLaunchGlob == false)
                {
                    Toast.MakeText(this, "No Internet Connection.\nTurn the Internet connection on and click \"Restart\"", ToastLength.Long).Show();
                }
                //fonts
                Typeface tf = Typeface.CreateFromAsset(Assets, "dosis.book.ttf");
                restartBn.SetTypeface(tf, TypefaceStyle.Bold);
                //fonts ended
                restartBn.Visibility = ViewStates.Visible;
                restartBn.Click += RestartBn_Click;
            }
            deleteIncorrectCity();
            FindViewById<Button>(Resource.Id.closeBN).Click += LoadingActivity_Click;
            FindViewById<Button>(Resource.Id.editBN).Click += LoadingActivity_Click1;
        }

        private void LoadingActivity_Click1(object sender, EventArgs e)
        {
            StartActivity(new Intent(this, typeof(EditActivity)));
        }

        private void LoadingActivity_Click(object sender, EventArgs e)
        {
            FinishAffinity();
        }

        private void deleteIncorrectCity()
        {
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
            var db = new SQLiteConnection(dbPath);
            var table = db.Table<DB.city>();
            dbr.CreateTable();

            foreach (var Item in table)
            {
                //we need to delete something like this "LosAngeles". "Los Angeles" will be correct.
                int countUppersChars = 0;

                //IMPORTANT!!! This checkup: "Item.City != null" is very important here to prevent this error: "Object reference not set to an instance of an object"
                if (Item.City != null)
                {
                    foreach (char m in Item.City)
                    {

                        if (Char.IsUpper(m))
                        {
                            //Toast.MakeText(this, "L", ToastLength.Short).Show();
                            countUppersChars++;
                        }
                        //Toast.MakeText(this, Convert.ToString(countUppersChars), ToastLength.Short).Show();
                        if (countUppersChars >= 2)
                        {
                            if (Item.City.Contains(" "))
                            {
                                //Toast.MakeText(this, Item.City + " Correct. Id in DB: " + Item.Id, ToastLength.Short).Show();
                            }
                            else
                            {
                                //Toast.MakeText(this, Item.City + " Incorrect. Id in DB: " + Item.Id, ToastLength.Short).Show();
                                try
                                {
                                    dbr.RemoveCity(Item.Id);
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                }
                //IMPORTANT!!! Deleting items with null values is very important here to prevent this error: "Object reference not set to an instance of an object"
                else if (Item.City == null)
                {
                    dbr.RemoveCity(Item.Id);
                }
            }
        }

        private void RestartBn_Click(object sender, EventArgs e)
        {
            StartActivity(new Intent(this, typeof(LoadingActivity)));
        }

        public async void Displaying()
        {
            WA.ParsingClass pr1 = new WA.ParsingClass();

            if (MAIN_Activity.cityButtonPressedIndicGlob == true)
            {
                //IMPORTANT!!! This Try-catch is here because if the user enters the incorrect city name the app crashes
                try
                {
                    await pr1.FetchAsync(MAIN_Activity.cityTextGlobal, StartActivity, new Intent(this, typeof(MainActivity)), SettingsActivity.c_f_Global);
                }
                catch
                {

                }
            }
            else if (MAIN_Activity.cityButtonPressedIndicGlob == false)
            {
                await pr1.FetchAsync(Convert.ToDouble(_x), Convert.ToDouble(_y), StartActivity, new Intent(this, typeof(MainActivity)), SettingsActivity.c_f_Global);
            }
        }

        private void firstMethod()
        {
            MAIN_Activity.firstAppLaunchGlob = true;
            StartActivity(new Intent(this, typeof(_1stOnlyActivity)));
        }
    }
}