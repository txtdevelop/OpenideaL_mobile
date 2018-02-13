using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Plugin.Settings.Abstractions;
using PSY.Innovative.Contracts;
using PSY.Innovative.Utils;

namespace PSY.Innovative.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IRestService _restService;
        private readonly ISettings _settings;

        public ICommand LoginClick { get; set; }

        public LoginViewModel(IRestService restService, ISettings settings) : base()
        {
            _restService = restService;
            _settings = settings;

            LoginClick = new MvxCommand(OnLoginClick);
        }

        private string username;
        public string Username
        {
            get => username;
            set
            {
                username = value;
                RaisePropertyChanged(() => Username);
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                RaisePropertyChanged(() => Password);
            }
            
        }

        private string message;
        public string Message
        {
            get => message;
            set
            {
                message = value;
                RaisePropertyChanged(() => Message);
            }
        }

        private void OnLoginClick()
        {

            Username = "admin";
            Password = "innovanext2016";

            Message = "";
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Message = "Username or Password are empty!";
            }
            else
            {
                Task.Run(async () =>
                {
                    var user = await _restService.LoginAsync(username, password, true);
                    if (user != null)
                    {
                        _settings.AddOrUpdateValue(SettingsKeys.IsUserLoggedIn, true);
                        _settings.AddOrUpdateValue(SettingsKeys.IdUser, user.Id);
                        Cancel();
                    }
                    else
                    {
                        Message = "Login Error!";
                    }
                });
            }
        }
        
    }
}
