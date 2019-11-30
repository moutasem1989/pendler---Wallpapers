using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace Libraries
{
    public class SendToast
    {
        public static void SendToasts(string message)
        {
            var toastContent = new ToastContent()
            {
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = message
                                }
                            }
                    }
                }
            };
            var toastNotif = new ToastNotification(toastContent.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }
    }
}
