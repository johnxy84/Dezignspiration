using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DezignSpiration.Pages;
using DezignSpiration.Helpers;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using Plugin.Connectivity;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DezignSpiration
{
    public partial class App : Application
    {
        public static Client NetworkClient;
        public static Random Random = new Random(DateTime.Now.Millisecond);

        public App()
        {
            InitializeComponent();
            //NetworkClient = new Client("http://DezignSpiration.xyz/api");
            NetworkClient = new Client("http://localhost:33300");
            MainPage = Settings.IsFirstTime ? new NavigationPage(new OnBoardinPage()) : (Page)new HomePage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            Microsoft.AppCenter.AppCenter.Start("android=c6b1c77b-086d-4f16-8b9d-c2275ea87592;", typeof(Analytics), typeof(Crashes), typeof(Push));
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
        }

        async void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            // Oly perform check if the need to arises
            if(Settings.ShouldRefresh && Settings.IsTimeToRefresh)
            {
                //Alert that if there's connection
                var connectivity = CrossConnectivity.Current;
                var reached = await connectivity.IsRemoteReachable(new Uri(Constants.HOME_URL), TimeSpan.FromSeconds(5));
                if(reached)
                {
                    MessagingCenter.Send(this, Constants.NETWORK_AVAILABLE_KEY);
                }
            }
        }


        protected override void OnSleep()
        {
            // Handle when your app sleeps
            CrossConnectivity.Current.ConnectivityChanged -= Current_ConnectivityChanged;

        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;

        }
    }
}
