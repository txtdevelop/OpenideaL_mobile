using System;
using System.IO;
using MvvmCross.Core.ViewModels;
using Plugin.Settings.Abstractions;
using PSY.Innovative.Contracts;
using PSY.Innovative.Helpers;
using PSY.Innovative.Resources;
using PSY.Innovative.Utils;

namespace PSY.Innovative.ViewModels
{
    public class TermsOfUseViewModel : BaseViewModel
    {
        private readonly IAppInfo _appInfo;
        private readonly ISettings _settings;
        private string _termsOfUse;

        public TermsOfUseViewModel( IAppInfo appInfo, ISettings settings)
        {
            _appInfo = appInfo;
            _settings = settings;
            Title = AppResources.TermsOfUse;
        }

        public override void Start()
        {
            base.Start();

            try
            {
                var resourceName = "PSY.Innovative.Resources.TermsOfUse.html";

                var termsOfUseText = string.Empty;
                var resourceStream = ResourceUtils.GetLocalizedResourceStream(LocalizationService, resourceName);
                if (resourceStream != null)
                {
                    using (var reader = new StreamReader(resourceStream))
                    {
                        termsOfUseText = reader.ReadToEnd();
                    }
                }

                _termsOfUse = termsOfUseText.Replace("#versionnr#", $"v{_appInfo.Version}");
            }
            catch (Exception ex)
            {
                Logger.Exception(nameof(Start), ex);
            }
        }

        public string TermsOfUse
        {
            get { return _termsOfUse; }
        }

        public bool IsModal
        {
            get { return ClassParameter != null && ClassParameter.IsModal; }
        }

        private MvxCommand _iReadCommand;
        public MvxCommand IReadCommand
        {
            get { return _iReadCommand ?? (_iReadCommand = new MvxCommand(IRead)); }
        }

        private void IRead()
        {
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