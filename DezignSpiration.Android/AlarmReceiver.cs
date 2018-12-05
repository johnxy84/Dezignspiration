using System;
using Android.Content;
using DezignSpiration.Models;
using DezignSpiration.Helpers;
using Android.Widget;
using Android.Support.V4.App;
using Newtonsoft.Json;

namespace DezignSpiration.Droid
{
    [BroadcastReceiver]
    public class AlarmReceiver: BroadcastReceiver
    {

        public override void OnReceive(Context context, Intent intent)
        {
            var notificationType = intent.GetStringExtra(Constants.NOTIFICTAIONTYPE_KEY);

            switch (intent.Action)
            {
                case Constants.SHARE_NOTIFICATION_QUOTE_INTENT_ACTION:

                    var designQuote = JsonConvert.DeserializeObject<DesignQuote>(intent.GetStringExtra(Constants.SHARE_NOTIFICATION_QUOTE_INTENT_ACTION));

                    try
                    {
                        // Close Notification Drawer
                        context.SendBroadcast(new Intent(Intent.ActionCloseSystemDialogs));

                        Helper.ShareQuote(context, designQuote);
                        var notificationManager = NotificationManagerCompat.From(context);

                        // Get notificationID which is the current count - 1
                        var notificationId = Settings.NotificationCount - 1;
                        notificationManager.Cancel(notificationId);
                    }
                    catch (Exception ex)
                    {
                        Utils.LogError(ex, "SharingQuote");
                    }

                    break;

                case Constants.NOTIFICATION_QUOTE_ACTION:

                    ScheduledNotification scheduledNotification;

                    // Get Notification Data based on type of notification
                    if (notificationType == NotificationType.RandomAlarm.ToString())
                    {
                        scheduledNotification = Settings.RandomNotificationData;
                        NotificationUtils.UpdateScheduledNotification(NotificationType.RandomAlarm, false);
                    }
                    else
                    {
                        scheduledNotification = Settings.DailyNotificationData;
                        NotificationUtils.UpdateScheduledNotification(NotificationType.DailyAlarm, false);
                    }

                    try
                    {
                        NotificationHelper.SendScheduledNotification(context, scheduledNotification);
                        NotificationHelper.SetScheduledNotifications(context);
                    }
                    catch (Exception ex)
                    {
                        Utils.LogError(ex, "SchedulingRandomQuote");
                    }
                    break;
            }
        }

    }
}
