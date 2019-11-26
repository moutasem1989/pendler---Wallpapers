using Windows.UI.Xaml.Media.Imaging;

namespace pendler
{
    public class TimerWallpaperBitmaps
    {
        public BitmapImage TimerWallpaperImageIcon { get; set; }
        public string TimerID { get; set; }
        public string TimerSetTime { get; set; }
        public string TimerRepeat { get; set; }
        public TimerWallpaperBitmaps(BitmapImage imgIconSourceModern, string folderName, string timerSetTime, string timerRepeat)
        {
            this.TimerWallpaperImageIcon = imgIconSourceModern;
            this.TimerID = folderName;
            this.TimerSetTime = timerSetTime;
            this.TimerRepeat = timerRepeat;
        }
    }
}
