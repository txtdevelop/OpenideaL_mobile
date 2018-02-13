using System;
using MvvmCross.Core.ViewModels;

namespace PSY.Innovative.ViewPresenter
{
    public class CloseModalPresentationHint : MvxPresentationHint
    {
        public CloseModalPresentationHint(Type viewModelToCloseType)
        {
            ViewModelToCloseType = viewModelToCloseType;
        }

        public Type ViewModelToCloseType { get; private set; }
    }

    public class GoBackToHomePresentationHint : MvxPresentationHint
    {
    }
}