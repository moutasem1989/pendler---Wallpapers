using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;

namespace pendler
{
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<DynamicWallpaperBitmaps> DynamicWallpaperBitmap = new ObservableCollection<DynamicWallpaperBitmaps>();
        public ObservableCollection<ModernWallpaperBitmaps> ModernWallpaperBitmap = new ObservableCollection<ModernWallpaperBitmaps>();
        public ObservableCollection<WeatherWallpaperBitmaps> WeatherWallpaperBitmap = new ObservableCollection<WeatherWallpaperBitmaps>();
        public ObservableCollection<TimerWallpaperBitmaps> TimerWallpaperBitmap = new ObservableCollection<TimerWallpaperBitmaps>();
        public static string imageType { get; set; }
        public static string imageTimerRepeat { get; set; }
        public static string imageTimerRepeatForCollection { get; set; }
        public bool notfirstloaded { get; private set; }
        public bool SetBackgroundTaskForTimer { get; private set; }
        private StorageFile selectedTimerImage;

        public MainPage()
        {
            this.InitializeComponent();
            ApplicationView.PreferredLaunchViewSize = new Windows.Foundation.Size(500, 1000);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            if (ApplicationData.Current.LocalSettings.Values["SliderValue"] == null)
            {
                ApplicationData.Current.LocalSettings.Values["SliderValue"] = Convert.ToDouble(30);
                BackgroundSlider.Value = (double)ApplicationData.Current.LocalSettings.Values["SliderValue"];
            }
            else
            {
                BackgroundSlider.Value = (double)ApplicationData.Current.LocalSettings.Values["SliderValue"];
            }
            ApplicationViewTitleBar appTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            appTitleBar.BackgroundColor = Colors.Transparent;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            GetTaskAsync();
        }
        public async void GetTaskAsync()
        {
            ProgressBar.Visibility = Visibility.Visible;
            var random = new Random();
            if (ApplicationData.Current.LocalSettings.Values["Acent"] == null) { ApplicationData.Current.LocalSettings.Values["Acent"] = String.Format("#{0:X6}", random.Next(0x1000000)); }
            if (ApplicationData.Current.LocalSettings.Values["Color"] == null) { ApplicationData.Current.LocalSettings.Values["Color"] = String.Format("#{0:X6}", random.Next(0x1000000)); }
            if (ApplicationData.Current.LocalSettings.Values["DynamicAccentA"] == null) { ApplicationData.Current.LocalSettings.Values["DynamicAccentA"] = String.Format("#{0:X6}", random.Next(0x1000000)); }
            if (ApplicationData.Current.LocalSettings.Values["DynamicAccentB"] == null) { ApplicationData.Current.LocalSettings.Values["DynamicAccentB"] = String.Format("#{0:X6}", random.Next(0x1000000)); }
            if (ApplicationData.Current.LocalSettings.Values["DynamicColorA"] == null) { ApplicationData.Current.LocalSettings.Values["DynamicColorA"] = String.Format("#{0:X6}", random.Next(0x1000000)); }
            if (ApplicationData.Current.LocalSettings.Values["DynamicColorB"] == null) { ApplicationData.Current.LocalSettings.Values["DynamicColorB"] = String.Format("#{0:X6}", random.Next(0x1000000)); }
            if (ApplicationData.Current.LocalSettings.Values["DynamicFolder"] == null) { ApplicationData.Current.LocalSettings.Values["DynamicFolder"] = String.Format("{0:X6}", random.Next(0x1000000)); }
            if (ApplicationData.Current.LocalSettings.Values["WeatherFolder"] == null) { ApplicationData.Current.LocalSettings.Values["WeatherFolder"] = String.Format("{0:X6}", random.Next(0x1000000)); }
            if (ApplicationData.Current.LocalSettings.Values["ModernFolder"] == null) { ApplicationData.Current.LocalSettings.Values["ModernFolder"] = String.Format("{0:X6}", random.Next(0x1000000)); }
            if (ApplicationData.Current.LocalSettings.Values["ModernFile"] == null) { ApplicationData.Current.LocalSettings.Values["ModernFile"] = String.Format("{0:X6}", random.Next(0x1000000)); }
            if (ApplicationData.Current.LocalSettings.Values["TimerFolder"] == null) { ApplicationData.Current.LocalSettings.Values["TimerFolder"] = String.Format("{0:X6}", random.Next(0x1000000)); }
            ApplicationData.Current.LocalSettings.Values["DynamicClicked"] = null;
            ApplicationData.Current.LocalSettings.Values["WeatherClicked"] = null;
            this.DynamicOperation.Text = "Add or create collections of images to be used for Dynamic and Weather wallpapers.";
            try
            {
                await FindNullSettings();
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0001x: Cannot Creat Null Settings"; }
            try
            {
                LoadCheckedToggles();
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0002x: Cannot Load Toggel Settings"; }
            try
            {
                await LoadDynamicIcon();
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0003x: Loading Dynamic Icon"; }
            try
            {
                await LoadModernIcon();
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0004x: Loading Modern Icon"; }
            try
            {
                await LoadWeatherIcon();
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0005x: Loading Weather Icon"; }
            try
            {
                LookForTimerImages();
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0006x: Finding Timer Images"; }
            try
            {
                await LookUpLibrariesDynamic();
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0007x: Loading Dynamic Library"; }
            try
            {
                await LookUpLibrariesWeather();
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0008x: Loading Weather Library"; }
            try
            {
                FirstRunIntro();
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0009x: Running First Start Dialog"; }
            try
            {
                StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["TimerFolder"], CreationCollisionOption.OpenIfExists);
                StorageFileQueryResult result = Folder.CreateFileQuery();
                var themeTimerFiles = await result.GetFilesAsync();
                string allIDs = (string)ApplicationData.Current.LocalSettings.Values["AllTimerIDs"];
                foreach (StorageFile themeDynamic in themeTimerFiles)
                {
                    if (allIDs.Contains(themeDynamic.DisplayName) != true)
                    {
                        await themeDynamic.DeleteAsync();
                    }
                }
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0010x: Loading Timer Images"; }
            ProgressBar.Visibility = Visibility.Collapsed;
        }
        public async void FirstRunIntro()
        {
            if (ApplicationData.Current.LocalSettings.Values["FirstRunIntro"] == null)
            {
                
                ImageIntro01.Source = new BitmapImage(new Uri("ms-appx:///Assets/monitor.png"));
                TextIntro01.Text = "Welcome to Wallpapers!";
                ImageIntro02.Source = new BitmapImage(new Uri("ms-appx:///Assets/lock.png"));
                TextIntro02.Text = "Privacy: Open Source with no server syncing or special access permissions - All data remain local.";
                ImageIntro03.Source = new BitmapImage(new Uri("ms-appx:///Assets/day.png"));
                TextIntro03.Text = "Create Dynamic Wallpaper corresponding to Sunrise and Sunset.";
                ImageIntro04.Source = new BitmapImage(new Uri("ms-appx:///Assets/umbrella.png"));
                TextIntro04.Text = "Use Weather information as wallpaper.";
                ImageIntro05.Source = new BitmapImage(new Uri("ms-appx:///Assets/paint-brush.png"));
                TextIntro05.Text = "Choose Wallpaper pattern with custom dynamic colours for easy personalization experience.";
                ImageIntro06.Source = new BitmapImage(new Uri("ms-appx:///Assets/placeholder.png"));
                TextIntro06.Text = "Use low accuracy location data to do all of the necessary calculations.";
                ImageIntro06.Source = new BitmapImage(new Uri("ms-appx:///Assets/placeholder.png"));
                ImageIntro07.Source = new BitmapImage(new Uri("ms-appx:///Assets/project.png"));

                await FirstRunIntroDialog.ShowAsync();
            }
        }
        private async Task FindNullSettings()
        {
            if (ApplicationData.Current.LocalSettings.Values["NullSettings"] == null)
            {
                
                ApplicationData.Current.LocalSettings.Values["Latitude"] = "0";
                ApplicationData.Current.LocalSettings.Values["Longitude"] = "0";
                ApplicationData.Current.LocalSettings.Values["Accuracy"] = "0";
                ApplicationData.Current.LocalSettings.Values["WeatherImageID"] = 0;
                ApplicationData.Current.LocalSettings.Values["DynamicImageID"] = 0;
                ApplicationData.Current.LocalSettings.Values["AllTimerIDs"] = "";
                await RERegisterBackgroundTask();
                ApplicationData.Current.LocalSettings.Values["NullSettings"] = "true";
            }
        }
        private void LoadCheckedToggles()
        {
            if ((string)ApplicationData.Current.LocalSettings.Values["DeskDynamicToggled"] == "true") { DeskDynamicToggled.IsChecked = true; } else { DeskDynamicToggled.IsChecked = false; }
            if ((string)ApplicationData.Current.LocalSettings.Values["LockDynamicToggled"] == "true") { LockDynamicToggled.IsChecked = true; } else { LockDynamicToggled.IsChecked = false; }
            if ((string)ApplicationData.Current.LocalSettings.Values["DeskModernToggled"] == "true") { DeskModernToggled.IsChecked = true; } else { DeskModernToggled.IsChecked = false; }
            if ((string)ApplicationData.Current.LocalSettings.Values["LockModernToggled"] == "true") { LockModernToggled.IsChecked = true; } else { LockModernToggled.IsChecked = false; }
            if ((string)ApplicationData.Current.LocalSettings.Values["DeskWeatherToggled"] == "true") { DeskWeatherToggled.IsChecked = true; } else { DeskWeatherToggled.IsChecked = false; }
            if ((string)ApplicationData.Current.LocalSettings.Values["LockWeatherToggled"] == "true") { LockWeatherToggled.IsChecked = true; } else { LockWeatherToggled.IsChecked = false; }
            var buttonSymbol = new SymbolIcon();
            buttonSymbol.Symbol = Symbol.Pictures;
            ImageTimerSelecter.Content = buttonSymbol;
            SelectedImage.Source = null;
            ProgressBar.Visibility = Visibility.Collapsed;
        }
        private void ColorButtonsColors()
        {
            if ((string)ApplicationData.Current.LocalSettings.Values["DynamicAccent"] == "true")
            {
                AcentGradient1Button.Visibility = Visibility.Visible;
                AcentGradSwitcher.Visibility = Visibility.Visible;
                AcentGradient2Button.Visibility = Visibility.Visible;
                AccentColorButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                AccentColorButton.Visibility = Visibility.Visible;
                AcentGradient1Button.Visibility = Visibility.Collapsed;
                AcentGradSwitcher.Visibility = Visibility.Collapsed;
                AcentGradient2Button.Visibility = Visibility.Collapsed;
            }
            if ((string)ApplicationData.Current.LocalSettings.Values["DynamicColor"] == "true")
            {
                ColorGradient1Button.Visibility = Visibility.Visible;
                ColorGradSwitcher.Visibility = Visibility.Visible;
                ColorGradient2Button.Visibility = Visibility.Visible;
                ColorColorButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                ColorColorButton.Visibility = Visibility.Visible;
                ColorGradient1Button.Visibility = Visibility.Collapsed;
                ColorGradSwitcher.Visibility = Visibility.Collapsed;
                ColorGradient2Button.Visibility = Visibility.Collapsed;
            }
            string flatWallpaperAcent = (string)ApplicationData.Current.LocalSettings.Values["Acent"];
            var a = flatWallpaperAcent.Replace("#", "");
            string flatWallpaperColor = (string)ApplicationData.Current.LocalSettings.Values["Color"];
            var b = flatWallpaperColor.Replace("#", "");
            string ga1 = (string)ApplicationData.Current.LocalSettings.Values["DynamicAccentA"];
            var a1 = ga1.Replace("#", "");
            string ga2 = (string)ApplicationData.Current.LocalSettings.Values["DynamicAccentB"];
            var a2 = ga2.Replace("#", "");
            string gc1 = (string)ApplicationData.Current.LocalSettings.Values["DynamicColorA"];
            var b1 = gc1.Replace("#", "");
            string gc2 = (string)ApplicationData.Current.LocalSettings.Values["DynamicColorB"];
            var b2 = gc2.Replace("#", "");
            Windows.UI.Color acent = Windows.UI.Color.FromArgb(255, byte.Parse(a.Substring(0, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(a.Substring(2, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(a.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
            Windows.UI.Color color = Windows.UI.Color.FromArgb(255, byte.Parse(b.Substring(0, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(b.Substring(2, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(b.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
            Windows.UI.Color acent1 = Windows.UI.Color.FromArgb(255, byte.Parse(a1.Substring(0, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(a1.Substring(2, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(a1.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
            Windows.UI.Color acent2 = Windows.UI.Color.FromArgb(255, byte.Parse(a2.Substring(0, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(a2.Substring(2, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(a2.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
            Windows.UI.Color color1 = Windows.UI.Color.FromArgb(255, byte.Parse(b1.Substring(0, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(b1.Substring(2, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(b1.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
            Windows.UI.Color color2 = Windows.UI.Color.FromArgb(255, byte.Parse(b2.Substring(0, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(b2.Substring(2, 2), System.Globalization.NumberStyles.HexNumber), byte.Parse(b2.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
            AcentGradient1.Color = acent1;
            AcentGradient2.Color = acent2;
            ColorGradient1.Color = color1;
            ColorGradient2.Color = color2;
            AcentGradient1Button.Background = new SolidColorBrush(acent1);
            AcentGradient1Picker.Color = acent1;
            AcentGradient2Button.Background = new SolidColorBrush(acent2);
            AcentGradient2Picker.Color = acent2;
            ColorGradient1Button.Background = new SolidColorBrush(color1);
            ColorGradient1Picker.Color = color1;
            ColorGradient2Button.Background = new SolidColorBrush(color2);
            ColorGradient2Picker.Color = color2;
            AccentColorButton.Background = new SolidColorBrush(acent);
            AccentColorPicker.Color = acent;
            ColorColorButton.Background = new SolidColorBrush(color);
            ColorColorPicker.Color = color;
        }
        private void DeskDynamic(object sender, TappedRoutedEventArgs e)
        {
            DeskModernToggled.IsChecked = false;
            DeskWeatherToggled.IsChecked = false;
            if (DeskDynamicToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["DeskDynamicToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["DeskDynamicToggled"] = null; }
            if (DeskModernToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["DeskModernToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["DeskModernToggled"] = null; }
            if (DeskWeatherToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["DeskWeatherToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["DeskWeatherToggled"] = null; }

        }
        private void LockDynamic(object sender, TappedRoutedEventArgs e)
        {
            LockModernToggled.IsChecked = false;
            LockWeatherToggled.IsChecked = false;
            if (LockDynamicToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["LockDynamicToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["LockDynamicToggled"] = null; }
            if (LockModernToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["LockModernToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["LockModernToggled"] = null; }
            if (LockWeatherToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["LockWeatherToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["LockWeatherToggled"] = null; }
        }
        private async void DeskModern(object sender, TappedRoutedEventArgs e)
        {
            DeskDynamicToggled.IsChecked = false;
            DeskWeatherToggled.IsChecked = false;
            if (DeskDynamicToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["DeskDynamicToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["DeskDynamicToggled"] = null; }
            if (DeskModernToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["DeskModernToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["DeskModernToggled"] = null; }
            if (DeskWeatherToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["DeskWeatherToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["DeskWeatherToggled"] = null; }

            await Libraries.ModernWallpaperImageHandler.HandleModernImage();
        }
        private async void LockModern(object sender, TappedRoutedEventArgs e)
        {
            LockDynamicToggled.IsChecked = false;
            LockWeatherToggled.IsChecked = false;
            if (LockDynamicToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["LockDynamicToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["LockDynamicToggled"] = null; }
            if (LockModernToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["LockModernToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["LockModernToggled"] = null; }
            if (LockWeatherToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["LockWeatherToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["LockWeatherToggled"] = null; }

            await Libraries.ModernWallpaperImageHandler.HandleModernImage();
        }
        private void DeskWeather(object sender, TappedRoutedEventArgs e)
        {
            DeskModernToggled.IsChecked = false;
            DeskDynamicToggled.IsChecked = false;
            if (DeskDynamicToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["DeskDynamicToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["DeskDynamicToggled"] = null; }
            if (DeskModernToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["DeskModernToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["DeskModernToggled"] = null; }
            if (DeskWeatherToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["DeskWeatherToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["DeskWeatherToggled"] = null; }
        }
        private void LockWeather(object sender, TappedRoutedEventArgs e)
        {
            LockModernToggled.IsChecked = false;
            LockDynamicToggled.IsChecked = false;
            if (LockDynamicToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["LockDynamicToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["LockDynamicToggled"] = null; }
            if (LockModernToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["LockModernToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["LockModernToggled"] = null; }
            if (LockWeatherToggled.IsChecked == true) { ApplicationData.Current.LocalSettings.Values["LockWeatherToggled"] = "true"; } else { ApplicationData.Current.LocalSettings.Values["LockWeatherToggled"] = null; }
        }
        private async void DynamicIconClick(object sender, RoutedEventArgs e)
        {
            await DynamicContentDialog.ShowAsync();
            if (DynamicWallpaperBitmap.Count != 0) { DynamicDeleteButton.IsEnabled = true; }
            else { DynamicDeleteButton.IsEnabled = false; }
        }
        private async void ModernIconClick(object sender, RoutedEventArgs e)
        {
            ModernIcon.IsEnabled = false;
            ColorButtonsColors();
            try
            {
                await LookingForModernThumbs();
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0101x: Cannot Look For Modern Images Thumbs"; }
            await ModernContentDialog.ShowAsync();
            ModernIcon.IsEnabled = true;
        }
        private async Task LookingForModernThumbs()
        {
            try
            {
                LoadImageColors.Visibility = Visibility.Visible;
                ModernWallpaperBitmap.Clear();
                IStorageItem storageItem = await Windows.Storage.ApplicationData.Current.LocalFolder.TryGetItemAsync((string)ApplicationData.Current.LocalSettings.Values["ModernFolder"]);
                Libraries.ImageColourManagement.GetColorsForModernWallpaper();
                if (storageItem != null)
                {
                    StorageFolder folder = await ApplicationData.Current.LocalFolder.GetFolderAsync((string)ApplicationData.Current.LocalSettings.Values["ModernFolder"]);
                    var queryOption = new QueryOptions { FolderDepth = FolderDepth.Deep };
                    var subFolders = await folder.CreateFolderQueryWithOptions(queryOption).GetFoldersAsync();
                    foreach (StorageFolder subFolder in subFolders)
                    {
                        string imagePathe = $@"{subFolder.Path}/";
                        string[] allImages = Directory.GetFiles(imagePathe, $"X{Libraries.ImageColourManagement.ModernAcent}-{Libraries.ImageColourManagement.ModernColor}.*");
                        if (allImages.Length == 0)
                        {
                            string[] oreginalImage = Directory.GetFiles(imagePathe, $"{(string)ApplicationData.Current.LocalSettings.Values["ModernFile"]}.*");
                            await Libraries.ImageColourManagement.ApplyColor(oreginalImage[0], subFolder.Name);
                            string[] newAllImages = Directory.GetFiles(imagePathe, $"{Libraries.ImageColourManagement.ModernAcent}-{Libraries.ImageColourManagement.ModernColor}.*");
                            BitmapImage bitmap = new BitmapImage(new Uri(newAllImages[0]));
                            bitmap.DecodePixelHeight = 50;
                            ModernWallpaperBitmap.Add(new ModernWallpaperBitmaps(bitmap, subFolder.Name.ToString()));
                        }
                        else
                        {
                            BitmapImage bitmap = new BitmapImage(new Uri(allImages[0]));
                            bitmap.DecodePixelHeight = 50;
                            ModernWallpaperBitmap.Add(new ModernWallpaperBitmaps(bitmap, subFolder.Name.ToString()));
                        }
                    }
                }
                LoadImageColors.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = "Error 0102x: Looking For Modern Imagis Thumbs";
            }
        }
        private async void WeatherIconClick(object sender, RoutedEventArgs e)
        {
            await WeatherContentDialog.ShowAsync();
            if (WeatherWallpaperBitmap.Count != 0 ) { WeatherDeleteButton.IsEnabled = true; }
            else { WeatherDeleteButton.IsEnabled = false; }
        }
        private async Task RERegisterBackgroundTask()
        {
            try
            {
                var result = await BackgroundExecutionManager.RequestAccessAsync();
                if (result != BackgroundAccessStatus.DeniedByUser)
                {
                    BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
                    builder.Name = "BackgroundTrigger";
                    builder.TaskEntryPoint = "RuntimeComponent_BackgroundTasks.ModernWallpaper";
                    builder.SetTrigger(new TimeTrigger(Convert.ToUInt32(BackgroundSlider.Value), false));
                    builder.AddCondition(new SystemCondition(SystemConditionType.BackgroundWorkCostNotHigh));
                    BackgroundTaskRegistration task = builder.Register();
                    ApplicationData.Current.LocalSettings.Values["taskRegistered"] = "true";
                }
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0105x: Background Execution Manager"; }
            
        }
        private void UNRegisterBackgroundTask()
        {
            foreach (var bgTask in BackgroundTaskRegistration.AllTasks)
                if (bgTask.Value.Name == "BackgroundTrigger")
                    bgTask.Value.Unregister(true);
            ApplicationData.Current.LocalSettings.Values["taskRegistered"] = null;
        }
        private async void CancelModernContentDialog(object sender, RoutedEventArgs e)
        {
            ModernContentDialog.Hide();
            await LoadModernIcon();
        }
        private async void CancelDynamicContentDialog(object sender, RoutedEventArgs e)
        {
            DynamicContentDialog.Hide();
            await LoadDynamicIcon();
        }
        private async void CancelWeatherContentDialog(object sender, RoutedEventArgs e)
        {
            WeatherContentDialog.Hide();
            await LoadWeatherIcon();
        }
        private void AccentColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            Windows.UI.Color color = Windows.UI.Color.FromArgb(255, AccentColorPicker.Color.R, AccentColorPicker.Color.G, AccentColorPicker.Color.B);
            ApplicationData.Current.LocalSettings.Values["Acent"] = color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            ApplicationData.Current.LocalSettings.Values["DynamicAccent"] = null;
            AccentColorButton.Background = new SolidColorBrush(color);
        }
        private void ColorColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            Windows.UI.Color color = Windows.UI.Color.FromArgb(255, ColorColorPicker.Color.R, ColorColorPicker.Color.G, ColorColorPicker.Color.B);
            ApplicationData.Current.LocalSettings.Values["Color"] = color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            ApplicationData.Current.LocalSettings.Values["DynamicColor"] = null;
            ColorColorButton.Background = new SolidColorBrush(color);
        }
        private async void SwitchColors(object sender, RoutedEventArgs e)
        {
            SwitchColoursButton.IsEnabled = false;
            var a = (string)ApplicationData.Current.LocalSettings.Values["DynamicColorA"];
            var b = (string)ApplicationData.Current.LocalSettings.Values["DynamicColorB"];
            var c = (string)ApplicationData.Current.LocalSettings.Values["DynamicAccentA"];
            var d = (string)ApplicationData.Current.LocalSettings.Values["DynamicAccentB"];
            var c1 = (string)ApplicationData.Current.LocalSettings.Values["Color"];
            var a1 = (string)ApplicationData.Current.LocalSettings.Values["Acent"];
            var e1 = (string)ApplicationData.Current.LocalSettings.Values["DynamicColor"];
            var f = (string)ApplicationData.Current.LocalSettings.Values["DynamicAccent"];

            ApplicationData.Current.LocalSettings.Values["DynamicColorA"] = c;
            ApplicationData.Current.LocalSettings.Values["DynamicColorB"] = d;
            ApplicationData.Current.LocalSettings.Values["DynamicAccentA"] = a;
            ApplicationData.Current.LocalSettings.Values["DynamicAccentB"] = b;
            ApplicationData.Current.LocalSettings.Values["Color"] = a1;
            ApplicationData.Current.LocalSettings.Values["Acent"] = c1;
            ApplicationData.Current.LocalSettings.Values["DynamicColor"] = f;
            ApplicationData.Current.LocalSettings.Values["DynamicAccent"] = e1;
            ColorButtonsColors();
            await LookingForModernThumbs();
            SwitchColoursButton.IsEnabled = true;
        }
        private void ColorGradient1Changed(ColorPicker sender, ColorChangedEventArgs args)
        {
            Windows.UI.Color color = Windows.UI.Color.FromArgb(255, ColorGradient1Picker.Color.R, ColorGradient1Picker.Color.G, ColorGradient1Picker.Color.B);
            ApplicationData.Current.LocalSettings.Values["DynamicColorA"] = color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            ApplicationData.Current.LocalSettings.Values["DynamicColor"] = "true";
            ColorGradient1Button.Background = new SolidColorBrush(color);
            ColorGradient1.Color = color;
        }
        private void ColorGradient2Changed(ColorPicker sender, ColorChangedEventArgs args)
        {
            Windows.UI.Color color = Windows.UI.Color.FromArgb(255, ColorGradient2Picker.Color.R, ColorGradient2Picker.Color.G, ColorGradient2Picker.Color.B);
            ApplicationData.Current.LocalSettings.Values["DynamicColorB"] = color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            ApplicationData.Current.LocalSettings.Values["DynamicColor"] = "true";
            ColorGradient2Button.Background = new SolidColorBrush(color);
            ColorGradient2.Color = color;
        }
        private async void SwitchColorGradient(object sender, RoutedEventArgs e)
        {
            ColorGradSwitcher.IsEnabled = false;
            var a = (string)ApplicationData.Current.LocalSettings.Values["DynamicColorA"];
            var b = (string)ApplicationData.Current.LocalSettings.Values["DynamicColorB"];
            ApplicationData.Current.LocalSettings.Values["DynamicColorA"] = b;
            ApplicationData.Current.LocalSettings.Values["DynamicColorB"] = a;
            await LookingForModernThumbs();
            ColorButtonsColors();
            ColorGradSwitcher.IsEnabled = true;
        }
        private void AcentGradient1Changed(ColorPicker sender, ColorChangedEventArgs args)
        {
            Windows.UI.Color color = Windows.UI.Color.FromArgb(255, AcentGradient1Picker.Color.R, AcentGradient1Picker.Color.G, AcentGradient1Picker.Color.B);
            ApplicationData.Current.LocalSettings.Values["DynamicAccentA"] = color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            ApplicationData.Current.LocalSettings.Values["DynamicAccent"] = "true";
            AcentGradient1Button.Background = new SolidColorBrush(color);
            AcentGradient1.Color = color;
        }
        private void AcentGradient2Changed(ColorPicker sender, ColorChangedEventArgs args)
        {
            Windows.UI.Color color = Windows.UI.Color.FromArgb(255, AcentGradient2Picker.Color.R, AcentGradient2Picker.Color.G, AcentGradient2Picker.Color.B);
            ApplicationData.Current.LocalSettings.Values["DynamicAccentB"] = color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            ApplicationData.Current.LocalSettings.Values["DynamicAccent"] = "true";
            AcentGradient2Button.Background = new SolidColorBrush(color);
            AcentGradient2.Color = color;
        }
        private async void SwitchAcentGradient(object sender, RoutedEventArgs e)
        {
            AcentGradSwitcher.IsEnabled = false;
            var a = (string)ApplicationData.Current.LocalSettings.Values["DynamicAccentA"];
            var b = (string)ApplicationData.Current.LocalSettings.Values["DynamicAccentB"];
            ApplicationData.Current.LocalSettings.Values["DynamicAccentA"] = b;
            ApplicationData.Current.LocalSettings.Values["DynamicAccentB"] = a;
            await LookingForModernThumbs();
            ColorButtonsColors();
            AcentGradSwitcher.IsEnabled = true;
        }
        private async void ColorConstant(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["DynamicColor"] = null;
            ColorButtonsColors();
            if (Fly1.IsOpen == true) { Fly1.Hide(); }
            else if (Fly2.IsOpen == true) { Fly2.Hide(); }
            else { await LookingForModernThumbs(); }
        }
        private async void ColorDynamic(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["DynamicColor"] = "true";
            ColorButtonsColors();
            if (Fly3.IsOpen == true) { Fly3.Hide(); }
            else { await LookingForModernThumbs(); }
        }
        private async void AcentConstant(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["DynamicAccent"] = null;
            ColorButtonsColors();
            if (Fly4.IsOpen == true) { Fly4.Hide(); }
            else if (Fly5.IsOpen == true) { Fly5.Hide(); }
            else { await LookingForModernThumbs(); }
        }
        private async void AcentDynamic(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["DynamicAccent"] = "true";
            ColorButtonsColors();
            if (Fly6.IsOpen == true) { Fly6.Hide(); }
            else { await LookingForModernThumbs(); }
        }
        private async void FlyClosing(Windows.UI.Xaml.Controls.Primitives.FlyoutBase sender, Windows.UI.Xaml.Controls.Primitives.FlyoutBaseClosingEventArgs args)
        {
            await LookingForModernThumbs();
        }
        private void ModernItemClicked(object sender, ItemClickEventArgs e)
        {
            var moderntheme = (ModernWallpaperBitmaps)e.ClickedItem;
            var clickedname = moderntheme.FolderName;
            ApplicationData.Current.LocalSettings.Values["ModernClicked"] = clickedname;
        }
        private async void ApplyModernTheme(object sender, RoutedEventArgs e)
        {
            ModernContentDialog.Hide();
            var modernthemeClicked = ApplicationData.Current.LocalSettings.Values["ModernClicked"];
            ApplicationData.Current.LocalSettings.Values["ModernTheme"] = modernthemeClicked;
            ApplicationData.Current.LocalSettings.Values["ModernClicked"] = null;
            try
            {
                await Libraries.ModernWallpaperImageHandler.HandleModernImage();
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0106x: Handle Modern Images"; }
            await LoadModernIcon();
        }
        private async Task LoadModernIcon()
        {
            GeneralProgressBar.Visibility = Visibility.Visible;
            ModernWallpaperIcon.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/ModernIcon.png"));
            ModernWallpaperIcon.Opacity = 0.5;
            DeskModernToggled.IsEnabled = false;
            LockModernToggled.IsEnabled = false;
            try
            {
                if (ApplicationData.Current.LocalSettings.Values["ModernTheme"] != null)
                {
                    string themeFolder = ApplicationData.Current.LocalSettings.Values["ModernTheme"].ToString();
                    StorageFolder modernFolderGripp = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFolderAsync((string)ApplicationData.Current.LocalSettings.Values["ModernFolder"]);
                    StorageFolder subFolder = await modernFolderGripp.GetFolderAsync(themeFolder);
                    Libraries.ImageColourManagement.GetColorsForModernWallpaper();

                    string imagePathe = $@"{subFolder.Path}/";
                    string[] allImages = Directory.GetFiles(imagePathe, $"X{Libraries.ImageColourManagement.ModernAcent}-{Libraries.ImageColourManagement.ModernColor}.*");
                    if (allImages.Length == 0)
                    {
                        string[] oreginalImage = Directory.GetFiles(imagePathe, $"{(string)ApplicationData.Current.LocalSettings.Values["ModernFile"]}.*");
                        await Libraries.ImageColourManagement.ApplyColor(oreginalImage[0], subFolder.Name);
                        string[] newAllImages = Directory.GetFiles(imagePathe, $"{Libraries.ImageColourManagement.ModernAcent}-{Libraries.ImageColourManagement.ModernColor}.*");
                        BitmapImage bitmap = new BitmapImage(new Uri(newAllImages[0]));
                        bitmap.DecodePixelHeight = 100;
                        ModernWallpaperIcon.ImageSource = bitmap;
                    }
                    else
                    {
                        BitmapImage bitmap = new BitmapImage(new Uri(allImages[0]));
                        bitmap.DecodePixelHeight = 100;
                        ModernWallpaperIcon.ImageSource = bitmap;
                    }
                    ModernWallpaperIcon.Opacity = 1;
                    DeskModernToggled.IsEnabled = true;
                    LockModernToggled.IsEnabled = true;
                }
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0107x: Loading Modern Icon"; }
            GeneralProgressBar.Visibility = Visibility.Collapsed;
        }
        private async Task LoadDynamicIcon()
        {
            GeneralProgressBar.Visibility = Visibility.Visible;
            DynamicWallpaperIcon.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/time-lapse.png"));
            DynamicWallpaperIcon.Opacity = 0.5;
            DeskDynamicToggled.IsEnabled = false;
            LockDynamicToggled.IsEnabled = false;
            try
            {
                if (ApplicationData.Current.LocalSettings.Values["DynamicTheme"] != null)
                {
                    Libraries.SunPositionDetector.DayORNightAsync();
                    StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["DynamicFolder"], CreationCollisionOption.OpenIfExists);

                    string imagePathe = $@"{Folder.Path}/{(string)ApplicationData.Current.LocalSettings.Values["DynamicTheme"]}/";
                    string[] allImages = Directory.GetFiles(imagePathe, $"X{ApplicationData.Current.LocalSettings.Values["DynamicImageID"].ToString()}.*");
                    string ImageWithExtention = Path.GetFullPath(allImages[0]);
                    if (File.Exists(ImageWithExtention))
                    {
                        BitmapImage bitmapImage = new BitmapImage(new Uri(ImageWithExtention));
                        bitmapImage.DecodePixelHeight = 400;
                        DynamicWallpaperIcon.ImageSource = bitmapImage;
                        DynamicWallpaperIcon.Opacity = 1;
                        DeskDynamicToggled.IsEnabled = true;
                        LockDynamicToggled.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0108x: Loading Dynamic Icon"; }
            GeneralProgressBar.Visibility = Visibility.Collapsed;
        }
        private async Task LoadWeatherIcon()
        {
            GeneralProgressBar.Visibility = Visibility.Visible;
            WeahterWallpaperIcon.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/windy.png"));
            WeahterWallpaperIcon.Opacity = 0.5;
            DeskWeatherToggled.IsEnabled = false;
            LockWeatherToggled.IsEnabled = false;
            try
            {
                if (ApplicationData.Current.LocalSettings.Values["WeatherTheme"] != null)
                {
                    StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["WeatherFolder"], CreationCollisionOption.OpenIfExists);
                    string imagePathe = $@"{Folder.Path}/{(string)ApplicationData.Current.LocalSettings.Values["WeatherTheme"]}/";
                    string[] allImages = Directory.GetFiles(imagePathe, $"X{ApplicationData.Current.LocalSettings.Values["WeatherImageID"].ToString()}.*");
                    string ImageWithExtention = Path.GetFullPath(allImages[0]);
                    if (File.Exists(ImageWithExtention))
                    {
                        BitmapImage bitmapImage = new BitmapImage(new Uri(ImageWithExtention));
                        bitmapImage.DecodePixelHeight = 400;
                        WeahterWallpaperIcon.ImageSource = bitmapImage;
                        WeahterWallpaperIcon.Opacity = 1;
                        DeskWeatherToggled.IsEnabled = true;
                        LockWeatherToggled.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0109x: Loading Weather Icon"; }
            GeneralProgressBar.Visibility = Visibility.Collapsed;
        }
        private async Task LookUpLibrariesDynamic()
        {
            try
            {
                DynamicWallpaperBitmap.Clear();
                StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["DynamicFolder"], CreationCollisionOption.OpenIfExists);
                var queryOption = new QueryOptions { FolderDepth = FolderDepth.Deep };
                var themeDynamicFolders = await Folder.CreateFolderQueryWithOptions(queryOption).GetFoldersAsync();
                foreach (StorageFolder themeDynamic in themeDynamicFolders)
                {
                    if (Directory.GetFiles(themeDynamic.Path, "*", SearchOption.AllDirectories).Length == 16)
                    {
                        string[] allImages = Directory.GetFiles(themeDynamic.Path, $"X{ApplicationData.Current.LocalSettings.Values["DynamicImageID"].ToString()}.*");
                        string ImageWithExtention = allImages[0];
                        BitmapImage bitmapImage = new BitmapImage(new Uri(ImageWithExtention));
                        bitmapImage.DecodePixelWidth = 200;
                        DynamicWallpaperBitmap.Add(new DynamicWallpaperBitmaps(bitmapImage, themeDynamic.Name));
                    }
                    else { await themeDynamic.DeleteAsync(); }
                }
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = "Error 0110x: Cannot Look For Dynamic Libraries";
            }
        }
        private async Task LookUpLibrariesWeather()
        {
            try
            {
                WeatherWallpaperBitmap.Clear();
                StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["WeatherFolder"], CreationCollisionOption.OpenIfExists);
                var queryOption = new QueryOptions { FolderDepth = FolderDepth.Deep };
                var themeWeatherFolders = await Folder.CreateFolderQueryWithOptions(queryOption).GetFoldersAsync();
                foreach (StorageFolder themeWeather in themeWeatherFolders)
                {
                    if (Directory.GetFiles(themeWeather.Path, "*", SearchOption.AllDirectories).Length == 18)
                    {
                        string[] allImages = Directory.GetFiles(themeWeather.Path, $"X{ApplicationData.Current.LocalSettings.Values["WeatherImageID"].ToString()}.*");
                        string ImageWithExtention = allImages[0];
                        BitmapImage bitmapImage = new BitmapImage(new Uri(ImageWithExtention));
                        bitmapImage.DecodePixelWidth = 200;
                        WeatherWallpaperBitmap.Add(new WeatherWallpaperBitmaps(bitmapImage, themeWeather.Name));
                    }
                    else { await themeWeather.DeleteAsync(); }
                }
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = "Error 0111x: Cannot Look For Weather Libraries";
            }
        }
        private async void AddHEICFileToLibrary(object sender, RoutedEventArgs e)
        {
            /*
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".heic");
            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            StorageFolder folder = null;
            if (file != null)
            {
                AddHeifButton.IsEnabled = false;
                ProgressBar.Visibility = Visibility.Visible;
                ApplicationData.Current.LocalSettings.Values["HEICfile"] = file.Path;
                this.DynamicOperation.Text = "This may take some time. If completed successfully you will see the Wallpaper in one of your Libraries.";
                try
                {
                    await System.Threading.Tasks.Task.Run(() => Libraries.HeifImageReader.Heifercollection());
                    this.DynamicOperation.Text = (string)ApplicationData.Current.LocalSettings.Values["DynamicOperation"];
                    ProgressBar.Visibility = Visibility.Collapsed;
                    AddHeifButton.IsEnabled = true;
                    await LookUpLibrariesDynamic();
                    await LookUpLibrariesWeather();
                }
                catch (Exception)
                {
                    this.DynamicOperation.Text = "Error Reading HEIC Container";
                    ProgressBar.Visibility = Visibility.Collapsed;
                    AddHeifButton.IsEnabled = true;
                    StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync((string)ApplicationData.Current.LocalSettings.Values["HEICThemePath"]);
                    await storageFolder.DeleteAsync();
                }
                
            }
            else
            {
                this.DynamicOperation.Text = "Operation cancelled.";
            }
            */
        }
        private async void AddZipFileToLibrary(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".zip");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                AddZipButton.IsEnabled = false;
                ProgressBar.Visibility = Visibility.Visible;
                DynamicOperation.Text = "This wont take long. If completed successfully you will see the Wallpaper in one of your Libraries.";
                await Task.Run(() => Libraries.HeifImageReader.Zipcollection(file));
                DynamicOperation.Text = (string)ApplicationData.Current.LocalSettings.Values["DynamicOperation"];
                ProgressBar.Visibility = Visibility.Collapsed;
                AddZipButton.IsEnabled = true;
                await LookUpLibrariesDynamic();
                await LookUpLibrariesWeather();
            }
            else
            {
                this.DynamicOperation.Text = "Operation cancelled.";
            }
        }
        private async void AddFolderToLibrary(object sender, RoutedEventArgs e)
        {
            
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                AddDirectoryButton.IsEnabled = false;
                ProgressBar.Visibility = Visibility.Visible;
                DynamicOperation.Text = "This wont take long. If completed successfully you will see the Wallpaper in one of your Libraries.";
                try
                { await Task.Run(() => Libraries.HeifImageReader.Foldercollection(folder)); }
                catch (UnauthorizedAccessException)
                {
                    MessageDialog requestPermissionDialog = new MessageDialog($"The app needs to access user's libraries. Press OK to open system settings and give this permission. If the app closes, reopen it afterwards. If you cancel the app wont work properly.");
                    var okCommand = new UICommand("OK");
                    requestPermissionDialog.Commands.Add(okCommand);
                    var cancelCommand = new UICommand("Cancel");
                    requestPermissionDialog.Commands.Add(cancelCommand);
                    requestPermissionDialog.DefaultCommandIndex = 0;
                    requestPermissionDialog.CancelCommandIndex = 1;
                    var requestPermissionResult = await requestPermissionDialog.ShowAsync();
                    if (requestPermissionResult != cancelCommand)
                    {
                        await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-broadfilesystemaccess"));
                        ApplicationData.Current.LocalSettings.Values["DynamicOperation"] = "Retry operation...";
                    }
                    else
                    {
                        ApplicationData.Current.LocalSettings.Values["DynamicOperation"] = "Cannot use this Feature...";
                    }
                }
                
                DynamicOperation.Text = (string)ApplicationData.Current.LocalSettings.Values["DynamicOperation"];
                ProgressBar.Visibility = Visibility.Collapsed;
                AddDirectoryButton.IsEnabled = true;
                await LookUpLibrariesDynamic();
                await LookUpLibrariesWeather();
            }
            else
            {
                this.DynamicOperation.Text = "Operation cancelled.";
            }
        }
        private void DynamicItemClick(object sender, ItemClickEventArgs e)
        {
            var moderntheme = (DynamicWallpaperBitmaps)e.ClickedItem;
            var clickedname = moderntheme.FolderName;
            ApplicationData.Current.LocalSettings.Values["DynamicClicked"] = clickedname;
        }
        private void WeatherItemClick(object sender, ItemClickEventArgs e)
        {
            var moderntheme = (WeatherWallpaperBitmaps)e.ClickedItem;
            var clickedname = moderntheme.FolderName;
            ApplicationData.Current.LocalSettings.Values["WeatherClicked"] = clickedname;
        }
        private async void UseSelectedDynamic(object sender, RoutedEventArgs e)
        {
            DynamicContentDialog.Hide();
            ApplicationData.Current.LocalSettings.Values["DynamicTheme"] = (string)ApplicationData.Current.LocalSettings.Values["DynamicClicked"];
            ApplicationData.Current.LocalSettings.Values["DynamicClicked"] = null;
            await LoadDynamicIcon();
            await Libraries.BackgroundSequance.RunBackgroundTaskAsync();
        }
        private async void UseWeatherSelected(object sender, RoutedEventArgs e)
        {
            WeatherContentDialog.Hide();
            ApplicationData.Current.LocalSettings.Values["WeatherTheme"] = (string)ApplicationData.Current.LocalSettings.Values["WeatherClicked"];
            ApplicationData.Current.LocalSettings.Values["WeatherClicked"] = null;
            await LoadWeatherIcon();
            await Libraries.BackgroundSequance.RunBackgroundTaskAsync();
        }
        private async void DeleteSelectedDynamic(object sender, RoutedEventArgs e)
        {
            var x = (string)ApplicationData.Current.LocalSettings.Values["DynamicClicked"];
            var y = (string)ApplicationData.Current.LocalSettings.Values["DynamicTheme"];
            if ( x == y )
            {
                ApplicationData.Current.LocalSettings.Values["DynamicTheme"] = null;
                await LoadDynamicIcon();
            }
            StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["DynamicFolder"], CreationCollisionOption.OpenIfExists);
            Directory.Delete($@"{Folder.Path}/{(string)ApplicationData.Current.LocalSettings.Values["DynamicClicked"]}",true);
            ApplicationData.Current.LocalSettings.Values["DynamicClicked"] = null;
            await LookUpLibrariesDynamic();
            AreYouSureFlyOut.Hide();
            if (DynamicWallpaperBitmap.Count != 0) { DynamicDeleteButton.IsEnabled = true; }
            else { DynamicDeleteButton.IsEnabled = false; }
        }
        private async void DeleteSelectedWeather(object sender, RoutedEventArgs e)
        {

            var x = (string)ApplicationData.Current.LocalSettings.Values["WeatherClicked"];
            var y = (string)ApplicationData.Current.LocalSettings.Values["WeatherTheme"];
            if (x == y)
            {
                ApplicationData.Current.LocalSettings.Values["WeatherTheme"] = null;
                await LoadWeatherIcon();
            }
            StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["WeatherFolder"], CreationCollisionOption.OpenIfExists);
            Directory.Delete($@"{Folder.Path}/{(string)ApplicationData.Current.LocalSettings.Values["WeatherClicked"]}", true);
            ApplicationData.Current.LocalSettings.Values["WeatherClicked"] = null;
            await LookUpLibrariesWeather();
            AreYouSureFlyOut2.Hide();
            if (WeatherWallpaperBitmap.Count != 0) { WeatherDeleteButton.IsEnabled = true; }
            else { WeatherDeleteButton.IsEnabled = false; }
        }
        private void GoToCreatePage(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CreatePage));
        }
        private async void CheckPermission(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-settings:appsfeatures-app"));
        }
        private async void CheckLocation(object sender, RoutedEventArgs e)
        {
            await Libraries.LocationManager.GetLocationTask();
            CurrentLocation.Text = (string)ApplicationData.Current.LocalSettings.Values["WeatherMessege"];
        }
        private void ChangeColomWidth(object sender, RoutedEventArgs e)
        {
            SplitViewXaml.IsPaneOpen = true;
        }
        private async void Start(object sender, RoutedEventArgs e)
        {
            FirstRunIntroDialog.Hide();
            ApplicationData.Current.LocalSettings.Values["FirstRunIntro"] = "true";
            await Libraries.LocationManager.GetLocationTask();
        }
        private async void UpdateWallpaper(object sender, RoutedEventArgs e)
        {
            await Libraries.BackgroundSequance.RunBackgroundTaskAsync();
        }
        private async void BackgroundTaskApplyClick(object sender, RoutedEventArgs e)
        {
            UNRegisterBackgroundTask();
            ApplicationData.Current.LocalSettings.Values["SliderValue"] = BackgroundSlider.Value;
            await RERegisterBackgroundTask();
        }
        private async void AddModernImage(object sender, RoutedEventArgs e)
        {
            LoadImageColors.Visibility = Visibility.Visible;
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
                var random = new Random();
                StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["ModernFolder"], CreationCollisionOption.OpenIfExists);
                StorageFolder destination = await Folder.CreateFolderAsync(String.Format("{0:X6}", random.Next(0x1000000)), CreationCollisionOption.GenerateUniqueName);
                await file.CopyAsync(destination, $"{(string)ApplicationData.Current.LocalSettings.Values["ModernFile"]}{file.FileType}", NameCollisionOption.ReplaceExisting);
                await LookingForModernThumbs();
            }
            LoadImageColors.Visibility = Visibility.Collapsed;
        }
        private async void DeleteModernWallpaper(object sender, RoutedEventArgs e)
        {
            var theme = (string)ApplicationData.Current.LocalSettings.Values["ModernTheme"];
            var clicked = (string)ApplicationData.Current.LocalSettings.Values["ModernClicked"];

            if (clicked != null)
            {
                StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["ModernFolder"], CreationCollisionOption.OpenIfExists);
                Directory.Delete($@"{Folder.Path}/{(string)ApplicationData.Current.LocalSettings.Values["ModernClicked"]}", true);
                if (clicked == theme)
                {
                    ApplicationData.Current.LocalSettings.Values["ModernTheme"] = null;
                }
                ApplicationData.Current.LocalSettings.Values["ModernClicked"] = null;

                await LookingForModernThumbs();
            }
        }
        private async void SelectTimerImageToAdd(object sender, RoutedEventArgs e)
        {
            var buttonSymbol = new SymbolIcon();
            buttonSymbol.Symbol = Symbol.Pictures;
            if (selectedTimerImage == null)
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
                    selectedTimerImage = file;
                    ImageTimerSelecter.Content = SelectedImage;
                    BitmapImage bitmapImage = await StorageFileToBitmapImage(file);
                    bitmapImage.DecodePixelHeight = 50;
                    SelectedImage.Source = bitmapImage;
                }
                else
                {
                    selectedTimerImage = null;

                    ImageTimerSelecter.Content = buttonSymbol;
                    SelectedImage.Source = null;
                }
            }
            else if (selectedTimerImage.Path.Contains((string)ApplicationData.Current.LocalSettings.Values["TimerFolder"]))
            {
                await selectedTimerImage.DeleteAsync();
                selectedTimerImage = null;
                ImageTimerSelecter.Content = buttonSymbol;
                SelectedImage.Source = null;
            }
            else
            {
                selectedTimerImage = null;
                ImageTimerSelecter.Content = buttonSymbol;
                SelectedImage.Source = null;
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
        private async void AddTimerImage(object sender, RoutedEventArgs e)
        {
            if (selectedTimerImage != null && (TimerDesk.IsChecked == true || TimerLock.IsChecked == true))
            {
                var random = new Random();
                string imageAlarmID = String.Format("{0:X6}", random.Next(0x1000000));
                string allIDs = (string)ApplicationData.Current.LocalSettings.Values["AllTimerIDs"];
                ApplicationData.Current.LocalSettings.Values["AllTimerIDs"] = allIDs + "#" + imageAlarmID;
                StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["TimerFolder"], CreationCollisionOption.OpenIfExists);
                StorageFile storage = await selectedTimerImage.CopyAsync(Folder, imageAlarmID + selectedTimerImage.FileType, NameCollisionOption.GenerateUniqueName);
                string pathToImage = $@"{storage.Path}";
                ApplicationData.Current.LocalSettings.Values[$"{imageAlarmID}Image"] = pathToImage;
                ApplicationData.Current.LocalSettings.Values[$"{imageAlarmID}Time_H"] = TimePicker.Time.Hours;
                ApplicationData.Current.LocalSettings.Values[$"{imageAlarmID}Time_M"] = TimePicker.Time.Minutes;
                imageTimerRepeat = null;
                imageTimerRepeatForCollection = null;
                if (Monday.IsChecked==true) { imageTimerRepeat = imageTimerRepeat + "#0"; imageTimerRepeatForCollection = imageTimerRepeatForCollection + " Mon"; }
                if (Tusday.IsChecked == true) { imageTimerRepeat = imageTimerRepeat + "#1"; imageTimerRepeatForCollection = imageTimerRepeatForCollection + " Tus"; }
                if (Wednesday.IsChecked == true) { imageTimerRepeat = imageTimerRepeat + "#2"; imageTimerRepeatForCollection = imageTimerRepeatForCollection + " Wed"; }
                if (Thursday.IsChecked == true) { imageTimerRepeat = imageTimerRepeat + "#3"; imageTimerRepeatForCollection = imageTimerRepeatForCollection + " Thu"; }
                if (Friday.IsChecked == true) { imageTimerRepeat = imageTimerRepeat + "#4"; imageTimerRepeatForCollection = imageTimerRepeatForCollection + " Fri"; }
                if (Saturday.IsChecked == true) { imageTimerRepeat = imageTimerRepeat + "#5"; imageTimerRepeatForCollection = imageTimerRepeatForCollection + " Sat"; }
                if (Sunday.IsChecked == true) { imageTimerRepeat = imageTimerRepeat + "#6"; imageTimerRepeatForCollection = imageTimerRepeatForCollection + " Sun"; }

                ApplicationData.Current.LocalSettings.Values[$"{imageAlarmID}Repeat"] = imageTimerRepeat;
                ApplicationData.Current.LocalSettings.Values[$"{imageAlarmID}TimeCollection"] = $"{TimePicker.Time.Hours} : {TimePicker.Time.Minutes}";
                if (TimerDesk.IsChecked == true && TimerLock.IsChecked == true)
                {
                    imageTimerRepeatForCollection = imageTimerRepeatForCollection + " for Desktop and Lockscreen";
                }
                else if (TimerDesk.IsChecked == true && TimerLock.IsChecked == false)
                {
                    imageTimerRepeatForCollection = imageTimerRepeatForCollection + " for Desktop only";
                }
                else
                {
                    imageTimerRepeatForCollection = imageTimerRepeatForCollection + " for Lockscreen only";
                }
                ApplicationData.Current.LocalSettings.Values[$"{imageAlarmID}RepeatCollection"] = imageTimerRepeatForCollection;
                if (TimerDesk.IsChecked == true) { ApplicationData.Current.LocalSettings.Values[$"{imageAlarmID}Desk"] = "true"; }
                else { ApplicationData.Current.LocalSettings.Values[$"{imageAlarmID}Desk"] = null; }
                if (TimerLock.IsChecked == true) { ApplicationData.Current.LocalSettings.Values[$"{imageAlarmID}Lock"] = "true"; }
                else { ApplicationData.Current.LocalSettings.Values[$"{imageAlarmID}Lock"] = null; }
                selectedTimerImage = null;

                var buttonSymbol = new SymbolIcon();
                buttonSymbol.Symbol = Symbol.Pictures;
                ImageTimerSelecter.Content = buttonSymbol;
                SelectedImage.Source = null;
                LookForTimerImages();
            }
        }
        private async void LookForTimerImages()
        {
            try
            {
                TimerWallpaperBitmap.Clear();
                SetBackgroundTaskForTimer = false;
                string allIDs = (string)ApplicationData.Current.LocalSettings.Values["AllTimerIDs"];
                String[] imageAlarmIDs = allIDs.Split("#", StringSplitOptions.RemoveEmptyEntries);
                if (imageAlarmIDs.Length != 0)
                {
                    foreach (String iD in imageAlarmIDs)
                    {
                        string imagePath = (string)ApplicationData.Current.LocalSettings.Values[$"{iD}Image"];
                        StorageFile file = await StorageFile.GetFileFromPathAsync(imagePath);
                        if (file != null)
                        {

                            BitmapImage bitmap = await StorageFileToBitmapImage(file);
                            bitmap.DecodePixelHeight = 50;
                            string repeatCollection = (string)ApplicationData.Current.LocalSettings.Values[$"{iD}RepeatCollection"];
                            string timerCollection = (string)ApplicationData.Current.LocalSettings.Values[$"{iD}TimeCollection"];
                            TimerWallpaperBitmap.Add(new TimerWallpaperBitmaps(bitmap, iD, timerCollection, repeatCollection));
                            if (ApplicationData.Current.LocalSettings.Values[$"{iD}Desk"] != null || ApplicationData.Current.LocalSettings.Values[$"{iD}Lock"] != null)
                            { SetBackgroundTaskForTimer = true; }
                        }
                        else
                        {
                            string newAllIDs = allIDs.Replace($"#{iD}", "");
                            ApplicationData.Current.LocalSettings.Values["AllTimerIDs"] = newAllIDs;
                        }
                    }
                }
                if (SetBackgroundTaskForTimer = true && BackgroundSlider.Value != 15)
                {
                    BackgroundSlider.Value = Convert.ToDouble(15);
                    ApplicationData.Current.LocalSettings.Values["SliderValue"] = Convert.ToDouble(15);
                    UNRegisterBackgroundTask();
                    await RERegisterBackgroundTask();
                }
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0112x: Cannot Load Timer Libraries"; }
            
        }
        private async void TimerItemClick(object sender, ItemClickEventArgs e)
        {
            var timerID = (TimerWallpaperBitmaps)e.ClickedItem;
            var clickedname = timerID.TimerID;
            string allIDs = (string)ApplicationData.Current.LocalSettings.Values["AllTimerIDs"];
            string newAllIDs = allIDs.Replace($"#{clickedname}", "");
            ApplicationData.Current.LocalSettings.Values["AllTimerIDs"] = newAllIDs;
            string repeat = (string)ApplicationData.Current.LocalSettings.Values[$"{clickedname}Repeat"];
            if (repeat != null)
            {
                if (repeat.Contains("0")) { Monday.IsChecked = true; } else { Monday.IsChecked = false; }
                if (repeat.Contains("1")) { Tusday.IsChecked = true; } else { Tusday.IsChecked = false; }
                if (repeat.Contains("2")) { Wednesday.IsChecked = true; } else { Wednesday.IsChecked = false; }
                if (repeat.Contains("3")) { Thursday.IsChecked = true; } else { Thursday.IsChecked = false; }
                if (repeat.Contains("4")) { Friday.IsChecked = true; } else { Friday.IsChecked = false; }
                if (repeat.Contains("5")) { Saturday.IsChecked = true; } else { Saturday.IsChecked = false; }
                if (repeat.Contains("6")) { Sunday.IsChecked = true; } else { Sunday.IsChecked = false; }
            }
            if (ApplicationData.Current.LocalSettings.Values[$"{clickedname}Desk"] != null) { TimerDesk.IsChecked = true; } else { TimerDesk.IsChecked = false; }
            if (ApplicationData.Current.LocalSettings.Values[$"{clickedname}Lock"] != null) { TimerLock.IsChecked = true; } else { TimerLock.IsChecked = false; }
            int hours = (int)ApplicationData.Current.LocalSettings.Values[$"{clickedname}Time_H"];
            int minuts = (int)ApplicationData.Current.LocalSettings.Values[$"{clickedname}Time_M"];
            TimeSpan timeSpan = new TimeSpan(hours, minuts, 0);
            TimePicker.Time = timeSpan;
            string imagePath = (string)ApplicationData.Current.LocalSettings.Values[$"{clickedname}Image"];
            StorageFile file = await StorageFile.GetFileFromPathAsync(imagePath);
            selectedTimerImage = file;
            BitmapImage bitmap = await StorageFileToBitmapImage(file); ;
            bitmap.DecodePixelHeight = 50;
            ImageTimerSelecter.Content = SelectedImage;
            SelectedImage.Source = bitmap;
            LookForTimerImages();
        }
        private async void AddTIFFileToLibrary(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".tif");
            picker.FileTypeFilter.Add(".tiff");
            Windows.Storage.StorageFile MyOriginalfile = await picker.PickSingleFileAsync();
            if (MyOriginalfile != null)
            {
                AddTIFButton.IsEnabled = false;
                ProgressBar.Visibility = Visibility.Visible;
                this.DynamicOperation.Text = "This won't take long. If completed successfully you will see the Wallpaper in one of your Libraries.";
                try
                {
                    uint frameCount;
                    var random = new Random();
                    StorageFolder storage = null;
                    using (IRandomAccessStream randomAccessStream = await MyOriginalfile.OpenAsync(FileAccessMode.Read, StorageOpenOptions.None))
                    {
                        Windows.Graphics.Imaging.BitmapDecoder bitmapDecoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(Windows.Graphics.Imaging.BitmapDecoder.TiffDecoderId, randomAccessStream);
                        frameCount = bitmapDecoder.FrameCount;
                        if (frameCount == 16)
                        {
                            StorageFolder mainfolder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["DynamicFolder"], CreationCollisionOption.OpenIfExists);
                            storage = await mainfolder.CreateFolderAsync(String.Format("{0:X6}", random.Next(0x1000000)), CreationCollisionOption.ReplaceExisting);
                        }
                        else if (frameCount == 18)
                        {
                            StorageFolder mainfolder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["WeatherFolder"], CreationCollisionOption.OpenIfExists);
                            storage = await mainfolder.CreateFolderAsync(String.Format("{0:X6}", random.Next(0x1000000)), CreationCollisionOption.ReplaceExisting);
                        }
                        else { ErrorTextBlock.Text = "Error 0000x: Unusable TIFF Image"; }
                        if (storage != null)
                        {
                            for (int frame = 0; frame < frameCount; frame++)
                            {
                                var bitmapFrame = await bitmapDecoder.GetFrameAsync(Convert.ToUInt32(frame));
                                var softImage = await bitmapFrame.GetSoftwareBitmapAsync();
                                var bmif = await storage.CreateFileAsync($"X{frame}.png", CreationCollisionOption.ReplaceExisting);
                                SaveSoftwareBitmapToFile(softImage, bmif);
                            }
                        }
                    }
                }
                catch (Exception ex) { ErrorTextBlock.Text = "Error 0113x: Cannot Add This TIFF Image"; }
                ProgressBar.Visibility = Visibility.Collapsed;
                AddTIFButton.IsEnabled = true;
                await LookUpLibrariesDynamic();
                await LookUpLibrariesWeather();
            }
            else
            {
                this.DynamicOperation.Text = "Operation cancelled.";
            }
        }
        private async void SaveSoftwareBitmapToFile(SoftwareBitmap softwareBitmap, StorageFile outputFile)
        {
            using (IRandomAccessStream stream = await outputFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                // Create an encoder with the desired format
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                // Set the software bitmap
                encoder.SetSoftwareBitmap(softwareBitmap);
                encoder.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Fant;
                encoder.IsThumbnailGenerated = true;
                try
                {
                    await encoder.FlushAsync();
                }
                catch (Exception err)
                {
                    const int WINCODEC_ERR_UNSUPPORTEDOPERATION = unchecked((int)0x88982F81);
                    switch (err.HResult)
                    {
                        case WINCODEC_ERR_UNSUPPORTEDOPERATION:
                            // If the encoder does not support writing a thumbnail, then try again
                            // but disable thumbnail generation.
                            encoder.IsThumbnailGenerated = false;
                            break;
                        default:
                            throw;
                    }
                }
                if (encoder.IsThumbnailGenerated == false)
                {
                    await encoder.FlushAsync();
                }
            }
        }
        private async Task ExportTheme(StorageFolder storageFolder, int num)
        {
            try
            {
                StorageFile saveTIFF = null;
                WriteableBitmap[] writeableimage = new WriteableBitmap[num];
                var savePicker = new Windows.Storage.Pickers.FileSavePicker();
                savePicker.SuggestedStartLocation =
                    Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".tif" });
                savePicker.SuggestedFileName = "Dynamic Theme";
                saveTIFF = await savePicker.PickSaveFileAsync();
                if (saveTIFF != null)
                {
                    using (IRandomAccessStream ras = await saveTIFF.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.None))
                    {
                        BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.TiffEncoderId, ras);
                        Windows.Storage.CachedFileManager.DeferUpdates(saveTIFF);
                        for (int frame = 0; frame < num; frame++)
                        {
                            string[] allImages = Directory.GetFiles(storageFolder.Path, $"X{frame}.*");
                            string ImageWithExtention = Path.GetFullPath(allImages[0]);
                            if (File.Exists(ImageWithExtention))
                            {
                                StorageFile file = await StorageFile.GetFileFromPathAsync(ImageWithExtention);
                                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                                {
                                    SoftwareBitmap softwareBitmap;
                                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                                    softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                                    writeableimage[frame] = new WriteableBitmap(softwareBitmap.PixelWidth, softwareBitmap.PixelHeight);
                                    writeableimage[frame].SetSource(stream);
                                    var addFrame = writeableimage[frame].PixelBuffer.AsStream();
                                    byte[] frameBuffer = new byte[addFrame.Length];
                                    await addFrame.ReadAsync(frameBuffer, 0, frameBuffer.Length);
                                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)writeableimage[frame].PixelWidth, (uint)writeableimage[frame].PixelHeight, 96.0, 96.0, frameBuffer);
                                    await encoder.GoToNextFrameAsync();
                                    writeableimage[frame] = null;
                                    Array.Clear(frameBuffer, 0, frameBuffer.Length);
                                }
                            }
                        }
                        await encoder.FlushAsync();
                    }
                }
            }
            catch (Exception ex) { ErrorTextBlock.Text = "Error 0115x: Cannot Save This Theme"; }
        }
        private async void ExportWeatherTheme(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.Current.LocalSettings.Values["WeatherClicked"] != null)
            {
                StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["WeatherFolder"], CreationCollisionOption.OpenIfExists);
                StorageFolder themeFolder = await Folder.GetFolderAsync((string)ApplicationData.Current.LocalSettings.Values["WeatherClicked"]);
                WeatherExportButton.IsEnabled = false;
                try { await Task.Run(() => ExportTheme(themeFolder, 18)); }
                catch (Exception exception) { }
                WeatherExportButton.IsEnabled = true;
                ApplicationData.Current.LocalSettings.Values["WeatherClicked"] = null;
            }
        }
        private async void ExportDynamicTheme(object sender, RoutedEventArgs e)
        {
            if (ApplicationData.Current.LocalSettings.Values["DynamicClicked"] != null)
            {
                StorageFolder Folder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync((string)ApplicationData.Current.LocalSettings.Values["DynamicFolder"], CreationCollisionOption.OpenIfExists);
                StorageFolder themeFolder = await Folder.GetFolderAsync((string)ApplicationData.Current.LocalSettings.Values["DynamicClicked"]);
                DynamicExportButton.IsEnabled = false;
                try { await Task.Run(() => ExportTheme(themeFolder, 16)); }
                catch (Exception exception) { }
                DynamicExportButton.IsEnabled = true;
                ApplicationData.Current.LocalSettings.Values["DynamicClicked"] = null;
            }
        }
    }
}
