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

namespace Weather.Droid.adapter
{
    class adapter_listview : BaseAdapter<clases.cls_Libro>
    {

        List<clases.cls_Libro> items;
        Activity context;

        public adapter_listview(Activity context, List<clases.cls_Libro> items) : base()
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
                view = context.LayoutInflater.Inflate(Resource.Layout.custom_item, null);
            //fonts
            var view1 = view.FindViewById<TextView>(Resource.Id.txtNombre);
            var view2 = view.FindViewById<TextView>(Resource.Id.txtAutor);
            var view3 = view.FindViewById<TextView>(Resource.Id.txtPaginas);

            view1.Text = item.Nombre;
            view2.Text = item.Autor;
            view3.Text = item.Pag_leidas.ToString() + " DE " + item.Total_pag.ToString() + " PAG. ";

            Typeface tf = Typeface.CreateFromAsset(context.Assets, "dosis.book.ttf");
            view1.SetTypeface(tf, TypefaceStyle.Bold);
            view2.SetTypeface(tf, TypefaceStyle.Bold);
            view3.SetTypeface(tf, TypefaceStyle.Bold);
            //fonts ended

            int id_img = context.Resources.GetIdentifier("img" + item.Id_libro.ToString(), "drawable", context.PackageName);
            view.FindViewById<ImageView>(Resource.Id.imgPortada_item).SetImageResource(id_img);
            return view;
        }
        public override int Count
        {
            get { return items.Count; }
        }

        #endregion

        #region implemented abstract members of BaseAdapter
        public override clases.cls_Libro this[int position]
        {
            get { return items[position]; }
        }
        #endregion
    }

}