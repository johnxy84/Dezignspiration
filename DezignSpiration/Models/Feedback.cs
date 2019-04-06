using System;
using Newtonsoft.Json;
namespace DezignSpiration.Models
{
    public class Feedback : ObservableObject
    {
        private string feedbackContent = string.Empty;
        private string category = string.Empty;
        private string contact = string.Empty;

        [JsonProperty("feedback")]
        public string FeedbackContent
        {
            get => feedbackContent; set
            {
                feedbackContent = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("category")]
        public string Category
        {
            get => Category; set
            {
                category = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("contact")]
        public string Contact
        {
            get => contact;
            set
            {
                contact = value;
                OnPropertyChanged();
            }
        }

    }
}
