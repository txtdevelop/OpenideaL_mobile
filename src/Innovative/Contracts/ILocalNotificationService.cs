namespace PSY.Innovative.Contracts
{
    /// <summary>
    /// Local Notification Interface
    /// </summary>
    public interface ILocalNotificationService
    {
        /// <summary>
        /// Show a local notification
        /// </summary>
        /// <param name="title">Title of the notification</param>
        /// <param name="body">Body or description of the notification</param>
        /// <param name="id">Id of the notification</param>
        void Show(string title, string body, int id = 0);

        /// <summary>
        /// Cancel a local notification
        /// </summary>
        /// <param name="id">Id of the scheduled notification you'd like to cancel</param>
        void Cancel(int id);
    }
}
