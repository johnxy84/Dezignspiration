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
            BindingContext = DI.ViewModelLocator.FeedbackViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Utils.TrackEvent("FeedbackPage");
        }
    }
}
