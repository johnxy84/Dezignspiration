using System;
using DezignSpiration.Models;
using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace DezignSpiration.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string IndexKey = "currentIndex";
        // The index should start from -1 so pre-increment (++index) would begin at 0
        private static readonly int IndexDefault = -1;
        private const string CurrentDateKey = "currentDateIndex";
        private static readonly DateTime CurrentDateDefault = DateTime.Today.AddDays(-1);
        private const string QuotesKey = "quotesKey";
        private static readonly string QuotesDefault = JsonConvert.SerializeObject(Utils.GetDefaultQuotes());
        private const string ConfigKey = "configKey";
        private static readonly string ConfigDefault = JsonConvert.SerializeObject(new Config());
        private const string SyncDateKey = "syncDateKey";
        private static readonly DateTime SyncDateDefault = DateTime.Now;
        private const string IsFirstTimeKey = "isFirstTimeKey";
        private const string IsDailyNotificationSetKey = "isDailyNotificationSetKey";
        private const string IsRandomNotificationSetKey = "isRandomNotificationSetKey";
        private const string DailyAlarmKey = "dailyAlarmKey";
        private const string RandomAlarmKey = "randomAlarmKey";
        private const string NotificationCountKey = "notificationCountKey";
        private const string ShouldRefresKey = "shouldRefreshKey";
        private const string LastRetryTimeKey = "lastRetryTimeKey";
        private const string LastRefreshColorsTimeKey = "lastRefreshColorsTimeKey";
        private const string ColorsKey = "colorsKey";
        private static readonly string ColorsDefault = JsonConvert.SerializeObject(new ObservableRangeCollection<Color> {
            new Color {
                Id = 1,
                PrimaryColor = "#257873",
                SecondaryColor = "#f8d591"
            },        
            new Color {
                Id = 2,
                PrimaryColor = "#003d73",
                SecondaryColor = "#1ecfd6"
            },         
            new Color {
                Id = 3,
                PrimaryColor = "#36688d",
                SecondaryColor = "#f3cd05"
            }
        });

        #endregion


        public static int CurrentIndex
        {
            get => AppSettings.GetValueOrDefault(IndexKey, IndexDefault);
            set => AppSettings.AddOrUpdateValue(IndexKey, value);
        }

        public static DateTime CurrentDate
        {
            get => AppSettings.GetValueOrDefault(CurrentDateKey, CurrentDateDefault);
            set => AppSettings.AddOrUpdateValue(CurrentDateKey, value);
        }

        public static ObservableRangeCollection<DesignQuote> QuotesData
        {
            get => JsonConvert.DeserializeObject<ObservableRangeCollection<DesignQuote>>(AppSettings.GetValueOrDefault(QuotesKey, QuotesDefault));
            set => AppSettings.AddOrUpdateValue(QuotesKey, JsonConvert.SerializeObject(value));
        }

        public static DateTime SyncDate
        {
            get => AppSettings.GetValueOrDefault(SyncDateKey, SyncDateDefault);
            set => AppSettings.AddOrUpdateValue(SyncDateKey, value);
        }

        public static bool IsFirstTime
        {
            get => AppSettings.GetValueOrDefault(IsFirstTimeKey, true);
            set => AppSettings.AddOrUpdateValue(IsFirstTimeKey, value);
        }

        public static Config SettingsConfig
        {
            get => JsonConvert.DeserializeObject<Config>(AppSettings.GetValueOrDefault(ConfigKey, ConfigDefault));
            set => AppSettings.AddOrUpdateValue(ConfigKey, JsonConvert.SerializeObject(value));
        }

        public static bool IsDailyNotificationSet
        {
            get => AppSettings.GetValueOrDefault(IsDailyNotificationSetKey, false);
            set => AppSettings.AddOrUpdateValue(IsDailyNotificationSetKey, value);
        }

        public static bool ShouldRefresh
        {
            get => AppSettings.GetValueOrDefault(ShouldRefresKey, false);
            set => AppSettings.AddOrUpdateValue(ShouldRefresKey, value);
        }

        public static bool IsRandomNotificationSet
        {
            get => AppSettings.GetValueOrDefault(IsRandomNotificationSetKey, false);
            set => AppSettings.AddOrUpdateValue(IsRandomNotificationSetKey, value);
        }

        public static ScheduledNotification DailyNotificationData
        {
            get => JsonConvert.DeserializeObject<ScheduledNotification>(AppSettings.GetValueOrDefault(DailyAlarmKey, null));
            set => AppSettings.AddOrUpdateValue(DailyAlarmKey, JsonConvert.SerializeObject(value));
        }

        public static ScheduledNotification RandomNotificationData
        {
            get => JsonConvert.DeserializeObject<ScheduledNotification>(AppSettings.GetValueOrDefault(RandomAlarmKey, null));
            set => AppSettings.AddOrUpdateValue(RandomAlarmKey, JsonConvert.SerializeObject(value));
        }

        public static int NotificationCount
        {
            get => AppSettings.GetValueOrDefault(NotificationCountKey, 0);
            set
            {
                // Reset if this value reaches the max int value
                // Meaning the User has gotten that amount of notifications
                if (value == int.MaxValue) value = 0;
                AppSettings.AddOrUpdateValue(NotificationCountKey, value);
            }
        }

        public static DateTime LastRetryTime
        {
            get => AppSettings.GetValueOrDefault(LastRetryTimeKey, DateTime.Now);
            set => AppSettings.AddOrUpdateValue(LastRetryTimeKey, value);
        }

        public static bool IsTimeToRefresh => (LastRetryTime - DateTime.Now).TotalMinutes > 5;

        public static DateTime LastColorRefreshTime
        {
            get => AppSettings.GetValueOrDefault(LastRefreshColorsTimeKey, DateTime.Now);
            set => AppSettings.AddOrUpdateValue(LastRefreshColorsTimeKey, value);
        }

        public static bool ShouldRefreshColors => (LastColorRefreshTime - DateTime.Now).Days > 14;

        public static ObservableRangeCollection<Color> ColorsData
        {
            get => JsonConvert.DeserializeObject<ObservableRangeCollection<Color>>(AppSettings.GetValueOrDefault(ColorsKey, ColorsDefault));
            set => AppSettings.AddOrUpdateValue(ColorsKey, JsonConvert.SerializeObject(value));
        }

    }
}
