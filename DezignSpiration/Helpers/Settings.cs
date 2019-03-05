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

        private const string IndexKey = nameof(IndexKey);
        private const string CurrentDateKey = nameof(CurrentDateKey);
        private const string FlagReasonsKey = nameof(FlagReasonsKey);
        private const string ConfigKey = nameof(ConfigKey);
        private const string IsFirstTimeKey = nameof(IsFirstTimeKey);
        private const string IsDailyNotificationSetKey = nameof(IsDailyNotificationSetKey);
        private const string IsRandomNotificationSetKey = nameof(IsRandomNotificationSetKey);
        private const string DailyAlarmKey = nameof(DailyAlarmKey);
        private const string RandomAlarmKey = nameof(RandomAlarmKey);
        private const string NotificationCountKey = nameof(NotificationCountKey);
        private const string ShouldRefresKey = nameof(ShouldRefresKey);
        private const string LastRetryTimeKey = nameof(LastRetryTimeKey);
        private const string LastRefreshColorsTimeKey = nameof(LastRefreshColorsTimeKey);
        private const string LastRefreshFlagReasonsTimeKey = nameof(LastRefreshFlagReasonsTimeKey);
        private const string ColorsKey = nameof(ColorsKey);
        private const string FlaggedQuotesKey = nameof(FlaggedQuotesKey);
        private const string CanSwipeKey = nameof(CanSwipeKey);
        private const string SwipeCountKey = nameof(SwipeCountKey);
        private const string SwipeDisabledDateKey = nameof(SwipeDisabledDateKey);

        #endregion


        #region Defaults

        private static readonly string flagReasonsDefault = JsonConvert.SerializeObject(Utils.GetDefaultFlagReasons());
        private static readonly string flaggedQuotesDefault = JsonConvert.SerializeObject(new ObservableRangeCollection<int>());
        private static readonly string configDefault = JsonConvert.SerializeObject(new Config());
        private static readonly DateTime defaultDate = DateTime.Now;

        #endregion

        public static int CurrentIndex
        {
            get => AppSettings.GetValueOrDefault(IndexKey, 0);
            set
            {
                AppSettings.AddOrUpdateValue(IndexKey, value);
                CurrentDate = DateTime.Today;
            }
        }

        public static int SwipeCount
        {
            get => AppSettings.GetValueOrDefault(SwipeCountKey, 0);
            set
            {
                AppSettings.AddOrUpdateValue(SwipeCountKey, value);
            }
        }

        public static DateTime CurrentDate
        {
            get => AppSettings.GetValueOrDefault(CurrentDateKey, defaultDate);
            set => AppSettings.AddOrUpdateValue(CurrentDateKey, value);
        }

        public static ObservableRangeCollection<FlagReason> FlagReasons
        {
            get => JsonConvert.DeserializeObject<ObservableRangeCollection<FlagReason>>(AppSettings.GetValueOrDefault(FlagReasonsKey, flagReasonsDefault));
            set => AppSettings.AddOrUpdateValue(FlagReasonsKey, JsonConvert.SerializeObject(value));
        }

        public static ObservableRangeCollection<int> FlagedQuoteIds
        {
            get => JsonConvert.DeserializeObject<ObservableRangeCollection<int>>(AppSettings.GetValueOrDefault(FlaggedQuotesKey, flaggedQuotesDefault));
            set => AppSettings.AddOrUpdateValue(FlagReasonsKey, JsonConvert.SerializeObject(value));
        }

        public static bool IsFirstTime
        {
            get => AppSettings.GetValueOrDefault(IsFirstTimeKey, true);
            set => AppSettings.AddOrUpdateValue(IsFirstTimeKey, value);
        }

        public static Config SettingsConfig
        {
            get => JsonConvert.DeserializeObject<Config>(AppSettings.GetValueOrDefault(ConfigKey, configDefault));
            set => AppSettings.AddOrUpdateValue(ConfigKey, JsonConvert.SerializeObject(value));
        }

        public static bool IsDailyNotificationSet
        {
            get => AppSettings.GetValueOrDefault(IsDailyNotificationSetKey, false);
            set => AppSettings.AddOrUpdateValue(IsDailyNotificationSetKey, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this the quotes collection is due for refreshing
        /// </summary>
        /// <value><c>true</c> if should refresh; otherwise, <c>false</c>.</value>
        public static bool ShouldRetryQuotes
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
            get => AppSettings.GetValueOrDefault(LastRetryTimeKey, defaultDate);
            set => AppSettings.AddOrUpdateValue(LastRetryTimeKey, DateTime.SpecifyKind(value, DateTimeKind.Utc));
        }

        public static bool ShouldRefreshQuotes => (DateTime.Now - LastRetryTime).TotalMinutes > 2;

        public static DateTime LastColorRefreshTime
        {
            get => AppSettings.GetValueOrDefault(LastRefreshColorsTimeKey, defaultDate);
            set => AppSettings.AddOrUpdateValue(LastRefreshColorsTimeKey, DateTime.SpecifyKind(value, DateTimeKind.Utc));
        }

        public static bool ShouldRefreshColors => (DateTime.Now - LastColorRefreshTime).Days > 14;

        public static DateTime LastFlagReasonsRefreshTime
        {
            get => AppSettings.GetValueOrDefault(LastRefreshFlagReasonsTimeKey, defaultDate);
            set => AppSettings.AddOrUpdateValue(LastRefreshFlagReasonsTimeKey, DateTime.SpecifyKind(value, DateTimeKind.Utc));
        }

        public static DateTime SwipeDisabledDate
        {
            get => AppSettings.GetValueOrDefault(SwipeDisabledDateKey, defaultDate.AddDays(-1));
            set => AppSettings.AddOrUpdateValue(SwipeDisabledDateKey, DateTime.SpecifyKind(value, DateTimeKind.Utc));
        }

        public static bool ShouldRefreshFlagReasons => (DateTime.Now - LastFlagReasonsRefreshTime).Days > 30;

    }
}
