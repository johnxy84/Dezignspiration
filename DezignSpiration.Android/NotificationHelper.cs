using System;
using Android.App;
using Android.Content;
using Android.OS;
using DezignSpiration.Models;
using DezignSpiration.Helpers;
using Android.Support.V4.App;
using Android.Media;
using Newtonsoft.Json;
using DezignSpiration.Interfaces;
using System.Collections.Generic;
using CommonServiceLocator;
using System.Threading.Tasks;

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
                NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
                notificationManager?.CreateNotificationChannel(channel);
            }
        }

        public static async Task SendScheduledNotification(Context context, INotification notification)
        {
            var notificationIntent = new Intent(context, typeof(MainActivity));
            var quotesRepository = ServiceLocator.Current.GetInstance<IQuotesRepository>();
            var designQuote = await notification.GetDesignQuote(quotesRepository);
            notificationIntent.SetFlags(ActivityFlags.SingleTop);

            PendingIntent tappedPendingIntent = PendingIntent.GetActivity(context, Constants.TOUCH_NOTIFICATON_REQUEST_CODE, notificationIntent, PendingIntentFlags.UpdateCurrent);

            // Set action for share button
            Intent shareIntent = new Intent(context, typeof(AlarmReceiver));
            shareIntent.SetAction(Constants.SHARE_NOTIFICATION_QUOTE_INTENT_ACTION);
            shareIntent.PutExtra(Constants.NOTIFICTAIONTYPE_KEY, notification.GetType().ToString());
            // Add This notification to the intent just in case the user needs to share it
            shareIntent.PutExtra(Constants.SHARE_NOTIFICATION_QUOTE_INTENT_ACTION, JsonConvert.SerializeObject(designQuote));
            PendingIntent shareNotificationPendingIntent = PendingIntent.GetBroadcast(context, Constants.SHARE_NOTIFICATION_REQUEST_CODE, shareIntent, PendingIntentFlags.CancelCurrent);

            var notificationBuilder = new NotificationCompat.Builder(context, Constants.NOTIFICTAIONTYPE_CHANNEL_ID)
                .SetContentTitle(designQuote.Author)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetSmallIcon(Resource.Drawable.notify_icon)
                .SetContentIntent(tappedPendingIntent)
                .SetContentText(designQuote.Quote)
                .SetPriority(NotificationCompat.PriorityDefault)
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(designQuote.Quote))
                .AddAction(Resource.Drawable.share, "Share Quote", shareNotificationPendingIntent)
                .SetContentText(designQuote.Quote)
                .SetAutoCancel(true);

            if (notification.GetNotificationType() == NotificationType.DailyAlarm)
            {
                notificationBuilder.SetSubText("Daily Quote");
            }

            // Show the notification.
            var notificationManager = NotificationManagerCompat.From(Application.Context);
            notificationManager.Notify(Settings.NotificationCount++, notificationBuilder.Build());
        }

        public static void ScheduleNotification(Context context, INotification notification)
        {
            try
            {
                var pendingIntent = GetPendingNotificationIntent(notification, context);

                AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
                long elapsedTime = (long)(Java.Lang.JavaSystem.CurrentTimeMillis() + notification.TimeToSend().TotalMilliseconds);

                // Set the Alarm
                alarmManager.Set(AlarmType.RtcWakeup, elapsedTime, pendingIntent);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex, "CreatingScheduledNotification");
            }
        }

        public static void SetScheduledNotifications(Context context, List<INotification> notifications)
        {
            foreach (var notification in notifications)
            {
                if (notification.ShouldCreateNotification())
                {
                    try
                    {
                        ScheduleNotification(context, notification);
                        notification.ToggleNotificationIsSet(true);
                    }
                    catch (Exception ex)
                    {
                        Utils.LogError(ex, "SettingNotification", notification.GetType().ToString());
                    }
                }
            }
        }

        public static PendingIntent GetPendingNotificationIntent(INotification notification, Context context)
        {
            Intent intent = new Intent(context, typeof(AlarmReceiver));
            intent.SetAction(Constants.NOTIFICATION_QUOTE_ACTION);
            intent.PutExtra(Constants.NOTIFICTAIONTYPE_KEY, notification.GetNotificationType().ToString());
            int requestCode = notification.GetNotificationId();

            return PendingIntent.GetBroadcast(Application.Context, requestCode, intent, PendingIntentFlags.UpdateCurrent);
        }

        public static void SetOrphanedNotifications(Context context)
        {
            try
            {
                foreach (var notification in App.notificationService.Notifications)
                {
                    // notification is marked as scheduled but it's not scheduled
                    if (!IsNotificationSchdeuled(notification, context) && notification.IsSet())
                    {
                        SendNotification(Application.Context, $"Scheduliing {notification.GetType()}");
                        ScheduleNotification(context, notification);
                    }
                }
            }
            catch (Exception ex)
            {
                SendNotification(Application.Context, $"Scheduliing Orphaned Error");
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

        public static void SendNotification(Context context, string text)
        {
            var notificationIntent = new Intent(context, typeof(MainActivity));
            notificationIntent.SetFlags(ActivityFlags.SingleTop);

            var pendingIntent = PendingIntent.GetActivity(context, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);

            var notificationBuilder = new NotificationCompat.Builder(context, Constants.NOTIFICTAIONTYPE_CHANNEL_ID)
            .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
            .SetSmallIcon(Resource.Mipmap.icon)
            .SetContentIntent(pendingIntent)
            .SetPriority(NotificationCompat.PriorityDefault)
            .SetContentText(text)
            .SetAutoCancel(true);

            NotificationManagerCompat.From(Application.Context).Notify(DateTime.Now.Millisecond, notificationBuilder.Build());

        }

        public static void ScheduleSwipeEnabledNotification(Context context, double hours)
        {
            Intent intent = new Intent(context, typeof(AlarmReceiver));
            intent.SetAction(Constants.SWIPE_ENABLED_ACTION);
            var pendingIntent = PendingIntent.GetBroadcast(context, Constants.SWIPE_ENABLED_REQUEST_CODE, intent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
            // schedule for X hours time
            double totalMilliseconds = (DateTime.Now.AddHours(hours) - DateTime.Now).TotalMilliseconds;
            long elapsedTime = (long)(Java.Lang.JavaSystem.CurrentTimeMillis() + totalMilliseconds);

            alarmManager.Set(AlarmType.RtcWakeup, elapsedTime, pendingIntent);
        }

        public static bool IsNotificationSchdeuled(INotification notification, Context context)
        {
            var intent = new Intent(context, typeof(AlarmReceiver));
            intent.SetAction(Constants.NOTIFICATION_QUOTE_ACTION);
            intent.PutExtra(Constants.NOTIFICTAIONTYPE_KEY, notification.GetType().ToString());
            return PendingIntent.GetBroadcast(context, notification.GetNotificationId(), intent, PendingIntentFlags.NoCreate) != null;
        }
    }
}
