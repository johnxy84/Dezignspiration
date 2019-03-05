using System;
using System.Threading.Tasks;
using DezignSpiration.ViewModels;

namespace DezignSpiration.Interfaces
{
    public interface INavigationService
    {

        Task InitializeAsync();

        Task NavigateToAsync<TViewModel>(bool isModal = false, object parameter = null) where TViewModel : BaseViewModel;

        Task GoBackAsync(bool isModal = false);

    }
}
