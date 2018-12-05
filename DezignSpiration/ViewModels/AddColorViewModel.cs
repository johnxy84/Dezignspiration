using System;
using Xamarin.Forms;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DezignSpiration.Helpers;

namespace DezignSpiration.ViewModels
{
    public class AddColorViewModel : BaseViewModel
    {
        private readonly Regex colorRegex = new Regex(@"^[A-Fa-f0-9]{6}|[A-Fa-f0-9]{3}$");

        private Models.Color color = new Models.Color();

        private string primaryColor = String.Empty;
        private string secondaryColor = String.Empty;

        public string PrimaryColor
        {
            get => primaryColor;
            set
            {
                if (colorRegex.IsMatch(value))
                {
                    primaryColor = value;
                    Color.PrimaryColor = $"#{value}";
                    OnPropertyChanged();
                }
                OnPropertyChanged(nameof(CanSubmit));
            }
        }

        public string SecondaryColor
        {
            get => secondaryColor;
            set
            {
                if (colorRegex.IsMatch(value))
                {
                    secondaryColor = value;
                    Color.SecondaryColor = $"#{value}";
                    OnPropertyChanged();
                }
                OnPropertyChanged(nameof(CanSubmit));
            }
        }

        public bool CanSubmit => IsNotBusy && colorRegex.IsMatch(PrimaryColor) && colorRegex.IsMatch(SecondaryColor);

        public Models.Color Color
        {
            get => color;
            set
            {
                color = value;
                OnPropertyChanged();
            }
        }

        public Command AddColorCommand { get; }
        public Command GoBackCommand { get; }

        public AddColorViewModel(INavigation navigation) : base(navigation)
        {
            AddColorCommand = new Command(async () => await SubmitColor());
            GoBackCommand = new Command(() => { Navigation.PopModalAsync(); });
        }

        async Task SubmitColor()
        {
            try
            {
                if (!CanSubmit)
                {
                    Helper?.ShowAlert("Please fill in valid Hex Colors", false);
                    return;
                }

                IsBusy = true;
                        
                // For Development Purposes
                await Task.Delay(3000);
                Helper?.ShowAlert("Your color is being reviewed and would be added soon just because it's Awesome!", false);
                await Navigation.PopModalAsync();
                return;

                var response = await App.NetworkClient.Post("/v1/colors", new
                {
                    primary_color = Color.PrimaryColor,
                    secondary_color = Color.SecondaryColor
                });

                if (response.IsSuccessStatusCode)
                {
                    Helper?.ShowAlert("Your color is being reviewed and would be added soon just because it's Awesome!", false);
                    await Navigation.PopModalAsync(true);
                }
                else
                {
                    Helper?.ShowAlert("There was an issue adding your color, Please try again Later", true, false, "Try again", async (choice) =>
                    {
                        await SubmitColor();
                    });
                }
            }
            catch (Exception ex)
            {
                Utils.LogError(ex, "Adding QuoteError");
                Helper?.ShowAlert($"There was an issue adding your color: {ex.Message}", true, false, "Try again", async (choice) =>
                {
                    await SubmitColor();
                });
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
