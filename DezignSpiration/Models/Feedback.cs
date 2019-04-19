using System;
using Newtonsoft.Json;
namespace DezignSpiration.Models
{
    public class Feedback : ObservableObject
    {
        private string feedbackContent = string.Empty;
        private string contact;

        [JsonProperty("feedback")]
        public string FeedbackContent
        {
            get => feedbackContent; set
            {
                feedbackContent = value;
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
