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
        private static bool
            consoleOutputMessage = false,
            consoleOutputAPI = false,
            consoleOutputError = false,
            consoleOutputRawData = false;

        private static object locker = new object();

        /// <summary>
        /// Output messages to the console.
        /// </summary>
        public static bool ConsoleOutputMessage
        {
            get { return consoleOutputMessage; }
            set { consoleOutputMessage = value; }
        }

        /// <summary>
        /// Output API events to the console.
        /// </summary>
        public static bool ConsoleOutputAPI
        {
            get { return consoleOutputAPI; }
            set { consoleOutputAPI = value; }
        }

        /// <summary>
        /// Output errors to the console.
        /// </summary>
        public static bool ConsoleOutputError
        {
            get { return consoleOutputError; }
            set { consoleOutputError = value; }
        }

        /// <summary>
        /// Output raw data to the console.
        /// </summary>
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

        public static void SetAllConsoleOutput(bool state)
        {
            ConsoleOutputMessage = state;
            ConsoleOutputAPI = state;
            ConsoleOutputError = state;
            ConsoleOutputRawData = state;
        }

        public static void Message(string data)
        {
            string text = String.Format("[{0}] {1}", FormatTime(), data);
            Write(text);
            if (ConsoleOutputMessage) Console.WriteLine(text);
        }

        public static void APICheck(string senderName)
        {
            string text = String.Format("[{0}] {1} is checking.", FormatTime(), senderName);
            Write(text);

            if (ConsoleOutputAPI) Console.WriteLine(text);
        }

        public static void APISend(string senderName, string patternName, bool success = true)
        {
            string text;
            if (success) text = String.Format("[{0}] {1} played pattern {2}.", FormatTime(), senderName, patternName);
            else text = String.Format("[{0}] {1} failed to play pattern {2}.", FormatTime(), senderName, patternName);

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
            string text = String.Format("[{0}] RAW: {1}", FormatTime(), data);
            Write(text, "RawData_");
            if (ConsoleOutputRawData) Console.WriteLine(text);
        }

        public static string Read(string location, string name)
        {
            if (name.Substring(name.Length - 4) != ".log")
                name += ".log";


            try
            {
                using (StreamReader sr = new StreamReader(location + "/" + name))
                {
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                Error("Could not open file for reading.");
                return String.Empty;
            }
        }

        public static string Read(string name)
        {
            return Read(defaultLocation, name);
        }

        public static string[] List(string location)
        {
            string[] temp = Directory.GetFiles(location);
            List<string> result = new List<string>();

            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].Substring(temp[i].Length - 4) == ".log")
                {
                    // Split on both kinds of slashes, Windows uses \, Linux uses /
                    string[] filesplit = temp[i].Split('\\', '/', '.');
                    result.Add(filesplit[filesplit.Length - 2]);
                }
            }

            return result.ToArray();
        }

        public static string[] List()
        {
            return List(defaultLocation);
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
            lock (locker)
            {
                if (!Directory.Exists(location))
                {
                    Directory.CreateDirectory(location);
                }

                using (StreamWriter sw = File.AppendText(location + "/" + FormatFileName(name)))
                {
                    sw.WriteLine(text);
                }
            }
        }

        private static string FormatFileName()
        {
            return FormatFileName(String.Empty);
        }

        private static string FormatFileName(string name)
        {
            return name + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
        }

        private static string FormatTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

    }
}
