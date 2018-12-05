using System;
using System.Threading.Tasks;
using DezignSpiration.Models;
using Xamarin.Forms;
using DezignSpiration.Helpers;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace DezignSpiration.ViewModels
{
    public class AddQuoteViewModel : BaseViewModel
    {
        DesignQuote designQuote = new DesignQuote();
        Models.Color selectedColor = new Models.Color();
        ObservableCollection<Models.Color> colors = new ObservableCollection<Models.Color>();
        bool colorInverted;

        public Command AddQuoteCommand { get; }
        public Command GoBackCommand { get; }

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
            get =>  selectedColor;
            set
            {
                selectedColor = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Models.Color> Colors
        {
            get => colors;
            set
            {
                colors = value;
                OnPropertyChanged();
            }
        }


        public AddQuoteViewModel(INavigation navigation) : base(navigation)
        {
            AddQuoteCommand = new Command(async () => await SubmitQuote());
            GoBackCommand = new Command(() => { Navigation.PopModalAsync(); });

            Task.Run(async () => await RefreshColors());
        }

        async Task SubmitQuote()
        {
            try
            {
                if(!CanSubmit)
                {
                    Helper?.ShowAlert("Please check that your quote is valid", false);
                    return;
                }
                IsBusy = true;

                // For Development Purposes
                await Task.Delay(3000);
                Helper?.ShowAlert("Your quote is being reviewed and would be added to our quotes if it's OK", false);
                await Navigation.PopModalAsync();
                return;

                var response = await App.NetworkClient.Post("v1/quotes", new
                {
                    color_id = DesignQuote.Color.Id,
                    quote = DesignQuote.Quote,
                    description_title = DesignQuote.DescriptionTitle,
                    description = DesignQuote.Description,
                    author = DesignQuote.Author,
                });

                if (response.IsSuccessStatusCode)
                {
                    Helper?.ShowAlert("Your quote is being reviewed and would be added to our quotes if it's OK", false);
                    await Navigation.PopModalAsync(true);
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
            try
            {
                IsBusy = true;
                // Check if date has passed to get new colors
                //if (Settings.ShouldRefreshColors)
                //{
                    var response = await App.NetworkClient.Get($"v1/list/colors?page={Settings.ColorsData.Count}&limit={Constants.MAX_FETCH_QUOTE}");
                    var content = await response.Content.ReadAsStringAsync();
                    var updatedColors = JsonConvert.DeserializeObject<ObservableRangeCollection<Models.Color>>(content);
                    Colors = Settings.ColorsData = updatedColors;
                //}
                //Colors = Settings.ColorsData;
            }
            catch (Exception ex)
            {
                Utils.LogError(ex, "ErrorFetchingColors");
                Colors = Settings.ColorsData;
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
