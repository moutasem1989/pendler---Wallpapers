using Windows.UI.Xaml.Media.Imaging;

namespace pendler
{
    public class DynamicWallpaperBitmaps
    {
        public BitmapImage DynamicWallpaperImageIcon { get; set; }
        public string FolderName { get; set; }
        public DynamicWallpaperBitmaps(BitmapImage imgIconSourceModern, string folderName)
        {
            this.DynamicWallpaperImageIcon = imgIconSourceModern;
            this.FolderName = folderName;
        }
    }
}
