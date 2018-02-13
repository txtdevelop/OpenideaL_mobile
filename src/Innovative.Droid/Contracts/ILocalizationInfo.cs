using System.Globalization;

namespace PSY.Innovative.Droid.Contracts
{
    public interface ILocalizationInfo
    {
        CultureInfo GetCurrentCultureInfo();
    }
}