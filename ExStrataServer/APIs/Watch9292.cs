using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExStrataServer.APIs
{
    public class Watch9292 : APIWatcher
    {
        public Watch9292(double delay) : base(delay, "9292OV")
        {

        }

        protected override void Check(object Sender = null, EventArgs e = null)
        {
            //TODO: Actually check the API

            base.Check(Sender, e);
        }
    }
}
