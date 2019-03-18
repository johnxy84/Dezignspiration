using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DezignSpiration.ViewModels
{
    public class FeedbackViewModel : BaseViewModel
    {
        private int selectedCategory;

        public List<string> Categories => new List<string>
        {
            "Design", "Performance", "Feature Request"
        };

        public int SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                OnPropertyChanged();
            }
        }

        public Command SubmitFeedbackCommand { get; }

        public FeedbackViewModel()
        {
            SubmitFeedbackCommand = new Command(async () => await SubmitFeedback());
        }

        public override Task InitializeAsync(object navigationData)
        {
            return base.InitializeAsync(navigationData);
        }

        async Task SubmitFeedback()
        {
            IsBusy = true;
            await Task.Delay(5000);
            IsBusy = false;
        }

    }
}
