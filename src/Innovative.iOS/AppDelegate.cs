using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using MvvmCross.Platform;
using UIKit;
using Xamarin.Forms;
using PSY.Innovative.Helpers;
using PSY.Innovative.ViewPresenter;
using PSY.Innovative;
using Xamarin.Forms.Platform.iOS;
using MvvmCross.Core.ViewModels;
using Firebase.Core;
using UserNotifications;
using Firebase.CloudMessaging;
using MvvmCross.Forms.iOS;
using System;
using Firebase.InstanceID;
using PSY.Innovative.Contracts;

namespace PSY.Innovative.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : MvxFormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Forms.Init();
            ImageCircleRenderer.Init();

            var setup = new Setup(this, Window);
            setup.Initialize();

            // UserDialogs is initialized automatically

            var presenter = Mvx.Resolve<MasterDetailViewPresenter>();
            if (presenter == null)
                return false;

            var mvxFormsApp = new InnovativeApp();
            presenter.FormsApplication = mvxFormsApp;

            LoadApplication(mvxFormsApp);

            Mvx.Resolve<IMvxAppStart>().Start();
            Mvx.RegisterSingleton(this);

            try
            {
                App.Configure();
                RegisterForRemoteNotifications();
            }
            catch (Exception e)
            {
                Logger.Error("App.Configure", e);
            }

            // create a new window instance based on the screen size
            //Window = new UIWindow(UIScreen.MainScreen.Bounds);

            // If you have defined a root view controller, set it here:
            // Window.RootViewController = myViewController;

            // make the window visible
            //Window.MakeKeyAndVisible();

            return base.FinishedLaunching(application, launchOptions);
        }

        private void RegisterForRemoteNotifications()
        {
            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10 or later
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
                    //Console.WriteLine(granted);
                    Logger.Trace(granted.ToString());
                });

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;

                // For iOS 10 data message (sent via FCM)
                //Messaging.SharedInstance.RemoteMessageDelegate = this;
                Messaging.SharedInstance.Delegate = this;
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();
            Messaging.SharedInstance.Subscribe("All");

            InstanceId.Notifications.ObserveTokenRefresh((sender, e) => {
                // Note that this callback will be fired everytime a new token is generated, including the first
                // time. So if you need to retrieve the token as soon as it is available this is where that
                // should be done.
                var refreshedToken = InstanceId.SharedInstance.Token;

                Logger.Trace("TOKEN: " + refreshedToken);

                IFirebaseService firebaseService = Mvx.Resolve<IFirebaseService>();
                IRestService restService = Mvx.Resolve<IRestService>();
                firebaseService.RefreshedToken = refreshedToken;
                restService.UpdateRefreshedTokenAsync();
                
            });

            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;

            //Messaging.SharedInstance.Connect(error => {
            //    if (error != null)
            //    {
            //        // Handle if something went wrong while connecting
            //    }
            //    else
            //    {
            //        // Let the user know that connection was successful
            //    }
            //});
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.

            Messaging.SharedInstance.ShouldEstablishDirectChannel = false;
            //Messaging.SharedInstance.Disconnect();
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.

            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }

        // 
        public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        {
            Logger.Trace("TOKEN void: " + fcmToken);

            IFirebaseService firebaseService = Mvx.Resolve<IFirebaseService>();
            IRestService restService = Mvx.Resolve<IRestService>();
            firebaseService.RefreshedToken = fcmToken;
            restService.UpdateRefreshedTokenAsync();

        }

        /*
         * *********************
         * * FOR THE MESSAGING *
         * *********************
         */

        // To receive notifications in foregroung on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired 'till the user taps on the notification launching the application.

            // Do your magic to handle the notification data
            System.Console.WriteLine(userInfo);
        }

        // To receive notifications in foreground on iOS 10 devices.
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Do your magic to handle the notification data
            System.Console.WriteLine(notification.Request.Content.UserInfo);
        }

        // Receive data message on iOS 10 devices.
        public void ApplicationReceivedRemoteMessage(RemoteMessage remoteMessage)
        {
            Console.WriteLine(remoteMessage.AppData);
        }
    }
}


