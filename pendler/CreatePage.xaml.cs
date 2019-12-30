using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace pendler
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreatePage : Page
    {
        public static int i { get; set; }
        public static string imageType { get; set; }
        private StorageFile fileDynamic01;
        private StorageFile fileDynamic02;
        private StorageFile fileDynamic03;
        private StorageFile fileDynamic04;
        private StorageFile fileDynamic05;
        private StorageFile fileDynamic06;
        private StorageFile fileDynamic07;
        private StorageFile fileDynamic08;
        private StorageFile fileDynamic09;
        private StorageFile fileDynamic10;
        private StorageFile fileDynamic11;
        private StorageFile fileDynamic12;
        private StorageFile fileDynamic13;
        private StorageFile fileDynamic14;
        private StorageFile fileDynamic15;
        private StorageFile fileDynamic16;
        private StorageFile fileWeather01;
        private StorageFile fileWeather02;
        private StorageFile fileWeather03;
        private StorageFile fileWeather04;
        private StorageFile fileWeather05;
        private StorageFile fileWeather06;
        private StorageFile fileWeather07;
        private StorageFile fileWeather08;
        private StorageFile fileWeather09;
        private StorageFile fileWeather10;
        private StorageFile fileWeather11;
        private StorageFile fileWeather12;
        private StorageFile fileWeather13;
        private StorageFile fileWeather14;
        private StorageFile fileWeather15;
        private StorageFile fileWeather16;
        private StorageFile fileWeather17;
        private StorageFile fileWeather18;
        private string imageURI;

        public CreatePage()
        {
            this.InitializeComponent();
            ApplicationViewTitleBar appTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            appTitleBar.BackgroundColor = Colors.Transparent;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            imageURI = "ms-appx:///Assets/ImageIcon.png";
            ClickDynamic01.Content = "Sunset";
            ClickDynamic02.Content = "Sunrise";
            ClickDynamic03.Content = "Sunrise";
            ClickDynamic04.Content = "Sunrise";
            ClickDynamic05.Content = "Day";
            ClickDynamic06.Content = "Day";
            ClickDynamic07.Content = "Day";
            ClickDynamic08.Content = "Day";
            ClickDynamic09.Content = "Day";
            ClickDynamic10.Content = "Day";
            ClickDynamic11.Content = "Day";
            ClickDynamic12.Content = "Sunset";
            ClickDynamic13.Content = "Sunset";
            ClickDynamic14.Content = "Night";
            ClickDynamic15.Content = "Night";
            ClickDynamic16.Content = "Night";
            ImageDynamic01.Source = new BitmapImage(new Uri(imageURI));
            ImageDynamic02.Source = new BitmapImage(new Uri(imageURI));
            ImageDynamic03.Source = new BitmapImage(new Uri(imageURI));
            ImageDynamic04.Source = new BitmapImage(new Uri(imageURI));
            ImageDynamic05.Source = new BitmapImage(new Uri(imageURI));
            ImageDynamic06.Source = new BitmapImage(new Uri(imageURI));
            ImageDynamic07.Source = new BitmapImage(new Uri(imageURI));
            ImageDynamic08.Source = new BitmapImage(new Uri(imageURI));
            ImageDynamic09.Source = new BitmapImage(new Uri(imageURI));
            ImageDynamic10.Source = new BitmapImage(new Uri(imageURI));
            ImageDynamic11.Source = new BitmapImage(new Uri(imageURI));
            ImageDynamic12.Source = new BitmapImage(new Uri(imageURI));
            ImageDynamic13.Source = new BitmapImage(new Uri(imageURI));
            ImageDynamic14.Source = new BitmapImage(new Uri(imageURI));
            ImageDynamic15.Source = new BitmapImage(new Uri(imageURI));
            ImageDynamic16.Source = new BitmapImage(new Uri(imageURI));
            AddDynamicTheme.IsEnabled = false;

            ClickWeather01.Content = "Clear sky - Day";
            ClickWeather02.Content = "Clear sky - Night";
            ClickWeather03.Content = "Few clouds - Day";
            ClickWeather04.Content = "Few clouds - Night";
            ClickWeather05.Content = "Scattered clouds - Day";
            ClickWeather06.Content = "Scattered clouds - Night";
            ClickWeather07.Content = "Broken clouds - Day";
            ClickWeather08.Content = "Broken clouds - Night";
            ClickWeather09.Content = "Shower rain - Day";
            ClickWeather10.Content = "Shower rain - Night";
            ClickWeather11.Content = "Rain - Day";
            ClickWeather12.Content = "Rain - Night";
            ClickWeather13.Content = "Thunderstorm - Day";
            ClickWeather14.Content = "Thunderstorm - Night";
            ClickWeather15.Content = "Snow - Day";
            ClickWeather16.Content = "Snow - Night";
            ClickWeather17.Content = "Mist - Day";
            ClickWeather18.Content = "Mist - Night";
            ImageWeather01.Source = new BitmapImage(new Uri(imageURI));
            ImageWeather02.Source = new BitmapImage(new Uri(imageURI));
            ImageWeather03.Source = new BitmapImage(new Uri(imageURI));
            ImageWeather04.Source = new BitmapImage(new Uri(imageURI));
            ImageWeather05.Source = new BitmapImage(new Uri(imageURI));
            ImageWeather06.Source = new BitmapImage(new Uri(imageURI));
            ImageWeather07.Source = new BitmapImage(new Uri(imageURI));
            ImageWeather08.Source = new BitmapImage(new Uri(imageURI));
            ImageWeather09.Source = new BitmapImage(new Uri(imageURI));
            ImageWeather10.Source = new BitmapImage(new Uri(imageURI));
            ImageWeather11.Source = new BitmapImage(new Uri(imageURI));
            ImageWeather12.Source = new BitmapImage(new Uri(imageURI));
            ImageWeather13.Source = new BitmapImage(new Uri(imageURI));
            ImageWeather14.Source = new BitmapImage(new Uri(imageURI));
            ImageWeather15.Source = new BitmapImage(new Uri(imageURI));
            ImageWeather16.Source = new BitmapImage(new Uri(imageURI));
            ImageWeather17.Source = new BitmapImage(new Uri(imageURI));
            ImageWeather18.Source = new BitmapImage(new Uri(imageURI));
            AddWeatherTheme.IsEnabled = false;
        }
        public void CheckAddDynamicThemeVisability()
        {
            if (fileDynamic01 != null && fileDynamic02 != null && fileDynamic03 != null && fileDynamic04 != null && fileDynamic05 != null && fileDynamic06 != null && fileDynamic07 != null && fileDynamic08 != null && fileDynamic09 != null && fileDynamic10 != null && fileDynamic11 != null && fileDynamic12 != null && fileDynamic13 != null && fileDynamic14 != null && fileDynamic15 != null && fileDynamic16 != null)
            { AddDynamicTheme.IsEnabled = true; }
            else
            { AddDynamicTheme.IsEnabled = false; }
        }
        public void CheckAddWeatherThemeVisability()
        {
            if (fileWeather01 != null && fileWeather02 != null && fileWeather03 != null && fileWeather04 != null && fileWeather05 != null && fileWeather06 != null && fileWeather07 != null && fileWeather08 != null && fileWeather09 != null && fileWeather10 != null && fileWeather11 != null && fileWeather12 != null && fileWeather13 != null && fileWeather14 != null && fileWeather15 != null && fileWeather16 != null && fileWeather17 != null && fileWeather18 != null)
            { AddWeatherTheme.IsEnabled = true; }
            else
            { AddWeatherTheme.IsEnabled = false; }
        }
        public static async Task<StorageFile> PicturePicker()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".bmp");
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                return file;
            }
            else
            {
                return null;
            }
        }
        public static async Task<BitmapImage> StorageFileToBitmapImage(StorageFile storageFile)
        {
            using (IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.Read))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.DecodePixelHeight = 50;
                await bitmap.SetSourceAsync(fileStream);
                return bitmap;
            }
        }
        private async void XClickDynamic01(object sender, RoutedEventArgs e)
        {
            if (fileDynamic01 == null)
            {
                fileDynamic01 = await PicturePicker();
                if (fileDynamic01 != null)
                {
                    ClickDynamic01.Content = fileDynamic01.Name;
                    ImageDynamic01.Source = await StorageFileToBitmapImage(fileDynamic01);
                }
                else { fileDynamic01 = null; ClickDynamic01.Content = "Sunset"; ImageDynamic01.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileDynamic01 = null; ClickDynamic01.Content = "Sunset"; ImageDynamic01.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddDynamicThemeVisability();
        }
        private async void XClickDynamic02(object sender, RoutedEventArgs e)
        {
            if (fileDynamic02 == null)
            {
                fileDynamic02 = await PicturePicker();
                if (fileDynamic02 != null)
                {
                    ClickDynamic02.Content = fileDynamic02.Name;
                    ImageDynamic02.Source = await StorageFileToBitmapImage(fileDynamic02);
                }
                else { fileDynamic02 = null; ClickDynamic02.Content = "Sunset"; ImageDynamic02.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileDynamic02 = null; ClickDynamic02.Content = "Sunset"; ImageDynamic02.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddDynamicThemeVisability();
        }
        private async void XClickDynamic03(object sender, RoutedEventArgs e)
        {
            if (fileDynamic03 == null)
            {
                fileDynamic03 = await PicturePicker();
                if (fileDynamic03 != null)
                {
                    ClickDynamic03.Content = fileDynamic03.Name;
                    ImageDynamic03.Source = await StorageFileToBitmapImage(fileDynamic03);
                }
                else { fileDynamic03 = null; ClickDynamic03.Content = "Sunset"; ImageDynamic03.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileDynamic03 = null; ClickDynamic03.Content = "Sunset"; ImageDynamic03.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddDynamicThemeVisability();
        }
        private async void XClickDynamic04(object sender, RoutedEventArgs e)
        {
            if (fileDynamic04 == null)
            {
                fileDynamic04 = await PicturePicker();
                if (fileDynamic04 != null)
                {
                    ClickDynamic04.Content = fileDynamic04.Name;
                    ImageDynamic04.Source = await StorageFileToBitmapImage(fileDynamic04);
                }
                else { fileDynamic04 = null; ClickDynamic04.Content = "Sunset"; ImageDynamic04.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileDynamic04 = null; ClickDynamic04.Content = "Sunset"; ImageDynamic04.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddDynamicThemeVisability();
        }
        private async void XClickDynamic05(object sender, RoutedEventArgs e)
        {
            if (fileDynamic05 == null)
            {
                fileDynamic05 = await PicturePicker();
                if (fileDynamic05 != null)
                {
                    ClickDynamic05.Content = fileDynamic05.Name;
                    ImageDynamic05.Source = await StorageFileToBitmapImage(fileDynamic05);
                }
                else { fileDynamic05 = null; ClickDynamic05.Content = "Sunset"; ImageDynamic05.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileDynamic05 = null; ClickDynamic05.Content = "Sunset"; ImageDynamic05.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddDynamicThemeVisability();
        }
        private async void XClickDynamic06(object sender, RoutedEventArgs e)
        {
            if (fileDynamic06 == null)
            {
                fileDynamic06 = await PicturePicker();
                if (fileDynamic06 != null)
                {
                    ClickDynamic06.Content = fileDynamic06.Name;
                    ImageDynamic06.Source = await StorageFileToBitmapImage(fileDynamic06);
                }
                else { fileDynamic06 = null; ClickDynamic06.Content = "Sunset"; ImageDynamic06.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileDynamic06 = null; ClickDynamic06.Content = "Sunset"; ImageDynamic06.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddDynamicThemeVisability();
        }
        private async void XClickDynamic07(object sender, RoutedEventArgs e)
        {
            if (fileDynamic07 == null)
            {
                fileDynamic07 = await PicturePicker();
                if (fileDynamic07 != null)
                {
                    ClickDynamic07.Content = fileDynamic07.Name;
                    ImageDynamic07.Source = await StorageFileToBitmapImage(fileDynamic07);
                }
                else { fileDynamic07 = null; ClickDynamic07.Content = "Sunset"; ImageDynamic07.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileDynamic07 = null; ClickDynamic07.Content = "Sunset"; ImageDynamic07.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddDynamicThemeVisability();
        }
        private async void XClickDynamic08(object sender, RoutedEventArgs e)
        {
            if (fileDynamic08 == null)
            {
                fileDynamic08 = await PicturePicker();
                if (fileDynamic08 != null)
                {
                    ClickDynamic08.Content = fileDynamic08.Name;
                    ImageDynamic08.Source = await StorageFileToBitmapImage(fileDynamic08);
                }
                else { fileDynamic08 = null; ClickDynamic08.Content = "Sunset"; ImageDynamic08.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileDynamic08 = null; ClickDynamic08.Content = "Sunset"; ImageDynamic08.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddDynamicThemeVisability();
        }
        private async void XClickDynamic09(object sender, RoutedEventArgs e)
        {
            if (fileDynamic09 == null)
            {
                fileDynamic09 = await PicturePicker();
                if (fileDynamic09 != null)
                {
                    ClickDynamic09.Content = fileDynamic09.Name;
                    ImageDynamic09.Source = await StorageFileToBitmapImage(fileDynamic09);
                }
                else { fileDynamic09 = null; ClickDynamic09.Content = "Sunset"; ImageDynamic09.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileDynamic09 = null; ClickDynamic09.Content = "Sunset"; ImageDynamic09.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddDynamicThemeVisability();
        }
        private async void XClickDynamic10(object sender, RoutedEventArgs e)
        {
            if (fileDynamic10 == null)
            {
                fileDynamic10 = await PicturePicker();
                if (fileDynamic10 != null)
                {
                    ClickDynamic10.Content = fileDynamic10.Name;
                    ImageDynamic10.Source = await StorageFileToBitmapImage(fileDynamic10);
                }
                else { fileDynamic10 = null; ClickDynamic10.Content = "Sunset"; ImageDynamic10.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileDynamic10 = null; ClickDynamic10.Content = "Sunset"; ImageDynamic10.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddDynamicThemeVisability();
        }
        private async void XClickDynamic11(object sender, RoutedEventArgs e)
        {
            if (fileDynamic11 == null)
            {
                fileDynamic11 = await PicturePicker();
                if (fileDynamic11 != null)
                {
                    ClickDynamic11.Content = fileDynamic11.Name;
                    ImageDynamic11.Source = await StorageFileToBitmapImage(fileDynamic11);
                }
                else { fileDynamic11 = null; ClickDynamic11.Content = "Sunset"; ImageDynamic11.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileDynamic11 = null; ClickDynamic11.Content = "Sunset"; ImageDynamic11.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddDynamicThemeVisability();
        }
        private async void XClickDynamic12(object sender, RoutedEventArgs e)
        {
            if (fileDynamic12 == null)
            {
                fileDynamic12 = await PicturePicker();
                if (fileDynamic12 != null)
                {
                    ClickDynamic12.Content = fileDynamic12.Name;
                    ImageDynamic12.Source = await StorageFileToBitmapImage(fileDynamic12);
                }
                else { fileDynamic12 = null; ClickDynamic12.Content = "Sunset"; ImageDynamic12.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileDynamic12 = null; ClickDynamic12.Content = "Sunset"; ImageDynamic12.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddDynamicThemeVisability();
        }
        private async void XClickDynamic13(object sender, RoutedEventArgs e)
        {
            if (fileDynamic13 == null)
            {
                fileDynamic13 = await PicturePicker();
                if (fileDynamic13 != null)
                {
                    ClickDynamic13.Content = fileDynamic13.Name;
                    ImageDynamic13.Source = await StorageFileToBitmapImage(fileDynamic13);
                }
                else { fileDynamic13 = null; ClickDynamic13.Content = "Sunset"; ImageDynamic13.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileDynamic13 = null; ClickDynamic13.Content = "Sunset"; ImageDynamic13.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddDynamicThemeVisability();
        }
        private async void XClickDynamic14(object sender, RoutedEventArgs e)
        {
            if (fileDynamic14 == null)
            {
                fileDynamic14 = await PicturePicker();
                if (fileDynamic14 != null)
                {
                    ClickDynamic14.Content = fileDynamic14.Name;
                    ImageDynamic14.Source = await StorageFileToBitmapImage(fileDynamic14);
                }
                else { fileDynamic14 = null; ClickDynamic14.Content = "Sunset"; ImageDynamic14.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileDynamic14 = null; ClickDynamic14.Content = "Sunset"; ImageDynamic14.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddDynamicThemeVisability();
        }
        private async void XClickDynamic15(object sender, RoutedEventArgs e)
        {
            if (fileDynamic15 == null)
            {
                fileDynamic15 = await PicturePicker();
                if (fileDynamic15 != null)
                {
                    ClickDynamic15.Content = fileDynamic15.Name;
                    ImageDynamic15.Source = await StorageFileToBitmapImage(fileDynamic15);
                }
                else { fileDynamic15 = null; ClickDynamic15.Content = "Sunset"; ImageDynamic15.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileDynamic15 = null; ClickDynamic15.Content = "Sunset"; ImageDynamic15.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddDynamicThemeVisability();
        }
        private async void XClickDynamic16(object sender, RoutedEventArgs e)
        {
            if (fileDynamic16 == null)
            {
                fileDynamic16 = await PicturePicker();
                if (fileDynamic16 != null)
                {
                    ClickDynamic16.Content = fileDynamic16.Name;
                    ImageDynamic16.Source = await StorageFileToBitmapImage(fileDynamic16);
                }
                else { fileDynamic16 = null; ClickDynamic16.Content = "Sunset"; ImageDynamic16.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileDynamic16 = null; ClickDynamic16.Content = "Sunset"; ImageDynamic16.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddDynamicThemeVisability();
        }
        private async void SaveDynamicCollection(object sender, RoutedEventArgs e)
        {
            i = 0;
            StorageFile[] storageFiles = new StorageFile[] { fileDynamic01 , fileDynamic02 , fileDynamic03 , fileDynamic04 , fileDynamic05 , fileDynamic06 , fileDynamic07 , fileDynamic08 , fileDynamic09 , fileDynamic11 , fileDynamic10 , fileDynamic12 , fileDynamic13 , fileDynamic14 , fileDynamic15 , fileDynamic16 };
            StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["DynamicFolder"], CreationCollisionOption.OpenIfExists);
            Random random = new Random();
            StorageFolder destination = await Folder.CreateFolderAsync("UserTheme" + random.Next(1000).ToString(), CreationCollisionOption.GenerateUniqueName);
            foreach (StorageFile file in storageFiles)
            { 
                if (file.FileType.Contains("png"))
                { imageType = "png"; }
                else if (file.FileType.Contains("jpeg"))
                { imageType = "jpeg"; }
                else if (file.FileType.Contains("jpg"))
                { imageType = "jpg"; }
                else if (file.FileType.Contains("bmp"))
                { imageType = "bmp"; }
                await file.CopyAsync(destination, $"X{i}.{imageType}", NameCollisionOption.ReplaceExisting);
                i = i + 1;
            }
            AddDynamicTheme.Content = "Collection has been added";
            AddDynamicTheme.IsEnabled = false;
        }
        private async void XClickWeather01(object sender, RoutedEventArgs e)
        {
            if (fileWeather01 == null)
            {
                fileWeather01 = await PicturePicker();
                if (fileWeather01 != null)
                {
                    ClickWeather01.Content = fileWeather01.Name;
                    ImageWeather01.Source = await StorageFileToBitmapImage(fileWeather01);
                }
                else { fileWeather01 = null; ClickWeather01.Content = "Clear sky - Day"; ImageWeather01.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather01 = null; ClickWeather01.Content = "Clear sky - Day"; ImageWeather01.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void XClickWeather02(object sender, RoutedEventArgs e)
        {
            if (fileWeather02 == null)
            {
                fileWeather02 = await PicturePicker();
                if (fileWeather02 != null)
                {
                    ClickWeather02.Content = fileWeather02.Name;
                    ImageWeather02.Source = await StorageFileToBitmapImage(fileWeather02);
                }
                else { fileWeather02 = null; ClickWeather02.Content = "Clear sky - Night"; ImageWeather02.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather02 = null; ClickWeather02.Content = "Clear sky - Night"; ImageWeather02.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void XClickWeather03(object sender, RoutedEventArgs e)
        {
            if (fileWeather03 == null)
            {
                fileWeather03 = await PicturePicker();
                if (fileWeather03 != null)
                {
                    ClickWeather03.Content = fileWeather03.Name;
                    ImageWeather03.Source = await StorageFileToBitmapImage(fileWeather03);
                }
                else { fileWeather03 = null; ClickWeather03.Content = "Few clouds - Day"; ImageWeather03.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather03 = null; ClickWeather03.Content = "Few clouds - Day"; ImageWeather03.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void XClickWeather04(object sender, RoutedEventArgs e)
        {
            if (fileWeather04 == null)
            {
                fileWeather04 = await PicturePicker();
                if (fileWeather04 != null)
                {
                    ClickWeather04.Content = fileWeather04.Name;
                    ImageWeather04.Source = await StorageFileToBitmapImage(fileWeather04);
                }
                else { fileWeather04 = null; ClickWeather04.Content = "Few clouds - Night"; ImageWeather04.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather04 = null; ClickWeather04.Content = "Few clouds - Night"; ImageWeather04.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void XClickWeather05(object sender, RoutedEventArgs e)
        {
            if (fileWeather05 == null)
            {
                fileWeather05 = await PicturePicker();
                if (fileWeather05 != null)
                {
                    ClickWeather05.Content = fileWeather05.Name;
                    ImageWeather05.Source = await StorageFileToBitmapImage(fileWeather05);
                }
                else { fileWeather05 = null; ClickWeather05.Content = "Scattered clouds - Day"; ImageWeather05.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather05 = null; ClickWeather05.Content = "Scattered clouds - Day"; ImageWeather05.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void XClickWeather06(object sender, RoutedEventArgs e)
        {
            if (fileWeather06 == null)
            {
                fileWeather06 = await PicturePicker();
                if (fileWeather06 != null)
                {
                    ClickWeather06.Content = fileWeather06.Name;
                    ImageWeather06.Source = await StorageFileToBitmapImage(fileWeather06);
                }
                else { fileWeather06 = null; ClickWeather06.Content = "Scattered clouds - Night"; ImageWeather06.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather06 = null; ClickWeather06.Content = "Scattered clouds - Night"; ImageWeather06.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void XClickWeather07(object sender, RoutedEventArgs e)
        {
            if (fileWeather07 == null)
            {
                fileWeather07 = await PicturePicker();
                if (fileWeather07 != null)
                {
                    ClickWeather07.Content = fileWeather07.Name;
                    ImageWeather07.Source = await StorageFileToBitmapImage(fileWeather07);
                }
                else { fileWeather07 = null; ClickWeather07.Content = "Broken clouds - Day"; ImageWeather07.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather07 = null; ClickWeather07.Content = "Broken clouds - Day"; ImageWeather07.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void XClickWeather08(object sender, RoutedEventArgs e)
        {
            if (fileWeather08 == null)
            {
                fileWeather08 = await PicturePicker();
                if (fileWeather08 != null)
                {
                    ClickWeather08.Content = fileWeather08.Name;
                    ImageWeather08.Source = await StorageFileToBitmapImage(fileWeather08);
                }
                else { fileWeather08 = null; ClickWeather08.Content = "Broken clouds - Night"; ImageWeather08.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather08 = null; ClickWeather08.Content = "Broken clouds - Night"; ImageWeather08.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void XClickWeather09(object sender, RoutedEventArgs e)
        {
            if (fileWeather09 == null)
            {
                fileWeather09 = await PicturePicker();
                if (fileWeather09 != null)
                {
                    ClickWeather09.Content = fileWeather09.Name;
                    ImageWeather09.Source = await StorageFileToBitmapImage(fileWeather09);
                }
                else { fileWeather09 = null; ClickWeather09.Content = "Shower rain - Day"; ImageWeather09.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather09 = null; ClickWeather09.Content = "Shower rain - Day"; ImageWeather09.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void XClickWeather10(object sender, RoutedEventArgs e)
        {
            if (fileWeather10 == null)
            {
                fileWeather10 = await PicturePicker();
                if (fileWeather10 != null)
                {
                    ClickWeather10.Content = fileWeather10.Name;
                    ImageWeather10.Source = await StorageFileToBitmapImage(fileWeather10);
                }
                else { fileWeather10 = null; ClickWeather10.Content = "Shower rain - Night"; ImageWeather10.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather10 = null; ClickWeather10.Content = "Shower rain - Night"; ImageWeather10.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void XClickWeather11(object sender, RoutedEventArgs e)
        {
            if (fileWeather11 == null)
            {
                fileWeather11 = await PicturePicker();
                if (fileWeather11 != null)
                {
                    ClickWeather11.Content = fileWeather11.Name;
                    ImageWeather11.Source = await StorageFileToBitmapImage(fileWeather11);
                }
                else { fileWeather11 = null; ClickWeather11.Content = "Rain - Day"; ImageWeather11.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather11 = null; ClickWeather11.Content = "Rain - Day"; ImageWeather11.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void XClickWeather12(object sender, RoutedEventArgs e)
        {
            if (fileWeather12 == null)
            {
                fileWeather12 = await PicturePicker();
                if (fileWeather12 != null)
                {
                    ClickWeather12.Content = fileWeather12.Name;
                    ImageWeather12.Source = await StorageFileToBitmapImage(fileWeather12);
                }
                else { fileWeather12 = null; ClickWeather12.Content = "Rain - Night"; ImageWeather12.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather12 = null; ClickWeather12.Content = "Rain - Night"; ImageWeather12.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void XClickWeather13(object sender, RoutedEventArgs e)
        {
            if (fileWeather13 == null)
            {
                fileWeather13 = await PicturePicker();
                if (fileWeather13 != null)
                {
                    ClickWeather13.Content = fileWeather13.Name;
                    ImageWeather13.Source = await StorageFileToBitmapImage(fileWeather13);
                }
                else { fileWeather13 = null; ClickWeather13.Content = "Thunderstorm - Day"; ImageWeather13.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather13 = null; ClickWeather13.Content = "Thunderstorm - Day"; ImageWeather13.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void XClickWeather14(object sender, RoutedEventArgs e)
        {
            if (fileWeather14 == null)
            {
                fileWeather14 = await PicturePicker();
                if (fileWeather14 != null)
                {
                    ClickWeather14.Content = fileWeather14.Name;
                    ImageWeather14.Source = await StorageFileToBitmapImage(fileWeather14);
                }
                else { fileWeather14 = null; ClickWeather14.Content = "Thunderstorm - Night"; ImageWeather14.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather14 = null; ClickWeather14.Content = "Thunderstorm - Night"; ImageWeather14.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void XClickWeather15(object sender, RoutedEventArgs e)
        {
            if (fileWeather15 == null)
            {
                fileWeather15 = await PicturePicker();
                if (fileWeather15 != null)
                {
                    ClickWeather15.Content = fileWeather15.Name;
                    ImageWeather15.Source = await StorageFileToBitmapImage(fileWeather15);
                }
                else { fileWeather15 = null; ClickWeather15.Content = "Snow - Day"; ImageWeather15.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather15 = null; ClickWeather15.Content = "Snow - Day"; ImageWeather15.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void XClickWeather16(object sender, RoutedEventArgs e)
        {
            if (fileWeather16 == null)
            {
                fileWeather16 = await PicturePicker();
                if (fileWeather16 != null)
                {
                    ClickWeather16.Content = fileWeather16.Name;
                    ImageWeather16.Source = await StorageFileToBitmapImage(fileWeather16);
                }
                else { fileWeather16 = null; ClickWeather16.Content = "Snow - Night"; ImageWeather16.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather16 = null; ClickWeather16.Content = "Snow - Night"; ImageWeather16.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void XClickWeather17(object sender, RoutedEventArgs e)
        {
            if (fileWeather17 == null)
            {
                fileWeather17 = await PicturePicker();
                if (fileWeather17 != null)
                {
                    ClickWeather17.Content = fileWeather17.Name;
                    ImageWeather17.Source = await StorageFileToBitmapImage(fileWeather17);
                }
                else { fileWeather17 = null; ClickWeather17.Content = "Mist - Day"; ImageWeather17.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather17 = null; ClickWeather17.Content = "Mist - Day"; ImageWeather17.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void XClickWeather18(object sender, RoutedEventArgs e)
        {
            if (fileWeather18 == null)
            {
                fileWeather18 = await PicturePicker();
                if (fileWeather18 != null)
                {
                    ClickWeather18.Content = fileWeather18.Name;
                    ImageWeather18.Source = await StorageFileToBitmapImage(fileWeather18);
                }
                else { fileWeather18 = null; ClickWeather18.Content = "Mist - Night"; ImageWeather18.Source = new BitmapImage(new Uri(imageURI)); }
            }
            else { fileWeather18 = null; ClickWeather18.Content = "Mist - Night"; ImageWeather18.Source = new BitmapImage(new Uri(imageURI)); }
            CheckAddWeatherThemeVisability();
        }
        private async void SaveWeatherCollection(object sender, RoutedEventArgs e)
        {
            i = 0;
            StorageFile[] storageFiles = new StorageFile[] { fileWeather01, fileWeather02, fileWeather03, fileWeather04, fileWeather05, fileWeather06, fileWeather07, fileWeather08, fileWeather09, fileWeather11, fileWeather10, fileWeather12, fileWeather13, fileWeather14, fileWeather15, fileWeather16, fileWeather17, fileWeather18 };
            StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["WeatherFolder"], CreationCollisionOption.OpenIfExists);
            Random random = new Random();
            StorageFolder destination = await Folder.CreateFolderAsync("UserTheme" + random.Next(1000).ToString(), CreationCollisionOption.GenerateUniqueName);
            foreach (StorageFile file in storageFiles)
            {
                if (file.FileType.Contains("png"))
                { imageType = "png"; }
                else if (file.FileType.Contains("jpeg"))
                { imageType = "jpeg"; }
                else if (file.FileType.Contains("jpg"))
                { imageType = "jpg"; }
                else if (file.FileType.Contains("bmp"))
                { imageType = "bmp"; }
                await file.CopyAsync(destination, $"X{i}.{imageType}", NameCollisionOption.ReplaceExisting);
                i = i + 1;
            }
            AddWeatherTheme.Content = "Collection has been added";
            AddWeatherTheme.IsEnabled = false;
        }

        private void GoBackToMainPage(object sender, RoutedEventArgs e)
        {
            fileDynamic01 = null;
            fileDynamic02 = null;
            fileDynamic03 = null;
            fileDynamic04 = null;
            fileDynamic05 = null;
            fileDynamic06 = null;
            fileDynamic07 = null;
            fileDynamic08 = null;
            fileDynamic09 = null;
            fileDynamic10 = null;
            fileDynamic11 = null;
            fileDynamic12 = null;
            fileDynamic13 = null;
            fileDynamic14 = null;
            fileDynamic15 = null;
            fileDynamic16 = null;
            fileWeather01 = null;
            fileWeather02 = null;
            fileWeather03 = null;
            fileWeather04 = null;
            fileWeather05 = null;
            fileWeather06 = null;
            fileWeather07 = null;
            fileWeather08 = null;
            fileWeather09 = null;
            fileWeather10 = null;
            fileWeather11 = null;
            fileWeather12 = null;
            fileWeather13 = null;
            fileWeather14 = null;
            fileWeather15 = null;
            fileWeather16 = null;
            fileWeather17 = null;
            fileWeather18 = null;
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
