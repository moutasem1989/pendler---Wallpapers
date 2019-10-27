using Innovative.SolarCalculator;
using System;
using Windows.Storage;

namespace Libraries
{
    public class SunPositionDetector
    {
        public static DateTime SunSetYesterday { get; set; }
        public static DateTime SunRiseToday { get; set; }
        public static DateTime SunSetToday { get; set; }
        public static DateTime SunRiseTomorrow { get; set; }
        public static int imageID { get; set; }
        public static void DayORNightAsync()
        {
            GetSolarDataForTodayAsync();
            int resultDay1 = DateTime.Compare(SunRiseToday, DateTime.Now);
            int resultDay2 = DateTime.Compare(DateTime.Now, SunSetToday);
            if (resultDay1 < 0)
            {
                if (resultDay2 < 0)
                {
                    ApplicationData.Current.LocalSettings.Values["DayOrNight"] = "Day";
                    WriteImageID(SunRiseToday, SunSetToday, 11);
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["DayOrNight"] = "Night";
                    GetSolarDataForTomorrow();
                    WriteImageID(SunSetToday, SunRiseTomorrow, 5);
                }
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values["DayOrNight"] = "Night";
                GetSolarDataForYesterday();
                WriteImageID(SunSetYesterday, SunRiseToday, 5);
            }
        }
        public static void GetSolarDataForTodayAsync()
        {
            DateTime today = DateTime.Today;
            double latitude = Double.Parse((string)ApplicationData.Current.LocalSettings.Values["Latitude"]);
            double longitude = Double.Parse((string)ApplicationData.Current.LocalSettings.Values["Longitude"]);
            SolarTimes solarTimes = new SolarTimes(today, latitude, longitude);
            SunRiseToday = solarTimes.Sunrise;
            SunSetToday = solarTimes.Sunset;
        }
        public static void GetSolarDataForYesterday()
        {
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            double latitude = Double.Parse((string)ApplicationData.Current.LocalSettings.Values["Latitude"]);
            double longitude = Double.Parse((string)ApplicationData.Current.LocalSettings.Values["Longitude"]);
            SolarTimes solarTimes = new SolarTimes(yesterday, latitude, longitude);
            SunSetYesterday = solarTimes.Sunset;
        }
        public static void GetSolarDataForTomorrow()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            double latitude = Double.Parse((string)ApplicationData.Current.LocalSettings.Values["Latitude"]);
            double longitude = Double.Parse((string)ApplicationData.Current.LocalSettings.Values["Longitude"]);
            SolarTimes solarTimes = new SolarTimes(tomorrow, latitude, longitude);
            SunRiseTomorrow = solarTimes.Sunrise;
        }
        public static void WriteImageID(DateTime TimeA, DateTime TimeB, int ImagesCount)
        {
            TimeSpan howLongDay = TimeB.Subtract(TimeA);
            TimeSpan timerLingth = new TimeSpan(howLongDay.Ticks / ImagesCount);
            TimeSpan dayTimeSpan = DateTime.Now.Subtract(TimeA);
            int imageNumber = (int)(dayTimeSpan.Ticks / timerLingth.Ticks) + 1;
            int imageID = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(imageNumber)));
            if (imageID == 0)
            {
                imageID = 1;
            }
            else if (ImagesCount < imageID)
            {
                imageID = ImagesCount;
            }
            SetImageIDToSettings(imageID);
        }
        public static void SetImageIDToSettings(int imageID)
        {
            string dayNight = (string)ApplicationData.Current.LocalSettings.Values["DayOrNight"];
            if (imageID == 1 && dayNight == "Day") { ApplicationData.Current.LocalSettings.Values["DynamicImageID"] = 1; }
            else if (imageID == 2 && dayNight == "Day") { ApplicationData.Current.LocalSettings.Values["DynamicImageID"] = 2; }
            else if (imageID == 3 && dayNight == "Day") { ApplicationData.Current.LocalSettings.Values["DynamicImageID"] = 3; }
            else if (imageID == 4 && dayNight == "Day") { ApplicationData.Current.LocalSettings.Values["DynamicImageID"] = 4; }
            else if (imageID == 5 && dayNight == "Day") { ApplicationData.Current.LocalSettings.Values["DynamicImageID"] = 5; }
            else if (imageID == 6 && dayNight == "Day") { ApplicationData.Current.LocalSettings.Values["DynamicImageID"] = 6; }
            else if (imageID == 7 && dayNight == "Day") { ApplicationData.Current.LocalSettings.Values["DynamicImageID"] = 7; }
            else if (imageID == 8 && dayNight == "Day") { ApplicationData.Current.LocalSettings.Values["DynamicImageID"] = 8; }
            else if (imageID == 9 && dayNight == "Day") { ApplicationData.Current.LocalSettings.Values["DynamicImageID"] = 9; }
            else if (imageID == 10 && dayNight == "Day") { ApplicationData.Current.LocalSettings.Values["DynamicImageID"] = 10; }
            else if (imageID == 11 && dayNight == "Day") { ApplicationData.Current.LocalSettings.Values["DynamicImageID"] = 0; }
            else if (imageID == 1 && dayNight == "Night") { ApplicationData.Current.LocalSettings.Values["DynamicImageID"] = 11; }
            else if (imageID == 2 && dayNight == "Night") { ApplicationData.Current.LocalSettings.Values["DynamicImageID"] = 12; }
            else if (imageID == 3 && dayNight == "Night") { ApplicationData.Current.LocalSettings.Values["DynamicImageID"] = 13; }
            else if (imageID == 4 && dayNight == "Night") { ApplicationData.Current.LocalSettings.Values["DynamicImageID"] = 14; }
            else if (imageID == 5 && dayNight == "Night") { ApplicationData.Current.LocalSettings.Values["DynamicImageID"] = 15; }
        }


    }
}
