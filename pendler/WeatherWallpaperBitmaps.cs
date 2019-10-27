using Windows.UI.Xaml.Media.Imaging;

namespace pendler
{
    public class WeatherWallpaperBitmaps
    {
        public BitmapImage WeatherWallpaperImageIcon { get; set; }
        public string FolderName { get; set; }
        public WeatherWallpaperBitmaps(BitmapImage imgIconSourceModern, string folderName)
        {
            this.WeatherWallpaperImageIcon = imgIconSourceModern;
            this.FolderName = folderName;
        }
    }
}
