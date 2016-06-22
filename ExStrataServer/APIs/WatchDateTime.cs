using System;
using System.Collections.Generic;
using ExStrataServer.ColourPattern;

namespace ExStrataServer.APIs
{
    public class WatchDateTime : APIWatcher
    {
        private const string name = "DateTime";
        private const string description = "Controleer of het een bepaalde datum of tijd is. Als dit het geval is wordt het patroon afgespeeld. Als een datum is ingevuld zal het pattern alleen gaan als de huidige minuten deelbaar zijn door 9.";

        private int month = -1;
        private int day = -1;
        private int hour = -1;
        private int minute = -1;

        public int Month
        {
            get { return month; }
            set { month = value; }
        }

        public int Day
        {
            get { return day; }
            set { day = value; }
        }

        public int Hour
        {
            get { return hour; }
            set { hour = value; }
        }

        public int Minute
        {
            get { return minute; }
            set { minute = value; }
        }

        public WatchDateTime()
        {
            Name = name;
            Description = description;
            duration = 60;
        }

        public WatchDateTime(int delay, int month, int day, int hour, int minute, string colour, int patternDuration = 60) : base(delay, name, description, patternDuration)
        {
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;

            InstanceInfo = FormatTime();
            DefaultPattern = GetPattern(Colour.FromString(colour));
        }

        protected override void Check(object sender = null, EventArgs e = null)
        {
            base.Check(sender, e);

            // Go only every 19 minutes if month or day is set
            if ((Month != -1 || Day != -1) && DateTime.Now.Minute % 9 == 0)
            {
                if (IsDateTimeMatch())
                {
                    Send();
                }
            }
        }

        private string FormatTime()
        {
            string result = String.Empty;
            if (Month != -1) result += $"Maand: {Month} ";
            if (Day != -1) result += $"Dag: {Day} ";
            if (Hour != -1) result += $"Uur: {Hour} ";
            if (Minute != -1) result += $"Minuut: {Minute} ";

            return result;
        }

        private bool IsDateTimeMatch()
        {
            DateTime now = DateTime.Now;

            if (Month != -1 && Month != now.Month) return false;
            if (Day != -1 && Day != now.Day) return false;
            if (Hour != -1 && Hour != now.Hour) return false;
            if (Minute != -1 && Minute != now.Minute) return false;

            return true;
        }

        private Pattern GetPattern(Colour colour)
        {
            return new Pattern(
                "DateTime Colour",
                duration,
                new List<Frame> { new Frame(colour) });
        }

        public override Pattern GetPattern()
        {
            return GetPattern(Colour.Green);
        }
    }
}
