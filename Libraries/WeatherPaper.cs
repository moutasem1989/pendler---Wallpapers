using System;
using System.Threading.Tasks;
using WeatherNet;
using WeatherNet.Clients;
using WeatherNet.Model;
using Windows.Storage;

namespace Libraries
{
    public class WeatherPaper
    {
        public static SingleResult<CurrentWeatherResult> result;
        public static async Task GetCurrentWeather()
        {
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            ClientSettings.ApiUrl = "http://api.openweathermap.org/data/2.5";
            ClientSettings.ApiKey = "35db326245973bc87974d9158d6aa3cb";
            string lat = (string)ApplicationData.Current.LocalSettings.Values["Latitude"];
            double latitude = Double.Parse(lat);
            string lon = (string)ApplicationData.Current.LocalSettings.Values["Longitude"];
            double longitude = Double.Parse(lon);
            result = await CurrentWeather.GetByCoordinatesAsync(latitude, longitude);
            if (result.Item.Icon.ToString() == "01d") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 0; }
            else if (result.Item.Icon.ToString() == "01n") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 1; }
            else if (result.Item.Icon.ToString() == "02d") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 2; }
            else if (result.Item.Icon.ToString() == "02n") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 3; }
            else if (result.Item.Icon.ToString() == "03d") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 4; }
            else if (result.Item.Icon.ToString() == "03n") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 5; }
            else if (result.Item.Icon.ToString() == "04d") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 6; }
            else if (result.Item.Icon.ToString() == "04n") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 7; }
            else if (result.Item.Icon.ToString() == "09d") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 8; }
            else if (result.Item.Icon.ToString() == "09n") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 9; }
            else if (result.Item.Icon.ToString() == "10d") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 10; }
            else if (result.Item.Icon.ToString() == "10n") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 11; }
            else if (result.Item.Icon.ToString() == "11d") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 12; }
            else if (result.Item.Icon.ToString() == "11n") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 13; }
            else if (result.Item.Icon.ToString() == "13d") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 14; }
            else if (result.Item.Icon.ToString() == "13n") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 15; }
            else if (result.Item.Icon.ToString() == "50d") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 16; }
            else if (result.Item.Icon.ToString() == "50n") { ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 17; }
        }
    }
}
