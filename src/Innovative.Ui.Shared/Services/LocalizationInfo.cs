using System.Globalization;
using PSY.Innovative.Contracts;
#if __IOS__
namespace PSY.Innovative.iOS.Services
#else
namespace PSY.Innovative.Droid.Services
#endif
{
    public class LocalizationInfo : ILocalizationInfo
    {
        public CultureInfo GetCurrentCultureInfo()
        {
#if __IOS__
            return CultureInfo.CurrentCulture;
#else
            var androidLocale = Java.Util.Locale.Default;
            var netLanguage = androidLocale.ToString().Replace("_", "-");
            try
            {
                return new CultureInfo(netLanguage);
            }
            catch (System.Exception)
            {
                return new CultureInfo("en");
            }
#endif
        }
    }
}