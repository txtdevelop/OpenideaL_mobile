using System.Globalization;

namespace PSY.Innovative.Models
{
    public class Language
    {
        public string Name
        {
            get { return CultureInfo.NativeName; }
        }

        public CultureInfo CultureInfo { get; set; }
    }
}