using Xamarin.Forms;
using DezignSpiration.Helpers;

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
            return true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Utils.TrackEvent("CrashPage");
        }
    }
}
