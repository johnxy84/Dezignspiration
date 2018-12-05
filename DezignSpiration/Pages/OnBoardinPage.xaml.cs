using System;
using System.Collections.Generic;

using Xamarin.Forms;
using DezignSpiration.Helpers;
using DezignSpiration.Models;

namespace DezignSpiration.Pages
{
    public partial class OnBoardinPage : ContentPage
    {
        List<BoxView> indicators = new List<BoxView>();
        List<OnBoardItem> onBoardTexts = new List<OnBoardItem> {
                new OnBoardItem {
                    Text = "Daily Inspirational quotes to Fire up your creative juices!",
                    Color = "Blue"
                },
                new OnBoardItem {
                    Text = "Share cool and meaningful quotes with your peers and socials",
                    Color = "Red",
                },
        };

        public OnBoardinPage()
        {

            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            SetupPageIndicators();
            SelectVisualState(0);

            Something.ItemsSource = onBoardTexts;
            Something.ItemSelected += OnboardPageChanged;
        }

        void SetupPageIndicators()
        {
            foreach (var text in onBoardTexts)
            {
                var boxView = new BoxView();
                IndicatorsLayout.Children.Add(boxView);
                indicators.Add(boxView);
            }
        }
        void GetStartedClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HomePage());
        }

        void OnboardPageChanged(object sender, SelectedItemChangedEventArgs e)
        {
            if (sender is CarouselView pages)
            {
                SelectVisualState(pages.Position);
            }
        }

        void SelectVisualState(int pageIndex)
        {
            for (int i = 0; i < indicators.Count; i++)
            {
                // Reset all to their normal state
                VisualStateManager.GoToState(indicators[i], "UnSelected");
            }
            try
            {
                VisualStateManager.GoToState(indicators[pageIndex], "Selected");
            }
            catch (Exception ex)
            {
                Utils.LogError(ex, "OnBoardPageChanging");
            }
        }

    }
}
