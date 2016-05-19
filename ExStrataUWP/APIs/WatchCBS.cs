using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ExStrataUWP.APIs;
using ExStrataUWP.Communication;
using ExStrataUWP.ColourPattern;

namespace ExStrataUWP.APIs
{
    class WatchCBS : APIWatcher
    {
        private static string name = "CBS";

        public WatchCBS(int delay) : base(delay, name)
        {
        }

        protected override async void Check(object Sender = null, EventArgs e = null)
        {
            if (true || DateTime.Now.Hour == 13 && DateTime.Now.Minute == 55)
            {
                JObject result;
                string URI = String.Format("https://cbs.nl/nl-nl/visualisaties/bevolkingsteller/-/media/cbs/Infographics/Bevolkingsteller/{0}_{1}_{2}.json", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                string JSON = "{'data':" + await Request.GetDataAsync(URI) + "}";
                if (ExtensionMethods.Extensions.TryParseJObject(JSON, out result))
                {
                    int born = 0,
                        died = 0;

                    JToken stats = result["data"];
                    for (int i = 1; i < stats.Count(); i++)
                    {
                        int delta = (int)stats[i] - (int)stats[i - 1];
                        if (delta > 0)
                        {
                            born += delta;
                        }
                        else if (delta < 0)
                        {
                            died -= delta;
                        }
                    }

                    int babyRings = born / 25;

                    Pattern babyGradient = new Pattern("Babies", 60 * 1000);
                    babyGradient.Add(Frame.Gradient(new Frame.GradientColour[]
                    {
                        new Frame.GradientColour(0, Colour.Pink),
                        new Frame.GradientColour(100, Colour.Maroon)
                    }, 0, babyRings));

                    Send(babyGradient);
                }
            }
            else
            {
                Log.Error("Could not parse CBS data");
                return;
            }
        }
    }
}
