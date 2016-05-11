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

        public Watch9292(double delay, string destination) : base(delay, name)
        {
            Destination = destination;
        }

        protected override void Check(object Sender = null, EventArgs e = null)
        {
            //TODO: Actually check the API

            string data = Request.GetData("https://api.9292.nl/0.1/locations/station-leeuwarden/departure-times?lang=nl-NL");

            try
            {
                JObject parsedData = JObject.Parse(data);
            } 
            catch (Exception exception)
            {
                Log.AddError("Could not parse 9292OV data: " + exception.Message);
            }

            base.Check(Sender, e);
        }
    }
}
