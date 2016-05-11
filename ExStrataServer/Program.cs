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
            //WatchWeather WeatherWatcher = new WatchWeather(10000, "NL", "Leeuwarden");
            Frame RedToYellow = Gradient.GetFrame(new Gradient.GradientColour[]
            {
                new Gradient.GradientColour(0, Colour.Red),
                new Gradient.GradientColour(20, Colour.Blue),
                new Gradient.GradientColour(40, Colour.Green),
                new Gradient.GradientColour(70, Colour.Pink),
                new Gradient.GradientColour(100, Colour.Yellow)
            });

            Console.WriteLine(RedToYellow);

            Console.ReadKey();
        }
    }
}
