using System;
using Android.Content;
using DezignSpiration.Models;
using DezignSpiration.Helpers;
using Android.Support.V4.App;
using Newtonsoft.Json;
using DezignSpiration.Interfaces;
using CommonServiceLocator;
using System.Linq;
using Xamarin.Forms;

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

                            Helper.ShareQuote(context, designQuote, designQuote.Quote.Length > Helpers.Constants.MAX_QUOTE_LENGTH);
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
                        try
                        {
                            NotificationType notificationType = intent.GetStringExtra(Constants.NOTIFICTAIONTYPE_KEY) == NotificationType.RandomAlarm.ToString() ? NotificationType.RandomAlarm : NotificationType.DailyAlarm;
                            var quotesRepository = ServiceLocator.Current.GetInstance<IQuotesRepository>();
                            INotification notification = App.notificationService.GetNotification(notificationType);
                            DesignQuote designQuote = await notification.GetDesignQuote(quotesRepository);
                            notification.ToggleNotificationIsSet(false);

                            await NotificationHelper.SendScheduledNotification(context, notification);
                            NotificationHelper.SetScheduledNotifications(context, App.notificationService.Notifications);
                            MessagingCenter.Send(SwipeToggled.Message, Helpers.Constants.SWIPE_TOGGLED, true);
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
                        NotificationHelper.SendSwipeEnabledNotification(context);
                        MessagingCenter.Send(SwipeToggled.Message, Helpers.Constants.SWIPE_TOGGLED, true);
                    }
                    catch (Exception ex)
                    {
                        Utils.LogError(ex, "ErrorEnablingSwipe");
                    }
                    break;
                case Constants.BACKGROUND_SERVICE_ACTION:
                    Android.Widget.Toast.MakeText(context, "BG Alarm Received", Android.Widget.ToastLength.Long);
                    Intent restartIntent = new Intent(context, typeof(KillStopper));
                    context.StartService(restartIntent);
                    break;

            }
        }

    }
}
