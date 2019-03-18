using System;
using DezignSpiration.Models;
namespace DezignSpiration.Interfaces
{
    public interface IHelper
    {
        void DisplayMessage(string title, string message, string positive, string negative, Action<bool> choice);

        void ShowAlert(string message, bool isLongAlert = false, bool isToast = true, string actionMessage = null, Action<object> action = null);

        void CancelScheduledNotification(ScheduledNotification notificationData);

        void SetScheduledNotifications();

        void OpenUrl(string url);

        void ShareQuote(DesignQuote quote, bool isLongQuote = false);

        void ShowOptions(string title, string[] options, Action<object> choice, string cancelText = "Cancel");

        void BeginSwipeEnableCountdown();
    }
}
