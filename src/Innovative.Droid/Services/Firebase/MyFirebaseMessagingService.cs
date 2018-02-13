using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V7.App;
using Firebase.Messaging;
using MvvmCross.Platform;
using PSY.Innovative.Contracts;
using PSY.Innovative.Helpers;

namespace PSY.Innovative.Droid.Services.Firebase
{
    [Service]
    [IntentFilter(new string[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        private readonly ILocalNotificationService _localNotificationService;

        public MyFirebaseMessagingService() : base()
        {
            _localNotificationService = new LocalNotificationService();
        }

        public override void OnMessageReceived(RemoteMessage remoteMessage)
        {
            //Displaying data in log
            //It is optional 
            Logger.Trace("From: " + remoteMessage.From);
            Logger.Trace("Notification Message Title: " + remoteMessage.GetNotification().Title);
            Logger.Trace("Notification Message Body: " + remoteMessage.GetNotification().Body);

            //Calling method to generate notification
            _localNotificationService.Show(remoteMessage.GetNotification().Title ?? "PSY Innovate", remoteMessage.GetNotification().Body);

            base.OnMessageReceived(remoteMessage);
        }
    }
}