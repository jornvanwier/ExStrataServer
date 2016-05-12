using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ExStrataServer.Communication;
using ExStrataServer.ColourPattern;

namespace ExStrataServer.APIs
{
    class WatchWeather : APIWatcher
    {
        private static string name = "Weather";

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

        public WatchWeather(int delay, string country, string city) : base(delay, name)
        {
            Country = country;
            City = city;
        }

        protected override void Check(object Sender = null, EventArgs e = null)
        {
            if (DateTime.Now.Minute % 15 == 0)
            {
                JObject result;
                if (ExtensionMethods.Extensions.TryParseJObject(Request.GetData("http://api.wunderground.com/api/009779345fb40d94/conditions/q/" + Country + "/" + City + ".json"), out result))
                {
                    float temperatureC = (float)result["current_observation"]["temp_c"];

                    int temperatureRings = (int)((temperatureC + 5) / 35 * 80);

                    Pattern temperaturepGradient = new Pattern("Temperature", 60 * 1000);
                    temperaturepGradient.Add(Gradient.GetFrame(new Gradient.GradientColour[]
                    {
                    new Gradient.GradientColour(0, new Colour(0,200,220)),
                    new Gradient.GradientColour(50, new Colour(255,200,0)),
                    new Gradient.GradientColour(100, new Colour(255,70,0))
                    }, 0, temperatureRings));
                    Console.WriteLine(temperaturepGradient.ToString());
                }
                else
                {
                    Log.AddError("Could not parse weather data: ");
                    return;
                }
            }
        }
    }
}
