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
            Frame gradients = Gradient.GetFrame(new Gradient.GradientColour[]
            {
                new Gradient.GradientColour(0, Colour.Red),
                new Gradient.GradientColour(20, Colour.Blue),
                new Gradient.GradientColour(40, Colour.Green),
                new Gradient.GradientColour(70, Colour.Pink),
                new Gradient.GradientColour(100, Colour.Yellow)
            });
            Frame redToGreen = Gradient.GetFrame(new Gradient.GradientColour[]
            {
                new Gradient.GradientColour(0, Colour.Red),
                new Gradient.GradientColour(100, Colour.Green)
            });

            Pattern testPattern = new Pattern("TestPattern", 500, new List<Frame>()
            {
                new Frame(new Colour[]
                {
                    Colour.Red,
                    Colour.Blue
                    }),
                new Frame(new Colour[]
                {
                    Colour.Aquamarine,
                    Colour.Blanchedalmond,
                    Colour.Bisque
                    }),
                new Frame(new Colour[]
                {
                    Colour.Black,
                    Colour.Black
                    }),
            });


            int temperatureRings = (int)(((float)5 + (float)5) / (float)35 * (float)80);

            Pattern temperaturepGradient = new Pattern("Temperature", 60 * 1000);
            temperaturepGradient.Add(Gradient.GetFrame(new Gradient.GradientColour[]
            {
                    new Gradient.GradientColour(0, new Colour(0,200,220)),
                    new Gradient.GradientColour(50, new Colour(255,200,0)),
                    new Gradient.GradientColour(100, new Colour(255,70,0))
            }, 0, temperatureRings));
            Console.WriteLine(temperaturepGradient.ToString());

            Pattern pattern = new Pattern("test", 100, new List<Frame>() { gradients, redToGreen });
            string framejson = gradients.ToString();
            string patternjson = pattern.ToString();

            //ExStrataAPI.PlayPattern(temperaturepGradient);

            Console.ReadKey();

        }
    }
}
