using System.Windows.Input;
using Acr.UserDialogs;
using PSY.Innovative.Resources;
using Xamarin.Forms;

namespace PSY.Innovative.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IUserDialogs _userDialogs;

        private string _languageName;
        private string _flag;

        public SettingsViewModel(IUserDialogs userDialogs)
        {
            Title = AppResources.Settings;
            _userDialogs = userDialogs;

            SetCurrentLaunguage();

            OpenLanguageViewCommand = new Command(() => ShowViewModel<LanguageViewModel>());
            OpenAboutCommand = new Command(() => ShowViewModel<AboutViewModel>());
        }

        public ICommand OpenLanguageViewCommand { get; }
        public ICommand OpenAboutCommand { get; }

        public string LanguageName
        {
            get { return _languageName; }
            set
            {
                _languageName = value;
                RaisePropertyChanged(nameof(LanguageName));
            }
        }

        public string Flag
        {
            get { return _flag; }
            set
            {
                _flag = value;
                RaisePropertyChanged(nameof(Flag));
            }
        }

        public override bool IsBusy
        {
            get { return base.IsBusy; }
            set
            {
                base.IsBusy = value;
                if (IsBusy)
                    _userDialogs.ShowLoading();
                else
                    _userDialogs.HideLoading();
            }
        }

        public override void Resumed()
        {
            base.Resumed();
            SetCurrentLaunguage();
        }

        private void SetCurrentLaunguage()
        {
            LanguageName = LocalizationService.CurrentLanguage.Name.ToUpper();
            Flag = LocalizationService.GetLocalizedFileName("flag.png");
        }
    }
}