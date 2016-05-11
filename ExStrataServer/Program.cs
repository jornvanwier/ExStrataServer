using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExStrataServer.ColorPattern;
using ExStrataServer.APIs;

namespace ExStrataServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Add("Piet", "Sok");

            Watch9292 _9292 = new Watch9292(1000 * 15, "NHL Stenden Hogeschool");
        }
    }
}
