using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace Libraries
{
    public class BackgroundSequance
    {
        public static async Task RunBackgroundTaskAsync()
        {
            if (((string)ApplicationData.Current.LocalSettings.Values["DeskModernToggled"] != null || (string)ApplicationData.Current.LocalSettings.Values["LockModernToggled"] != null) && ((string)ApplicationData.Current.LocalSettings.Values["ModernTheme"] != null) && ((string)ApplicationData.Current.LocalSettings.Values["DynamicAccent"] != null || (string)ApplicationData.Current.LocalSettings.Values["DynamicColor"] != null))
            {
                var x = ApplicationData.Current.LocalSettings.Values["MixedDynamicAccent"];
                var y = ApplicationData.Current.LocalSettings.Values["MixedDynamicColor"];
                Libraries.DynamicColorManager.WriteAccentColorID();
                if (ApplicationData.Current.LocalSettings.Values["MixedDynamicAccent"] != x || ApplicationData.Current.LocalSettings.Values["MixedDynamicColor"] != y)
                { await Libraries.ModernWallpaperImageHandler.HandleModernImage(); }
            }
            if ((ApplicationData.Current.LocalSettings.Values["DeskWeatherToggled"] != null || ApplicationData.Current.LocalSettings.Values["LockWeatherToggled"] != null) && ApplicationData.Current.LocalSettings.Values["WeatherTheme"] != null)
            {
                await DoINeedLocation();
                await Libraries.WeatherPaper.GetCurrentWeather();
                StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["WeatherFolder"], CreationCollisionOption.OpenIfExists);

                string imagePathe = $@"{Folder.Path}/{(string)ApplicationData.Current.LocalSettings.Values["WeatherTheme"]}/";
                string[] allImages = Directory.GetFiles(imagePathe, $"X{ApplicationData.Current.LocalSettings.Values["WeatherImageID"].ToString()}.*");
                string ImageWithExtention = Path.GetFullPath(allImages[0]);
                if ((string)ApplicationData.Current.LocalSettings.Values["WeatherLastImageDesk"] != ImageWithExtention && File.Exists(ImageWithExtention))
                {
                    StorageFile imageSource = await StorageFile.GetFileFromPathAsync(ImageWithExtention);
                    if ((string)ApplicationData.Current.LocalSettings.Values["DeskWeatherToggled"] != null)
                    { await Libraries.ApplyWallpaper.ApplyToDesktop(imageSource); }
                    ApplicationData.Current.LocalSettings.Values["WeatherLastImageDesk"] = ImageWithExtention;
                }
                if ((string)ApplicationData.Current.LocalSettings.Values["WeatherLastImageLock"] != ImageWithExtention && File.Exists(ImageWithExtention))
                {
                    StorageFile imageSource = await StorageFile.GetFileFromPathAsync(ImageWithExtention);
                    if ((string)ApplicationData.Current.LocalSettings.Values["LockWeatherToggled"] != null)
                    { await Libraries.ApplyWallpaper.ApplyToLockscreen(imageSource); }
                    ApplicationData.Current.LocalSettings.Values["WeatherLastImageLock"] = ImageWithExtention;
                }
            }
            if ((ApplicationData.Current.LocalSettings.Values["DeskDynamicToggled"] != null || ApplicationData.Current.LocalSettings.Values["LockDynamicToggled"] != null) && ApplicationData.Current.LocalSettings.Values["DynamicTheme"] != null)
            {
                await DoINeedLocation();
                Libraries.SunPositionDetector.DayORNightAsync();
                StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["DynamicFolder"], CreationCollisionOption.OpenIfExists);
                string imagePathe = $@"{Folder.Path}/{(string)ApplicationData.Current.LocalSettings.Values["DynamicTheme"]}/";
                string[] allImages = Directory.GetFiles(imagePathe, $"X{ApplicationData.Current.LocalSettings.Values["DynamicImageID"].ToString()}.*");
                string ImageWithExtention = Path.GetFullPath(allImages[0]);
                
                
                if ((string)ApplicationData.Current.LocalSettings.Values["DynamicLastImageDesk"] != ImageWithExtention && File.Exists(ImageWithExtention))
                {
                    StorageFile imageSource = await StorageFile.GetFileFromPathAsync(ImageWithExtention);
                    if ((string)ApplicationData.Current.LocalSettings.Values["DeskDynamicToggled"] != null)
                    { await Libraries.ApplyWallpaper.ApplyToDesktop(imageSource); }
                    ApplicationData.Current.LocalSettings.Values["DynamicLastIDynamicLastImageDeskmage"] = ImageWithExtention;
                }
                if ((string)ApplicationData.Current.LocalSettings.Values["DynamicLastImageLock"] != ImageWithExtention && File.Exists(ImageWithExtention))
                {
                    StorageFile imageSource = await StorageFile.GetFileFromPathAsync(ImageWithExtention);
                    if ((string)ApplicationData.Current.LocalSettings.Values["LockDynamicToggled"] != null)
                    { await Libraries.ApplyWallpaper.ApplyToLockscreen(imageSource); }
                    ApplicationData.Current.LocalSettings.Values["DynamicLastImageLock"] = ImageWithExtention;
                }
            }
        }
        public static async Task DoINeedLocation()
        {
            if (ApplicationData.Current.LocalSettings.Values["Latitude"] == null || ApplicationData.Current.LocalSettings.Values["Longitude"] == null || (string)ApplicationData.Current.LocalSettings.Values["GetLocation"] != DateTime.Now.Day.ToString())
            {
                await Libraries.LocationManager.GetLocationTask();
                ApplicationData.Current.LocalSettings.Values["GetLocation"] = DateTime.Now.Day.ToString();
            }
        }
    }
}
