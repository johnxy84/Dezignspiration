using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DezignSpiration.Helpers;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using DezignSpiration.Interfaces;
using DezignSpiration.ViewModels;
using Xamarin.Essentials;
using CommonServiceLocator;
using Constants = DezignSpiration.Helpers.Constants;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DezignSpiration
{
    public partial class App : Application
    {
        private INavigationService navigationService;

        public App()
        {
            InitializeComponent();
            InitializeApp();
        }

        private void InitializeApp()
        {
            DI.InitializeDI();
            navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService?.InitializeAsync();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            Microsoft.AppCenter.AppCenter.Start(Constants.APP_CENTER_SECRET, typeof(Analytics), typeof(Crashes), typeof(Push));
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            // Only perform check if the need to arises
            if (Settings.ShouldRetryQuotes && Settings.ShouldRefreshQuotes)
            {
                //Alert if there's connection
                if (e.NetworkAccess == NetworkAccess.Internet)
                {
                    MessagingCenter.Send(NetworkAvailable.Message, Constants.NETWORK_AVAILABLE_KEY);
                }
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        public void ProcessShareAction(string sharedMessage)
        {
            navigationService?.NavigateToAsync<AddQuoteViewModel>(true, sharedMessage);
        }
    }
}
