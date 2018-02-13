using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using PSY.Innovative.Contracts;

namespace PSY.Innovative.iOS.Services
{
    /// <summary>
    /// Notifier implementation for Ios
    /// </summary>
    public class LocalNotificationService : ILocalNotificationService
    {
        /// <summary>
        /// Get or Set Resource Icon to display
        /// </summary>
        public static int NotificationIconId { get; set; }

        /// <summary>
        /// Show a local notification in the Notification Area and Drawer.
        /// </summary>
        /// <param name="title">Title of the notification</param>
        /// <param name="body">Body or description of the notification</param>
        /// <param name="id">Id of notifications</param>
        public void Show(string title, string body, int id = 0)
        {
            // create the notification
            var notification = new UILocalNotification();

            // set the fire date (the date time in which it will fire)
            notification.FireDate = NSDate.FromTimeIntervalSinceNow(1);

            // configure the alert
            notification.AlertAction = "View Alert";
            notification.AlertTitle = title;
            notification.AlertBody = body;

            // modify the badge
            notification.ApplicationIconBadgeNumber = 1;

            // set the sound to be the default sound
            notification.SoundName = UILocalNotification.DefaultSoundName;

            // schedule it
            UIApplication.SharedApplication.ScheduleLocalNotification(notification);
        }

        /// <summary>
        /// Show a local toast notification.  Notification will also appear in the Notification Center on Windows Phone 8.1.
        /// </summary>
        /// <param name="id">Id of the scheduled notification you'd like to cancel</param>
        public void Cancel(int id)
        {
            //var notificationManager = NotificationManagerCompat.From(Application.Context);
            //notificationManager.Cancel(id);
        }
    }
}