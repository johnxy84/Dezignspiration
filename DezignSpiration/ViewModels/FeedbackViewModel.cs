using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;
using DezignSpiration.Helpers;
using DezignSpiration.Models;
using DezignSpiration.Interfaces;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace DezignSpiration.ViewModels
{
    public class FeedbackViewModel : BaseViewModel
    {
        private int selectedCategory;
        private Feedback feedback = new Feedback();
        private readonly INetworkClient client;

        public List<string> Categories => new List<string>
        {
            "Design", "Performance", "Feature Request", "Other"
        };

        public Feedback Feedback
        {
            get => feedback;
            set
            {
                if (SetProperty(ref feedback, value))
                {
                    OnPropertyChanged();
                }
            }
        }

        public int SelectedCategory
        {
            get => selectedCategory;
            set
            {
                if (SetProperty(ref selectedCategory, value))
                {
                    OnPropertyChanged();
                }
            }
        }

        public Command SubmitFeedbackCommand { get; }

        public FeedbackViewModel(INetworkClient client)
        {
            SubmitFeedbackCommand = new Command(async () => await SubmitFeedback());
            this.client = client;
        }

        public override Task InitializeAsync(object navigationData)
        {
            return base.InitializeAsync(navigationData);
        }

        async Task SubmitFeedback()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Feedback.FeedbackContent))
                {
                    Helper?.ShowAlert("Can you check again? You seem to be missing something");
                    return;
                }

                IsBusy = true;
                var deviceId = await Microsoft.AppCenter.AppCenter.GetInstallIdAsync();

                var payload = new StringContent(JsonConvert.SerializeObject(new
                {
                    feedback = Feedback.FeedbackContent,
                    category = Categories[SelectedCategory],
                    contact = Feedback.Contact,
                    device_id = deviceId
                }), Encoding.UTF8, "application/json");

                var result = await client.Post("/api/v1/feedback", payload);
                if (result.IsSuccessStatusCode)
                {
                    Helper?.ShowAlert("Thanks for your feedback, we'll be reviewing it");
                    await Navigation.GoBackAsync(isModal: true);
                }
                else
                {
                    var content = await result.Content.ReadAsStringAsync();
                    throw new Exception(content);
                }
            }
            catch (Exception ex)
            {
                Helper?.ShowAlert("There was an issue sending your Feedback, Please try again");
                Utils.LogError(ex, "ErrorSubmittingFeedback");
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
