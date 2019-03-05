using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DezignSpiration.Helpers;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;
using Unity;
using DezignSpiration.Interfaces;
using DezignSpiration.ViewModels;
using DezignSpiration.Services;
using System.Threading.Tasks;
using SQLite;
using System.Diagnostics;
using Xamarin.Essentials;
using CommonServiceLocator;
using Unity.ServiceLocation;
using Constants = DezignSpiration.Helpers.Constants;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DezignSpiration
{
    public partial class App : Application
    {
        public static Random Random = new Random(DateTime.Now.Millisecond);
        public static ViewModelLocator ViewModelLocator;
        public static SQLiteAsyncConnection dbConnection;
        private INavigationService navigationService;


        public App()
        {
            InitializeComponent();
            UnityContainer container = new UnityContainer();
            dbConnection = new SQLiteAsyncConnection(Utils.GetDatabasePath())
            {
                Tracer = new Action<string>(q => Debug.WriteLine(q)),
                Trace = true
            };
            ViewModelLocator = new ViewModelLocator(container);
            SetupDI(container);
            InitializeNavigation();
        }

        private void SetupDI(UnityContainer container)
        {
            container.RegisterSingleton<IColorsRepository, ColorsRepository>();
            container.RegisterSingleton<IQuotesRepository, QuotesRepository>();
            container.RegisterSingleton<INetworkClient, NetworkClient>();
            container.RegisterSingleton<INavigationService, NavigationService>();
            container.RegisterSingleton<IFlagReasonService, FlagService>();

            IServiceLocator serviceLocator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }

        private Task InitializeNavigation()
        {
            navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            return navigationService?.InitializeAsync();
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
