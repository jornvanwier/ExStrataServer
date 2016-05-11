﻿using System;
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
            string data = Request.GetData("https://api.9292.nl/0.1/locations/station-leeuwarden/departure-times?lang=en-GB");

            try
            {
                JObject parsedData = JObject.Parse(data);

                JToken departures = parsedData["tabs"]["departures"];

                foreach(JToken departure in departures)
                {
                    //Bus is at destination
                    if ((string)departure["destinationName"] == Destination && (string)departure["time"] == DateTime.Now.ToString("HH:mm"))
                    {
                        Send();
                    }
                }
            } 
            catch (Exception exception)
            {
                Log.AddError("Could not parse 9292OV data: " + exception.Message);
            }
        }
    }
}
