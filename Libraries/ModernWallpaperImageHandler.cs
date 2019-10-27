using System;
using Windows.Storage;

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
                IStorageItem subFolder = await modernFolderGripp.TryGetItemAsync("BB" + themeFolder);

                if (subFolder != null)
                {
                    StorageFolder subFolderGripp = await modernFolderGripp.GetFolderAsync("BB" + themeFolder);
                    IStorageItem imageItem = await subFolderGripp.TryGetItemAsync($"{Libraries.ImageColourManagement.ModernAcent}-{Libraries.ImageColourManagement.ModernColor}.jpg");
                    if (imageItem != null)
                    {
                        StorageFile imageSource = await subFolderGripp.GetFileAsync($"{Libraries.ImageColourManagement.ModernAcent}-{Libraries.ImageColourManagement.ModernColor}.jpg");
                        await ApplyImage(imageSource);
                    }
                    else
                    {
                        await ColorModernImage(modernFolderGripp, themeFolder);
                    }
                }
                else
                {
                    await ColorModernImage(modernFolderGripp, themeFolder);
                }
            }
        }
        public static async System.Threading.Tasks.Task ColorModernImage(StorageFolder modernFolderGripp, string themeFolder)
        {
            StorageFolder subFolderGripp = await modernFolderGripp.CreateFolderAsync("BB" + themeFolder, CreationCollisionOption.OpenIfExists);
            await Libraries.ImageColourManagement.ApplyColor($"ms-appx:///Modern/{themeFolder}.jpg", "BB" + themeFolder);
            StorageFile imageSource = await subFolderGripp.GetFileAsync($"{Libraries.ImageColourManagement.ModernAcent}-{Libraries.ImageColourManagement.ModernColor}.jpg");
            await ApplyImage(imageSource);
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
