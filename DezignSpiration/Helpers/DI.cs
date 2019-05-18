using System;
using System.Diagnostics;
using CommonServiceLocator;
using DezignSpiration.Interfaces;
using DezignSpiration.Services;
using DezignSpiration.ViewModels;
using SQLite;
using Unity;
using Unity.Lifetime;
using Unity.ServiceLocation;

namespace DezignSpiration.Helpers
{
    public static class DI
    {
        private static bool isInitialized;

        public static ViewModelLocator ViewModelLocator { get; private set; }

        public static Random Random = new Random(DateTime.Now.Millisecond);

        public static NotificationService NotificationService { get; private set; }

        public static SQLiteAsyncConnection DbConnection { get; private set; }

        public static void InitializeDI()
        {
            if (!isInitialized)
            {
                UnityContainer container = new UnityContainer();
                container.RegisterSingleton<IColorsRepository, ColorsRepository>();
                container.RegisterSingleton<IQuotesRepository, QuotesRepository>();
                container.RegisterSingleton<INetworkClient, NetworkClient>();
                container.RegisterSingleton<INavigationService, NavigationService>();
                container.RegisterSingleton<IFlagReasonService, FlagService>();

                ViewModelLocator = new ViewModelLocator(container);
                NotificationService = new NotificationService();

                DbConnection = new SQLiteAsyncConnection(Utils.DatabasePath)
                {
                    Tracer = new Action<string>(q => Debug.WriteLine(q)),
                    Trace = true
                };

                container.RegisterType<NotificationService>(new ContainerControlledLifetimeManager());
                IServiceLocator serviceLocator = new UnityServiceLocator(container);
                ServiceLocator.SetLocatorProvider(() => serviceLocator);

                isInitialized = true;
            }
        }
    }
}
