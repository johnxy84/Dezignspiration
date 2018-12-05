using System;
using Newtonsoft.Json;

namespace DezignSpiration.Models
{
    public class Color: ObservableObject
    {
        private string primaryColor = "#ffffff"; // White
        private string secondaryColor = "#000000"; // Black
        private int id;

        [JsonProperty("id")]
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("primary_color")]
        public string PrimaryColor
        {
            get => primaryColor;
            set
            {
                primaryColor = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("secondary_color")]
        public string SecondaryColor
        {
            get => secondaryColor;
            set
            {
                secondaryColor = value;
                OnPropertyChanged();
            }
        }
    }
}
