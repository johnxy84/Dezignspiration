using System;
using Android.Content;
using DezignSpiration.Models;
using DezignSpiration.Helpers;
using Android.Support.V4.App;
using Newtonsoft.Json;
using DezignSpiration.Interfaces;
using CommonServiceLocator;

namespace DezignSpiration.Droid
{
    [BroadcastReceiver]
    public class AlarmReceiver : BroadcastReceiver
    {

        public override async void OnReceive(Context context, Intent intent)
        {
            switch (intent.Action)
            {
                case Constants.SHARE_NOTIFICATION_QUOTE_INTENT_ACTION:
                    {
                        try
                        {
                            DesignQuote designQuote = JsonConvert.DeserializeObject<DesignQuote>(intent.GetStringExtra(Constants.SHARE_NOTIFICATION_QUOTE_INTENT_ACTION));

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
                    }
                    break;

                case Constants.NOTIFICATION_QUOTE_ACTION:
                    {
                        Settings.SwipeCount = 0;
                        NotificationType notificationType = intent.GetStringExtra(Constants.NOTIFICTAIONTYPE_KEY) == NotificationType.RandomAlarm.ToString() ? NotificationType.RandomAlarm : NotificationType.DailyAlarm;
                        var quotesRepository = ServiceLocator.Current.GetInstance<IQuotesRepository>();
                        var colorsRepository = ServiceLocator.Current.GetInstance<IColorsRepository>();
                        DesignQuote designQuote;
                        switch (notificationType)
                        {
                            case NotificationType.RandomAlarm:
                                int randomIndex = App.Random.Next(0, await quotesRepository.CountQuotes());
                                designQuote = await quotesRepository.GetQuote(randomIndex);
                                NotificationUtils.UpdateScheduledNotification(NotificationType.RandomAlarm, false);
                                break;
                            // Assume default alarm 
                            default:
                                designQuote = await quotesRepository.GetQuote(Utils.GetCurrentDisplayIndex());
                                NotificationUtils.UpdateScheduledNotification(NotificationType.DailyAlarm, false);
                                break;
                        }

                        try
                        {
                            NotificationHelper.SendScheduledNotification(context, notificationType, designQuote);
                            NotificationHelper.SetScheduledNotifications(context);
                        }
                        catch (Exception ex)
                        {
                            Utils.LogError(ex, "DisplayingQuoteNotification");
                        }
                    }
                    break;
                case Constants.SWIPE_ENABLED_ACTION:
                    try
                    {
                        // Reset counter
                        Settings.SwipeCount = 0;
                        NotificationHelper.SendSwipeEnabledNotification(context);
                    }
                    catch (Exception ex)
                    {
                        Utils.LogError(ex, "ErrorEnablingSwipe");
                    }
                    break;
            }
        }

    }
}
