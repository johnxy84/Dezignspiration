using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;
using DezignSpiration.Models;
using Microsoft.AppCenter.Crashes;
using System.IO;

namespace DezignSpiration.Helpers
{
    public static class Utils
    {
        /// <summary>
        /// Shuffle the array.
        /// </summary>
        /// <typeparam name="T">Array element type.</typeparam>
        /// <param name="collection">Collection to shuffle.</param>
        public static ObservableRangeCollection<T> Shuffle<T>(ObservableRangeCollection<T> collection)
        {
            ObservableRangeCollection<T> randomList = new ObservableRangeCollection<T>();

            int randomIndex = 0;
            while (collection.Count > 0)
            {
                randomIndex = App.Random.Next(0, collection.Count);
                randomList.Add(collection[randomIndex]);
                collection.RemoveAt(randomIndex);
            }

            return randomList;
        }

        public static void TrackEvent(string eventName = "Error", params string[] extraParams)
        {
            Dictionary<string, string> eventParams = new Dictionary<string, string>();
            for (int i = 0; i < extraParams.Length; i++)
            {
                eventParams.Add($"Param {i}", extraParams[i]);
            }
            Analytics.TrackEvent(eventName, eventParams);
        }

        public static ObservableRangeCollection<DesignQuote> GetDefaultQuotes()
        {
            return new ObservableRangeCollection<DesignQuote>
            {
                new DesignQuote {
                    Id = 1,
                    Quote = "The designer does not begin with preconceived idea." +
                        " Rather, the idea is the result of careful study and observation and the design is a product of that idea.",
                    Author = "Paul Rand",
                    Color = new Color {
                        Id = 5,
                        PrimaryColor = "#222060",
                        SecondaryColor = "#EC192B"
                    },
                },
                new DesignQuote {
                    Id = 2,
                    Quote = "Recognizing the need is the primary condition for design.",
                    Author = "Charles Eames",
                    Color = new Color {
                        Id = 1,
                        PrimaryColor = "#FDD32B",
                        SecondaryColor = "#06070D"
                    },
                },
                new DesignQuote {
                    Id = 3,
                    Quote = "Design is concerned with how things work, how they are controlled and the nature of the interaction between" +
                        " people and technology. When done well, the results are briliiant, pleasurable products.",
                    Author = "Don Norman",
                    Color = new Color {
                        Id = 4,
                        PrimaryColor = "#3C2F2F",
                        SecondaryColor = "#FFF1E6"
                    }
                },
                new DesignQuote {
                    Id = 4,
                    Quote = "A great product isn't just a collection of features. It's how it all works together.",
                    Author = "Tim Cook",
                    Color = new Color {
                        Id = 2,
                        PrimaryColor = "#0091E5",
                        SecondaryColor = "#FAECAA"
                    }
                },
                new DesignQuote {
                    Id = 5,
                    Quote = "It's through Mistakes that you can grow. You have to get bad in order to get good.",
                    Author = "Paula Scher",
                    Color = new Color{
                        Id = 1,
                        PrimaryColor = "#FDD32B",
                        SecondaryColor = "#06070D"
                    }
                },
                new DesignQuote {
                    Id = 6,
                    Quote = "Any intelligent fool can make things bigger and more complex." +
                        " It takes a touch of genius- and a lot of courage- to move in the opposite direction.",
                    Author = "E. F Schumacher",
                    Color = new Color{
                        Id = 2,
                        PrimaryColor = "#0091E5",
                        SecondaryColor = "#FAECAA"
                    }
                },
                new DesignQuote {
                    Id = 7,
                    Quote = "If you think Good design is expensive, you should look at the cost of Bad Design.",
                    Author = "Ralph Speth",
                    Color = new Color{
                        Id = 5,
                        PrimaryColor = "#222060",
                        SecondaryColor = "#EC192B"
                    }

                },
                new DesignQuote {
                    Id = 8,
                    Quote = "Good design is Obvious. Great design is Transparent.",
                    Author = "Joe Soparano",
                    Color = new Color{
                        Id = 3,
                        PrimaryColor = "#2B463C",
                        SecondaryColor = "#B1D182"
                    }

                },
                new DesignQuote {
                    Id = 9,
                    Quote = "The challenge is about taking things that are complex and making them simpler" +
                        " and more understandable.",
                    Author = "Robert Greenberg",
                    Color = new Color {
                        Id = 4,
                        PrimaryColor = "#3C2F2F",
                        SecondaryColor = "#FFF1E6"
                    }

                },
                new DesignQuote {
                    Id = 10,
                    Quote = "I strive for two things in design: simplicity and clarity. Great design is born of those two things.",
                    Author = "Lindo Leader",
                    Color = new Color {
                        Id = 2,
                        PrimaryColor = "#0091E5",
                        SecondaryColor = "#FAECAA"
                    }

                },
                new DesignQuote {
                    Id = 11,
                    Quote = "Great design will not sell an inferior product, but it will enable a great product to achieve it's maximum potential.",
                    Author = "Thomas Watson, Jr",
                    Color = new Color {
                        Id = 1,
                        PrimaryColor = "#FDD32B",
                        SecondaryColor = "#06070D"
                    }
                }
            };
        }

        public static ObservableRangeCollection<FlagReason> GetDefaultFlagReasons()
        {
            return new ObservableRangeCollection<FlagReason>
            {
                new FlagReason
                {
                    Id = 1,
                    Reason = "I/They did not Say this"
                },
                new FlagReason
                {
                    Id = 2,
                    Reason = "This is Offensive"
                },
                new FlagReason
                {
                    Id = 3,
                    Reason = "This needs more context"
                },
                new FlagReason
                {
                    Id = 4,
                    Reason = "This does not make sense"
                },
                new FlagReason
                {
                    Id = 5,
                    Reason = "Something Else"
                },
            };
        }

        public static ObservableRangeCollection<Color> GetDefaultColors()
        {
            return new ObservableRangeCollection<Color> {
                new Color {
                    Id = 1,
                    PrimaryColor = "#FDD32B",
                    SecondaryColor = "#06070D"
                },
                new Color {
                    Id = 2,
                    PrimaryColor = "#0091E5",
                    SecondaryColor = "#FAECAA"
                },
                new Color {
                    Id = 3,
                    PrimaryColor = "#2B463C",
                    SecondaryColor = "#B1D182"
                },
                new Color {
                    Id = 4,
                    PrimaryColor = "#3C2F2F",
                    SecondaryColor = "#FFF1E6"
                },
                new Color {
                    Id = 5,
                    PrimaryColor = "#222060",
                    SecondaryColor = "#EC192B"
                }
            };
        }

        public static void LogError(Exception ex, params string[] extraParams)
        {
            Dictionary<string, string> crashProperties = new Dictionary<string, string>();
            for (int i = 0; i < extraParams.Length; i++)
            {
                crashProperties.Add($"Action {i}", extraParams[i]);
            }
            Crashes.TrackError(ex, crashProperties);
        }

        /// <summary>
        /// Gets the quote to display.
        /// Same quote or a new one
        /// </summary>
        /// <returns>The quote to display.</returns>
        public static int GetCurrentDisplayIndex()
        {
            // Check if date is same day else should show a new quote
            bool isNewValue = (DateTime.Today - Settings.CurrentDate).Days >= 1;

            // Increment current index before using it
            return isNewValue ? ++Settings.CurrentIndex : Settings.CurrentIndex;
        }

        public static RGBColor ExtractRGBFromHex(string hexColor)
        {
            return new RGBColor
            {
                Red = Convert.ToInt32(hexColor.Substring(1, 2), 16),
                Green = Convert.ToInt32(hexColor.Substring(3, 2), 16),
                Blue = Convert.ToInt32(hexColor.Substring(5, 2), 16)
            };
        }

        public static string DatabasePath
        {
            get
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "dezignspiration.db");
                return path;
            }
        }

        /// <summary>
        /// Should Help decide if you should show an annoying message ti the user.
        /// This was put because I couldn't think of anoter way to display messages "occasionally" without being annoying
        /// </summary>
        /// <returns><c>boolean</c>,
        public static bool ShouldShowAnnoyingMessage => App.Random.Next(1, 20) % 3 == 0;
    }
}
