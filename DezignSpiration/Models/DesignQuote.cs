using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DezignSpiration.Models
{
    public class DesignQuote : ObservableObject
    {
        private string quote = string.Empty;
        private string author = string.Empty;
        private Color color = new Color();
        private int id;
        private int flagCount;

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

        // Maximum length = 250
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
                OnPropertyChanged();
            }
        }


        [JsonIgnore]
        [ForeignKey(typeof(Color))]
        public int ColorId
        {
            get => Color.Id;
            set
            {
                Color.Id = value;
            }
        }

        [JsonProperty("color")]
        [OneToOne]
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        [Ignore]
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
        [Ignore]
        public double QuoteFontSize => Quote.Length > 150 ? 30 : 35;

    }

    public class DesignQuoteResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public DesignResponseData Data { get; set; }
    }

    public class DesignResponseData
    {
        [JsonProperty("quotes")]
        public ObservableRangeCollection<DesignQuote> Quotes { get; set; }
    }
}
