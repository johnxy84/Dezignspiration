using System;
using System.Threading.Tasks;
using DezignSpiration.Helpers;
using DezignSpiration.Interfaces;
using System.Linq;

namespace DezignSpiration.Models.Notifications
{
    public class RandomNotification : INotification
    {
        public void ClearNotification()
        {
            Settings.IsRandomNotificationSet = false;
        }

        public async Task<DesignQuote> GetDesignQuote(IQuotesRepository quotesRepository)
        {
            return await quotesRepository.GetRandomQuote();
        }

        public int GetNotificationId()
        {
            return 1;
        }

        public NotificationType GetNotificationType()
        {
            return NotificationType.RandomAlarm;
        }

        public bool IsSet()
        {
            return Settings.IsRandomNotificationSet;
        }

        public bool ShouldCreateNotification()
        {
            return !Settings.IsRandomNotificationSet && Settings.SettingsConfig.IsRandomQuoteEnabled;
        }

        public TimeSpan TimeToSend()
        {
            var config = Settings.SettingsConfig;
            var randomQuoteFrequency = config.RandomQuoteFrequencies.ElementAt(config.SelectedRandomQuoteIndex);
            int minHour = randomQuoteFrequency.MinHour;
            int maxHour = randomQuoteFrequency.MaxHour;
            return new TimeSpan(DI.Random.Next(minHour, maxHour), DI.Random.Next(0, 59), 0);
        }

        public void ToggleNotificationIsSet(bool isNotificationSet)
        {
            Settings.IsRandomNotificationSet = isNotificationSet;
        }
    }
}
