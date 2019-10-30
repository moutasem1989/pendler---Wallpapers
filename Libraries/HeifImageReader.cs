using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using ImageMagick;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;

namespace Libraries
{
    public class HeifImageReader
    {
        public static int i { get; set; }
        public static string imageType { get; set; }

        public static async Task Heifercollection()
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync((string)ApplicationData.Current.LocalSettings.Values["HEICfile"]);
            byte[] fileBytes = null;
            using (var stream = await file.OpenReadAsync())
            {
                fileBytes = new byte[stream.Size];
                using (var reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    reader.ReadBytes(fileBytes);
                }
            }
            using (MagickImageCollection collection = new MagickImageCollection(fileBytes))
            {
                var random = new Random();
                if (collection.Count==18)
                {
                    StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["WeatherFolder"], CreationCollisionOption.OpenIfExists);
                    StorageFolder storage = await Folder.CreateFolderAsync(String.Format("{0:X6}", random.Next(0x1000000)), CreationCollisionOption.ReplaceExisting);
                    int i = 0;
                    foreach( IMagickImage image in collection)
                    {
                        StorageFile imageFile = await storage.CreateFileAsync($"X{i}.png", CreationCollisionOption.ReplaceExisting);
                        image.Format = MagickFormat.Png;
                        image.Write(imageFile.Path);
                        ++i;
                    }
                    ApplicationData.Current.LocalSettings.Values["DynamicOperation"] = "Operation completed. Heic added to Weather Library.";
                    ApplicationData.Current.LocalSettings.Values["HEICfile"] = null;
                    ApplicationData.Current.LocalSettings.Values["AddedfolderWeather"] = storage.Name;
                }
                else if (collection.Count == 16)
                {
                    StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["DynamicFolder"], CreationCollisionOption.OpenIfExists);
                    StorageFolder storage = await Folder.CreateFolderAsync(String.Format("{0:X6}", random.Next(0x1000000)), CreationCollisionOption.ReplaceExisting);
                    int i = 0;
                    foreach (IMagickImage image in collection)
                    {
                        StorageFile imageFile = await storage.CreateFileAsync($"X{i}.png", CreationCollisionOption.ReplaceExisting);
                        image.Format = MagickFormat.Png;
                        image.Write(imageFile.Path);
                        ++i;
                    }
                    ApplicationData.Current.LocalSettings.Values["DynamicOperation"] = "Operation completed. Heic added to Dynamic Library.";
                    ApplicationData.Current.LocalSettings.Values["AddedfolderDynamic"] = storage.Name;
                    ApplicationData.Current.LocalSettings.Values["AddedfolderWeather"] = null;
                }
                else
                {
                    if (collection.Count == 0)
                    {
                        ApplicationData.Current.LocalSettings.Values["DynamicOperation"] = $"This file contains no Images and cannot be added to any library! To add a HEIC, file must contain exactly 16 Images for the Dynamic library and 18 for the Weather Library.";
                    }
                    else if (collection.Count == 1)
                    {
                        ApplicationData.Current.LocalSettings.Values["DynamicOperation"] = $"This file contains only one Image and cannot be added to any library! To add a HEIC, file must contain exactly 16 Images for the Dynamic library and 18 for the Weather Library.";
                    }
                    else
                    {
                        ApplicationData.Current.LocalSettings.Values["DynamicOperation"] = $"This file contains {collection.Count} Images and connot be added to any Library! To add a HEIC, file must contain exactly 16 Images for the Dynamic library and 18 for the Weather Library.";
                    }
                    
                }
            }
        }

        public static async Task Zipcollection(StorageFile file)
        {
            var random = new Random();
            Stream stream = await file.OpenStreamForReadAsync();
            ZipArchive archive = new ZipArchive(stream);
            int images = 0;
            i = 0;
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (entry.FullName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)|| entry.FullName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)|| entry.FullName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)|| entry.FullName.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                { ++images; }
            }
            if (images == 18)
            {
                StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["WeatherFolder"], CreationCollisionOption.OpenIfExists);
                StorageFolder storage = await Folder.CreateFolderAsync(String.Format("{0:X6}", random.Next(0x1000000)), CreationCollisionOption.ReplaceExisting);
                await ExtractEnteries(storage, archive);
                ApplicationData.Current.LocalSettings.Values["DynamicOperation"] = "Operation completed. Zip Archive added to Weather Library.";
            }
            else if (images == 16)
            {
                StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["DynamicFolder"], CreationCollisionOption.OpenIfExists);
                StorageFolder storage = await Folder.CreateFolderAsync(String.Format("{0:X6}", random.Next(0x1000000)), CreationCollisionOption.ReplaceExisting);
                await ExtractEnteries(storage, archive);
                ApplicationData.Current.LocalSettings.Values["DynamicOperation"] = "Operation completed. Zip Archive added to Dynamic Library.";
            }
            else
            {
                if (images == 0)
                {
                    ApplicationData.Current.LocalSettings.Values["DynamicOperation"] = $"This Zip Archive contains no Images and cannot be added to any library! To add a HEIC, file must contain exactly 16 Images for the Dynamic library and 18 for the Weather Library.";
                }
                else if (images == 1)
                {
                    ApplicationData.Current.LocalSettings.Values["DynamicOperation"] = $"This Zip Archive contains only one Image and cannot be added to any library! To add a HEIC, file must contain exactly 16 Images for the Dynamic library and 18 for the Weather Library.";
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["DynamicOperation"] = $"This Zip Archive contains {images} Images and connot be added to any Library! To add a HEIC, file must contain exactly 16 Images for the Dynamic library and 18 for the Weather Library.";
                }

            }

        }
        public static async Task ExtractEnteries(StorageFolder storage, ZipArchive archive)
        {
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (entry.FullName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                { imageType = "png"; }
                else if (entry.FullName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase))
                { imageType = "jpeg"; }
                else if (entry.FullName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase))
                { imageType = "jpg"; }
                else if (entry.FullName.EndsWith("bmp", StringComparison.OrdinalIgnoreCase))
                { imageType = "bmp"; }
                string path = Path.Combine(storage.Path, $"X{i}.{imageType}");
                entry.ExtractToFile(path);
                i = i + 1;
            }
            archive.Dispose();
        }



        public static async Task Foldercollection(StorageFolder directory)
        {
            var queryOption = new QueryOptions { FolderDepth = FolderDepth.Deep };
            var givenFolder = await directory.CreateFileQueryWithOptions(queryOption).GetFilesAsync(0, 20);
            int images = 0;
            i = 0;
            foreach (StorageFile file in givenFolder)
            {
                if (file.FileType.Contains("png")|| file.FileType.Contains("jpeg")|| file.FileType.Contains("jpg")|| file.FileType.Contains("bmp"))
                { ++images; }
            }

            if (images == 18)
            {
                StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["WeatherFolder"], CreationCollisionOption.OpenIfExists);
                await CopyImages(Folder, directory, givenFolder);
                ApplicationData.Current.LocalSettings.Values["DynamicOperation"] = "Operation completed. Directory copied to Weather Library.";
            }
            else if (images == 16)
            {
                StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["DynamicFolder"], CreationCollisionOption.OpenIfExists);
                
                await CopyImages(Folder, directory, givenFolder);
                ApplicationData.Current.LocalSettings.Values["DynamicOperation"] = "Operation completed. Directory copied to Dynamic Library.";
            }
            else
            {
                if (images == 0)
                {
                    ApplicationData.Current.LocalSettings.Values["DynamicOperation"] = $"This Directory contains no Images and cannot be added to any library! To add a HEIC, file must contain exactly 16 Images for the Dynamic library and 18 for the Weather Library.";
                }
                else if (images == 1)
                {
                    ApplicationData.Current.LocalSettings.Values["DynamicOperation"] = $"This Directory contains only one Image and cannot be added to any library! To add a HEIC, file must contain exactly 16 Images for the Dynamic library and 18 for the Weather Library.";
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["DynamicOperation"] = $"This Directory contains {images} Images and connot be added to any Library! To add a HEIC, file must contain exactly 16 Images for the Dynamic library and 18 for the Weather Library.";
                }

            }

        }

        public static async Task CopyImages(StorageFolder Folder, StorageFolder directory, IReadOnlyList<StorageFile> readOnlyList)
        {
            var random = new Random();
            StorageFolder storage = await Folder.CreateFolderAsync(String.Format("{0:X6}", random.Next(0x1000000)), CreationCollisionOption.ReplaceExisting);
            foreach (StorageFile file in readOnlyList)
            {
                if (file.FileType.Contains("png") || file.FileType.Contains("jpeg") || file.FileType.Contains("jpg") || file.FileType.Contains("bmp"))
                {
                    if (file.FileType.Contains("png"))
                    { imageType = "png"; }
                    else if (file.FileType.Contains("jpeg"))
                    { imageType = "jpeg"; }
                    else if (file.FileType.Contains("jpg"))
                    { imageType = "jpg"; }
                    else if (file.FileType.Contains("bmp"))
                    { imageType = "bmp"; }
                    await file.CopyAsync(storage, $"X{i}.{imageType}", NameCollisionOption.ReplaceExisting);
                    i = i + 1;
                }
            }
        }
    }
}
