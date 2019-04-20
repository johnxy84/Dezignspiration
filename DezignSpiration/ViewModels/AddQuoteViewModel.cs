using System;
using System.Threading.Tasks;
using DezignSpiration.Models;
using Xamarin.Forms;
using DezignSpiration.Helpers;
using DezignSpiration.Interfaces;

namespace DezignSpiration.ViewModels
{
    public class AddQuoteViewModel : BaseViewModel
    {
        DesignQuote designQuote = new DesignQuote();
        Models.Color selectedColor = new Models.Color();
        ObservableRangeCollection<Models.Color> colors = new ObservableRangeCollection<Models.Color>();
        private readonly IColorsRepository colorsRepository;
        private readonly IQuotesRepository quotesRepository;
        private bool isInitialized;
        private bool isAnonymous;

        public Command AddQuoteCommand { get; }

        public DesignQuote DesignQuote
        {
            get => designQuote;
            set
            {
                designQuote = value;
                OnPropertyChanged();
            }
        }

        public bool CanSubmit => IsNotBusy && DesignQuote.Quote.Length >= 10;

        public Models.Color SelectedColor
        {
            get => selectedColor;
            set
            {
                selectedColor = value;
                designQuote.Color.Id = value.Id;
                OnPropertyChanged();
            }
        }

        public ObservableRangeCollection<Models.Color> Colors
        {
            get => colors;
            set
            {
                colors = value;
                OnPropertyChanged();
            }
        }

        public bool IsAnonymous
        {
            get => isAnonymous;
            set
            {
                if (SetProperty(ref isAnonymous, value))
                {
                    isAnonymous = value;
                    OnPropertyChanged();
                }
            }
        }


        public AddQuoteViewModel(IColorsRepository colorsRepository, IQuotesRepository quotesRepository)
        {
            AddQuoteCommand = new Command(async () => await SubmitQuote());
            this.colorsRepository = colorsRepository;
            this.quotesRepository = quotesRepository;
        }


        public override async Task InitializeAsync(object navigationData)
        {
            DesignQuote = new DesignQuote
            {
                Quote = navigationData as string ?? string.Empty
            };
            if (!isInitialized)
            {
                Colors = new ObservableRangeCollection<Models.Color>(await colorsRepository.GetAllColors());
                isInitialized = true;
            }
            await RefreshColors();
            await base.InitializeAsync(navigationData);
        }

        async Task SubmitQuote()
        {
            try
            {
                if (!CanSubmit)
                {
                    Helper?.ShowAlert("Your quote doesn't seem correct, please check again");
                    return;
                }
                IsBusy = true;

                var deviceId = await Microsoft.AppCenter.AppCenter.GetInstallIdAsync();

                bool added = await quotesRepository.AddQuote(DesignQuote, IsAnonymous, deviceId.ToString());

                if (added)
                {
                    Helper?.ShowAlert("Thanks for your awesome quote! You'll be seeing it soon", true);
                    Utils.TrackEvent("QuoteAdded");
                    await Navigation.GoBackAsync(isModal: true);
                }
                else
                {
                    Helper?.ShowAlert("There was an issue uploading your quote, Please try again Later", true, false, "Try again", async (choice) =>
                    {
                        await SubmitQuote();
                    });
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex, "AddngQuoteError");
                Helper?.ShowAlert($"There was an issue uploading your quote: {ex.Message}", true, false, "Try again", async (choice) =>
                {
                    await SubmitQuote();
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task RefreshColors()
        {
            if (!Settings.ShouldRefreshColors) return;
            var freshColors = await colorsRepository.GetFreshColors();
            if (freshColors != null)
            {
                Colors.AddRange(freshColors);
                await colorsRepository.InsertColors(freshColors);
            }
        }
    }
}
