using System;
using ExStrataServer.ColourPattern;
using ExStrataServer.Communication;
using Newtonsoft.Json.Linq;

namespace ExStrataServer.APIs
{
    class WatchWeather : APIWatcher
    {
        private const string name = "Weather";
        private const string description = "Bekijk het weer in Leeuwarden, en laat dit elk kwartier zien. Het aantal rijen van lampen dat aan staat geeft de temperatuur aan, op een schaal van -5 tot 30";

        private string city;
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        private string country;
        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        public WatchWeather()
        {
            Name = name;
            Description = description;
            duration = 60;
        }

        public WatchWeather(int delay, string country, string city, int patternDuration = 60) : base(delay, name, description, patternDuration)
        {
            DisplayDelay = "15 minuten op de klok";
            Country = country;
            City = city;
            InstanceInfo = city;
        }

        protected override async void Check(object Sender = null, EventArgs e = null)
        {
            base.Check();

            if (DateTime.Now.Minute%15 != 0) return;
            JObject result;
            if (Utilities.TryParseJObject(await Request.GetDataAsync("http://api.wunderground.com/api/009779345fb40d94/conditions/q/" + Country + "/" + City + ".json"), out result))
            {
                float temperatureC = (float)result["current_observation"]["temp_c"];

                Send(GetPattern(temperatureC));
            }
            else
            {
                Log.Error("Could not parse weather data");
            }
        }

        public override Pattern GetPattern()
        {
            return GetPattern(35);
        }

        private Pattern GetPattern(float temperatureC)
        {
            int temperatureRings = (int)((temperatureC + 5) / 35 * 80);
            Pattern temperatureGradient = new Pattern("Temperature", duration);
            temperatureGradient.Add(Frame.Gradient(new[]
            {
                        new Frame.GradientColour(0, new Colour(0,200,220)),
                        new Frame.GradientColour(50, new Colour(255,200,0)),
                        new Frame.GradientColour(100, new Colour(255,70,0))
            }, 0, temperatureRings));

            return temperatureGradient;
        }
    }
}
