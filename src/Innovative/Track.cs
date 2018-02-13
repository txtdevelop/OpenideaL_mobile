using System;
using PSY.Innovative.Helpers;

//using Severity = Xamarin.Insights.Severity;

namespace PSY.Innovative
{
    public static class Track
    {
        public const string XamarinInsightsAppKey = "b1213580f246e353cda0855a2bb95840115288f5";

        private static void InsightsTrack(this Exception ex, string key, string value, string severity = "Warning")
        {
            //if (Insights.IsInitialized)
            //{
            //    Insights.Report(ex, key, value, severity);
            //}
            Logger.Exception(nameof(key), ex);
        }

        public static void TrackWarning(this Exception ex, string key, string value)
        {
            ex.InsightsTrack(key, value);
        }

        public static void TrackError(this Exception ex, string key, string value)
        {
            ex.InsightsTrack(key, value, "Error");
        }

        public static void TrackCritical(this Exception ex, string key, string value)
        {
            ex.InsightsTrack(key, value, "Critical");
        }

        public static bool TryToIdentify()
        {
            try
            {
                //var userService = Mvx.Resolve<IUserService>();
                //var user = userService.GetUser();

                //if (string.IsNullOrEmpty(user?.Name) || string.IsNullOrEmpty(user.Company))
                //{
                //    throw new Exception("Failed to retrieve VALID user from settings");
                //}

#if !DEBUG
                //Insights.Identify(user.Name, new Dictionary<string, string>
                //{
                //    { Insights.Traits.Name, user.Name },
                //    { Insights.Traits.LastName, user.Company },
                //});
#endif

                //Logger.Trace("Identified user as {0} {1}", user.Name, user.Company);
                return true;
            }
            catch (Exception ex)
            {
                //if (Insights.IsInitialized)
                //{
                //    Insights.Identify(Insights.Traits.GuestIdentifier);
                //    Logger.Exception("Identified user guest. {0}", ex);
                //}
                Logger.Exception("Failed to identify user. {0}", ex);
                return false;
            }
        }
    }
}