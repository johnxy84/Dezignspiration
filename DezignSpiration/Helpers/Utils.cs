using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.AppCenter.Analytics;
using DezignSpiration.Models;
using Microsoft.AppCenter.Crashes;

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
            var shuffledCollection = new ObservableRangeCollection<T>(collection);
            int n = shuffledCollection.Count;
            for (int i = 0; i < n; i++)
            {
                // Use Next on random instance with an argument.
                // ... The argument is an exclusive bound.
                //     So we will not go past the end of the array.
                int r = i + App.Random.Next(n - i);
                T t = shuffledCollection[r];
                shuffledCollection[r] = shuffledCollection[i];
                shuffledCollection[i] = t;
            }

            return shuffledCollection;
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
                    Quote = "The designer does not begin with preconceived idea." +
                        " Rather, the idea is the result of careful study and observation and the design is a product of that idea.",
                    Author = "Paul Rand",
                    DescriptionTitle = "Good design is the result of a process",
                    Description = "Paul Rand was an American graphic designer best known for corporate logo " +
                        "design, like IBM and ABC. A great product is always the result of research and multiple iterations." +
                        " And the idea you have at the beginning, it may seem significant, but it always ends up to be different.",
                    Color = new Color{
                        PrimaryColor = "#3EE0A2",
                        SecondaryColor = "#5248C7"
                    }
                },
                new DesignQuote {
                    Quote = "Recognizing the need is the primary condition for design.",
                    Author = "Charles Eames",
                    DescriptionTitle = "Design satisfies a need",
                    Description = "Charles Eames, a furniture designer, meant that before there is any design, there is a problem" +
                        " that needs to be solved. And great design evolves around it.",
                    Color = new Color{
                        PrimaryColor = "#7A5EE1",
                        SecondaryColor = "#7FF9E3"
                    }
                },
                new DesignQuote {
                    Quote = "Design is concerned with how things work, how they are controlled and the nature of the interaction between" +
                        " people and technology. When done well, the results are briliiant, pleasurable products.",
                    Author = "Don Norman",
                    ColorsInverted = true,
                    DescriptionTitle = "Design is more than how it looks",
                    Description = "The quote was take from Don Norman’s book, The Design of Everyday Things. Knowing how things work will" +
                        " allow you to create human products that can be easily used by anyone.",
                    Color = new Color {
                        PrimaryColor = "#5349C6",
                        SecondaryColor = "#40DFA1"
                    }
                },
                new DesignQuote {
                    Quote = "A great product isn't just a collection of features. It's how it all works together.",
                    Author = "Tim Cook",
                    DescriptionTitle = "What is a great product",
                    Description = "The CEO of Apple tries to continue the mantra that Steve Jobs established during his time at Apple." +
                        " And it is: a product is everything we come in touch with. Whether it is the package, the sounds it makes," +
                        " the smell, etc. We also call it User Experience.",
                    Color = new Color {
                        PrimaryColor = "#9CF8E8",
                        SecondaryColor = "#4C65E0"
                    }
                },
                new DesignQuote {
                    Quote = "It's through Mistakes that you can grow. You have to get bad in order to get good.",
                    Author = "Paula Scher",
                    DescriptionTitle = "Making mistakes",
                    Description = "Paula Scher is an American graphic designer, painter, and educator. Her quote suggests that mistakes" +
                        " are a part of living. You need to make them in order to change.",
                    Color = new Color{
                        PrimaryColor = "#F7A88F",
                        SecondaryColor = "#734FA0"
                    }
                },
                new DesignQuote {
                    Quote = "Any intelligent fool can make things bigger and more complex." +
                        " It takes a touch of genius- and a lot of courage- to move in the opposite direction.",
                    Author = "E. F Schumacher",
                    DescriptionTitle = "It takes courage to makes things simple",
                    Description = "E. F. Schumacher was a German statistician and economist." +
                        " We should not forget that design is also about courage." +
                        " Not everybody is willing to go against the market or what the customers want or need.",
                    Color = new Color{
                        PrimaryColor = "#833072",
                        SecondaryColor = "#EA4379"
                    }
                },
                new DesignQuote {
                    Quote = "If you think Good design is expensive, you should look at the cost of Bad Design.",
                    Author = "Ralph Speth",
                    DescriptionTitle = "The costs are high for a bad design",
                    Description = "Ralf Speth, CEO of Jaguar Land Rover was onto something when said this." +
                        " Even I sometimes wish that people first go through a cheap and lousy design only to " +
                        "realise the importance of a great one. In this fast-changing world, there’s no time to cut corners." +
                        " You may win in the short term, but the marathon is always won by people who invested fully into it.",
                    Color = new Color{
                        PrimaryColor = "#724FA0",
                        SecondaryColor = "#F7A88F"
                    }

                },
                new DesignQuote {
                    Quote = "Good design is Obvious. Great design is Transparent.",
                    Author = "Joe Soparano",
                    DescriptionTitle = "Transparency in design",
                    Description = "Companies have to create products that people want and customers are going" +
                        " to help them make that decision — and that means quality, imagination and transparency.",
                    Color = new Color{
                        PrimaryColor = "#000000",
                        SecondaryColor = "#F8739B"
                    }

                },
                new DesignQuote {
                    Quote = "The challenge is about taking things that are complex and making them simpler" +
                        " and more understandable.",
                    Author = "Robert Greenberg",
                    DescriptionTitle = "Great design is about simplicity",
                    Description = "Mr. Greenberg, the chairman and chief executive of R/GA. Because of advances in technology and " +
                        "communication, we’re surrounded by information we see and hear. Overload is a huge issue. Mr. Greenberg said" +
                        " in an interview for NY Times that things are getting more complex. We get cluttered on a daily basis with new" +
                        " products and solutions and the ultimate goal will be to take all this clutter and simplify it.Create something" +
                        " truly useful that does not beg for your time but comes natural to us.",
                    Color = new Color {
                        PrimaryColor = "#DA608E",
                        SecondaryColor = "#FEC664"
                    }

                },
                new DesignQuote {
                    Quote = "I strive for two things in design: simplicity and clarity. Great design is born of those two things.",
                    Author = "Lindo Leader",
                    ColorsInverted = true,
                    DescriptionTitle = "Simplicity and Clarity",
                    Description = "Lindo Leader is a graphic designer and the creator of the award-winning FedEx logo. This quote " +
                        "is informative and tells you two of the most important things in design. Simplicity & clarity is not achieved" +
                        " that quickly, but through an ongoing iteration until it becomes evident to anyone that it is clear and " +
                        "straightforward. Even to someone without a design background.",
                    Color = new Color {
                        PrimaryColor = "#F7DC7B",
                        SecondaryColor = "#4C65E0"
                    }

                },
                new DesignQuote {
                    Quote = "Great design will not sell an inferior product, but it will enable a great product to achieve it's maximum potential.",
                    Author = "Thomas Watson, Jr",
                    DescriptionTitle = "Good design is a gateway",
                    Description = "Design is what enables great products to take the leap and get noticed from the clutter.",
                    Color = new Color {
                        PrimaryColor = "#FCB63E",
                        SecondaryColor = "#3A4398"
                    }

                }
            };
        }

        /// <summary>
        /// Shuffles the saved quotesData.
        /// </summary>
        public static void ShuffleQuotes()
        {
            Settings.QuotesData = Shuffle(Settings.QuotesData);
            Settings.CurrentIndex = -1;
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
        public static DesignQuote GetDisplayQuote()
        {
            DesignQuote displayQuote = new DesignQuote();

            // Check if date is same day else should show a new quote
            bool isNewValue = (DateTime.Today - Settings.CurrentDate).Days >= 1;

            // Increment current index before using it
            displayQuote = isNewValue ? Settings.QuotesData[++Settings.CurrentIndex] : Settings.QuotesData[Settings.CurrentIndex];
            Settings.CurrentDate = isNewValue ? DateTime.Today : Settings.CurrentDate;

            return displayQuote;
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
    }
}
