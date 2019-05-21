using Xamarin.Forms;
using DezignSpiration.Helpers;
using DezignSpiration.Interfaces;

namespace DezignSpiration.Pages
{
    public partial class CrashPage : ContentPage
    {
        public CrashPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = DI.ViewModelLocator.CrashViewModel;
        }

        protected override bool OnBackButtonPressed()
        {
            DependencyService.Get<IButton>()?.BackButtonPressed();
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Utils.TrackEvent("CrashPage");
        }
    }
}
