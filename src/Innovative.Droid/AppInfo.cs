using Android.Content;
using PSY.Innovative.Contracts;

namespace PSY.Innovative.Droid
{
    public class AppInfo : IAppInfo
    {
        private readonly Context _context;

        public AppInfo(Context context)
        {
            _context = context;
        }

        public string Version => _context.PackageManager.GetPackageInfo(_context.PackageName, 0).VersionName;

        public string ApplicationName => _context.PackageManager.GetApplicationLabel(_context.ApplicationInfo);
    }
}