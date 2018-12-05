using System;
using DezignSpiration.Models;
namespace DezignSpiration.Helpers
{
    public interface IHelper
    {
        void DisplayMessage(string title, string message, string positive, string negative, Action<bool> choice);

        void ShowAlert(string message, bool isLongAlert = true, bool isToast = true, string actionMessage = null, Action<object> action = null);

        void ScheduleNotification(ScheduledNotification scheduledNotification);

        void CancelScheduledNotification(ScheduledNotification notificationData);

        void SetScheduledNotifications();

        void OpenUrl(string url);

        void ShareQuote(DesignQuote quote);

        void BackButtonPressed();

        void ShowOptions(string title, string[] options, Action<object> choice);

    }
}
