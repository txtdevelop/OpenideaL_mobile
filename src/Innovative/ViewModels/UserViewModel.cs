using System.Windows.Input;
using Acr.UserDialogs;
using MvvmCross.Core.ViewModels;
using PSY.Innovative.Resources;

namespace PSY.Innovative.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;

        public UserViewModel(IUserDialogs userDialogs)
        {
            _userDialogs = userDialogs;

            OnSelectCommand = new MvxCommand(UserSelected);
        }

        public string UserName { get; set; }

        public ICommand OnSelectCommand { get; }
        private async void UserSelected()
        {
            IsBusy = true;

            //var config = new PromptConfig
            //{
            //    CancelText = AppResources.Cancel,
            //    InputType = InputType.Password,
            //    OkText = AppResources.Ok,
            //    Text = AppResources.Password,
            //    Title = AppResources.Password,
            //    Placeholder = AppResources.Password
            //};

        }
    }
}
