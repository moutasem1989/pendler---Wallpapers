using Windows.UI.Xaml.Media.Imaging;

namespace pendler
{
    public class ModernWallpaperBitmaps
    {
        public BitmapImage ModernWallpaperImageIcon { get; set; }
        public string FolderName { get; set; }
        public ModernWallpaperBitmaps(BitmapImage imgIconSourceModern, string folderName)
        {
            this.ModernWallpaperImageIcon = imgIconSourceModern;
            this.FolderName = folderName;
        }
    }
}
