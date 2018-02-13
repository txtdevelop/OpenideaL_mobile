using MvvmCross.Forms.Core;
using MvvmCross.Platform;
using PSY.Innovative.Helpers;
using PSY.Innovative.ViewModels;
using PSY.Innovative.ViewPresenter;
using Xamarin.Forms;

namespace PSY.Innovative
{
    public class InnovativeApp : MvxFormsApplication
    {
        public const string AppSuspendedMessage = "AppSuspendedMessage";
        public const string AppResumedMessage = "AppResumedMessage";

        protected override void OnStart()
        {
            // Handle when your app starts
#if DEBUG
            Logger.Trace("App start");
#endif
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
#if DEBUG
            Logger.Trace("App sleep");
#endif
            //Apparently android does not need this
            SuspendTopmostView();

            MessagingCenter.Send(this, AppSuspendedMessage);
            base.OnSleep();
        }

        private static void SuspendTopmostView()
        {
            var presenter = Mvx.Resolve<MasterDetailViewPresenter>();
            var topmost = presenter.TryGetTopmostViewModel();
            topmost?.Suspended();
        }

        protected override void OnResume()
        {
            base.OnResume();

            // Handle when your app resumes
#if DEBUG
            Logger.Trace("App resume");
#endif
            ResumeTopmostView();

            MessagingCenter.Send(this, AppResumedMessage);
        }

        private static void ResumeTopmostView()
        {
            var presenter = Mvx.Resolve<MasterDetailViewPresenter>();
            var topView = presenter.TryGetTopmostView();

            var topmost = topView?.BindingContext as BaseViewModel;
            topmost?.Resumed();
        }
    }
}
