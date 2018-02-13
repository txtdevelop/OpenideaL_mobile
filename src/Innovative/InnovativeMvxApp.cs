using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using PSY.Innovative.ViewModels;
using PSY.Innovative.ViewPresenter;

namespace PSY.Innovative
{
    public class InnovativeMvxApp : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.LazyConstructAndRegisterSingleton<IMvxFormsPageLoader, ViewLoader>();
            RegisterNavigationServiceAppStart<MasterDetailViewModel>();
        }
    }
}