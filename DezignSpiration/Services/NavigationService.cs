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
            Page page = CreatePage(viewModelType);

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

        private Page CreatePage(Type viewModelType)
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
                throw ex;
            }
        }
    }
}

/*
{System.Reflection.TargetInvocationException: Exception has been thrown by the target of an invocation. ---> Xamarin.Forms.Xaml.XamlParseException: Position 31:26. StaticResource not found for key HeaderBackground
  at Xamarin.Forms.Xaml.StaticResourceExtension.GetApplicationLevelResource (System.String key, System.Xml.IXmlLineInfo xmlLineInfo) [0x0002c] in D:\a\1\s\Xamarin.Forms.Xaml\MarkupExtensions\StaticResourceExtension.cs:80 
  at Xamarin.Forms.Xaml.StaticResourceExtension.ProvideValue (System.IServiceProvider serviceProvider) [0x000d4] in D:\a\1\s\Xamarin.Forms.Xaml\MarkupExtensions\StaticResourceExtension.cs:33 
  at DezignSpiration.Pages.AddQuotePage.InitializeComponent () [0x00045] in /Users/j.okoroafor/Movies/Projects/Projects/Dezignspiration/DezignSpiration/obj/Debug/netstandard2.0/Pages/AddQuotePage.xaml.g.cs:34 
  at DezignSpiration.Pages.AddQuotePage..ctor () [0x00008] in /Users/j.okoroafor/Movies/Projects/Projects/Dezignspiration/DezignSpiration/Pages/AddQuotePage.xaml.cs:14 
  at (wrapper managed-to-native) System.Reflection.MonoCMethod.InternalInvoke(System.Reflection.MonoCMethod,object,object[],System.Exception&)
  at System.Reflection.MonoCMethod.InternalInvoke (System.Object obj, System.Object[] parameters, System.Boolean wrapExceptions) [0x00005] in <58604b4522f748968296166e317b04b4>:0 
   --- End of inner exception stack trace ---
  at System.Reflection.MonoCMethod.InternalInvoke (System.Object obj, System.Object[] parameters, System.Boolean wrapExceptions) [0x0001a] in <58604b4522f748968296166e317b04b4>:0 
  at System.RuntimeType.CreateInstanceMono (System.Boolean nonPublic, System.Boolean wrapExceptions) [0x000a8] in <58604b4522f748968296166e317b04b4>:0 
  at System.RuntimeType.CreateInstanceSlow (System.Boolean publicOnly, System.Boolean wrapExceptions, System.Boolean skipCheckThis, System.Boolean fillCache, System.Threading.StackCrawlMark& stackMark) [0x00009] in <58604b4522f748968296166e317b04b4>:0 
  at System.RuntimeType.CreateInstanceDefaultCtor (System.Boolean publicOnly, System.Boolean skipCheckThis, System.Boolean fillCache, System.Boolean wrapExceptions, System.Threading.StackCrawlMark& stackMark) [0x00027] in <58604b4522f748968296166e317b04b4>:0 
  at System.Activator.CreateInstance (System.Type type, System.Boolean nonPublic, System.Boolean wrapExceptions) [0x0002c] in <58604b4522f748968296166e317b04b4>:0 
  at System.Activator.CreateInstance (System.Type type, System.Boolean nonPublic) [0x00000] in <58604b4522f748968296166e317b04b4>:0 
  at System.Activator.CreateInstance (System.Type type) [0x00000] in <58604b4522f748968296166e317b04b4>:0 
  at DezignSpiration.Services.NavigationService.CreatePage (System.Type viewModelType) [0x00028] in /Users/j.okoroafor/Movies/Projects/Projects/Dezignspiration/DezignSpiration/Services/NavigationService.cs:95 }
    */
