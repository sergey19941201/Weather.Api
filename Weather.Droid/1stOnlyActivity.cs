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
using Android.Graphics;

namespace Weather.Droid
{
    [Activity(Label = "_1stOnlyActivity", Icon = "@drawable/icon", Theme = "@style/MyTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class _1stOnlyActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Only1stStartScreen);

            FindViewById<Button>(Resource.Id.startBn).Click += _1stOnlyActivity_Click;
            //fonts
            var textview1 = FindViewById<TextView>(Resource.Id.textView1);
            var textview2 = FindViewById<TextView>(Resource.Id.textView2);
            var textview3 = FindViewById<TextView>(Resource.Id.textView3);
            var textview4 = FindViewById<TextView>(Resource.Id.textView4);
            var startBn = FindViewById<Button>(Resource.Id.startBn);
            Typeface tf = Typeface.CreateFromAsset(Assets, "dosis.book.ttf");
            textview1.SetTypeface(tf, TypefaceStyle.Bold);
            textview2.SetTypeface(tf, TypefaceStyle.Bold);
            textview3.SetTypeface(tf, TypefaceStyle.Bold);
            textview4.SetTypeface(tf, TypefaceStyle.Bold);
            startBn.SetTypeface(tf, TypefaceStyle.Bold);
            //fonts ended
        }

        private void _1stOnlyActivity_Click(object sender, EventArgs e)
        {
            StartActivity(new Intent(this, typeof(MAIN_Activity)));
        }
    }
}