using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExStrataServer.ColourPattern;
using ExStrataServer.APIs;
using ExStrataServer.Communication;
using ExStrataServer.Communication.Server;

namespace ExStrataServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press q to quit.");

            // Set log output to console
            Log.ConsoleOutputMessage = true;
            Log.ConsoleOutputAPI = true;
            Log.ConsoleOutputError = true;
            Log.ConsoleOutputRawData = false;

            Log.Message("Started program. Using .NET version " + Environment.Version);
            Log.Message("Mono: " + (Type.GetType("Mono.Runtime") != null));

            // Start the API Manager with a few APIs
            APIManager.Initialize(
                new Watch9292(1000 * 60, "NHL Stenden Hogeschool"),
                new WatchWeather(1000 * 60, "NL", "Leeuwarden"),
                new WatchTwitter(1000 * 60, "kaasMonsieur"),
                new WatchCBS(1000 * 60));
            

            AsyncSocketListener.Initialize();



            char c;

            do c = Console.ReadKey().KeyChar;
            while (c != 'q');

            Log.Message("Shutting down.");
        }
    }
}
