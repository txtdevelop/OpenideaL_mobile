using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using PSY.Innovative.ViewPresenter;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using PSY.Innovative.iOS.Services;
using Xamarin.Forms;

namespace PSY.Innovative.iOS.ViewPresenter
{
    class OsramIosMvxFromsViewPresenter : MasterDetailViewPresenter, IMvxIosViewPresenter
    {
        public void NativeModalViewControllerDisappearedOnItsOwn()
        {
            
        }

        public bool PresentModalViewController(UIViewController controller, bool animated)
        {
            return true;
        }

        protected override void OnPagePresented(Page page)
        {
            Mvx.Resolve<TabGestureService>().UpdateCurrentTabView(page);
        }
    }
}