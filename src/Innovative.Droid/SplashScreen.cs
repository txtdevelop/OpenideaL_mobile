using Android.App;
using Android.Content;
using Android.Views;
using MvvmCross.Droid.Views;

namespace PSY.Innovative.Droid
{
    [Activity(MainLauncher = true, Icon = "@drawable/icon", NoHistory = true)]
    [IntentFilter(new[] { Intent.ActionView },
        Categories = new[] { Intent.CategorySampleCode, "my.custom.category" })]
    public class SplashScreen : MvxSplashScreenActivity
    {
        private bool _isInitializationComplete;

        public SplashScreen()
            : base(Resource.Layout.SplashScreen)
        {
        }

        protected override void RequestWindowFeatures()
        {
            base.RequestWindowFeatures();

            // Hide the status bar.
            Window.DecorView.SystemUiVisibility = StatusBarVisibility.Hidden;
        }

        public override void InitializationComplete()
        {
            if (_isInitializationComplete)
                return;

            _isInitializationComplete = true;

            var intent = new Intent(Intent);
            intent.SetClass(ApplicationContext, typeof(MainActivity));
            StartActivity(intent);
        }
    }
}