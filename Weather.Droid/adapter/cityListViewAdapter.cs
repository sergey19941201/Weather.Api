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
using Android.Graphics;
using Android.Content.Res;
using SQLite;
using System.IO;

namespace Weather.Droid.adapter
{
    class cityListViewAdapter : BaseAdapter<clases.citiesClass>
    {
        List<clases.citiesClass> items;
        Activity context;

        DB.myDBRepo dbr = new DB.myDBRepo();

        private string cityName;

        public cityListViewAdapter(Activity context, List<clases.citiesClass> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        #region implemented abstract members of BaseAdapter
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null)
                //view = context.LayoutInflater.Inflate(Resource.Layout.custom_item, null);
                view = context.LayoutInflater.Inflate(Resource.Layout.customCityForList, null);
            //fonts
            var view1 = view.FindViewById<TextView>(Resource.Id.txtNombre);
            var cityBn = view.FindViewById<Button>(Resource.Id.cityBn);
            var delBn = view.FindViewById<Button>(Resource.Id.deleteBn);
            view1.Text = item.Nombre;
            Typeface tf = Typeface.CreateFromAsset(context.Assets, "dosis.book.ttf");
            view1.SetTypeface(tf, TypefaceStyle.Bold);
            cityBn.SetTypeface(tf, TypefaceStyle.Bold);
            delBn.SetTypeface(tf, TypefaceStyle.Bold);
            //fonts ended

            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(view.Context);
            builder.SetMessage("Delete?");
            builder.SetCancelable(true);

            //IMPORTANT!!! this code: "if (!delBn.HasOnClickListeners)" is used to prevent the multiple click on delete button
            if (!delBn.HasOnClickListeners)
            {
                delBn.Click += delegate
                {
                    builder.SetNegativeButton("Yes", (object sender, DialogClickEventArgs e) =>
                    {
                        Toast.MakeText(view.Context, "Deleted: " + cityBn.Text, ToastLength.Short).Show();
                        dbr.RemoveCity(Convert.ToInt32(view1.Text));
                        view.Context.StartActivity(typeof(EditActivity));
                    });
                    builder.SetPositiveButton("No", (object sender, DialogClickEventArgs e) =>
                    {
                        Toast.MakeText(view.Context, "Canceled", ToastLength.Short).Show();
                    //view.Context.StartActivity(typeof(EditActivity));
                });
                    Android.App.AlertDialog dialog = builder.Create();
                    dialog.Show();
                };
            }
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
            var db = new SQLiteConnection(dbPath);
            var table = db.Table<DB.city>();

            int i = 0;
            //!!!IMPORTANT  .AsEnumerable().Reverse() is for sorting cities at the end of the list
            foreach (var Item in table.AsEnumerable().Reverse())
            {
                if (i == position)
                {
                    if (Item.City != null)
                    {
                        //cityBn.Text = "    " + Item.City;
                        cityBn.Text = Item.City;
                    }
                }
                i++;
            }
            cityBn.Click += delegate
            {
                dbr.InsertcityOrCoordIndicator("city");
                //Console.WriteLine("this city is "+ cityBn.Text);
                foreach (var Item in table)
                {
                    //if ("    " + Item.City == cityBn.Text)
                    if (Item.City == cityBn.Text)
                    {
                        foreach (char c in cityBn.Text)
                        {
                            if (c != ' ')
                            {
                                cityName += c;
                            }
                        }

                        dbr.RemoveCity(Item.Id);
                        //dbr.InsertCity(cityBn.Text);
                        dbr.InsertCity(cityName);

                        MAIN_Activity.cityButtonPressedIndicGlob = true;
                        MAIN_Activity.cityTextGlobal = cityBn.Text.ToLower();

                        view.Context.StartActivity(typeof(LoadingActivity));
                    }
                }
            };

            return view;
        }

        public override int Count
        {
            get { return items.Count; }
        }

        #endregion

        #region implemented abstract members of BaseAdapter
        public override clases.citiesClass this[int position]
        {
            get { return items[position]; }
        }
        #endregion
    }
}