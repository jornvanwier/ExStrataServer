using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

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

            WebRequest request = WebRequest.Create("https://api.9292.nl/0.1/locations/station-leeuwarden/departure-times?lang=en-GB");

            using (WebResponse response = request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();

                using (string sok = "test")
                {

                }
            }


            base.Check(Sender, e);
        }
    }
}
