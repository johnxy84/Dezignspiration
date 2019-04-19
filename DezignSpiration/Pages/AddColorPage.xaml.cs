using System;
using Xamarin.Forms;
using System.Text.RegularExpressions;
using DezignSpiration.Helpers;

namespace DezignSpiration.Pages
{
    public partial class AddColorPage : ContentPage
    {
        private readonly Regex colorRegex = new Regex(@"^[A-Fa-f0-9]{6}|[A-Fa-f0-9]{3}$");
        private bool hasTypedPrimaryColor;
        private bool hasTypedSecondaryColor;

        public AddColorPage()
        {
            InitializeComponent();
            BindingContext = App.ViewModelLocator.AddColorViewModel;
            Utils.TrackEvent("AddColorPage");
        }

        void PrimaryTextCompleted(object sender, EventArgs e)
        {
            if (sender is Entry primaryEntry && hasTypedPrimaryColor)
            {
                if (!colorRegex.IsMatch(primaryEntry.Text))
                {
                    PrimaryErrorLabel.Text = "Please type a valid hex color code";
                    PrimaryErrorLabel.TextColor = Color.DarkRed;
                }
                else
                {
                    PrimaryErrorLabel.Text = "We're good to go here";
                    PrimaryErrorLabel.TextColor = Color.DarkGreen;
                }
            }
            hasTypedPrimaryColor = true;
        }

        void SecondaryTextCompleted(object sender, EventArgs e)
        {
            if (sender is Entry secondaryEntry && hasTypedSecondaryColor)
            {
                if (!colorRegex.IsMatch(secondaryEntry.Text))
                {
                    SecondaryErrorLabel.Text = "Please type a valid hex color code";
                    SecondaryErrorLabel.TextColor = Color.DarkRed;
                }
                else
                {
                    SecondaryErrorLabel.Text = "We're good to go here";
                    SecondaryErrorLabel.TextColor = Color.DarkGreen;
                }
            }

            hasTypedSecondaryColor = true;
        }
    }
}
