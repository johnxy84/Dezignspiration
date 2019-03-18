using System;
using Xamarin.Forms;
using DezignSpiration.Models;
using DezignSpiration.Helpers;
using Microsoft.AppCenter.Push;
namespace DezignSpiration.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public Config Config { get; } = Settings.SettingsConfig;

        public Command WebsiteCommand { get; }
        public Command AddQuoteCommand { get; }
        public Command FeedBackCommand { get; }

        public SettingsViewModel()
        {

            WebsiteCommand = new Command(() =>
            {
                Helper?.OpenUrl(Constants.BASE_URL);
            });
            AddQuoteCommand = new Command(() =>
            {
                Navigation.NavigateToAsync<AddQuoteViewModel>(isModal: true);
            });
            FeedBackCommand = new Command(() =>
            {
                Navigation.NavigateToAsync<FeedbackViewModel>(isModal: true);
            });
        }


        public void SaveSettings()
        {
            Settings.SettingsConfig = Config;
            NotificationUtils.ClearNotifications();
            Helper?.SetScheduledNotifications();
            Push.SetEnabledAsync(Config.IsReceivePushEnabled);
        }
    }
}
