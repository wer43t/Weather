using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;

namespace Weather
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string cityURL = $"https://api.ipgeolocation.io/ipgeo?apiKey={ConfigurationManager.AppSettings["geolocateAPI"]}&fields=city&";
                string response = WorkWithApi(cityURL);
                CityInfo cityInfo = JsonConvert.DeserializeObject<CityInfo>(response);

                string url = $"https://api.openweathermap.org/data/2.5/weather?q={cityInfo.City}&units=metric&appid={ConfigurationManager.AppSettings["weatherAPI"]}";
                response = WorkWithApi(url);
                WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);

                Console.WriteLine($"Температура в {weatherResponse.Name}: {weatherResponse.Main.Temp} {Environment.NewLine}" +
                    $"Ощущается как {weatherResponse.Main.Feels_like} " +
                    $"{Environment.NewLine} Скорость ветра {weatherResponse.Wind.Speed} м/c");
            }
            catch (Exception)
            {
                Console.WriteLine("Неправильно ввели город, попробуйте заново");
                Main(args);
            }

            Console.ReadKey();
        }

        static string WorkWithApi(string url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string response;

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            return response;
        }
    }
}
