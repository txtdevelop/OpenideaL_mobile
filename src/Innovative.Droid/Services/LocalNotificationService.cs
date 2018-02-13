using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.App;
using PSY.Innovative.Contracts;

namespace PSY.Innovative.Droid.Services
{
    /// <summary>
    /// Notifier implementation for Android
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
            var builder = new NotificationCompat.Builder(Application.Context);
            builder.SetContentTitle(title);
            builder.SetContentText(body);
            builder.SetOngoing(true);
            builder.SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification));

            if (NotificationIconId != 0)
            {
                builder.SetSmallIcon(NotificationIconId);
            }
            else
            {
                builder.SetSmallIcon(Resource.Drawable.icon);
            }

            var resumeIntent = new Intent(Application.Context, typeof(MainActivity));
            resumeIntent.SetAction(Intent.ActionMain);
            resumeIntent.AddCategory(Intent.CategoryLauncher);

            var resultPendingIntent = PendingIntent.GetActivity(Application.Context, 0, resumeIntent, PendingIntentFlags.UpdateCurrent);
            builder.SetContentIntent(resultPendingIntent);

            var notificationManager = NotificationManagerCompat.From(Application.Context);
            var notifcation = builder.Build();
            notifcation.Flags = NotificationFlags.OngoingEvent;
            notificationManager.Notify(id, builder.Build());
        }

        /// <summary>
        /// Show a local toast notification.  Notification will also appear in the Notification Center on Windows Phone 8.1.
        /// </summary>
        /// <param name="id">Id of the scheduled notification you'd like to cancel</param>
        public void Cancel(int id)
        {
            var notificationManager = NotificationManagerCompat.From(Application.Context);
            notificationManager.Cancel(id);
        }
    }
}