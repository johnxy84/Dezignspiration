using Xamarin.Forms;
namespace DezignSpiration.ViewModels
{
    public class CrashViewModel : BaseViewModel
    {
        public Command RefreshCommand { get; }

        public CrashViewModel()
        {
            RefreshCommand = new Command(async () =>
            {
                IsBusy = true;
                bool isSuccessful = await GetFreshQuotes(shouldAllowRetry: false);
                IsBusy = false;
                if (isSuccessful)
                {
                    await Navigation.NavigateToAsync<HomeViewModel>();
                }
            });
        }
    }
}
