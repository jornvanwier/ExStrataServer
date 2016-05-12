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
        private static bool consoleOutput = true;

        public static bool ConsoleOutput
        {
            get { return consoleOutput; }
            set { consoleOutput = value; }
        }

        public static string Name
        {
            get { return name; }
            set { name = value; }
        }

        public static void Add(string senderName, string patternName)
        {
            string text = String.Format("[{0}] {1} played pattern {2}.", FormatTime(), senderName, patternName);

            Write(text);
            if (ConsoleOutput) Console.WriteLine(text);
        }

        public static void AddError(string data)
        {
            string text = String.Format("[{0}] ERROR: {1}", FormatTime(), data);

            Write(text);
            if (ConsoleOutput) Console.WriteLine(text);
        }

        private static void Write(string text)
        {
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }

            using (StreamWriter sw = File.AppendText(location + "/" + FormatFileName()))
            {
                sw.WriteLine(text);
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
