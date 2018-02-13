using System;
using System.IO;
using MvvmCross.Core.ViewModels;
using PSY.Innovative.Contracts;
using PSY.Innovative.Helpers;
using PSY.Innovative.Resources;
using PSY.Innovative.Utils;
using Xamarin.Forms;

namespace PSY.Innovative.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public static int InstanceCount;
        private readonly IAppInfo _appInfo;

        public AboutViewModel(IAppInfo appInfo)
        {
            _appInfo = appInfo;

            Title = AppResources.About;

            InstanceCount++;

            Logger.Trace("{0} instance count {1}", nameof(AboutViewModel), InstanceCount);
        }

        public override void Start()
        {
            base.Start();

            try
            {
                var aboutHtml = string.Empty;
                var resourceStream = ResourceUtils.GetLocalizedResourceStream(LocalizationService, "PSY.Innovative.Resources.AboutInfo.html");
                if (resourceStream != null)
                {
                    using (var reader = new StreamReader(resourceStream))
                    {
                        aboutHtml = reader.ReadToEnd();
                    }
                }

                if (Device.RuntimePlatform == Device.Android)
                {
                    var iconFileName = "HtmlImages/icon.png";
                    AboutPage = aboutHtml.Replace("#versionnr#", Version).Replace("#icon#", iconFileName);
                }
                else
                {
                    AboutPage = aboutHtml.Replace("#versionnr#", Version).Replace("#icon#", "icon.png");
                }
            }
            catch (Exception ex)
            {
                Logger.Exception(nameof(Start), ex);
            }
        }

        public string ApplicationName
        {
            get { return _appInfo.ApplicationName; }
        }

        public string Version
        {
            get { return $"v{_appInfo.Version}"; }
        }

        public string AboutPage { get; set; }

        public MvxCommand GoToLicensesCommand
        {
            get { return new MvxCommand(() => ShowViewModel<LicencesViewModel>()); }
        }

        public MvxCommand GoToPrivacyAgreementCommand
        {
            get { return new MvxCommand(() => ShowViewModel<PrivacyAgreementViewModel>()); }
        }

        ~AboutViewModel()
        {
            InstanceCount--;

            Logger.Trace("{0} instance count {1}", nameof(AboutViewModel), InstanceCount);
        }
    }
}