using System;
using Xamarin.Forms;
using DezignSpiration.Models;
using DezignSpiration.Helpers;
using DezignSpiration.Pages;
namespace DezignSpiration.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        public Config Config { get; } = Settings.SettingsConfig;

        public Command GoBackCommand { get; }
        public Command GitHubCommand { get; }
        public Command AddQuoteCommand { get; }

        public SettingsViewModel(INavigation navigation) : base(navigation)
        {
            GoBackCommand = new Command(GoBack);
            GitHubCommand = new Command(() =>
            {
                Helper?.OpenUrl(Constants.GIT_URL);
            });
            AddQuoteCommand = new Command(() => {
                Navigation.PushModalAsync(new AddQuotePage(),true);
                //Helper?.OpenUrl(Constants.HOME_URL);
            });
        }

        void GoBack(object obj)
        {
            Navigation.PopModalAsync();
        }

        public void SaveSettings()
        {
            Settings.SettingsConfig = Config;
            NotificationUtils.ClearNotifications();
            Helper?.SetScheduledNotifications();
        }
    }
}
