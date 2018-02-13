
using Android.App;
using Firebase.Iid;
using MvvmCross.Platform;
using PSY.Innovative.Contracts;
using PSY.Innovative.Helpers;

namespace PSY.Innovative.Droid.Services.Firebase
{
    [Service]
    [IntentFilter(new string[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseInstanceIDService : FirebaseInstanceIdService
    {
        public override void OnTokenRefresh()
        {
            string refreshedToken = FirebaseInstanceId.Instance.Token;
            Logger.Trace("TOKEN: " + refreshedToken);

            IFirebaseService firebaseService = Mvx.Resolve<IFirebaseService>();
            IRestService restService = Mvx.Resolve<IRestService>();
            firebaseService.RefreshedToken = refreshedToken;
            restService.UpdateRefreshedTokenAsync();

            base.OnTokenRefresh();
        }
    }
}