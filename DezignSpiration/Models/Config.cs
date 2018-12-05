using System;
namespace DezignSpiration.Models
{
    public class Config : ObservableObject
    {
        private bool isDailyQuoteEnabled = true;
        private TimeSpan dailyReminderTime = new TimeSpan(7,0,0);
        private bool isRandomQuoteEnabled;
        private bool isReceivePushEnabled = true;

        public bool IsDailyQuoteEnabled
        {
            get => isDailyQuoteEnabled;
            set
            {
                isDailyQuoteEnabled = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan DailyReminderTime
        {
            get => dailyReminderTime;
            set
            {
                dailyReminderTime = value;
                OnPropertyChanged();
            }
        }

        public bool IsRandomQuoteEnabled
        {
            get => isRandomQuoteEnabled;
            set
            {
                isRandomQuoteEnabled = value;

                OnPropertyChanged();
            }
        }

        public bool IsReceivePushEnabled
        {
            get => isReceivePushEnabled;
            set
            {
                isReceivePushEnabled = value;
                OnPropertyChanged();
            }
        }
    }
}
