using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ExStrataServer.Communication;

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

        public WatchWeather(double delay, string country, string city) : base(delay, name)
        {
            Country = country;
            City = city;
        }

        protected override void Check(object Sender = null, EventArgs e = null)
        {
            try
            {
                JObject result = JObject.Parse(Request.GetData("http://api.wunderground.com/api/009779345fb40d94/conditions/q/" + Country + "/" + City + ".json"));
                int temp = (int)result["current_observation"]["temp_c"];
            }
            catch (Exception exception)
            {
                Log.AddError("Could not parse weather data: " + exception.Message);
                throw;
            }
        }
    }
}
