using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ExStrataUWP.Communication;
using ExStrataUWP.ColourPattern;


namespace ExStrataUWP.APIs
{
    public class Watch9292 : APIWatcher
    {
        private string destination;
        private static string name = "9292OV";

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        public Watch9292(int delay, string destination) : base(delay, name)
        {
            Destination = destination;
            pattern = Pattern.Animate(new Pattern.GradientFrame[]{
                            new Pattern.GradientFrame(0, Frame.Gradient(new Frame.GradientColour[]
                            {
                                new Frame.GradientColour(0, Colour.Blue),
                                new Frame.GradientColour(100, Colour.Blueviolet)
                            })),
                            new Pattern.GradientFrame(100, Frame.Gradient(new Frame.GradientColour[]
                            {
                                new Frame.GradientColour(0, Colour.Lightblue),
                                new Frame.GradientColour(100, Colour.Blue)
                            }))
                        }, "Animate", 20 / 14 * 1000, 14);
        }

        protected override async void Check(object Sender = null, EventArgs e = null)
        {
            string data = await Request.GetDataAsync("https://api.9292.nl/0.1/locations/station-leeuwarden/departure-times?lang=en-GB");

            JObject parsedData;
            if (ExtensionMethods.Extensions.TryParseJObject(data, out parsedData))
            {
                JToken departures = parsedData["tabs"][1]["departures"];

                foreach (JToken departure in departures)
                {
                    //Bus is at destination
                    string busDestination = (string)departure["destinationName"];
                    string departureTime = (string)departure["time"];
                    string timeNow = DateTime.Now.AddMinutes(1).ToString("HH:mm");
                    if (busDestination == Destination && departureTime == timeNow)
                    {
                        Send();
                    }
                }
            }
            else Log.Error("Could not parse 9292OV data");
        }
    }
}
