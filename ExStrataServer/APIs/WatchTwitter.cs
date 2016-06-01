﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ExStrataServer.Communication;
using ExStrataServer.ColourPattern;
using System.Text.RegularExpressions;

namespace ExStrataServer.APIs
{
    class WatchTwitter : APIWatcher
    {
        private const string name = "Twitter";
        private const string description = "Blijf up to date met NHL tweets! Als je dit patroon ziet betekent het dat het NHL Twitter account iets heeft gepost.";

        private List<string> previousTweets;

        private string user;
        public string User
        {
            get { return user; }
            set { user = value; }
        }

        public WatchTwitter()
        {
            Name = name;
            Description = description;
            Parameters.Add(new Parameter
            {
                Name = "user",
                Type = "string"
            });


        }

        public WatchTwitter(int delay, string user) : base(delay, name, description)
        {
            User = user;
            previousTweets = new List<string>();
            setTweets();
            DefaultPattern = GetPattern();
        }

        protected override async void Check(object Sender = null, EventArgs e = null)
        {
            base.Check();

            if (true||previousTweets.Count != 0)
            {
                List<string> newTweets = await getTweets();

                if (true || !Enumerable.SequenceEqual(previousTweets, newTweets))
                    Send(); // er is een nieuwe tweet
            }
        }

        private async Task<List<string>> getTweets()
        {
            string tweetsHtml = await Request.GetDataAsync("https://twitter.com/" + User);
            List<string> tweets = tweetsHtml.Split(new string[] { "tweet-text-container" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            tweets = tweets.GetRange(1, 5);

            for (int i = 0; i < tweets.Count; i++)
            {
                tweets[i] = Regex.Replace(
                    tweets[i].Split(
                        new string[] { "data-aria-label-part=\"0\">" },
                        StringSplitOptions.RemoveEmptyEntries)[1]
                    .Split(
                        new string[] { "<div class=\"expanded-content js-tweet-details-dropdown\">" },
                        StringSplitOptions.RemoveEmptyEntries)[0],
                    "<[^>]*>", "").Split('\n')[0];

                Console.WriteLine(tweets[i]);
            }

            return tweets;
        }
        private async void setTweets()
        {
            previousTweets = await getTweets();
        }

        public override Pattern GetPattern()
        {
            Pattern twitterGradient = Pattern.Animate(new Pattern.GradientFrame[]
            {
                new Pattern.GradientFrame(0, Frame.Gradient(new Frame.GradientColour[]
                {
                    new Frame.GradientColour(0, Colour.White),
                    new Frame.GradientColour(25, Colour.Blue),
                    new Frame.GradientColour(50, Colour.White),
                    new Frame.GradientColour(75, Colour.Blue),
                    new Frame.GradientColour(100, Colour.White),
                })),
                new Pattern.GradientFrame(25, Frame.Gradient(new Frame.GradientColour[]
                {
                    new Frame.GradientColour(0, Colour.Blue),
                    new Frame.GradientColour(25, Colour.White),
                    new Frame.GradientColour(50, Colour.Blue),
                    new Frame.GradientColour(75, Colour.White),
                    new Frame.GradientColour(100, Colour.Blue),
                })),
                new Pattern.GradientFrame(50, Frame.Gradient(new Frame.GradientColour[]
                {
                    new Frame.GradientColour(0, Colour.White),
                    new Frame.GradientColour(25, Colour.Blue),
                    new Frame.GradientColour(50, Colour.White),
                    new Frame.GradientColour(75, Colour.Blue),
                    new Frame.GradientColour(100, Colour.White),
                })),
                new Pattern.GradientFrame(75, Frame.Gradient(new Frame.GradientColour[]
                {
                    new Frame.GradientColour(0, Colour.Blue),
                    new Frame.GradientColour(25, Colour.White),
                    new Frame.GradientColour(50, Colour.Blue),
                    new Frame.GradientColour(75, Colour.White),
                    new Frame.GradientColour(100, Colour.Blue),
                })),
                new Pattern.GradientFrame(100, Frame.Gradient(new Frame.GradientColour[]
                {
                    new Frame.GradientColour(0, Colour.White),
                    new Frame.GradientColour(25, Colour.Blue),
                    new Frame.GradientColour(50, Colour.White),
                    new Frame.GradientColour(75, Colour.Blue),
                    new Frame.GradientColour(100, Colour.White),
                }))
            }, "TwitterPattern", 4000, 14);



            return twitterGradient;
        }
    }
}