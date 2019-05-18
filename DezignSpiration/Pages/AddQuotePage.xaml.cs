using System;
using Xamarin.Forms;
using DezignSpiration.Controls;
using DezignSpiration.Helpers;

namespace DezignSpiration.Pages
{
    public partial class AddQuotePage : ContentPage
    {
        private bool hasTypedQuote;

        public AddQuotePage()
        {
            InitializeComponent();
            BindingContext = DI.ViewModelLocator.AddQuoteViewModel;
            Utils.TrackEvent("AddQuotePage");
        }

        public void QuoteTextChanged(object sender, EventArgs e)
        {
            if (sender is FlexEditor quoteEditor && hasTypedQuote)
            {
                if (quoteEditor.Text.Length < Constants.MIN_QUOTE_LENGTH)
                {
                    QuoteErrorLabel.TextColor = Color.DarkRed;
                }
                else
                {
                    QuoteErrorLabel.TextColor = Color.DarkGreen;
                }
                QuoteErrorLabel.Text = $"{quoteEditor.Text.Length}/{Constants.MAX_QUOTE_LENGTH}";
            }
            hasTypedQuote = true;
        }
    }
}
