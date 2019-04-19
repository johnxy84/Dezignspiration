using System;
using DezignSpiration.Models;
using System.Collections.Generic;
namespace DezignSpiration.Interfaces
{
    public interface IHelper
    {
        void DisplayMessage(string title, string message, string positive, string negative, Action<bool> choice);

        void ShowAlert(string message, bool isLongAlert = false, bool isToast = true, string actionMessage = null, Action<object> action = null);

        void CancelScheduledNotification(INotification notificationData);

        void SetScheduledNotifications(List<INotification> notifications);

        void ShareQuote(DesignQuote quote, bool isLongQuote = false);

        void ShowOptions(string title, string[] options, Action<object> choice, string cancelText = "Cancel");

        void BeginSwipeEnableCountdown(double hours = 12);
    }
}
