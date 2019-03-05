using System;
using System.Collections.Generic;
using Xamarin.Forms;
using DezignSpiration.Helpers;
using DezignSpiration.ViewModels;
using DezignSpiration.Models;

namespace DezignSpiration.Pages
{
    public partial class OnBoardingPage : ContentPage
    {
        List<BoxView> indicators = new List<BoxView>();


        OnBoardingViewModel onBoardingViewModel;

        public OnBoardingPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = onBoardingViewModel = App.ViewModelLocator.OnBoardingViewModel;
            SetupPageIndicators();
            SelectVisualState(0);
            Settings.IsFirstTime = false;
            MessagingCenter.Subscribe<OnBoardingViewModel, int>(this, Constants.ONBOARDING_PAGE_CHANGED, (sender, position) =>
            {
                var x = Carousel;
                SelectVisualState(position);
            });
        }

        void SetupPageIndicators()
        {
            foreach (var text in onBoardingViewModel.OnBoardingItems)
            {
                var boxView = new BoxView();
                IndicatorsLayout.Children.Add(boxView);
                indicators.Add(boxView);
            }
        }

        //void OnboardPageChanged(object sender, SelectedPositionChangedEventArgs e)
        //{
        //    //if (sender is Xamarin.Forms.CarouselView pages)
        //    //{
        //    //    SelectVisualState(pages.Position);
        //    //}
        //}

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
