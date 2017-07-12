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

using System.Data;
using System.IO;
using SQLite;

namespace Weather.Droid.DB
{
    public class myDBRepo
    {
        public string CreateDB()
        {
            var output = "";
            output += "Creating DB if it doesn`t exist";
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
            var db = new SQLiteConnection(dbPath);
            output += "\nDatabaseCreated...";
            return output;
        }

        //Code to create the table
        public string CreateTable()
        {
            try
            {
                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
                var db = new SQLiteConnection(dbPath);
                db.CreateTable<city>();
                string result = "Table Created successfully";
                return result;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        //Code to insert record:
        public string InsertCity(string city)
        {
            try
            {
                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
                var db = new SQLiteConnection(dbPath);
                city item = new city();
                item.City = city;
                db.Insert(item);
                return "Record Added";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public string CreateCoordsTable()
        {
            try
            {
                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
                var db = new SQLiteConnection(dbPath);
                db.CreateTable<coordinates>();
                string result = "Table Created successfully";
                return result;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        //Code to insert record:
        public string InsertCoordinates(string x, string y)
        {
            try
            {
                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
                var db = new SQLiteConnection(dbPath);
                coordinates item = new coordinates();
                item._X = x;
                item._Y = y;
                db.Insert(item);
                return "Record Added";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        //Code to retrieve all the records
        public string GetAllRecords()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
            var db = new SQLiteConnection(dbPath);

            string output = "";
            var table = db.Table<city>();
            foreach (var item in table)
            {
                //MAIN_Activity.cityTextGlobal = null;
                MAIN_Activity._x = null;
                MAIN_Activity._y = null;
                output += "\n" + item.Id + " . City: " + item.City;
                MAIN_Activity.cityTextGlobal = item.City;
            }
            return output;
        }

        //code to remove the record
        public string RemoveCity(int id)
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
            var db = new SQLiteConnection(dbPath);
            var item = db.Get<city>(id);
            db.Delete(item);
            return "record Deleted..";
        }

        //Creating celsius/fahrenheit table
        public string CreateTableC_F()
        {
            try
            {
                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
                var db = new SQLiteConnection(dbPath);
                db.CreateTable<CelsiusFarenh_Indicator>();
                string result = "Table Created successfully";
                return result;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        //Code to insert record to celsius/fahrenheit table:
        public string InsertCelsius_Fahrenheit(string c_f)
        {
            try
            {
                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
                var db = new SQLiteConnection(dbPath);
                CelsiusFarenh_Indicator item = new CelsiusFarenh_Indicator();
                item.c_f = c_f;
                db.Insert(item);
                return "Record Added";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        //Code to get all records from celsius/fahrenheit table:
        public string GetAllRecordsC_F()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
            var db = new SQLiteConnection(dbPath);

            string output = "";
            var table = db.Table<CelsiusFarenh_Indicator>();
            foreach (var item in table)
            {
                output = item.c_f;
            }
            return output;
        }

        //Creating Indicator table
        public string CreateIndicatorTable()
        {
            try
            {
                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
                var db = new SQLiteConnection(dbPath);
                db.CreateTable<cityOrCoordIndicator>();
                string result = "Table Created successfully";
                return result;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        //cityOrCoordIndicatorInsertRecord
        public string InsertcityOrCoordIndicator(string indic)
        {
            try
            {
                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
                var db = new SQLiteConnection(dbPath);
                cityOrCoordIndicator item = new cityOrCoordIndicator();
                item.CityOrCoord = indic;
                db.Insert(item);
                return "Record Added";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        //Code to retrieve all the records from indicator
        public string GetAllRecordsIndicator()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "MyDB.db3");
            var db = new SQLiteConnection(dbPath);

            string output = "";
            var table = db.Table<cityOrCoordIndicator>();
            foreach (var item in table)
            {
                /*MAIN_Activity.cityTextGlobal = null;
                MAIN_Activity._x = null;
                MAIN_Activity._y = null;*/
                output += "\n" + item.CityOrCoord;
                //MAIN_Activity.cityTextGlobal = item.City;
            }
            return output;
        }
    }
}