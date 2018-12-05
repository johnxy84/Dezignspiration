using DezignSpiration.Helpers;
using DezignSpiration.Models;
using DezignSpiration.Pages;
using Xamarin.Forms;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DezignSpiration.ViewModels
{
    public class QuoteViewModel : BaseViewModel
    {
        private DesignQuote currentQuote;

        public DesignQuote CurrentQuote
        {
            get => currentQuote;
            set
            {
                currentQuote = value; OnPropertyChanged();
            }
        }

        public Command SeeMoreCommand { get; }
        public Command NextCommand { get; }
        public Command SettingsCommand { get; }
        public Command ShareCommand { get; }
        public Command FlagCommand { get; }
        public Command AddCommand { get; }

        public QuoteViewModel(INavigation navigation, DesignQuote quote) : base(navigation)
        {
            NextCommand = new Command(GoNext);
            SeeMoreCommand = new Command(SeeMore);
            SettingsCommand = new Command(ViewSettings);
            ShareCommand = new Command(ShareQuote);
            FlagCommand = new Command(FlagQuote);
            AddCommand = new Command(AddClicked);
            CurrentQuote = quote;
        }

        void AddClicked()
        {
            const string addQuote = "Add Quote";
            const string addColor = "Add Color";

            Helper?.ShowOptions(string.Empty, new string[] {addQuote, addColor}, (choice) => {
                switch (choice)
                {
                    case addQuote:
                        Navigation.PushModalAsync(new AddQuotePage());
                        break;
                    case addColor:
                        Navigation.PushModalAsync(new AddColorPage());
                        break;
                }
            });
        }

        void FlagQuote()
        {
            Helper?.DisplayMessage("Flag Quote", "This quote would be flagged and submitted for review", "Continue", "Cancel", (shouldFlag) => {
                if(shouldFlag)
                {
                    Settings.QuotesData.Remove(currentQuote);
                    GoToNextQuote();
                    Helper?.ShowAlert("Thanks for keeping an eye out for us. This we will be reviewing this quote");
                    Task.Run(async () =>
                    {
                        try
                        {
                            await App.NetworkClient.Update($"/v1/quotes/{currentQuote.Id}", new { flag_count = currentQuote.FlagCount + 1 });
                        }
                        catch (System.Exception ex)
                        {
                            Utils.LogError(ex, "FlaggingPostError", JsonConvert.SerializeObject(currentQuote));
                        }
                    });
                }
            });
        }


        void ShareQuote()
        {
            Helper?.ShareQuote(CurrentQuote);
        }

        void GoNext()
        {
            Helper?.DisplayMessage("", "Please Don't ruin the fun, Just wait for tommorow", "Ruin the Fun", "Don't ruin the fun", (choice) =>
            {
                if (choice)
                {
                    GoToNextQuote();
                }
                Utils.TrackEvent("NextQuoteNotClicked");

            });
        }

        void GoToNextQuote()
        {
            //Set Current date to Yesterday to force it to get a new Value
            var newDate = Settings.CurrentDate.AddDays(-1);
            Settings.CurrentDate = newDate;
            MessagingCenter.Send(this, Constants.UPDATE_PAGE_KEY);
            Utils.TrackEvent("NextQuoteClicked");
        }

        void SeeMore()
        {
            var detailPage = new DetailsPage()
            {
                BindingContext = currentQuote
            };
            Navigation.PushModalAsync(detailPage);
        }

        void ViewSettings()
        {
            Navigation.PushModalAsync(new SettingsPage());
        }

    }
}
