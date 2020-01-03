using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Storage;
using System.Net.NetworkInformation;


namespace Libraries
{
    public class LocationManager
    {
        public static async Task GetLocationTask()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            if (accessStatus != GeolocationAccessStatus.Allowed) throw new Exception();
            var geolocator = new Geolocator { DesiredAccuracyInMeters = 1000 };
            if (ApplicationData.Current.LocalSettings.Values["CustomLocation"] == null)
            {
                try
                {
                    var position = await geolocator.GetGeopositionAsync();
                    ApplicationData.Current.LocalSettings.Values["Latitude"] = position.Coordinate.Point.Position.Latitude.ToString();
                    ApplicationData.Current.LocalSettings.Values["Longitude"] = position.Coordinate.Point.Position.Longitude.ToString();
                    ApplicationData.Current.LocalSettings.Values["Accuracy"] = position.Coordinate.Accuracy.ToString();
                    ApplicationData.Current.LocalSettings.Values["WeatherMessege"] = $"Location determined with accuracy limited to {position.Coordinate.Accuracy.ToString()} m. Current position point is {position.Coordinate.Point.Position.Latitude.ToString()}-{position.Coordinate.Point.Position.Longitude.ToString()}";
                }
                catch (Exception)
                {
                    ApplicationData.Current.LocalSettings.Values["WeatherMessege"] = "Operation failed!";
                }
            }
        }
    }

}
