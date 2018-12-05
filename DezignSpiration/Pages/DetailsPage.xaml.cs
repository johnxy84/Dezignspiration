using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace DezignSpiration.Pages
{
    public partial class DetailsPage : ContentPage
    {
        public DetailsPage()
        {
            InitializeComponent();
        }

        void Close_Button_Tapped(object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}
