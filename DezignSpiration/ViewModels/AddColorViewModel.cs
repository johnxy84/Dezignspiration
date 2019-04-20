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
        private readonly Regex colorRegex = new Regex(Constants.COLOR_REGEX);
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

                await colorsRepository.AddColor(Color, deviceId.ToString());

                Helper?.ShowAlert("Your color would be added soon just because it's Awesome!", false);
                Utils.TrackEvent("ColorAdded");
                await Navigation.GoBackAsync(isModal: true);
            }
            catch (Exception ex)
            {
                Utils.LogError(ex, "Adding QuoteError");
                Helper?.ShowAlert($"There was an issue adding your color", true, false, "Try again", async (choice) =>
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
