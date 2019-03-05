using System;
using System.Collections.Generic;

using Xamarin.Forms;
using DezignSpiration.ViewModels;

namespace DezignSpiration.Pages
{
    public partial class SettingsPage : ContentPage
    {
        SettingsViewModel settingsViewModel;
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = settingsViewModel = App.ViewModelLocator.SettingsViewModel;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            settingsViewModel?.SaveSettings();
        }
    }
}
