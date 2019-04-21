using System;
using System.Collections.Generic;

namespace DezignSpiration.Models
{
    public class Config : ObservableObject
    {
        private bool isDailyQuoteEnabled = true;
        private TimeSpan dailyReminderTime = new TimeSpan(7, 0, 0);
        private bool isRandomQuoteEnabled;
        private bool isReceivePushEnabled = true;
        private int selectedRandomQuoteIndex;

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

        public int SelectedRandomQuoteIndex
        {
            get => selectedRandomQuoteIndex;
            set
            {
                selectedRandomQuoteIndex = value;
                OnPropertyChanged();
            }
        }

        public List<RandomQuoteFrequency> RandomQuoteFrequencies
        {
            get
            {
                return new List<RandomQuoteFrequency>
                {
                    new RandomQuoteFrequency(), new RandomQuoteFrequency(title: "Very Often", minHour: 3, maxHour: 5)
                };
            }
        }
    }

    public class RandomQuoteFrequency
    {
        public string Title { get; set; } = "Not often";

        public int MinHour { get; set; } = 6;

        public int MaxHour { get; set; } = 9;

        public RandomQuoteFrequency()
        {
        }

        public RandomQuoteFrequency(string title, int minHour, int maxHour)
        {
            Title = title;
            MinHour = minHour;
            MaxHour = maxHour;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
