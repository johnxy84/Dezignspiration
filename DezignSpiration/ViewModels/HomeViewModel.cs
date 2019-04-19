using System;
using System.Threading.Tasks;
using DezignSpiration.Helpers;
using DezignSpiration.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using DezignSpiration.Interfaces;
using System.Linq;

namespace DezignSpiration.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private ObservableRangeCollection<DesignQuote> quotes = new ObservableRangeCollection<DesignQuote>();
        private int currentIndex = Utils.GetCurrentDisplayIndex();
        private readonly IQuotesRepository quotesRepository;
        private bool canSwipe = Settings.SwipeCount < Constants.MAX_SWIPE_COUNT;

        public ObservableRangeCollection<DesignQuote> Quotes
        {
            get => quotes;
            set
            {
                SetProperty(ref quotes, value);
            }
        }

        public int CurrentIndex
        {
            get => currentIndex;
            set
            {
                UpdateSwipeAbility(currentIndex, value);
                if (SetProperty(ref currentIndex, value))
                {
                    Settings.CurrentIndex = currentIndex = value;
                    UpdateQuotesCollection();
                    OnPropertyChanged();
                }
            }
        }

        public bool CanSwipe
        {
            get
            {
                return canSwipe;
            }
            set
            {
                if (SetProperty(ref canSwipe, value))
                {
                    OnPropertyChanged();
                }
            }
        }

        public Command SettingsCommand { get; }
        public Command ShareCommand { get; }
        public Command FlagCommand { get; }
        public Command AddCommand { get; }

        public HomeViewModel(IQuotesRepository quotesRepository)
        {
            SettingsCommand = new Command(ViewSettings);
            ShareCommand = new Command(ShareQuote);
            FlagCommand = new Command(FlagQuote);
            AddCommand = new Command(AddClicked);
            this.quotesRepository = quotesRepository;
        }

        /// <summary>
        /// Initializes the page.
        /// </summary>
        public override async Task InitializeAsync(object navigationData)
        {
            try
            {
                SubscribeToEvents();
                LoadStoredQuotes();
                Helper?.SetScheduledNotifications(App.notificationService.Notifications);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex, "InitializeHomeViewModelError");
                await Navigation.NavigateToAsync<CrashViewModel>();
            }
            await base.InitializeAsync(navigationData);
        }

        private void LoadStoredQuotes()
        {
            IsBusy = true;
            Task.Run(async () =>
            {
                var storedQuotes = await quotesRepository.GetAllQuotes();
                Device.BeginInvokeOnMainThread(() =>
                {
                    Quotes = new ObservableRangeCollection<DesignQuote>(storedQuotes);
                });
            });
            IsBusy = false;
        }

        private void SubscribeToEvents()
        {
            MessagingCenter.Subscribe<NetworkAvailable>(this, Constants.NETWORK_AVAILABLE_KEY, async (s) =>
            {
                await GetFreshQuotes();
            });

            MessagingCenter.Subscribe<QuotesAdded, ObservableRangeCollection<DesignQuote>>(this, Constants.QUOTES_ADDED_KEY, (sender, quotes) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Quotes.AddRange(quotes, System.Collections.Specialized.NotifyCollectionChangedAction.Reset);
                });
            });

            MessagingCenter.Subscribe<SwipeToggled, bool>(this, Constants.SWIPE_TOGGLED, (sender, isSwipeEnabled) =>
            {
                CanSwipe = isSwipeEnabled;
                Settings.SwipeCount = isSwipeEnabled ? 0 : Settings.SwipeCount;
            });

        }

        private void UpdateSwipeAbility(int oldValue, int newValue)
        {
            //Reset swipe count if it's past wait time and should reset
            // Worst case Scenario if for some weird reason, countdown didn't fire 
            if ((DateTime.Now - Settings.SwipeDisabledDate).Hours > Constants.HOURS_TILL_COOL_DOWN && Settings.SwipeCount >= Constants.MAX_SWIPE_COUNT)
            {
                Settings.SwipeCount = 0;
            }

            if (newValue > oldValue)
            {
                Settings.SwipeCount++;
            }
            else if (newValue < oldValue)
            {
                Settings.SwipeCount--;
            }

            switch (Settings.SwipeCount)
            {
                case Constants.MAX_SWIPE_COUNT / 2:
                    if (Utils.ShouldShowAnnoyingMessage)
                    {
                        Helper?.ShowAlert($"Slow down champ, You've got {Constants.MAX_SWIPE_COUNT / 2} more forward swipes today", isLongAlert: true);
                    }
                    break;
                case Constants.MAX_SWIPE_COUNT:
                    Helper?.ShowAlert("You've maxed out your swipes. Try taking a breath of fresh air and come back later", isLongAlert: true);
                    Settings.SwipeDisabledDate = DateTime.Now;
                    Helper?.BeginSwipeEnableCountdown(Constants.HOURS_TILL_COOL_DOWN);
                    CanSwipe = false;
                    break;
            }
        }

        /// <summary>
        /// Updates the current quotes if need be.
        /// </summary>
        void UpdateQuotesCollection()
        {
            int quotesLeft = Quotes.Count - Settings.CurrentIndex;
            bool isLastQuote = Settings.CurrentIndex >= Quotes.Count - 1;

            if (isLastQuote && Quotes.Count != 0 && Utils.ShouldShowAnnoyingMessage)
            {
                Helper?.ShowAlert("Uhmm we're getting you more quotes. Take a step back for now :-)", false);
            }

            if (quotesLeft < 5 && Settings.ShouldRefreshQuotes)
            {
                Task.Run(async () =>
                {
                    await GetFreshQuotes();
                });
            }
        }

        void AddClicked()
        {
            const string addQuote = "Add Quote";
            const string addColor = "Add Color";

            Utils.TrackEvent("AddClicked");

            Helper?.ShowOptions(string.Empty, new string[] { addQuote, addColor }, (choice) =>
            {
                switch (choice)
                {
                    case addQuote:
                        Navigation.NavigateToAsync<AddQuoteViewModel>(isModal: true);
                        break;
                    case addColor:
                        Navigation.NavigateToAsync<AddColorViewModel>(isModal: true);
                        break;
                }
            });
        }

        void FlagQuote()
        {
            Helper?.ShowOptions("Why are you flagging this quote?", Settings.FlagReasons.Select(flagReason => flagReason.Reason).ToArray(), choice =>
            {
                if (choice == null) return;
                var flaggedQuote = Quotes[CurrentIndex];
                Quotes.RemoveAt(CurrentIndex);
                Task.Run(async () =>
                {
                    int flagReasonId = Settings.FlagReasons.FirstOrDefault(flagReason => flagReason.Reason == (string)choice).Id;

                    Helper?.ShowAlert("Thanks for keeping an eye out for us. We'll be reviewing this quote", isLongAlert: true);
                    try
                    {
                        await quotesRepository.DeleteQuote(flaggedQuote);
                        await quotesRepository.FlagQuote(flaggedQuote, flagReasonId);
                        Settings.FlagedQuoteIds.Add(flaggedQuote.Id);
                        Utils.TrackEvent("QuoteFlagged", choice.ToString());
                    }
                    catch (Exception ex)
                    {
                        Utils.LogError(ex, "FlaggingPostError", JsonConvert.SerializeObject(flaggedQuote), flagReasonId.ToString());
                    }
                });
            });
        }

        void ShareQuote()
        {
            bool isLongQuote = Quotes[currentIndex].Quote.Length > Constants.MAX_QUOTE_LENGTH;
            if (!Settings.HasShownLengthyQuoteWarning && isLongQuote)
            {
                Helper?.DisplayMessage(string.Empty, "We can't generate an Image because the quote is too long, we'll be sharing it as text instead. :-)", "OK", string.Empty, (s) =>
                {
                    Helper?.ShareQuote(Quotes[CurrentIndex], isLongQuote);
                });
                Settings.HasShownLengthyQuoteWarning = true;
            }
            else
            {
                Helper?.ShareQuote(Quotes[CurrentIndex], isLongQuote);
            }
            Utils.TrackEvent("QuoteShared");

        }

        void ViewSettings()
        {
            Navigation.NavigateToAsync<SettingsViewModel>(isModal: true);
        }
    }
}