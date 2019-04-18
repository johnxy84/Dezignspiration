using System;
using System.Collections.Generic;
using DezignSpiration.Helpers;
using Xamarin.Forms;

namespace DezignSpiration.Pages
{
    public partial class FeedbackPage : ContentPage
    {
        public FeedbackPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = App.ViewModelLocator.FeedbackViewModel;
            Utils.TrackEvent("FeedbackPage");
        }
    }
}
