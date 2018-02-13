using System;
using System.IO;
using System.Text.RegularExpressions;
using MvvmCross.Core.ViewModels;
using Plugin.Settings.Abstractions;
using PSY.Innovative.Helpers;
using PSY.Innovative.Resources;
using PSY.Innovative.Utils;

namespace PSY.Innovative.ViewModels
{
    public class PrivacyAgreementViewModel : BaseViewModel
    {
        private readonly ISettings _settings;
        private string _privacyAgreement;

        public PrivacyAgreementViewModel(ISettings settings)
        {
            _settings = settings;
            Title = AppResources.PrivacyAgreement;
        }

        public override void Start()
        {
            base.Start();

            try
            {
                var resource = "PSY.Innovative.Resources.PrivacyAgreement.html";
                var stream = ResourceUtils.GetLocalizedResourceStream(LocalizationService, resource);

                var privacyAgreementText = "";
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        privacyAgreementText = reader.ReadToEnd();
                    }
                }
                _privacyAgreement = "<div>" + Regex.Replace(privacyAgreementText, @"\r\n?|\n", "<br />") + "</div>";
            }
            catch (Exception ex)
            {
                Logger.Exception(nameof(Start), ex);
            }
        }

        public new Parameter ClassParameter => (Parameter)base.ClassParameter;

        public string PrivacyAgreement
        {
            get { return _privacyAgreement; }
        }

        public bool IsModal
        {
            get { return ClassParameter != null && ClassParameter.IsModal; }
        }

        private MvxCommand _iAcceptCommand;
        public MvxCommand IAcceptCommand
        {
            get
            {
                _iAcceptCommand = _iAcceptCommand ?? new MvxCommand(IAccept);
                return _iAcceptCommand;
            }
        }

        private void IAccept()
        {
            _settings.AddOrUpdateValue(SettingsKeys.PrivacyAgreementAccepted, true);
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