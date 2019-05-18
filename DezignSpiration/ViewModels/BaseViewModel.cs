using System;
using System.Threading.Tasks;
using DezignSpiration.Helpers;
using DezignSpiration.Models;
using Xamarin.Forms;
using DezignSpiration.Interfaces;
using CommonServiceLocator;
using Constants = DezignSpiration.Helpers.Constants;
using DezignSpiration.Services;

namespace DezignSpiration.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        private bool isBusy;
        private bool isNotBusy = true;
        protected IHelper Helper;
        private readonly IQuotesRepository quotesRepository;
        private readonly IColorsRepository colorsRepository;

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

        public Command GoBackCommand { get; }
        public INavigationService Navigation { get; set; }

        #endregion

        public BaseViewModel()
        {
            GoBackCommand = new Command(() =>
            {
                Navigation.GoBackAsync(isModal: true);
            });
            Navigation = ServiceLocator.Current.GetInstance<INavigationService>();
            quotesRepository = ServiceLocator.Current.GetInstance<IQuotesRepository>();
            colorsRepository = ServiceLocator.Current.GetInstance<IColorsRepository>();
            Helper = DependencyService.Get<IHelper>();
        }

        /// <summary>
        /// Fetches new quotes quotes.
        /// <paramref name="shouldAllowRetry"/>
        /// </summary>
        public async Task<bool> GetFreshQuotes(bool shouldAllowRetry = true)
        {
            Settings.LastRetryTime = DateTime.Now;
            try
            {
                // Get new Colors before dragging down new quotes to prevent db constraint errors
                await colorsRepository.GetFreshColors();
                var newQuotes = await quotesRepository.GetFreshQuotes();
                if (newQuotes != null)
                {
                    Settings.ShouldRetryQuotes = false;
                    MessagingCenter.Send(QuotesAdded.Message, Constants.QUOTES_ADDED_KEY, newQuotes);
                    Helper?.ShowAlert("Yassss. You just got some fresh, new quotes. :-)", true);
                    await quotesRepository.InsertQuotes(newQuotes);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Settings.ShouldRetryQuotes = true;
                Utils.LogError(ex, "RefreshQuotesException");

                Helper?.ShowAlert("We couldn't get more quotes, we'll try again Later", true, false, shouldAllowRetry ? "Try now" : string.Empty, async (obj) =>
                {
                    if (shouldAllowRetry)
                    {
                        await GetFreshQuotes();
                    }
                });

                return false;
            }
            finally
            {
                DI.NotificationService.ClearNotifications();
                Helper?.SetScheduledNotifications(NotificationService.Notifications);
            }
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}