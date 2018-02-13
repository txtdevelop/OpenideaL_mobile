using Foundation;
using PSY.Innovative.Contracts;

namespace PSY.Innovative.iOS
{
    public class AppInfo : IAppInfo
    {

        public string Version => NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();

        public string ApplicationName => NSBundle.MainBundle.InfoDictionary["CFBundleName"].ToString();
    }
}