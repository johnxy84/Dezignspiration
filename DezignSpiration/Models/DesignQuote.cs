using System;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace DezignSpiration.Models
{
    public class DesignQuote : ObservableObject
    {
        private string quote = string.Empty;
        private string author = string.Empty;
        private string authorUrl = string.Empty;
        private string description = string.Empty;
        private Color color = new Color();
        private string descriptionTitle = string.Empty;
        private int id;
        private int flagCount;
        private bool colorsInverted;

        [JsonProperty("id")]
        public int Id
        {
            get => id;
            set {
                id = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("flag_count")]
        public int FlagCount
        {
            get => flagCount;
            set
            {
                flagCount = value;
                OnPropertyChanged();
            }
        }

        // MAximum length = 250
        [JsonProperty("quote")]
        public string Quote
        {
            get => quote;
            set
            {
                quote = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("author")]
        public string Author
        {
            get => author;
            set
            {
                author = value;
                author = string.IsNullOrWhiteSpace(value) ? "Anonymous" : value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("authorUrl")]
        public string AuthorUrl
        {
            get => authorUrl;
            set
            {
                authorUrl = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("description_title")]
        public string DescriptionTitle
        {
            get => descriptionTitle;
            set
            {
                descriptionTitle = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("description")]
        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        [JsonProperty("colors_inverted")]
        public bool ColorsInverted
        {
            get => colorsInverted;
            set
            {
                colorsInverted = value;
                OnPropertyChanged();
            }
        }


        [JsonProperty("color")]
        public Color Color
        {
            get => color;
            set
            {
                if(ColorsInverted)
                {
                    color = new Color
                    {
                        PrimaryColor = value.SecondaryColor,
                        SecondaryColor = value.PrimaryColor
                    };
                }
                else
                {
                    color = value;
                }
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public Dictionary<string, string> ColorMap
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "stroke: #FF33FF;", $"stroke: {Color.SecondaryColor};" }
                };

            }
        }

        [JsonIgnore]
        public double QuoteFontSize { 
            get => Quote.Length > 150 ? 30 : 35;
        }

        [JsonIgnore]
        public bool HasDescription => !string.IsNullOrWhiteSpace(Description);

    }
}
