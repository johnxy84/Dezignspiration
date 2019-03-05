using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using DezignSpiration.Interfaces;
using DezignSpiration.ViewModels;
using Xamarin.Forms;
using DezignSpiration.Pages;
using DezignSpiration.Helpers;

namespace DezignSpiration.Services
{
    public class NavigationService : INavigationService
    {
        public Task InitializeAsync()
        {
            return Settings.IsFirstTime ? NavigateToAsync<OnBoardingViewModel>() : NavigateToAsync<HomeViewModel>();
        }

        public Task NavigateToAsync<TViewModel>(bool isModal = false, object parameter = null) where TViewModel : BaseViewModel
        {
            return InternalNavigateToAsync(typeof(TViewModel), isModal, parameter);
        }

        private async Task InternalNavigateToAsync(Type viewModelType, bool isModal, object parameter)
        {
            Page page = CreatePage(viewModelType, parameter);

            if (page is OnBoardingPage)
            {
                Application.Current.MainPage = new NavigationPage(page);
            }
            else
            {
                if (Application.Current.MainPage is NavigationPage navigationPage)
                {
                    if (isModal)
                    {
                        await navigationPage.Navigation.PushModalAsync(page, true);
                    }
                    else
                    {
                        await navigationPage.Navigation.PushAsync(page, true);
                    }
                }
                else
                {
                    Application.Current.MainPage = new NavigationPage(page);
                }
            }

            await (page.BindingContext as BaseViewModel).InitializeAsync(parameter);
        }

        public async Task GoBackAsync(bool isModal)
        {
            if (Application.Current.MainPage is NavigationPage navigationPage && (isModal || CanGoBack(navigationPage)))
            {
                if (isModal)
                {
                    await navigationPage.Navigation.PopModalAsync();
                }
                else
                {
                    await navigationPage.Navigation.PopAsync();
                }
            }
        }

        public bool CanGoBack(NavigationPage navigationPage)
        {
            // We don't want to navigate back from the homepage or onboarding page because, duhhh?
            return !(navigationPage.CurrentPage is HomePage) && !(navigationPage.CurrentPage is OnBoardingPage);
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("ViewModel", "Page");
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        private Page CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            try
            {
                Page page = Activator.CreateInstance(pageType) as Page;
                return page;
            }
            catch (Exception ex)
            {
                Utils.LogError(ex, "ErrorCreatingNavigationPage");
                throw ex;
            }
        }
    }
}
