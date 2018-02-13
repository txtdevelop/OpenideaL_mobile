using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Presenters;

namespace PSY.Innovative.ViewPresenter
{
    public class ViewLoader : MvxFormsPageLoader
    {
        protected override string GetPageName(MvxViewModelRequest request)
        {
            return base.GetPageName(request).Replace("Page", "View");
        }
    }
}