using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json;

namespace Weather.Api
{
    public class WeatherRepository
    {
        public async Task<RootObject> GetAllWeather()
        {
            var httpClient = GetHttpClient();

            var response = await httpClient.GetAsync(ServiceEndPoints.WeatherApiBaseUri).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content;

                string jsonString = await content.ReadAsStringAsync().ConfigureAwait(false);

                return JsonConvert.DeserializeObject<RootObject>(jsonString);
            }
            return new RootObject();
        }

        private HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient(new NativeMessageHandler())
            {
                BaseAddress = new Uri(ServiceEndPoints.WeatherApiBaseUri)
            };


            httpClient.DefaultRequestHeaders.Accept.Clear();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }
    }

    public class ServiceEndPoints
    {
        public static readonly string WeatherApiBaseUri = "http://api.openweathermap.org/data/2.5/weather?q=London&units=imperial&APPID=274c6def18f89eb1d9a444822d2574b5";
       
    }
}