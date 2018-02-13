using System.Globalization;

namespace PSY.Innovative.Contracts
{
    public interface ILocalizationInfo
    {
        CultureInfo GetCurrentCultureInfo();
    }
}