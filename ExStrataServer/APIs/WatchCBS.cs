﻿using System;
using System.Linq;
using ExStrataServer.ColourPattern;
using ExStrataServer.Communication;
using Newtonsoft.Json.Linq;

namespace ExStrataServer.APIs
{
    class WatchCBS : APIWatcher
    {
        private const string name = "CBS";
        private const string description = "Laat 1 keer per dag zien hoeveel baby's er die dag zijn geboren. Het aantal rijen van lampen dat aan staat geeft het aantal geboren babies die dag aan, op een schaal van 0 tot 2000.";

        public WatchCBS()
        {
            Name = name;
            duration = 120;
        }

        public WatchCBS(int delay, int patternDuration = 120) : base(delay, name, description, patternDuration)
        {
        }

        protected override async void Check(object Sender = null, EventArgs e = null)
        {
            base.Check();

            if (DateTime.Now.Hour == 13 && DateTime.Now.Minute == 55)
            {
                JObject result;
                string URI =
                    $"https://cbs.nl/nl-nl/visualisaties/bevolkingsteller/-/media/cbs/Infographics/Bevolkingsteller/{DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day}.json";
                string JSON = "{'data':" + await Request.GetDataAsync(URI) + "}";
                if (Utilities.TryParseJObject(JSON, out result))
                {
                    int born = 0,
                        died = 0;

                    JToken stats = result["data"];
                    for (int i = 1; i < stats.Count(); i++)
                    {
                        int delta = (int)stats[i] - (int)stats[i - 1];

                        if (delta > 0)
                            born += delta;

                        else if (delta < 0)
                            died -= delta;
                    }
                    
                    Send(GetPattern(born));
                }
                else
                {
                    Log.Error("Could not parse CBS data");
                }
            }
        }

        public override Pattern GetPattern()
        {
            return GetPattern(1300);
        }

        private Pattern GetPattern(int born)
        {
            int babyRings = born / 25;

            Pattern babyGradient = new Pattern("Babies", duration);
            babyGradient.Add(Frame.Gradient(new[]
            {
                        new Frame.GradientColour(0, Colour.Pink),
                        new Frame.GradientColour(100, Colour.Maroon)
            }, 0, babyRings));

            return babyGradient;
        }
    }
}
