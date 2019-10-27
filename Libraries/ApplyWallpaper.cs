using System;
using Windows.Storage;
using Windows.System.UserProfile;

namespace Libraries
{
    public class ApplyWallpaper
    {
        public static async System.Threading.Tasks.Task ApplyToDesktop(StorageFile storageFile)
        {
            await UserProfilePersonalizationSettings.Current.TrySetWallpaperImageAsync(storageFile);
        }

        public static async System.Threading.Tasks.Task ApplyToLockscreen(StorageFile storageFile)
        {
            await UserProfilePersonalizationSettings.Current.TrySetLockScreenImageAsync(storageFile);
        }
    }
}
