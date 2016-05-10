using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ExStrataServer
{
    public static class Log
    {
        private static string name;
        private static string location = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");

        public static string Name
        {
            get { return name; }
            set { name = value; }
        }

        public static void Add(string senderName, string patternName)
        {
            if(!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }

            using (StreamWriter sw = File.AppendText(location + "/" + FormatFileName()))
            {
                sw.WriteLine(String.Format("[{0}] {1} played pattern {2}.", FormatTime(), senderName, patternName));
            }
        }

        private static string FormatFileName()
        {
            return DateTime.Now.ToString("yyyy-MM-dd") + ".log";
        }

        private static string FormatTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

    }
}
