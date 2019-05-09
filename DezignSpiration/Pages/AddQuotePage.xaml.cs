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
                if (quoteEditor.Text.Length < 10)
                {
                    QuoteErrorLabel.Text = "Can you make it a teeny bit Longer?";
                    QuoteErrorLabel.TextColor = Color.DarkRed;
                }
                else
                {
                    QuoteErrorLabel.Text = "Yeah, this is fine";
                    QuoteErrorLabel.TextColor = Color.DarkGreen;
                }
            }
            hasTypedQuote = true;
        }
    }
}
