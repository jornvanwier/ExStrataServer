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
            // Set log output to console
            Log.ConsoleOutputMessage = true;
            Log.ConsoleOutputAPI = true;
            Log.ConsoleOutputError = true;
            Log.ConsoleOutputRawData = false;

            Log.Message("Started program.");

            // Start the API Manager with a few APIs
            APIManager manager = new APIManager(
                new WatchTest(1000 * 60 * 2),
                new Watch9292(1000 * 60, "NHL Stenden Hogeschool"),
                new WatchWeather(1000 * 60, "NL", "Leeuwarden"));

            manager.Add(new WatchCBS(1000 * 60));

            char c;
            Console.WriteLine("Press q to quit.");

            do c = Console.ReadKey().KeyChar;
            while (c != 'q');

            Log.Message("Shutting down.");
        }
    }
}
