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
        private int checkDelay;
        private string name;
        private string description;
        protected Pattern pattern;
        private string instanceInfo = String.Empty;
        private List<Parameter> parameters = new List<Parameter>()
        {
            new Parameter
            {
                Name = "delay",
                Type = "int"
            }
        };

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public Pattern DefaultPattern
        {
            get { return pattern; }
            protected set { pattern = value; }
        }

        public List<Parameter> Parameters
        {
            get { return parameters; }
            protected set { parameters = value; }
        }

        public string InstanceInfo
        {
            get { return instanceInfo; }
            protected set { instanceInfo = value; }
        }

        public APIWatcher()
        {
        }

        public APIWatcher(int delay, string name, string description)
        {
            checkDelay = delay;
            Name = name;
            Description = description;
        }

        public void Start()
        {
            checkTimer = new Timer((obj) =>
            {
                Check();
            }, null, 0, checkDelay);
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
        protected virtual void Check(object sender = null, EventArgs e = null)
        {
            //Console.WriteLine(Name);
        }

        /// <summary>
        /// Send a pattern to the EX STRATA and add a new entry to the log.
        /// </summary>
        /// <param name="pattern">The pattern to be played.</param>
        protected virtual async void Send(Pattern pattern)
        {
            if (pattern == null) throw new NullReferenceException("Pattern is not set.");

            // Send pattern with EX STRATA API
            if (await ExStrataAPI.PlayPattern(pattern)) Log.APISend(Name, pattern.Name);
            else Log.APISend(Name, pattern.Name, false);
        }

        protected virtual void Send()
        {
            Send(pattern);
        }

        public abstract Pattern GetPattern();
    }
}
