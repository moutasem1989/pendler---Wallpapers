using System.Threading;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml.Media.Imaging;

namespace RuntimeComponent_BackgroundTasks
{
    public sealed class ModernWallpaper : XamlRenderingBackgroundTask
    {
        private CancellationTokenSource _cts = null;
        BackgroundTaskDeferral _deferral;
        protected override async void OnRun(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            await Libraries.BackgroundSequance.RunBackgroundTaskAsync();
            _deferral.Complete();
        }
    }
}
