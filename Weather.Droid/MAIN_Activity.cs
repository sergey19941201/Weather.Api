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
using Android.Content.PM;
using Android.Locations;
using Android.Util;
using System.Threading.Tasks;
using Weather.Api;
using WA = Weather.Api;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using System.IO;
using Android.Graphics;
using SQLite;

namespace Weather.Droid
{
    [Activity(Label = "MAIN_Activity", Icon = "@drawable/icon", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MAIN_Activity : AppCompatActivity, ILocationListener
    {
        //location
        public static string _addressTextGlobalVariable, _locationTextGlobalVariable, cityTextGlobal;
        public static bool cityButtonPressedIndicGlob, firstAppLaunchGlob, ipressedGlob;
        public static string _x, _y;
        //private bool cityExists;

        //database
        public static string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
        //database ended

        public void OnProviderDisabled(string provider) { }

        public void OnProviderEnabled(string provider) { }

        public void OnStatusChanged(string provider, Availability status, Bundle extras) { }
        static readonly string TAG = "X:" + typeof(MAIN_Activity).Name;

        Location _currentLocation;
        LocationManager _locationManager;

        string _locationProvider;

        private bool isGPSenabled;
        static readonly string tag = "X:" + typeof(MAIN_Activity).Name;
        LocationManager locMgr;
        protected override void OnResume()
        {
            base.OnResume();
            string Provider = LocationManager.GpsProvider;

            if (locMgr.IsProviderEnabled(Provider))
            {
                isGPSenabled = true;
                locMgr.RequestLocationUpdates(Provider, 2000, 1, this);
            }
            else
            {
                isGPSenabled = false;
                Log.Info(tag, Provider + " is not available. Does the device have location services enabled?");
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        async Task<Address> ReverseGeocodeCurrentLocation()
        {

            Geocoder geocoder = new Geocoder(this);

            if (_currentLocation != null)
            {
                //crashes here:
                IList<Address> addressList =
                await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);
                Address address = addressList.FirstOrDefault();
                return address;
            }

            Address address1 = null;
            return address1;
        }

        void DisplayAddress(Address address)
        {
            if (address != null)
            {
                StringBuilder deviceAddress = new StringBuilder();
                for (int i = 0; i < address.MaxAddressLineIndex; i++)
                {
                    deviceAddress.AppendLine(address.GetAddressLine(i));
                }
                // Remove the last comma from the end of the address.
                _addressTextGlobalVariable = deviceAddress.ToString();
            }
            else
            {
                _addressTextGlobalVariable = "Unable to determine the address. Try again in a few minutes.";
            }
        }

        public async void OnLocationChanged(Location location)
        {
            //       POINT 1
            _currentLocation = location;
            if (_currentLocation == null)
            {
                _locationTextGlobalVariable = "Unable to determine your location. Try again in a short while.";
            }
            else
            {
                _locationTextGlobalVariable = string.Format("{0:f6},{1:f6}", _currentLocation.Latitude, _currentLocation.Longitude);
                Address address = await ReverseGeocodeCurrentLocation();
                DisplayAddress(address);
            }
        }
        //location ends here


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MAIN_);

            locMgr = GetSystemService(Context.LocationService) as LocationManager;

            cityTextGlobal = null;

            FindViewById<Button>(Resource.Id.get_address_button).Click += FindLocBN_clicked;

            FindViewById<Button>(Resource.Id.EnterYourCity).Click += MAIN_Activity_Click;

            FindViewById<Button>(Resource.Id.closeBN).Click += MAIN_Activity_Click1;

            //Create the DB:
            DB.myDBRepo dbr = new DB.myDBRepo();
            dbr.CreateDB();
            //Create table:
            dbr.CreateTable();
            dbr.CreateIndicatorTable();
            //fonts
            var ORtextView = FindViewById<TextView>(Resource.Id.ORtextView);
            var enterBN = FindViewById<Button>(Resource.Id.EnterYourCity);
            Typeface tf = Typeface.CreateFromAsset(Assets, "dosis.book.ttf");
            ORtextView.SetTypeface(tf, TypefaceStyle.Bold);
            enterBN.SetTypeface(tf, TypefaceStyle.Bold);
            //fonts ended
            //InitializeLocationManager();
        }

        private void MAIN_Activity_Click1(object sender, EventArgs e)
        {
            FinishAffinity();
        }

        //location
        void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
            Log.Debug(TAG, "Using " + _locationProvider + ".");
        }

        private void MAIN_Activity_Click(object sender, EventArgs e)
        {
            InitializeLocationManager();
            firstAppLaunchGlob = false;
            cityButtonPressedIndicGlob = true;
            ipressedGlob = true;
            cityTextGlobal = FindViewById<EditText>(Resource.Id.cityNameET).Text;
            //Insert city to db
            DB.myDBRepo dbr = new DB.myDBRepo();
            dbr.InsertcityOrCoordIndicator("city");

            if (cityTextGlobal == null || cityTextGlobal == "" || cityTextGlobal == " " || cityTextGlobal == "  ")
            {
                Toast.MakeText(this, "Enter your city name", ToastLength.Long).Show();
            }
            else { StartActivity(new Intent(this, typeof(LoadingActivity))); }
        }

        public async void FindLocBN_clicked(object sender, EventArgs e)
        {
            InitializeLocationManager();
            if (isGPSenabled == false)
            {
                turnGPSon();
            }
            else
            {
                cityButtonPressedIndicGlob = false;
                cityTextGlobal = null;
                ipressedGlob = false;
                if (_currentLocation == null)
                {
                    _addressTextGlobalVariable = "Can't determine the current address. Try again in a few minutes.";
                }
                try
                {
                    Address address = await ReverseGeocodeCurrentLocation();
                    DisplayAddress(address);
                    //spliting string with coordinates
                    string[] values = _locationTextGlobalVariable.Split(',');
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = values[i].Trim();
                        if (i == 0)
                        {
                            _x = values[i];
                        }

                        if (i == 1)
                        {
                            _y = values[i];
                        }
                    }
                    //Insert Coordinates to DB 
                    DB.myDBRepo dbr = new DB.myDBRepo();
                    dbr.InsertCoordinates(_x, _y);
                    dbr.InsertcityOrCoordIndicator("coord");

                    StartActivity(new Intent(this, typeof(LoadingActivity)));
                }
                catch
                {
                    Toast.MakeText(this, "Determination failed", ToastLength.Short).Show();
                }
            }
        }
        //My method to turn the GPS on
        private void turnGPSon()
        {
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
            builder.SetTitle("Location services are disabled.");
            builder.SetMessage("Turn them on?");
            builder.SetCancelable(true);
            builder.SetPositiveButton("No", (object sender1, DialogClickEventArgs e1) =>
            { });
            builder.SetNegativeButton("Yes", (object sender1, DialogClickEventArgs e1) =>
            {
                StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
            });
            Android.App.AlertDialog dialog = builder.Create();
            dialog.Show();
        }
        //My method to turn the GPS on ENDED
    }
}