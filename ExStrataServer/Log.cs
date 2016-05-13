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
        private static string defaultLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
        private static bool consoleOutputAPI = true,
            consoleOutputError = true,
            consoleOutputRawData = false;

        public static bool ConsoleOutputAPI
        {
            get { return consoleOutputAPI; }
            set { consoleOutputAPI = value; }
        }
        public static bool ConsoleOutputError
        {
            get { return consoleOutputError; }
            set { consoleOutputError = value; }
        }
        public static bool ConsoleOutputRawData
        {
            get { return consoleOutputRawData; }
            set { consoleOutputRawData = value; }
        }

        public static string Name
        {
            get { return name; }
            set { name = value; }
        }

        public static void APIEvent(string senderName, string patternName)
        {
            string text = String.Format("[{0}] {1} played pattern {2}.", FormatTime(), senderName, patternName);

            Write(text);
            if (ConsoleOutputAPI) Console.WriteLine(text);
        }

        public static void Error(string data)
        {
            string text = String.Format("[{0}] ERROR: {1}", FormatTime(), data);

            Write(text);
            if (ConsoleOutputError) Console.WriteLine(text);
        }

        public static void RawData(string data)
        {
            string text = string.Format("[{0}] RAW: {1}", FormatTime(), data);
            Write(text, "RawData");
            if (ConsoleOutputRawData) Console.WriteLine(text);
        }

        private static void Write(string text)
        {
            Write(text, defaultLocation, String.Empty);
        }

        private static void Write(string text, string name)
        {
            Write(text, defaultLocation, name);
        }

        private static void Write(string text, string location, string name)
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
            return FormatFileName(String.Empty);
        }

        private static string FormatFileName(string name)
        {
            return name + "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
        }

        private static string FormatTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

    }
}
