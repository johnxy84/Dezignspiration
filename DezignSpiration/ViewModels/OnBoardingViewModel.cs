using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using DezignSpiration.Interfaces;
using DezignSpiration.Helpers;
using System.Collections.Generic;
using DezignSpiration.Models;

namespace DezignSpiration.ViewModels
{
    public class OnBoardingViewModel : BaseViewModel
    {
        private int selectedPosition;
        private readonly IQuotesRepository quotesRepository;
        private readonly IFlagReasonService flagReasonService;
        private readonly IColorsRepository colorsRepository;

        public int SelectedPosition
        {
            get => selectedPosition;
            set
            {
                selectedPosition = value;
                MessagingCenter.Send(this, Constants.ONBOARDING_PAGE_CHANGED, value);
                OnPropertyChanged();
            }
        }

        public List<OnBoardItem> OnBoardingItems
        {
            get
            {
                return new List<OnBoardItem> {
                    new OnBoardItem {
                        Text = "Inspirational quotes to Fire up your Design juices!",
                        Animation = "fireworks.json"
                    },
                    new OnBoardItem {
                        Text = "Light up your Socials with Dezignspirationals",
                        Animation = "fluid_loading_animation.json"
                    },
                    new OnBoardItem {
                        Text = "Get your Design voice heard",
                        Animation = "soft_loading.json"
                    },
                    new OnBoardItem {
                        Text = "Be you or be Anonymous",
                        Animation = "anon.json"
                    }
                };
            }
        }


        public Command GetStartedCommand { get; }

        public OnBoardingViewModel(IColorsRepository colorsRepository, IQuotesRepository quotesRepository, IFlagReasonService flagReasonService)
        {
            GetStartedCommand = new Command(async () => { await Navigation.NavigateToAsync<HomeViewModel>(); });
            this.quotesRepository = quotesRepository;
            this.flagReasonService = flagReasonService;
            this.colorsRepository = colorsRepository;
        }

        public override Task InitializeAsync(object navigationData)
        {
            Task.Run(async () =>
            {
                try
                {
                    await colorsRepository?.InsertColors(Utils.GetDefaultColors());
                    await quotesRepository?.InsertQuotes(Utils.GetDefaultQuotes());
                    await flagReasonService.GetFlagReasons();
                }
                catch (Exception ex)
                {
                    Utils.LogError(ex, "PopulatingDefaultData");
                }
            });
            return base.InitializeAsync(navigationData);
        }
    }
}
