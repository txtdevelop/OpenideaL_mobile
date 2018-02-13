using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Presenters;
using MvvmCross.Platform;
using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Plugin.Settings;
using PSY.Innovative.Contracts;
using PSY.Innovative.Helpers;
using PSY.Innovative.Services;
using PSY.Innovative.ViewPresenter;

#if __ANDROID__
using PSY.Innovative.Droid.Services;
namespace PSY.Innovative.Droid
#else
using PSY.Innovative.iOS.Services;
namespace PSY.Innovative.iOS
#endif
{
    public partial class Setup
    {
        static Setup()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        protected override IMvxApplication CreateApp()
        {
            return new InnovativeMvxApp();
        }
        
        protected override void InitializeIoC()
        {
            base.InitializeIoC();

            Mvx.LazyConstructAndRegisterSingleton<IMvxFormsPageLoader, ViewLoader>();

            // plugins services
            Mvx.LazyConstructAndRegisterSingleton(() => UserDialogs.Instance);
            Mvx.LazyConstructAndRegisterSingleton(() => CrossSettings.Current);
            
            // common services
            Mvx.LazyConstructAndRegisterSingleton<IPlatformSpecificCalls, PlatformSpecificCalls>();
            Mvx.LazyConstructAndRegisterSingleton<ILocalNotificationService, LocalNotificationService>();

            // shared services
            Mvx.LazyConstructAndRegisterSingleton<ILocalizationInfo, LocalizationInfo>();

            InitializePlatformSpecificIoC();
#if !DEBUG
            //Identify user for tracking
            Track.TryToIdentify();
#endif
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            bool isTerminating = e.IsTerminating;
            var exception = e.ExceptionObject as Exception;
            Logger.Error($"UnhandledException: {exception?.ToString() ?? e.ExceptionObject.ToString()}");
        }

        private static void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            bool isObserved = e.Observed;
            var exception = e.Exception;
            Logger.Exception("UnobservedTaskException", exception);
        }
    }
}