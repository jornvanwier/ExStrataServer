using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExStrataServer.ColourPattern;
using ExStrataServer.APIs;

namespace ExStrataServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Add("Piet", "Sok");
            //WatchWeather WeatherWatcher = new WatchWeather(60*1000, "NL", "Leeuwarden");
            Frame redToYellow = Gradient.GetFrame(new Gradient.GradientColour[] {
                new Gradient.GradientColour(0, Colour.Red),
                new Gradient.GradientColour(100, Colour.Yellow)
            });
            Console.ReadKey();
        }
    }
}
