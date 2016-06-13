using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ExStrataServer.Communication;
using ExStrataServer.ColourPattern;

namespace ExStrataServer.APIs
{
    public class Watch9292OV : APIWatcher
    {
        private string destination;
        private const string name = "9292OV";
        private const string description = "Kijkt of er een bus aan komt. Als het patroon afgespeeld wordt, betekent het dat er over één minuut een bus aan komt.";

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        public Watch9292OV() : base()
        {
            Name = name;
        }

        public Watch9292OV(int delay, string destination, int patternDuration = 30) : base(delay, name, description, patternDuration)
        {
            Destination = destination;
            pattern = GetPattern();
            InstanceInfo = destination;
        }

        protected override async void Check(object Sender = null, EventArgs e = null)
        {
            base.Check();

            string data = await Request.GetDataAsync("https://api.9292.nl/0.1/locations/station-leeuwarden/departure-times?lang=en-GB");

            JObject parsedData;
            if (Utilities.TryParseJObject(data, out parsedData))
            {
                JToken departures = parsedData["tabs"][1]["departures"];

                foreach (JToken departure in departures)
                {
                    //Bus is at destination
                    string busDestination = (string)departure["destinationName"];
                    string departureTime = (string)departure["time"];
                    string timeNow = DateTime.Now.AddMinutes(1).ToString("HH:mm");
                    //Console.WriteLine("departuretime {0} timenow {1}, destination {2}", departureTime, timeNow, busDestination);
                    if (busDestination == Destination && departureTime == timeNow)
                    {
                        Send();
                    }
                }
            }
            else Log.Error("Could not parse 9292OV data");
        }

        public override Pattern GetPattern()
        {
            return Pattern.Animate(new[]{
                            new Pattern.GradientFrame(0, Frame.Gradient(new[]
                            {
                                new Frame.GradientColour(0, Colour.Blue),
                                new Frame.GradientColour(100, Colour.Blueviolet)
                            })),
                            new Pattern.GradientFrame(100, Frame.Gradient(new[]
                            {
                                new Frame.GradientColour(0, Colour.Lightblue),
                                new Frame.GradientColour(100, Colour.Blue)
                            }))
                        }, "Animate", duration);
        }
    }
}
