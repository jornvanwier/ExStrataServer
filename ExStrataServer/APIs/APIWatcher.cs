using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ExStrataServer.ColorPattern;

namespace ExStrataServer.APIs
{
    public abstract class APIWatcher
    {
        private Timer checkTimer;
        private double checkDelay;
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public APIWatcher(double delay, string name)
        {
            Name = name;

            // Initialize the timer.
            checkTimer.Interval = delay;
            checkTimer.Elapsed += Check;
            checkTimer.AutoReset = false;
            checkTimer.Start();
        }

        /// <summary>
        /// Check if the API has any data that interests us. Should be attached to the Elapsed event of a Timer.
        /// </summary>
        /// <param name="sender">The object that sends the event.</param>
        /// <param name="e">The EventArgs.</param>
        protected virtual void Check(object sender = null, EventArgs e = null)
        {
            // Reset the timer after the API has been checked.
            checkTimer.Start();
        }

        /// <summary>
        /// Send a pattern to the EX STRATA and add a new entry to the log.
        /// </summary>
        /// <param name="pattern">The pattern to be played.</param>
        protected virtual void Send(Pattern pattern)
        {
            Log.Add(Name, pattern.Name);
            throw new NotImplementedException();
        }
    }
}
