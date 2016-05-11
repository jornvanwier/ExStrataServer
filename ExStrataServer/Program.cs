using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExStrataServer.ColourPattern;
using ExStrataServer.APIs;
using ExStrataServer.Communication;

namespace ExStrataServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Add("Piet", "Sok");
            //WatchWeather WeatherWatcher = new WatchWeather(10000, "NL", "Leeuwarden");

            ExStrataAPI.PlayPattern(new Pattern("sok"));

            Console.ReadKey();

        }
    }
}
