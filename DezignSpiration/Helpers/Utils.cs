using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;
using DezignSpiration.Models;
using Microsoft.AppCenter.Crashes;
using System.IO;
using System.Linq;

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
            ObservableRangeCollection<T> randomList = new ObservableRangeCollection<T>(collection.OrderBy(item => App.Random.Next(0, collection.Count)).ToList());

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
            return Shuffle(new ObservableRangeCollection<DesignQuote>
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
                },
                new DesignQuote {
                    Id = 13,
                    Quote = "Personas Are Useful, But Scenarios Are More Useful.",
                    Author = "Jared M Spool",
                    Color = new Color {
                        Id = 14,
                        PrimaryColor = "#272B3A",
                        SecondaryColor = "#FFBF25"
                    }
                },
                new DesignQuote {
                    Id = 14,
                    Quote = "In design, we need to remember that our goal is to solve problems, not create them.",
                    Author = "Tiffany Eaton",
                    Color = new Color {
                        Id = 6,
                        PrimaryColor = "#ED962B",
                        SecondaryColor = "#984098"
                    }
                },
                new DesignQuote {
                    Id = 15,
                    Quote = "The beauty of starting with vision is it plays to our strengths. Great UX design leaders are inherently great storytellers. The vision is a great story.",
                    Author = "Jared M Spool",
                    Color = new Color {
                        Id = 1,
                        PrimaryColor = "#FDD32B",
                        SecondaryColor = "#06070D"
                    }
                },
                new DesignQuote {
                    Id = 16,
                    Quote = "If you are to succeed, you have to get end users excited and enabled even if they aren’t the ones paying for it. And product education plays a major role in that.",
                    Author = "Misha Abasov",
                    Color = new Color {
                        Id = 13,
                        PrimaryColor = "#18288D",
                        SecondaryColor = "#FFC13D"
                    }
                },
                new DesignQuote {
                    Id = 17,
                    Quote = "Gaining a better understanding of how something works is vital to doing your best work.",
                    Author = "Andrew Couldwell",
                    Color = new Color {
                        Id = 14,
                        PrimaryColor = "#272B3A",
                        SecondaryColor = "#FFBF25"
                    }
                },
                new DesignQuote {
                    Id = 18,
                    Quote = "Don’t use blue for non-link text, even if you don’t use blue as your link color. Blue is still the color with the strongest perceived affordance of clickability.",
                    Author = "Josh Byrne",
                    Color = new Color {
                        Id = 11,
                        PrimaryColor = "#334E4C",
                        SecondaryColor = "#FFE074"
                    }
                },
                new DesignQuote {
                    Id = 19,
                    Quote = "When designers only focus on delightful animations, humorous copywriting or fun interaction, they are missing deeper, more important aspects of user experience. This is what we call Surface Delight.",
                    Author = "Aaron Walter",
                    Color = new Color {
                        Id = 11,
                        PrimaryColor = "#334E4C",
                        SecondaryColor = "#FFE074"
                    }
                },
                new DesignQuote {
                    Id = 20,
                    Quote = "Whether a design is functional, decorative, or carries a message, Form and Function explores the relationship between the visual form and its purpose. This relationship helps us explore and make better decisions.",
                    Author = "Eleana Gkogka",
                    Color = new Color {
                        Id = 16,
                        PrimaryColor = "#393D50",
                        SecondaryColor = "#F7F6F4"
                    }
                },
                new DesignQuote {
                    Id = 21,
                    Quote = "Help people find solutions; don’t tell them what to do: Telling people what you would have done doesn’t necessarily help them.",
                    Author = "Stuart Norrie",
                    Color = new Color {
                        Id = 16,
                        PrimaryColor = "#393D50",
                        SecondaryColor = "#F7F6F4"
                    }
                },

            });
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
            return Shuffle(new ObservableRangeCollection<Color> {
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
                },
                new Color {
                    Id = 6,
                    PrimaryColor = "#ED962B",
                    SecondaryColor = "#984098"
                },
                new Color {
                    Id = 11,
                    PrimaryColor = "#334E4C",
                    SecondaryColor = "#FFE074"
                },
                new Color {
                    Id = 13,
                    PrimaryColor = "#18288D",
                    SecondaryColor = "#FFC13D"
                },
                new Color {
                    Id = 14,
                    PrimaryColor = "#272B3A",
                    SecondaryColor = "#FFBF25"
                },
                new Color {
                    Id = 16,
                    PrimaryColor = "#393D50",
                    SecondaryColor = "#F7F6F4"
                },
            });
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

        public static TimeSpan GetTimeToScheduleNotification(TimeSpan scheduledTime)
        {
            // Convert both time to seconds
            double currentTimeInSeconds = DateTime.Now.TimeOfDay.TotalSeconds;
            double scheduledTimeInSeconds = scheduledTime.TotalSeconds;
            // Total seconds in a day
            double maxTimeInSeconds = 86400;

            double requiredTime = currentTimeInSeconds > scheduledTimeInSeconds
                ? (maxTimeInSeconds - currentTimeInSeconds) + scheduledTimeInSeconds
                : scheduledTimeInSeconds - currentTimeInSeconds;

            return TimeSpan.FromSeconds(requiredTime);
        }

        /// <summary>
        /// Should Help decide if you should show an annoying message ti the user.
        /// This was put because I couldn't think of anoter way to display messages "occasionally" without being annoying
        /// </summary>
        /// <returns><c>boolean</c>,
        public static bool ShouldShowAnnoyingMessage => App.Random.Next(1, 20) % 3 == 0;

        public static string GetHexString(this Xamarin.Forms.Color color)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            var alpha = (int)(color.A * 255);
            var hex = $"#{alpha:X2}{red:X2}{green:X2}{blue:X2}";

            return hex;
        }

    }
}
