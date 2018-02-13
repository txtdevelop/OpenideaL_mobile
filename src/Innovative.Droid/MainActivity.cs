using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Firebase;
using Firebase.Crash;
using Firebase.Messaging;
using ImageCircle.Forms.Plugin.Droid;
using Java.Lang;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Plugin.Permissions;
using PSY.Innovative.Helpers;
using PSY.Innovative.ViewPresenter;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace PSY.Innovative.Droid
{
    [Activity(Icon = "@drawable/icon",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait,
        LaunchMode = LaunchMode.SingleTask
    )]
    [MetaData("com.google.firebase.messaging.default_notification_icon", Resource = "@drawable/icon")]
    //[MetaData("com.google.firebase.messaging.default_notification_color", Resource = "@color/ColorAccent")]
    public class MainActivity : FormsAppCompatActivity, IMvxAndroidView, IMvxAndroidCurrentTopActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            // Must be called before base.OnCreate (bundle);
            ToolbarResource = Resource.Layout.Toolbar;
            TabLayoutResource = Resource.Layout.TabLayout;

            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            ImageCircleRenderer.Init();

            // Leverage controls' StyleId attrib. to Xamarin.UITest
            Forms.ViewInitialized += (sender, e) =>
            {
                var viewStyleId = e.View.StyleId;
                if (!string.IsNullOrWhiteSpace(viewStyleId))
                {
                    e.NativeView.ContentDescription = viewStyleId;
                }
            };

            UserDialogs.Init(() => (Activity)Forms.Context);

            var presenter = Mvx.Resolve<MasterDetailViewPresenter>();
            if (presenter == null)
                return;

            var mvxFormsApp = new InnovativeApp();
            presenter.FormsApplication = mvxFormsApp;

            LoadApplication(mvxFormsApp);

            Mvx.Resolve<IMvxAppStart>().Start();
            Mvx.RegisterSingleton(this);
            Mvx.RegisterSingleton<IMvxAndroidCurrentTopActivity>(this);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            FirebaseApp.InitializeApp(this);
            IsPlayServicesAvailable();
            try
            {
                FirebaseMessaging.Instance.SubscribeToTopic("All");
            }
            catch (Exception e)
            {
                Logger.Error("Error", e);
            }
            //FirebaseCrash.Report(new Exception("PROVA"));

            // Handle possible data accompanying notification message.
            // [START handle_data_extras]
            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    var value = Intent.Extras.GetString(key);
                    Logger.Trace($"Key: {key} Value: {value}");
                }
            }
            // [END handle_data_extras]
        }

        private bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Logger.Error(GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Logger.Error("This device is not supported");
                    Finish();
                }
                return false;
            }
            else
            {
                Logger.Trace("Google Play Services is available.");
                return true;
            }
        }

        //protected override void OnNewIntent(Intent intent)
        //{
        //    base.OnNewIntent(intent);
        //    StartupIntent.SetIntent(intent);
        //}

        public object DataContext { get; set; }
        public IMvxViewModel ViewModel { get; set; }
        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            StartActivityForResult(intent, requestCode);
        }

        public IMvxBindingContext BindingContext { get; set; }
        public Activity Activity => this;

        protected override void OnPause()
        {
            //this is needed on android because of the ordering in which the app events are called (the forms app pause and the ones in the view).
            // used for view models to suspend vs cleanup
            Innovative.AppState = AppState.Suspended;
            base.OnPause();
        }

        protected override void OnResume()
        {
            Innovative.AppState = AppState.Resumed;
            IsPlayServicesAvailable();
            base.OnResume();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

