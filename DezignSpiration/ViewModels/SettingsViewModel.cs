using System;
using Xamarin.Forms;
using DezignSpiration.Models;
using DezignSpiration.Helpers;
using DezignSpiration.Pages;
using Microsoft.AppCenter.Push;
namespace DezignSpiration.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public Config Config { get; } = Settings.SettingsConfig;

        public Command GoBackCommand { get; }
        public Command GitHubCommand { get; }
        public Command AddQuoteCommand { get; }
        public Command FeedBackCommand { get; }

        public SettingsViewModel()
        {
            GoBackCommand = new Command(GoBack);
            GitHubCommand = new Command(() =>
            {
                Helper?.OpenUrl(Constants.GIT_URL);
            });
            AddQuoteCommand = new Command(() =>
            {
                Navigation.NavigateToAsync<AddQuoteViewModel>(isModal: true);
            });
            FeedBackCommand = new Command(() => { Helper?.ShowAlert("Feedback Clicked"); });
        }

        void GoBack(object obj)
        {
            Navigation.GoBackAsync(isModal: true);
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
