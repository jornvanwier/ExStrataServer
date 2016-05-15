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
    public abstract class APIWatcher : IDisposable
    {
        private Timer checkTimer;
        private double checkDelay;
        private string name;
        protected Pattern pattern;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public APIWatcher(int delay, string name)
        {
            Name = name;

            // Initialize the timer.
            StartTimer(delay);
        }

        public void StartTimer(int delay)
        {
            checkTimer = new Timer((obj) =>
            {
                Check();
            }, null, 0, delay);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(checkTimer != null)
                {
                    checkTimer.Dispose();
                    checkTimer = null;
                }
            }
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
        protected virtual void Send(Pattern pattern)
        {
            if (pattern == null) throw new NullReferenceException("Pattern is not set.");

            Log.APIEvent(Name, pattern.Name);
            // Send current pattern with EX STRATA API
            ExStrataAPI.PlayPattern(pattern);
        }

        protected virtual void Send()
        {
            Send(pattern);
        }
    }
}
