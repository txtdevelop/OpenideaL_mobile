using MvvmCross.Core.ViewModels;
using Plugin.Settings.Abstractions;
using PSY.Innovative.Resources;
using PSY.Innovative.Utils;

namespace PSY.Innovative.ViewModels
{
    public class AgreementViewModel : BaseViewModel
    {
        private readonly ISettings _settings;

        public AgreementViewModel(  ISettings settings)
        {
            _settings = settings;
            Title = AppResources.PrivacyAgreement;
        }

        public bool IsModal
        {
            get { return ClassParameter != null && ClassParameter.IsModal; }
        }

        private MvxCommand _privacyPolicyCommand;
        public MvxCommand PrivacyPolicyCommand
        {
            get { return _privacyPolicyCommand ?? (_privacyPolicyCommand = new MvxCommand(PrivacyPolicy)); }
        }
        private void PrivacyPolicy()
        {
            ShowViewModel<PrivacyAgreementViewModel>();
        }

        private MvxCommand _termsOfUseCommand;
        public MvxCommand TermsOfUseCommand
        {
            get { return _termsOfUseCommand ?? (_termsOfUseCommand = new MvxCommand(TermsOfUse)); }
        }
        private void TermsOfUse()
        {
            ShowViewModel<TermsOfUseViewModel>();
        }

        private MvxCommand _acceptPrivacyAgreementCommand;
        public MvxCommand IAcceptCommand
        {
            get { return _acceptPrivacyAgreementCommand ?? (_acceptPrivacyAgreementCommand = new MvxCommand(AcceptAgreement)); }
        }
        private void AcceptAgreement()
        {
            _settings.AddOrUpdateValue(SettingsKeys.AgreementsAccepted, true);
            Cancel();
        }

        public class Parameter : BaseViewModelParameter
        {
            public Parameter(bool isModal) : base(isModal)
            {
            }
        }
    }
}