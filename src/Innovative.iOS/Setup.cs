using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using MvvmCross.iOS.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using PSY.Innovative.ViewPresenter;
using PSY.Innovative.iOS.ViewPresenter;
using PSY.Innovative.Contracts;
using PSY.Innovative.iOS.Services;

namespace PSY.Innovative.iOS
{
    public partial class Setup : MvxIosSetup
    {
        public Setup(IMvxApplicationDelegate applicationDelegate, UIWindow window) : base(applicationDelegate, window)
        {

        }
        public Setup(IMvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter) : base(applicationDelegate, presenter)
        {

        }

        protected override IMvxIosViewPresenter CreatePresenter()
        {
            var presenter = new OsramIosMvxFromsViewPresenter();
            Mvx.RegisterSingleton<MasterDetailViewPresenter>(presenter);
            return presenter;
        }

        private void InitializePlatformSpecificIoC()
        {
            Mvx.LazyConstructAndRegisterSingleton<TabGestureService, TabGestureService>();
            Mvx.LazyConstructAndRegisterSingleton<IAppInfo, AppInfo>();
        }
    }
}