using System;
using System.Collections.Generic;
using DezignSpiration.Helpers;
using DezignSpiration.Interfaces;
using DezignSpiration.Models.Notifications;
using Xamarin.Forms;
using DezignSpiration.Models;

namespace DezignSpiration.Services
{
    public class NotificationService
    {
        private List<INotification> notifications;
        private readonly IHelper helper;

        public List<INotification> Notifications
        {
            get
            {
                if (notifications == null)
                {
                    notifications = new List<INotification>
                    {
                        new DailyNotification(), new RandomNotification()
                    };
                }

                return notifications;
            }
        }

        public NotificationService(IHelper helper)
        {
            this.helper = helper;
        }

        public void ClearNotifications()
        {
            // Unset notifications already set
            Settings.IsRandomNotificationSet = Settings.IsDailyNotificationSet = false;
            // Cancel all scheduled notifications
            foreach (var notification in Notifications)
            {
                helper.CancelScheduledNotification(notification);
            }
        }

        public INotification GetNotification(NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.DailyAlarm:
                    return new DailyNotification();
                case NotificationType.RandomAlarm:
                    return new RandomNotification();
                default:
                    throw new Exception($"Invalid Notification Type {notificationType.ToString()}");
            }
        }
    }
}
