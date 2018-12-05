using System;
using System.Threading.Tasks;
using DezignSpiration.Helpers;
using DezignSpiration.Models;
using DezignSpiration.Pages;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace DezignSpiration.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel(INavigation navigation) : base(navigation)
        {
            if (Settings.IsFirstTime)
            {
                // Shuffle Default quotes 
                Utils.ShuffleQuotes();
                Settings.IsFirstTime = false;
            }
            MessagingCenter.Subscribe<QuoteViewModel>(this, Constants.UPDATE_PAGE_KEY, (s) =>
            {
                InitializePage();
            });
            MessagingCenter.Subscribe<App>(this, Constants.NETWORK_AVAILABLE_KEY, async (s) =>
            {
                await GetFreshQuotes();
            });

        }

        /// <summary>
        /// Initializes the page.
        /// </summary>
        public void InitializePage()
        {
            var currentQuote = Utils.GetDisplayQuote();
            var page = new QuotePage
            {
                BindingContext = new QuoteViewModel(Navigation, currentQuote)
            };
            MessagingCenter.Send(this, Constants.PAGE_UPDATED_KEY, page);

            // Check if notifications have been set and set them if they should be set
            Helper?.SetScheduledNotifications();
            UpdateQuotes();

        }

        /// <summary>
        /// Updates the current quotes if need be.
        /// </summary>
        void UpdateQuotes()
        {
            int quotesLeft = Settings.QuotesData.Count - Settings.CurrentIndex;
            bool isLastQuote = Settings.CurrentIndex >= Settings.QuotesData.Count - 1;

            if (isLastQuote)
            {
                Utils.ShuffleQuotes();
                Task.Run(async () =>
                {
                    await GetFreshQuotes();
                });
            }
            else if (quotesLeft < 5 && Settings.IsTimeToRefresh)
            {
                Task.Run(async () =>
                {
                    await GetFreshQuotes();
                });
            }
        }

        /// <summary>
        /// Fetches new quotes quotes.
        /// </summary>
        public async Task GetFreshQuotes()
        {
            try
            {

                //await Task.Delay(5000);
                var result = await App.NetworkClient.Get($"/v1/list/quotes?offset={Settings.QuotesData.Count}&limit={Constants.MAX_FETCH_QUOTE}");
                var response = await result.Content.ReadAsStringAsync();
                var quotesData = JsonConvert.DeserializeObject<ObservableRangeCollection<DesignQuote>>(response);

                //Got data, do the needfuls
                if (quotesData != null)
                {
                    Settings.ShouldRefresh = false;
                    Settings.QuotesData.AddRange(quotesData);

                    // If Quotes has already been refreshed, set index to lates quotes
                    if (Settings.ShouldRefresh)
                    {
                        Settings.CurrentIndex = Settings.QuotesData.Count - Constants.MAX_FETCH_QUOTE;
                    }
                    Helper?.ShowAlert("Yassss. You just got some fresh, new quotes. :-)");
                }
            }
            catch (Exception ex)
            {
                Settings.ShouldRefresh = true;
                Utils.LogError(ex, "RefreshCquotes");
                Helper?.ShowAlert("Aww, we couldn't refresh quotes, we'll try again Later", true, false, "Try now", async (obj) =>
                {
                    await GetFreshQuotes();
                });
            }
            finally
            {
                NotificationUtils.ClearNotifications();
                Helper?.SetScheduledNotifications();
            }
        }
    }
}