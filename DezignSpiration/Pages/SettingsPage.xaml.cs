using System;
using System.Collections.Generic;

using Xamarin.Forms;
using DezignSpiration.ViewModels;
using DezignSpiration.Helpers;

namespace DezignSpiration.Pages
{
    public partial class SettingsPage : ContentPage
    {
        SettingsViewModel settingsViewModel;
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = settingsViewModel = App.ViewModelLocator.SettingsViewModel;
            Utils.TrackEvent("SettingsPage");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            settingsViewModel?.SaveSettings();
        }
    }
}
