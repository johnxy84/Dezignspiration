using System;
using Android.App;
using Android.Content;
using Android.OS;
using DezignSpiration.Models;
using DezignSpiration.Helpers;
using Android.Support.V4.App;
using Android.Widget;
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

        public static void SendScheduledNotification(Context context, ScheduledNotification scheduledNotification)
        {
            var notificationIntent = new Intent(context, typeof(MainActivity));
            notificationIntent.SetFlags(ActivityFlags.SingleTop);

            PendingIntent tappedPendingIntent = PendingIntent.GetActivity(context, Constants.TOUCH_NOTIFICATON_REQUEST_CODE, notificationIntent,PendingIntentFlags.UpdateCurrent);

            // Set action for share button
            Intent shareIntent = new Intent(context, typeof(AlarmReceiver));
            shareIntent.SetAction(Constants.SHARE_NOTIFICATION_QUOTE_INTENT_ACTION);
            shareIntent.PutExtra(Constants.NOTIFICTAIONTYPE_KEY, scheduledNotification.NotificationType.ToString());
            // Add This notification to the intent if the user needs to share it
            shareIntent.PutExtra(Constants.SHARE_NOTIFICATION_QUOTE_INTENT_ACTION, JsonConvert.SerializeObject(scheduledNotification.DesignQuote));
            PendingIntent shareNotificationPendingIntent = PendingIntent.GetBroadcast(context, Constants.SHARE_NOTIFICATION_REQUEST_CODE, shareIntent, PendingIntentFlags.CancelCurrent);

            var notificationBuilder = new NotificationCompat.Builder(context, Constants.NOTIFICTAIONTYPE_CHANNEL_ID)
                        .SetContentTitle(scheduledNotification.DesignQuote.Author)
                        .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                        .SetSmallIcon(Resource.Mipmap.icon)
                        .SetContentIntent(tappedPendingIntent)
                        .SetPriority(NotificationCompat.PriorityDefault)
                        .SetStyle(new NotificationCompat.BigTextStyle().BigText(scheduledNotification.DesignQuote.Quote))
                        .AddAction(Resource.Drawable.share, "Share Quote", shareNotificationPendingIntent)
                        .SetContentText(scheduledNotification.DesignQuote.Quote)
                        .SetAutoCancel(true);

            if(scheduledNotification.NotificationType == NotificationType.DailyAlarm)
            {
                notificationBuilder.SetSubText("Daily Quote");
            }

            // Show the notification.
            var notificationManager = NotificationManagerCompat.From(Application.Context);
            notificationManager.Notify(Settings.NotificationCount++, notificationBuilder.Build());
        }

        public static void ScheduledNotification(Context context, ScheduledNotification scheduledNotification)
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
                    ScheduledNotification(context, scheduledNotification);
                    NotificationUtils.UpdateScheduledNotification(NotificationType.DailyAlarm, true);
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex, "SetScheduledDailyAlarm");
                }
            }

            if (NotificationUtils.ShouldCreateNotification(NotificationType.RandomAlarm))
            {
                var scheduledNotification = NotificationUtils.GetScheduledNotification(NotificationType.RandomAlarm);
                ScheduledNotification(context, scheduledNotification);
                NotificationUtils.UpdateScheduledNotification(NotificationType.RandomAlarm, true);
            }
        }

        public static PendingIntent GetPendingNotificationIntent(ScheduledNotification scheduledNotification, Context context)
        {
            Intent intent = new Intent(context, typeof(AlarmReceiver));
            intent.SetAction(Constants.NOTIFICATION_QUOTE_ACTION);
            intent.PutExtra(Constants.SCHEDULED_NOTIFICATION_KEY, JsonConvert.SerializeObject(scheduledNotification));
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
    }
}
