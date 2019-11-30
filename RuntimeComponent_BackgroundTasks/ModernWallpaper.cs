using System.Threading;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using System;
using System.Collections.Generic;

namespace RuntimeComponent_BackgroundTasks
{
    public sealed class ModernWallpaper : XamlRenderingBackgroundTask
    {
        private CancellationTokenSource _cts = null;
        BackgroundTaskDeferral _deferral;
        protected override async void OnRun(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            try
            {
                await Libraries.BackgroundSequance.RunBackgroundTaskAsync();
            }
            catch (Exception ex) { Libraries.SendToast.SendToasts("Error 1000x: Cannot Initiate Background Task! Try To Reset The App."); }
            
            string allIDs = (string)ApplicationData.Current.LocalSettings.Values["AllTimerIDs"];
            try
            {
                String[] imageAlarmIDs = allIDs.Split("#", StringSplitOptions.RemoveEmptyEntries);
                foreach (String iD in imageAlarmIDs)
                {
                    string imagePath = (string)ApplicationData.Current.LocalSettings.Values[$"{iD}Image"];
                    StorageFile file = await StorageFile.GetFileFromPathAsync(imagePath);
                    if (file != null)
                    {
                        string repeating = (string)ApplicationData.Current.LocalSettings.Values[$"{iD}Repeat"];
                        if (repeating != null && repeating != "")
                        {
                            List<DayOfWeek> list = new List<DayOfWeek>();
                            if (repeating.Contains("0")) { list.Add(DayOfWeek.Monday); }
                            if (repeating.Contains("1")) { list.Add(DayOfWeek.Tuesday); }
                            if (repeating.Contains("2")) { list.Add(DayOfWeek.Wednesday); }
                            if (repeating.Contains("3")) { list.Add(DayOfWeek.Thursday); }
                            if (repeating.Contains("4")) { list.Add(DayOfWeek.Friday); }
                            if (repeating.Contains("5")) { list.Add(DayOfWeek.Saturday); }
                            if (repeating.Contains("6")) { list.Add(DayOfWeek.Sunday); }
                            DateTime dateTime = DateTime.Now;
                            if (list.Contains(dateTime.DayOfWeek))
                            {
                                CheckIfItsTime(iD);
                            }
                        }
                        else
                        {
                            CheckIfItsTime(iD);
                        }
                    }
                    else
                    {
                        string newAllIDs = allIDs.Replace($"#{iD}", "");
                        ApplicationData.Current.LocalSettings.Values["AllTimerIDs"] = newAllIDs;
                    }
                }
            }
            catch (Exception ex) { Libraries.SendToast.SendToasts("Error 1010x: Timer Image Feature Is Not Working!"); }
            
            _deferral.Complete();
        }
        private async void CheckIfItsTime(string iD)
        {
            string imagePath = (string)ApplicationData.Current.LocalSettings.Values[$"{iD}Image"];
            int hTime = (int)ApplicationData.Current.LocalSettings.Values[$"{iD}Time_H"];
            int mTime = (int)ApplicationData.Current.LocalSettings.Values[$"{iD}Time_M"];
            string imageToDesk = (string)ApplicationData.Current.LocalSettings.Values[$"{iD}Desk"];
            string imageToLock = (string)ApplicationData.Current.LocalSettings.Values[$"{iD}Lock"];
            DateTime dateTime = DateTime.Now;
            DateTime imageDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hTime, mTime, 0);
            DateTime dateTimeMax = dateTime.AddMinutes(30);
            var firtval = DateTime.Compare(imageDateTime, dateTime);
            var secval = DateTime.Compare(imageDateTime, dateTimeMax);
            if (firtval < 0 && secval < 0)
            {
                StorageFile imageSource = await StorageFile.GetFileFromPathAsync(imagePath);
                if (imageToDesk != null)
                {
                    await Libraries.ApplyWallpaper.ApplyToDesktop(imageSource);
                }
                if (imageToLock != null)
                {
                    await Libraries.ApplyWallpaper.ApplyToLockscreen(imageSource);
                }
                string repeate = (string)ApplicationData.Current.LocalSettings.Values[$"{iD}Repeat"];
                if (repeate == null || repeate == "")
                {
                    string allIDs = (string)ApplicationData.Current.LocalSettings.Values["AllTimerIDs"];
                    string newAllIDs = allIDs.Replace($"#{iD}", "");
                    ApplicationData.Current.LocalSettings.Values["AllTimerIDs"] = newAllIDs;
                }
            }
        }
    }
}
