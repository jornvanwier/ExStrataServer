using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Parameters.Add(new Parameter
            {
                Name = "month",
                Type = "int",
                Value = Month.ToString()
            });
            Parameters.Add(new Parameter
            {
                Name = "day",
                Type = "int",
                Value = Day.ToString()
            });
            Parameters.Add(new Parameter
            {
                Name = "hour",
                Type = "int",
                Value = Hour.ToString()
            });
            Parameters.Add(new Parameter
            {
                Name = "minute",
                Type = "int",
                Value = Minute.ToString()
            });
            Parameters.Add(new Parameter
            {
                Name = "colour",
                Type = "colour"
            });
        }

        public WatchDateTime(int delay, int month, int day, int hour, int minute, string colour) : base(delay, name, description)
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
            if (Month != -1) result += String.Format("Maand: {0} ", Month);
            if (Day != -1) result += String.Format("Dag: {0} ", Day);
            if (Hour != -1) result += String.Format("Uur: {0} ", Hour);
            if (Minute != -1) result += String.Format("Minuut: {0} ", Minute);

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
                1000 * 60 * 10,
                new List<Frame> { new Frame(colour) });
        }

        public override Pattern GetPattern()
        {
            return GetPattern(Colour.Green);
        }
    }
}
