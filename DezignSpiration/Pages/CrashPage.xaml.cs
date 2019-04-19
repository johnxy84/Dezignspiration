using System;
using System.Collections.Generic;

using Xamarin.Forms;
using DezignSpiration.ViewModels;
using DezignSpiration.Helpers;

namespace DezignSpiration.Pages
{
    public partial class CrashPage : ContentPage
    {
        public CrashPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = App.ViewModelLocator.CrashViewModel;
            Utils.TrackEvent("CrashPage");
        }
    }
}
