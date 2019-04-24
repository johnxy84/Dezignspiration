using System;
using System.Linq;
using System.Threading.Tasks;
using DezignSpiration.Helpers;
using DezignSpiration.Interfaces;

namespace DezignSpiration.Models.Notifications
{
    public class DailyNotification : INotification
    {
        public void ClearNotification()
        {
            Settings.IsDailyNotificationSet = false;
        }

        public async Task<DesignQuote> GetDesignQuote(IQuotesRepository quotesRepository)
        {
            return (await quotesRepository.GetAllQuotes()).ElementAt(Utils.GetCurrentDisplayIndex());
        }

        public int GetNotificationId()
        {
            return 0;
        }

        public NotificationType GetNotificationType()
        {
            return NotificationType.DailyAlarm;
        }

        public bool IsSet()
        {
            return Settings.IsDailyNotificationSet;
        }

        public bool ShouldCreateNotification()
        {
            return !Settings.IsDailyNotificationSet && Settings.SettingsConfig.IsDailyQuoteEnabled;
        }

        public TimeSpan TimeToSend()
        {
            return Utils.GetTimeToScheduleNotification(Settings.SettingsConfig.DailyReminderTime);
        }



        public void ToggleNotificationIsSet(bool updateValue)
        {
            Settings.IsDailyNotificationSet = updateValue;
        }
    }
}
