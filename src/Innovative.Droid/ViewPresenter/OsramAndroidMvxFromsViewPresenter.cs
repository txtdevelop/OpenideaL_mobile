using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using PSY.Innovative.Droid.Services;
using PSY.Innovative.ViewPresenter;
using Xamarin.Forms;

namespace PSY.Innovative.Droid.ViewPresenter
{
    public class OsramAndroidMvxFromsViewPresenter : MasterDetailViewPresenter, IMvxAndroidViewPresenter
    {
        protected override void OnPagePresented(Page page)
        {
            Mvx.Resolve<TabGestureService>().UpdateCurrentTabView(page);

            //if (page is AvalableDevicesListView || page is SearchingDeviceView)
            //{
            //    Mvx.Resolve<MainActivity>().PromptBleStart();
            //}
        }
    }
}