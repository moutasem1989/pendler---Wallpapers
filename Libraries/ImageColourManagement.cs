using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace Libraries
{
    public class ImageColourManagement
    {
        public static string ModernAcent { get; set; }
        public static string ModernColor { get; set; }
        public static async Task<BitmapImage> ApplyColor(string sourceIm, string folderName)
        {
            var sourceImage = new Uri($"{sourceIm}");
            Color replaceBlack = Windows.UI.Color.FromArgb(255, byte.Parse(ModernColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(ModernColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(ModernColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
            Color replaceWhite = Windows.UI.Color.FromArgb(255, byte.Parse(ModernAcent.Substring(0, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(ModernAcent.Substring(2, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(ModernAcent.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
            WriteableBitmap source = await GetImageFile(sourceImage);
            byte[] byteArray = null;
            using (Stream stream = source.PixelBuffer.AsStream())
            {
                long streamLength = stream.Length;
                byteArray = new byte[streamLength];
                await stream.ReadAsync(byteArray, 0, byteArray.Length);
                if (streamLength > 0)
                {
                    for (int i = 0; i < streamLength; i += 4)
                    {
                        if (byteArray[i + 3] != 0)
                        {
                            int b = Convert.ToInt32(byteArray[i]);
                            int g = Convert.ToInt32(byteArray[i + 1]);
                            int r = Convert.ToInt32(byteArray[i + 2]);

                            double rB = ((b / 255.0) * replaceBlack.B) + (((255 - b) / 255.0) * replaceWhite.B);
                            double rG = ((g / 255.0) * replaceBlack.G) + (((255 - g) / 255.0) * replaceWhite.G);
                            double rR = ((r / 255.0) * replaceBlack.R) + (((255 - r) / 255.0) * replaceWhite.R);

                            byte blue = Convert.ToByte(rB);
                            byte green = Convert.ToByte(rG);
                            byte red = Convert.ToByte(rR);

                            byteArray[i] = blue; // Blue
                            byteArray[i + 1] = green;  // Green
                            byteArray[i + 2] = red; // Red
                        }
                    }
                }
            }
            if (byteArray != null)
            {
                WriteableBitmap result = await PixelBufferToWritableBitmap(byteArray, source.PixelWidth, source.PixelHeight);
                StorageFile image = await WriteableBitmapToStorageFile(result, $"{ModernAcent}-{ModernColor}", folderName);
                BitmapImage imageSource = await StorageFileToBitmapImage(image);
                return imageSource;
            }
            return null;
        }
        public static void GetColorsForModernWallpaper()
        {
            Libraries.DynamicColorManager.WriteAccentColorID();
            if ((string)ApplicationData.Current.LocalSettings.Values["DynamicAccent"] != null)
            {
                string flatWallpaperAcent = (string)ApplicationData.Current.LocalSettings.Values["MixedDynamicAccent"];
                ModernAcent = flatWallpaperAcent.Replace("#", "");
            }
            else
            {
                string flatWallpaperAcent = (string)ApplicationData.Current.LocalSettings.Values["Acent"];
                ModernAcent = flatWallpaperAcent.Replace("#", "");
            }
            if ((string)ApplicationData.Current.LocalSettings.Values["DynamicColor"] != null)
            {
                string flatWallpaperAcent = (string)ApplicationData.Current.LocalSettings.Values["MixedDynamicColor"];
                ModernColor = flatWallpaperAcent.Replace("#", "");
            }
            else
            {

                string flatWallpaperAcent = (string)ApplicationData.Current.LocalSettings.Values["Color"];
                ModernColor = flatWallpaperAcent.Replace("#", "");
            }
        }
        private static async Task<BitmapImage> StorageFileToBitmapImage(StorageFile savedStorageFile)
        {
            using (IRandomAccessStream fileStream = await savedStorageFile.OpenAsync(Windows.Storage.FileAccessMode.Read))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.DecodePixelHeight = 100;
                bitmapImage.DecodePixelWidth = 100;
                await bitmapImage.SetSourceAsync(fileStream);
                return bitmapImage;
            }
        }
        private static async Task<StorageFile> WriteableBitmapToStorageFile(WriteableBitmap WB, string fileName, string folderName)
        {
            Guid BitmapEncoderGuid = BitmapEncoder.PngEncoderId;

            StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFolder createFlatUIFolder = await localFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["ModernFolder"], CreationCollisionOption.OpenIfExists);
            StorageFolder desegnatedFolder = await createFlatUIFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
            var bmif = await desegnatedFolder.CreateFileAsync($"{fileName}.jpg", CreationCollisionOption.ReplaceExisting);
            using (IRandomAccessStream stream = await bmif.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoderGuid, stream);
                Stream pixelStream = WB.PixelBuffer.AsStream();
                byte[] pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                                    (uint)WB.PixelWidth,
                                    (uint)WB.PixelHeight,
                                    96.0,
                                    96.0,
                                    pixels);
                await encoder.FlushAsync();
            }
            return bmif;
        }
        private static async Task<WriteableBitmap> GetImageFile(Uri fileUri)
        {
            StorageFile imageFile = await StorageFile.GetFileFromApplicationUriAsync(fileUri);

            WriteableBitmap writeableBitmap = null;
            using (IRandomAccessStream imageStream = await imageFile.OpenReadAsync())
            {
                BitmapDecoder bitmapDecoder = await BitmapDecoder.CreateAsync(imageStream);

                BitmapTransform dummyTransform = new BitmapTransform();
                PixelDataProvider pixelDataProvider =
                   await bitmapDecoder.GetPixelDataAsync(BitmapPixelFormat.Bgra8,
                   BitmapAlphaMode.Premultiplied, dummyTransform,
                   ExifOrientationMode.RespectExifOrientation,
                   ColorManagementMode.ColorManageToSRgb);
                byte[] pixelData = pixelDataProvider.DetachPixelData();

                writeableBitmap = new WriteableBitmap(
                   (int)bitmapDecoder.OrientedPixelWidth,
                   (int)bitmapDecoder.OrientedPixelHeight);
                using (Stream pixelStream = writeableBitmap.PixelBuffer.AsStream())
                {
                    await pixelStream.WriteAsync(pixelData, 0, pixelData.Length);
                }
            }
            return writeableBitmap;
        }


        private static async Task PixelBufferToWritableBitmap(WriteableBitmap wb, byte[] bgra)
        {
            using (Stream stream = wb.PixelBuffer.AsStream())
            {
                await stream.WriteAsync(bgra, 0, bgra.Length);
            }
        }
        private static async Task<WriteableBitmap> PixelBufferToWritableBitmap(byte[] bgra, int width, int height)
        {
            var wb = new WriteableBitmap(width, height);
            await PixelBufferToWritableBitmap(wb, bgra);
            return wb;
        }
    }
}
