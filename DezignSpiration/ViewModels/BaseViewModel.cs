using System;
using System.Threading.Tasks;
using DezignSpiration.Helpers;
using DezignSpiration.Models;
using Xamarin.Forms;

namespace DezignSpiration.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        private bool isBusy;
        private bool isNotBusy = true;
        protected IHelper Helper;

        #region Properties

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (SetProperty(ref isBusy, value))
                    IsNotBusy = !isBusy;
            }
        }

        public bool IsNotBusy
        {
            get => isNotBusy; 
            set
            {
                if (SetProperty(ref isNotBusy, value))
                    IsBusy = !isNotBusy;
            }
        }

        public INavigation Navigation { get; set; }

        #endregion


        public BaseViewModel(INavigation navigation = null)
        {
            Navigation = navigation;
            Helper = DependencyService.Get<IHelper>();
        }


        public async Task PushModalAsync(Page page)
        {
            if (Navigation != null)
                await Navigation.PushModalAsync(page);
        }

        public async Task PopModalAsync()
        {
            if (Navigation != null)
                await Navigation.PopModalAsync();
        }

        public async Task PushAsync(Page page)
        {
            if (Navigation != null)
                await Navigation.PushAsync(page);
        }

        public async Task PopAsync()
        {
            if (Navigation != null)
                await Navigation.PopAsync();
        }

    }
}

