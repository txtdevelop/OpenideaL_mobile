using Android.Content;
using MvvmCross.Droid.Platform;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using PSY.Innovative.Contracts;
using PSY.Innovative.Droid.Services;
using PSY.Innovative.Droid.ViewPresenter;
using PSY.Innovative.ViewPresenter;
using ISystemUtils = PSY.Innovative.Droid.Contracts.ISystemUtils;

namespace PSY.Innovative.Droid
{
    public partial class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext)
            : base(applicationContext)
        {

        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            var presenter = new OsramAndroidMvxFromsViewPresenter();
            Mvx.RegisterSingleton<MasterDetailViewPresenter>(presenter);
            return presenter;
        }

        private void InitializePlatformSpecificIoC()
        {
            var androidUtils = new AndroidUtils();
            Mvx.RegisterSingleton<IAndroidUtils>(androidUtils);
            Mvx.RegisterSingleton<ISystemUtils>(androidUtils);
            Mvx.LazyConstructAndRegisterSingleton<TabGestureService, TabGestureService>();
            Mvx.LazyConstructAndRegisterSingleton<IAppInfo>(() => new AppInfo(ApplicationContext));
        }
    }
}