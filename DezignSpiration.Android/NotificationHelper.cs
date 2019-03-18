using System;
using Android.App;
using Android.Content;
using Android.OS;
using DezignSpiration.Models;
using DezignSpiration.Helpers;
using Android.Support.V4.App;
using Android.Media;
using Newtonsoft.Json;

namespace DezignSpiration.Droid
{
    public static class NotificationHelper
    {
        public static void CreateNotificationChannel(Context context)
        {
            // Create the NotificationChannel, but only on API 26+ because
            // the NotificationChannel class is new and not in the support library
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                string channelName = Constants.NOTIFICATION_CHANNEL_NAME;
                string channelId = Constants.NOTIFICTAIONTYPE_CHANNEL_ID;

                var channel = new NotificationChannel(channelId, channelName, NotificationImportance.Default)
                {
                    Description = Constants.NOTIFICATION_CHANNEL_DESCRIPTION
                };
                // Register the channel with the system; you can't change the importance
                // or other notification behaviors after this
                NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
                notificationManager?.CreateNotificationChannel(channel);
            }
        }

        public static void SendScheduledNotification(Context context, NotificationType notificationType, DesignQuote designQuote)
        {
            var notificationIntent = new Intent(context, typeof(MainActivity));
            notificationIntent.SetFlags(ActivityFlags.SingleTop);

            PendingIntent tappedPendingIntent = PendingIntent.GetActivity(context, Constants.TOUCH_NOTIFICATON_REQUEST_CODE, notificationIntent, PendingIntentFlags.UpdateCurrent);

            // Set action for share button
            Intent shareIntent = new Intent(context, typeof(AlarmReceiver));
            shareIntent.SetAction(Constants.SHARE_NOTIFICATION_QUOTE_INTENT_ACTION);
            shareIntent.PutExtra(Constants.NOTIFICTAIONTYPE_KEY, notificationType.ToString());
            // Add This notification to the intent just in case the user needs to share it
            shareIntent.PutExtra(Constants.SHARE_NOTIFICATION_QUOTE_INTENT_ACTION, JsonConvert.SerializeObject(designQuote));
            PendingIntent shareNotificationPendingIntent = PendingIntent.GetBroadcast(context, Constants.SHARE_NOTIFICATION_REQUEST_CODE, shareIntent, PendingIntentFlags.CancelCurrent);

            var notificationBuilder = new NotificationCompat.Builder(context, Constants.NOTIFICTAIONTYPE_CHANNEL_ID)
                        .SetContentTitle(designQuote.Author)
                        .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                        .SetSmallIcon(Resource.Drawable.notify_icon)
                        .SetContentIntent(tappedPendingIntent)
                        .SetPriority(NotificationCompat.PriorityDefault)
                        .SetStyle(new NotificationCompat.BigTextStyle().BigText(designQuote.Quote))
                        .AddAction(Resource.Drawable.share, "Share Quote", shareNotificationPendingIntent)
                        .SetContentText(designQuote.Quote)
                        .SetAutoCancel(true);

            if (notificationType == NotificationType.DailyAlarm)
            {
                notificationBuilder.SetSubText("Daily Quote");
            }

            // Show the notification.
            var notificationManager = NotificationManagerCompat.From(Application.Context);
            notificationManager.Notify(Settings.NotificationCount++, notificationBuilder.Build());
        }

        public static void ScheduleNotification(Context context, ScheduledNotification scheduledNotification)
        {
            try
            {
                var pendingIntent = GetPendingNotificationIntent(scheduledNotification, context);

                AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
                long elapsedTime = (long)(Java.Lang.JavaSystem.CurrentTimeMillis() + scheduledNotification.TimeSpan.TotalMilliseconds);

                // Set the Alarm
                alarmManager.Set(AlarmType.RtcWakeup, elapsedTime, pendingIntent);

                // Save Alarm information if we need to cancel it later
                switch (scheduledNotification.NotificationType)
                {
                    case NotificationType.DailyAlarm:
                        Settings.DailyNotificationData = scheduledNotification;
                        break;
                    case NotificationType.RandomAlarm:
                        Settings.RandomNotificationData = scheduledNotification;
                        break;
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex, "CreatingScheduledNotification");
            }
        }

        public static void SetScheduledNotifications(Context context)
        {

            if (NotificationUtils.ShouldCreateNotification(NotificationType.DailyAlarm))
            {
                try
                {
                    var scheduledNotification = NotificationUtils.GetScheduledNotification(NotificationType.DailyAlarm);
                    ScheduleNotification(context, scheduledNotification);
                    NotificationUtils.UpdateScheduledNotification(NotificationType.DailyAlarm, true);
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex, "SetScheduledDailyAlarm");
                }
            }

            if (NotificationUtils.ShouldCreateNotification(NotificationType.RandomAlarm))
            {
                try
                {
                    var scheduledNotification = NotificationUtils.GetScheduledNotification(NotificationType.RandomAlarm);
                    ScheduleNotification(context, scheduledNotification);
                    NotificationUtils.UpdateScheduledNotification(NotificationType.RandomAlarm, true);
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex, "SetScheduledRandomAlarm");
                }
            }
        }

        public static PendingIntent GetPendingNotificationIntent(ScheduledNotification scheduledNotification, Context context)
        {
            Intent intent = new Intent(context, typeof(AlarmReceiver));
            intent.SetAction(Constants.NOTIFICATION_QUOTE_ACTION);
            intent.PutExtra(Constants.NOTIFICTAIONTYPE_KEY, scheduledNotification.NotificationType.ToString());
            // Use different Request Codes when setting alarms to prevent collision
            int requestCode = scheduledNotification.NotificationType == NotificationType.DailyAlarm ?
                                                   Constants.DAILY_ALARM_REQUEST_CODE : Constants.RANDOM_ALARM_REQUEST_CODE;

            return PendingIntent.GetBroadcast(Application.Context, requestCode, intent, PendingIntentFlags.UpdateCurrent);
        }

        public static void SetFreshNotifications(Context context)
        {
            try
            {
                // Unset Notifications to enable us create new ones
                NotificationUtils.UnSetNotifications();

                // initialize notifications
                SetScheduledNotifications(context);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex, "SchedulingFreshNotification");
            }
        }

        public static void SendSwipeEnabledNotification(Context context)
        {
            var notificationIntent = new Intent(context, typeof(MainActivity));
            notificationIntent.SetFlags(ActivityFlags.SingleTop);

            var pendingIntent = PendingIntent.GetActivity(context, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);

            var notificationBuilder = new NotificationCompat.Builder(context, Constants.NOTIFICTAIONTYPE_CHANNEL_ID)
            .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
            .SetSmallIcon(Resource.Mipmap.icon)
            .SetContentIntent(pendingIntent)
            .SetPriority(NotificationCompat.PriorityDefault)
            .SetContentText("Hey, Time to get back to swiping")
            .SetAutoCancel(true);

            NotificationManagerCompat.From(Application.Context).Notify(DateTime.Now.Millisecond, notificationBuilder.Build());
        }

        public static void ScheduleSwipeEnabledNotification(Context context)
        {
            Intent intent = new Intent(context, typeof(AlarmReceiver));
            intent.SetAction(Constants.SWIPE_ENABLED_ACTION);
            var pendingIntent = PendingIntent.GetBroadcast(context, Constants.SWIPE_ENABLED_REQUEST_CODE, intent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
            // schedule for 24 hours time
            double totalMilliseconds = (DateTime.Now.AddDays(1) - DateTime.Now).TotalMilliseconds;
            long elapsedTime = (long)(Java.Lang.JavaSystem.CurrentTimeMillis() + totalMilliseconds);

            alarmManager.Set(AlarmType.RtcWakeup, elapsedTime, pendingIntent);
        }
    }
}
