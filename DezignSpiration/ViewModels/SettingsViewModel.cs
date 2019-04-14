using System;
using Xamarin.Forms;
using DezignSpiration.Models;
using DezignSpiration.Helpers;
using Microsoft.AppCenter.Push;
using Xamarin.Essentials;

namespace DezignSpiration.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public Config Config { get; } = Settings.SettingsConfig;

        public Command WebsiteCommand { get; }
        public Command AddQuoteCommand { get; }
        public Command FeedBackCommand { get; }
        public Command ReviewCommand { get; }

        public SettingsViewModel()
        {

            WebsiteCommand = new Command(() =>
            {
                Browser.OpenAsync(new Uri(Constants.BASE_URL), BrowserLaunchMode.SystemPreferred);
            });
            AddQuoteCommand = new Command(() =>
            {
                Navigation.NavigateToAsync<AddQuoteViewModel>(isModal: true);
            });
            FeedBackCommand = new Command(() =>
            {
                Navigation.NavigateToAsync<FeedbackViewModel>(isModal: true);
            });
            ReviewCommand = new Command(() =>
            {
                var storeUrl = Device.RuntimePlatform == Device.Android ? Constants.PLAY_STORE_URL : Constants.APP_STORE_URL;
                Browser.OpenAsync(new Uri(storeUrl), BrowserLaunchMode.SystemPreferred);
            });
        }

        public void SaveSettings()
        {
            Settings.SettingsConfig = Config;
            App.notificationService.ClearNotifications();
            Helper?.SetScheduledNotifications(App.notificationService.Notifications);
            Push.SetEnabledAsync(Config.IsReceivePushEnabled);
        }
    }
}
