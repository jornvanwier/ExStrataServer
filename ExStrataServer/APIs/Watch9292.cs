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

namespace ExStrataServer.APIs
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
        }

        protected override async void Check(object Sender = null, EventArgs e = null)
        {
            string data = await Request.GetDataAsync("https://api.9292.nl/0.1/locations/station-leeuwarden/departure-times?lang=en-GB");

            JObject parsedData;
            if (ExtensionMethods.Extensions.TryParseJObject(data, out parsedData))
            {
                JToken departures = parsedData["tabs"]["departures"];

                foreach (JToken departure in departures)
                {
                    //Bus is at destination
                    if ((string)departure["destinationName"] == Destination && (string)departure["time"] == DateTime.Now.ToString("HH:mm"))
                    {
                        Send();
                    }
                }
            }
            else Log.Error("Could not parse 9292OV data");
        }
    }
}
