using Unity;
using Unity.Lifetime;

namespace DezignSpiration.ViewModels
{
    public class ViewModelLocator
    {
        private readonly UnityContainer container;

        public ViewModelLocator(UnityContainer container)
        {
            this.container = container;
            RegisterViewModels();
        }

        public HomeViewModel HomeViewModel => container.Resolve<HomeViewModel>();

        public AddColorViewModel AddColorViewModel => container.Resolve<AddColorViewModel>();

        public AddQuoteViewModel AddQuoteViewModel => container.Resolve<AddQuoteViewModel>();

        public SettingsViewModel SettingsViewModel => container.Resolve<SettingsViewModel>();

        public CrashViewModel CrashViewModel => container.Resolve<CrashViewModel>();

        public OnBoardingViewModel OnBoardingViewModel => container.Resolve<OnBoardingViewModel>();

        public FeedbackViewModel FeedbackViewModel => container.Resolve<FeedbackViewModel>();

        public void RegisterViewModels()
        {
            container.RegisterType<HomeViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<AddColorViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<AddQuoteViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<SettingsViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<CrashViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<OnBoardingViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<FeedbackViewModel>(new ContainerControlledLifetimeManager());
        }
    }
}
