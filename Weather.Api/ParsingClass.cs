using Android.App;
using Android.Content;
using Java.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Weather.Api
{
    public class ParsingClass
    {
        private static string AppID = "cb613e81430eb868b89ee9a28f905f03";

        public static string cityTextGlobal, weatherDescGlob, windSpeedGlob, weatherGlob, tempGlobal, tempAlternative;
        private static string GovnoTemperature, windLocal;
        private string jsonString;

        //variables for procedure that prevents such values of temperature as "-0"
        private static bool minusIsTrue = false;
        private static char firstDigit;
        //variables for procedure that prevents such values of temperature as "-0" ENDED

        public async Task<string> FetchAsync(double lat, double lon, Action<Intent> navigationFunc, Intent intent, string c_f)
        {
            jsonString = null;

            var url = "";
            if (c_f == "celsius")
            {
                url = String.Format(
              "http://api.openweathermap.org/data/2.5/weather?lat=" + lat + "&lon=" + lon + "&units=metric&APPID=" + AppID);
            }
            else
            {
                url = String.Format(
              "http://api.openweathermap.org/data/2.5/weather?lat=" + lat + "&lon=" + lon + "&units=imperial&APPID=" + AppID);
            }

            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var stream = await httpClient.GetStreamAsync(url);
                StreamReader reader = new StreamReader(stream);
                jsonString = reader.ReadToEnd();
            }

            var json = jsonString;

            JsonValue firstitem = json;
            var mydata = JObject.Parse(json);

            cityTextGlobal = (mydata["name"]).ToString();

            string wind = (mydata["wind"]).ToString();
            //spliting string
            string[] windSpeed = wind.Split(':');
            for (int i = 1; i < windSpeed.Length; i++)
            {
                windSpeed[i] = windSpeed[i].Trim();
                if (i == 1)
                {
                    windLocal = windSpeed[i];
                    break;
                }
            }
            windSpeedGlob = null;
            foreach (char c in windLocal)
            {
                if (c == ',' || c == '}' || c == ')')
                {
                    break;
                }
                else
                {
                    windSpeedGlob += c.ToString();
                }
            }

            weatherDescGlob = (mydata["weather"]).ToString();
            //spliting string
            string[] weatherDesc = weatherDescGlob.Split('"');
            for (int i = 5; i < weatherDesc.Length; i++)
            {
                weatherDesc[i] = weatherDesc[i].Trim();
                if (i == 5)
                {
                    weatherGlob = weatherDesc[i];
                }
                if (i == 9)
                {
                    weatherDescGlob = weatherDesc[i];
                    break;
                }
            }

            string GovnoData = (mydata["main"]).ToString();

            //spliting string
            string[] values = GovnoData.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();
                if (i == 0)
                {
                    //tempGlobal = values[i];
                    GovnoTemperature = values[i];
                }
            }
            tempGlobal = null;
            foreach (char c in GovnoTemperature)
            {
                if (c == '.')
                {
                    break;
                }
                if (c == '-' || char.IsDigit(c) == true || c == '.')
                {
                    tempGlobal += c.ToString();
                }
            }

            preventMinusZero();
            MySharedMethod();
            navigationFunc(intent);

            return jsonString;
        }

        //public async Task<string> FetchAsync(string url)
        public async Task<string> FetchAsync(string city, Action<Intent> navigationFunc, Intent intent, string c_f)
        {
            jsonString = null;
            var url = "";
            if (c_f == "celsius")
            {
                url = "http://api.openweathermap.org/data/2.5/weather?q=" + city + "&units=metric&APPID=" + AppID;
            }
            else
            {
                url = "http://api.openweathermap.org/data/2.5/weather?q=" + city + "&units=imperial&APPID=" + AppID;
            }

            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var stream = await httpClient.GetStreamAsync(url);
                StreamReader reader = new StreamReader(stream);
                jsonString = reader.ReadToEnd();
            }

            var json = jsonString;

            JsonValue firstitem = json;
            var mydata = JObject.Parse(json);

            cityTextGlobal = (mydata["name"]).ToString();

            string wind = (mydata["wind"]).ToString();
            //spliting string
            string[] windSpeed = wind.Split(':');
            for (int i = 1; i < windSpeed.Length; i++)
            {
                windSpeed[i] = windSpeed[i].Trim();
                if (i == 1)
                {
                    windLocal = windSpeed[i];
                    break;
                }
            }
            windSpeedGlob = null;
            foreach (char c in windLocal)
            {
                if (c == ',' || c == '}' || c == ')')
                {
                    break;
                }
                else
                {
                    windSpeedGlob += c.ToString();
                }
            }

            weatherDescGlob = (mydata["weather"]).ToString();
            //spliting string
            string[] weatherDesc = weatherDescGlob.Split('"');
            for (int i = 5; i < weatherDesc.Length; i++)
            {
                weatherDesc[i] = weatherDesc[i].Trim();
                if (i == 5)
                {
                    weatherGlob = weatherDesc[i];
                }
                if (i == 9)
                {
                    weatherDescGlob = weatherDesc[i];
                    break;
                }
            }

            string GovnoData = (mydata["main"]).ToString();

            //spliting string
            string[] values = GovnoData.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();
                if (i == 0)
                {
                    //tempGlobal = values[i];
                    GovnoTemperature = values[i];
                }
            }
            tempGlobal = null;
            foreach (char c in GovnoTemperature)
            {
                if (c == '.')
                {
                    break;
                }
                if (c == '-' || char.IsDigit(c) == true || c == '.')
                {
                    tempGlobal += c.ToString();
                }
            }

            preventMinusZero();
            MySharedMethod();
            navigationFunc(intent);

            return jsonString;
        }

        //this procedure is to prevent such values of temperature as "-0"
        private void preventMinusZero()
        {
            foreach (char c in tempGlobal)
            {
                if (c == '-')
                {
                    minusIsTrue = true;
                    break;
                }
                else
                {
                    minusIsTrue = false;
                    break;
                }
            }

            if (minusIsTrue == true)
            {
                foreach (char c1 in tempGlobal)
                {
                    if (Char.IsDigit(c1))
                    {
                        firstDigit = c1;
                        break;
                    }
                }
            }

            if (firstDigit == '0')
            {
                foreach (char c2 in tempGlobal)
                {
                    if (c2 != '-')
                    {
                        tempAlternative += c2;
                    }
                }
                tempGlobal = tempAlternative;
            }
        }
        //this procedure is to prevent such values of temperature as "-0" ENDED

        private void MySharedMethod()
        {
            ISharedPreferences pref = Application.Context.GetSharedPreferences("WeatherData", FileCreationMode.Private);
            ISharedPreferencesEditor edit = pref.Edit();
            if (cityTextGlobal != null)
            {
                edit.PutString("City", cityTextGlobal);
            }
            if (tempGlobal != null)
            {
                edit.PutString("tempGlobal", tempGlobal);
            }
            if (weatherDescGlob != null)
            {
                edit.PutString("weatherDescGlob", weatherDescGlob);
            }
            if (windSpeedGlob != null)
            {
                edit.PutString("windSpeedGlob", windSpeedGlob);
            }
            if (weatherGlob != null)
            {
                edit.PutString("weatherGlob", weatherGlob);
            }
            edit.Apply();
        }
    }


    public class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public int temp_min { get; set; }
        public int temp_max { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
        public double deg { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public double message { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public class RootObject
    {
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }
}
