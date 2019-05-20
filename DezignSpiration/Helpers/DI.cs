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
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DezignSpiration.Helpers
{
    public static class DI
    {
        private static bool isInitialized;
        private static bool isDevicePptsInitialized;

        public static ViewModelLocator ViewModelLocator { get; private set; }

        public static Random Random = new Random(DateTime.Now.Millisecond);

        public static NotificationService NotificationService { get; private set; }

        public static SQLiteAsyncConnection DbConnection { get; private set; }

        public static Dictionary<string, string> DeviceInfo { get; set; } = new Dictionary<string, string>();

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

        public static void InitDeviceProperties()
        {
            if (!isDevicePptsInitialized)
            {
                Task.Run(async () =>
                {
                    var deviceId = await Microsoft.AppCenter.AppCenter.GetInstallIdAsync();

                    DeviceInfo.Add(Constants.DEVICE_INSTALLATION_ID, deviceId.ToString());
                    DeviceInfo.Add(Constants.DEVICE_MODEL, Xamarin.Essentials.DeviceInfo.Model);
                    DeviceInfo.Add(Constants.DEVICE_MANUFACTURER, Xamarin.Essentials.DeviceInfo.Manufacturer);
                    DeviceInfo.Add(Constants.DEVICE_OS, Xamarin.Essentials.DeviceInfo.VersionString);
                    DeviceInfo.Add(Constants.DEVICE_PLATFORM, Xamarin.Essentials.DeviceInfo.Platform.ToString());

                    isDevicePptsInitialized = true;
                });
            }
        }
    }
}
