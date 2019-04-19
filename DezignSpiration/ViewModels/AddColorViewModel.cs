using System;
using Xamarin.Forms;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DezignSpiration.Helpers;
using DezignSpiration.Interfaces;

namespace DezignSpiration.ViewModels
{
    public class AddColorViewModel : BaseViewModel
    {
        private readonly Regex colorRegex = new Regex(@"^[A-Fa-f0-9]{6}|[A-Fa-f0-9]{3}$");
        private readonly IColorsRepository colorsRepository;
        private Models.Color color = new Models.Color();

        private string primaryColor = string.Empty;
        private string secondaryColor = string.Empty;

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

        public bool CanSubmit
        {
            get
            {
                return IsNotBusy && colorRegex.IsMatch(PrimaryColor) && colorRegex.IsMatch(SecondaryColor);
            }
        }

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

        public AddColorViewModel(IColorsRepository colorsRepository)
        {
            AddColorCommand = new Command(async () => await SubmitColor());
            this.colorsRepository = colorsRepository;
        }

        public override Task InitializeAsync(object navigationData)
        {
            PrimaryColor = string.Empty;
            SecondaryColor = string.Empty;
            return base.InitializeAsync(navigationData);
        }

        async Task SubmitColor()
        {
            try
            {
                if (!CanSubmit)
                {
                    Helper?.ShowAlert("Please fill in valid Hex Colors");
                    return;
                }

                IsBusy = true;
                var deviceId = await Microsoft.AppCenter.AppCenter.GetInstallIdAsync();

                var added = await colorsRepository.AddColor(Color, deviceId.ToString());
                if (added)
                {
                    Helper?.ShowAlert("Your color would be added soon just because it's Awesome!", false);
                    Utils.TrackEvent("ColorAdded");
                    await Navigation.GoBackAsync(isModal: true);
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
