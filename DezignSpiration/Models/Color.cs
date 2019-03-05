using Newtonsoft.Json;
using SQLite;

namespace DezignSpiration.Models
{
    public class Color : ObservableObject
    {
        private string primaryColor = "#ffffff"; // White
        private string secondaryColor = "#000000"; // Black
        private int id;

        [JsonProperty("id")]
        [PrimaryKey]
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

    public class ColorsResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public ColorResponseData Data { get; set; }
    }

    public class ColorResponseData
    {
        [JsonProperty("colors")]
        public ObservableRangeCollection<Color> Colors { get; set; }
    }
}
