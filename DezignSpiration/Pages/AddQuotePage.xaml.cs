using System;
using Xamarin.Forms;
using DezignSpiration.Controls;

namespace DezignSpiration.Pages
{
    public partial class AddQuotePage : ContentPage
    {
        private bool hasTypedQuote;

        public AddQuotePage()
        {
            InitializeComponent();
            var addQuoteViewModel = App.ViewModelLocator.AddQuoteViewModel;
            BindingContext = addQuoteViewModel;
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
