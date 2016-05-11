using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ExStrataServer.ColourPattern;
using ExStrataServer.Communication;

namespace ExStrataServer.APIs
{
    public abstract class APIWatcher
    {
        private Timer checkTimer;
        private double checkDelay;
        private string name;
        private Pattern pattern;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public APIWatcher(double delay, string name)
        {
            Name = name;
            StartTimer();

            // Initialize the timer.
        }

        public void StartTimer()
        {
            checkTimer = new Timer((obj) =>
            {
                if (DateTime.Now.Minute % 15 == 0)
                {
                    Check();
                }
            }, null, 0, 500);
        }

        /// <summary>
        /// Check if the API has any data that interests us. Should be attached to the Elapsed event of a Timer.
        /// </summary>
        /// <param name="sender">The object that sends the event.</param>
        /// <param name="e">The EventArgs.</param>
        protected abstract void Check(object sender = null, EventArgs e = null);

        /// <summary>
        /// Send a pattern to the EX STRATA and add a new entry to the log.
        /// </summary>
        /// <param name="pattern">The pattern to be played.</param>
        protected virtual void Send()
        {
            Log.Add(Name, pattern.Name);
            // Send current pattern with EX STRATA API
            ExStrataAPI.PlayPattern(pattern);
        }
    }
}
