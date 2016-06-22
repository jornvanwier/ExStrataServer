using System;
using ExStrataServer.ColourPattern;
using ExStrataServer.Communication;
using Newtonsoft.Json.Linq;

namespace ExStrataServer.APIs
{
    class WatchDemo : APIWatcher
    {
        private const string name = "Demo";
        private const string description = "Simpele APIWatcher om de laten zien hoe een nieuwe aangemaakt kan worden.";

        public WatchDemo()
        {
            Name = name;
            Description = description;
            duration = 60;
        }

        public WatchDemo(string parameter, int delay, int duration) : base(delay, name, description, duration)
        {
            InstanceInfo = parameter;    
        }

        protected async override void Check(object sender = null, EventArgs e = null)
        {
            string data = await Request.GetDataAsync("https://api.example.com/0.1/parameter/data/");

            JObject parsedData;
            if (Utilities.TryParseJObject(data, out parsedData))
            {
                JToken selection = parsedData["key"];

                if ((string)selection == "demo")
                    Send();
            }
            else Log.Error("Could not parse demo data");
        }


        public override Pattern GetPattern()
        {
            return Pattern.Animate(new[]
            {
                new Pattern.GradientFrame(0, Frame.Gradient(new []
                {
                    new Frame.GradientColour(0, Colour.Rebeccapurple),
                    new Frame.GradientColour(100, Colour.Palegoldenrod)
                })),
                new Pattern.GradientFrame(100, new Frame(Colour.Beige))
            }, "Demo", duration);
        }
    }
}
