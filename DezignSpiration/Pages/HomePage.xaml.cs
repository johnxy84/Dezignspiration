using System;
using System.Collections.Generic;
using DezignSpiration.ViewModels;
using Xamarin.Forms;
using DezignSpiration.Helpers;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DezignSpiration.Pages
{
    public partial class HomePage : CarouselPage
    {
        readonly HomeViewModel homeViewModel;

        public HomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            homeViewModel = new HomeViewModel(Navigation);

            BindingContext = homeViewModel;
            MessagingCenter.Subscribe<HomeViewModel, QuotePage>(this, Constants.PAGE_UPDATED_KEY, (sender, page) =>
            {
                Children.Add(page);
                CurrentPage = page;
            });
            homeViewModel.InitializePage();
        }


        protected override bool OnBackButtonPressed()
        {
            var helper = DependencyService.Get<IHelper>();
            helper.BackButtonPressed();
            return true;
        }

        async void PageChanged(object sender, EventArgs e)
        {
            var carouselPage = sender as CarouselPage;
            var firstPage = carouselPage.Children.FirstOrDefault();
            if (firstPage != null && carouselPage.Children.Count > 1)
            {
                // Allow for carousel page to slide
                await Task.Delay(500);
                carouselPage.Children.Remove(firstPage);
            }
        }
    }
}
