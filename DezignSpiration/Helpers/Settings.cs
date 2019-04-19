using System;
using DezignSpiration.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace DezignSpiration.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {

        #region Setting Constants

        private const string IndexKey = nameof(IndexKey);
        private const string CurrentDateKey = nameof(CurrentDateKey);
        private const string FlagReasonsKey = nameof(FlagReasonsKey);
        private const string ConfigKey = nameof(ConfigKey);
        private const string IsFirstTimeKey = nameof(IsFirstTimeKey);
        private const string IsDailyNotificationSetKey = nameof(IsDailyNotificationSetKey);
        private const string IsRandomNotificationSetKey = nameof(IsRandomNotificationSetKey);
        private const string Notif = nameof(Notif);
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
        private const string LengthyQuoteShareKey = nameof(LengthyQuoteShareKey);
        private const string tokenExpiryKey = nameof(tokenExpiryKey);
        private const string tokenKey = nameof(tokenKey);

        #endregion


        #region Defaults

        private static readonly string flagReasonsDefault = JsonConvert.SerializeObject(Utils.GetDefaultFlagReasons());
        private static readonly string flaggedQuotesDefault = JsonConvert.SerializeObject(new ObservableRangeCollection<int>());
        private static readonly string configDefault = JsonConvert.SerializeObject(new Config());
        private static readonly DateTime defaultDate = DateTime.Now;

        #endregion

        public static DateTime TokenExpiry
        {
            get => Preferences.Get(tokenExpiryKey, DateTime.Now);
            set
            {
                Preferences.Set(tokenExpiryKey, value);
            }
        }

        public static string Token
        {
            get => Preferences.Get(tokenKey, string.Empty);
            set
            {
                Preferences.Set(tokenKey, value);
            }
        }

        public static int CurrentIndex
        {
            get => Preferences.Get(IndexKey, 0);
            set
            {
                Preferences.Set(IndexKey, value);
                CurrentDate = DateTime.Today;
            }
        }

        public static int SwipeCount
        {
            get => Preferences.Get(SwipeCountKey, 0);
            set
            {
                // Count shouldn't be going below zero because reasons
                Preferences.Set(SwipeCountKey, value < 0 ? 0 : value);
            }
        }

        public static DateTime CurrentDate
        {
            get => Preferences.Get(CurrentDateKey, defaultDate);
            set => Preferences.Set(CurrentDateKey, value);
        }

        public static ObservableRangeCollection<FlagReason> FlagReasons
        {
            get => JsonConvert.DeserializeObject<ObservableRangeCollection<FlagReason>>(Preferences.Get(FlagReasonsKey, flagReasonsDefault));
            set => Preferences.Set(FlagReasonsKey, JsonConvert.SerializeObject(value));
        }

        public static ObservableRangeCollection<int> FlagedQuoteIds
        {
            get => JsonConvert.DeserializeObject<ObservableRangeCollection<int>>(Preferences.Get(FlaggedQuotesKey, flaggedQuotesDefault));
            set => Preferences.Set(FlagReasonsKey, JsonConvert.SerializeObject(value));
        }

        public static bool IsFirstTime
        {
            get => Preferences.Get(IsFirstTimeKey, true);
            set => Preferences.Set(IsFirstTimeKey, value);
        }

        public static Config SettingsConfig
        {
            get => JsonConvert.DeserializeObject<Config>(Preferences.Get(ConfigKey, configDefault));
            set => Preferences.Set(ConfigKey, JsonConvert.SerializeObject(value));
        }

        public static bool IsDailyNotificationSet
        {
            get => Preferences.Get(IsDailyNotificationSetKey, false);
            set => Preferences.Set(IsDailyNotificationSetKey, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this the quotes collection is due for refreshing
        /// </summary>
        /// <value><c>true</c> if should refresh; otherwise, <c>false</c>.</value>
        public static bool ShouldRetryQuotes
        {
            get => Preferences.Get(ShouldRefresKey, false);
            set => Preferences.Set(ShouldRefresKey, value);
        }

        public static bool IsRandomNotificationSet
        {
            get => Preferences.Get(IsRandomNotificationSetKey, false);
            set => Preferences.Set(IsRandomNotificationSetKey, value);
        }

        public static int NotificationCount
        {
            get => Preferences.Get(NotificationCountKey, 0);
            set
            {
                // Reset if this value reaches the max int value
                // Meaning the User has gotten that amount of notifications
                if (value == int.MaxValue) value = 0;
                Preferences.Set(NotificationCountKey, value);
            }
        }

        public static DateTime LastRetryTime
        {
            get => Preferences.Get(LastRetryTimeKey, defaultDate);
            set => Preferences.Set(LastRetryTimeKey, DateTime.SpecifyKind(value, DateTimeKind.Utc));
        }

        public static bool ShouldRefreshQuotes
        {
            get
            {
                return (DateTime.Now - LastRetryTime).TotalMinutes > 1;
            }
        }

        public static DateTime LastColorRefreshTime
        {
            get => Preferences.Get(LastRefreshColorsTimeKey, defaultDate);
            set => Preferences.Set(LastRefreshColorsTimeKey, DateTime.SpecifyKind(value, DateTimeKind.Utc));
        }

        public static bool ShouldRefreshColors => (DateTime.Now - LastColorRefreshTime).Days > 14;

        public static DateTime LastFlagReasonsRefreshTime
        {
            get => Preferences.Get(LastRefreshFlagReasonsTimeKey, defaultDate);
            set => Preferences.Set(LastRefreshFlagReasonsTimeKey, DateTime.SpecifyKind(value, DateTimeKind.Utc));
        }

        public static DateTime SwipeDisabledDate
        {
            get => Preferences.Get(SwipeDisabledDateKey, defaultDate.AddDays(-1));
            set => Preferences.Set(SwipeDisabledDateKey, DateTime.SpecifyKind(value, DateTimeKind.Utc));
        }

        public static bool ShouldRefreshFlagReasons => (DateTime.Now - LastFlagReasonsRefreshTime).Days > 30;

        public static bool HasShownLengthyQuoteWarning
        {
            get => Preferences.Get(LengthyQuoteShareKey, false);
            set => Preferences.Set(LengthyQuoteShareKey, value);
        }

    }
}
