using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
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
            


            base.Check(Sender, e);
        }
    }
}
