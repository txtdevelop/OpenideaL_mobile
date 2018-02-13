using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using Plugin.Settings.Abstractions;
using PSY.Innovative.Contracts;
using PSY.Innovative.Resources;
using PSY.Innovative.Utils;
using PSY.Innovative.ViewModels.User;
using Xamarin.Forms;
using System.Threading.Tasks;
using System;
using System.Threading;
using PSY.Innovative.Helpers;

namespace PSY.Innovative.ViewModels
{
    public class MasterDetailViewModel : BaseViewModel
    {
        private readonly IRestService _restService;
        private readonly ISettings _settings;

        public ICommand OpenUserInfoCommand { get; }
        public ICommand OpenUserPointsCommand { get; }
        public ICommand OpenVotesCommand { get; }
        public ICommand LogoutCommand { get; }


        //keep all these references here even if not apparently used this ensures they are created
        public MasterDetailViewModel(IRestService restService, ISettings settings)
        {
            _restService = restService;
            _settings = settings;

            OpenUserInfoCommand = new Command(() => ShowViewModel<UserInfoViewModel>());
            OpenUserPointsCommand = new Command(() => ShowViewModel<UserPointsViewModel>());
            OpenVotesCommand = new Command(() => ShowViewModel<UserVotesViewModel>());
            LogoutCommand = new Command(() => Logout());

            _restService.IdUser = _settings.GetValueOrDefault(SettingsKeys.IdUser, 0);
            PeriodicFooAsync(new TimeSpan(0, 0, 30), new TimeSpan(0, 0, 3), new CancellationToken());
        }

        private void Logout()
        {
            _restService.LogoutAsync(true);
            ShowModalViewModel<LoginViewModel>();
        }

        private async Task PeriodicFooAsync(TimeSpan intervalRest, TimeSpan intervalNotRest, CancellationToken cancellationToken)
        {
            while (true)
            {
                if (_settings.GetValueOrDefault(SettingsKeys.IdUser, 0) > 0)
                {
                    try
                    {
                        await Task.WhenAll(
                            _restService.GetUserAsync(true),
                            _restService.GetIdeasAsync(true),
                            _restService.GetPointsAsync(true),
                            _restService.GetVotesAsync(true));
                    } catch(Exception e)
                    {
                        Logger.Error("Error in WhenAll", e);
                    }

                    await Task.Delay(intervalRest, cancellationToken);
                } else
                {
                    await Task.Delay(intervalNotRest, cancellationToken);
                }
                
            }
        }

        public string Version
        {
            get
            {
                var debug = string.Empty;
#if DEBUG
                debug = " debug";
#endif
                var appInfo = GetService<IAppInfo>();
                return $"{appInfo.ApplicationName} v{appInfo.Version}{debug}";
            }
        }

        public override void Loaded()
        {
            base.Loaded();

            Mvx.RegisterSingleton<MasterDetailViewModel>(this);
            ShowAsRootViewModel<HomeViewModel>();
        }

        public override void Resumed()
        {
            base.Resumed();

            var isUserLoggedIn = _settings.GetValueOrDefault(SettingsKeys.IsUserLoggedIn, false);
            var idUser = _settings.GetValueOrDefault(SettingsKeys.IdUser, 0);
            if (!isUserLoggedIn || idUser <= 0)
            {
                ShowLoginPage();
            }
        }

        private void ShowLoginPage()
        {
            ShowModalViewModel<LoginViewModel>();
        }
    }
}