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

namespace Weather.Droid.clases
{
    public class citiesClass
    {
        /*VARIABLES*/
        int _id_libro;
        string _nombre;

        /*CONSTRUCTOR*/
        public citiesClass(string nombre)
        {
            this._nombre = nombre;
        }
        /*PROPIEDADES*/

        public string Nombre
        {
            get
            {
                return _nombre;
            }
            set
            {
                _nombre = value;
            }
        }
    }
}