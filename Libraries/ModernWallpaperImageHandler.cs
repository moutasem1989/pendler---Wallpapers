using System;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Libraries
{
    public class ModernWallpaperImageHandler
    {
        public static async System.Threading.Tasks.Task HandleModernImage()
        {
            Libraries.ImageColourManagement.GetColorsForModernWallpaper();
            string themeFolder = (string)ApplicationData.Current.LocalSettings.Values["ModernTheme"];
            if (themeFolder != null)
            {
                StorageFolder modernFolderGripp = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync((string)ApplicationData.Current.LocalSettings.Values["ModernFolder"]);
                IStorageItem subFolder = await modernFolderGripp.TryGetItemAsync(themeFolder);
                if (subFolder != null)
                {
                    StorageFolder subFolderGripp = await modernFolderGripp.GetFolderAsync(themeFolder);
                    string imagePathe = $@"{subFolderGripp.Path}/";
                    string[] allImages = Directory.GetFiles(imagePathe, $"{Libraries.ImageColourManagement.ModernAcent}-{Libraries.ImageColourManagement.ModernColor}.*");
                    if (allImages.Length == 0)
                    {
                        string[] oreginalImage = Directory.GetFiles(imagePathe, $"{(string)ApplicationData.Current.LocalSettings.Values["ModernFile"]}.*");
                        await Libraries.ImageColourManagement.ApplyColor(oreginalImage[0], subFolder.Name);
                        string[] newAllImages = Directory.GetFiles(imagePathe, $"{Libraries.ImageColourManagement.ModernAcent}-{Libraries.ImageColourManagement.ModernColor}.*");
                        String fixSourceIm = newAllImages[0].Replace(@"/", @"\");
                        StorageFile file = await StorageFile.GetFileFromPathAsync(fixSourceIm);
                        await ApplyImage(file);
                    }
                    else
                    {
                        String fixSourceIm = allImages[0].Replace(@"/", @"\");
                        StorageFile file = await StorageFile.GetFileFromPathAsync(fixSourceIm);
                        await ApplyImage(file);
                    }
                }
            }
        }
        public static async System.Threading.Tasks.Task ApplyImage(StorageFile file)
        {
            if ((string)ApplicationData.Current.LocalSettings.Values["DeskModernToggled"] != null)
            { await Libraries.ApplyWallpaper.ApplyToDesktop(file); }
            if ((string)ApplicationData.Current.LocalSettings.Values["LockModernToggled"] != null)
            { await Libraries.ApplyWallpaper.ApplyToLockscreen(file); }
        }

    }
}
