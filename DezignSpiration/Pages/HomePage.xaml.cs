using System;
using DezignSpiration.ViewModels;
using Xamarin.Forms;
using DezignSpiration.Helpers;
using DezignSpiration.Interfaces;
using System.Threading.Tasks;

namespace DezignSpiration.Pages
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = DI.ViewModelLocator.HomeViewModel;
            Utils.TrackEvent("HomePage");
        }

        protected override bool OnBackButtonPressed()
        {
            DependencyService.Get<IButton>()?.BackButtonPressed();
            return true;
        }
    }
}
