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
            Utils.TrackEvent("CrashPage");
        }
    }
}
