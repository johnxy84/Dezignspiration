using System;
using DezignSpiration.Interfaces;
using DezignSpiration.Models;
using Xamarin.Forms;

namespace DezignSpiration.Helpers
{
    public static class NotificationUtils
    {

        public static bool ShouldCreateNotification(NotificationType notificationType)
        {

            switch (notificationType)
            {
                case NotificationType.DailyAlarm:
                    return !Settings.IsDailyNotificationSet && Settings.SettingsConfig.IsDailyQuoteEnabled;
                default:
                    // Assume Random notification by default
                    return !Settings.IsRandomNotificationSet && Settings.SettingsConfig.IsRandomQuoteEnabled;
            }
        }

        public static void UpdateScheduledNotification(NotificationType notificationType, bool updateValue = true)
        {
            switch (notificationType)
            {
                case NotificationType.DailyAlarm:
                    Settings.IsDailyNotificationSet = updateValue;
                    break;
                case NotificationType.RandomAlarm:
                    Settings.IsRandomNotificationSet = updateValue;
                    break;
            }
        }

        public static ScheduledNotification GetScheduledNotification(NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.DailyAlarm:
                    return new ScheduledNotification
                    {
                        NotificationType = notificationType,
                        TimeSpan = GetTimeToScheduleNotification(Settings.SettingsConfig.DailyReminderTime)
                    };
                default:
                    // Assume it's a Random Notificataion by default
                    var randomTimeSpan = new TimeSpan(App.Random.Next(6, 9), App.Random.Next(0, 59), 0);
                    return new ScheduledNotification
                    {
                        NotificationType = notificationType,
                        TimeSpan = randomTimeSpan
                    };
            }
        }

        public static void ClearNotifications()
        {
            var Helper = DependencyService.Get<IHelper>();

            UnSetNotifications();

            // Cancel all scheduled notifications
            Helper?.CancelScheduledNotification(Settings.DailyNotificationData);
            Helper?.CancelScheduledNotification(Settings.RandomNotificationData);
        }

        static TimeSpan GetTimeToScheduleNotification(TimeSpan scheduledTime)
        {
            // Convert both time to seconds
            double currentTimeInSeconds = DateTime.Now.TimeOfDay.TotalSeconds;
            double scheduledTimeInSeconds = scheduledTime.TotalSeconds;
            // Total seconds in a day
            double maxTimeInSeconds = 86400;

            double requiredTime = currentTimeInSeconds > scheduledTimeInSeconds
                ? (maxTimeInSeconds - currentTimeInSeconds) + scheduledTimeInSeconds
                : scheduledTimeInSeconds - currentTimeInSeconds;

            return TimeSpan.FromSeconds(requiredTime);
        }

        public static void UnSetNotifications()
        {
            // Unset notifications already set
            Settings.IsRandomNotificationSet = Settings.IsDailyNotificationSet = false;
            Settings.DailyNotificationData = Settings.RandomNotificationData = null;
        }
    }
}
