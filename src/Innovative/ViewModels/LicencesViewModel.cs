using System;
using System.IO;
using System.Text.RegularExpressions;
using PSY.Innovative.Helpers;
using PSY.Innovative.Resources;
using PSY.Innovative.Utils;

namespace PSY.Innovative.ViewModels
{
    public class LicencesViewModel : BaseViewModel
    {
        private string _thirdPartyLicences;

        public LicencesViewModel()
        {           
            Title = AppResources.Licenses;
        }

        public override void Start()
        {
            base.Start();

                try
                {
                    var licencesTxt = "";
                    var stream = ResourceUtils.GetLocalizedResourceStream(LocalizationService, "PSY.Innovative.Resources.LicenseInfo.txt");
                    if (stream != null)  
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            licencesTxt = reader.ReadToEnd();
                        }
                    }
                    _thirdPartyLicences = "<div>" + Regex.Replace(licencesTxt, @"\r\n?|\n", "<br />") + "</div>";
                }
                catch (Exception ex)
                {
                    Logger.Exception(nameof(Start), ex);
                }
        }

        public string ThirdPartyLicences
        {
            get { return _thirdPartyLicences; }
        }
    }
}