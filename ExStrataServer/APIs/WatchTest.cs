using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExStrataServer.ColourPattern;

namespace ExStrataServer.APIs
{
    public class WatchTest : APIWatcher
    {
        public WatchTest(int delay) : base(delay, "TestAPI")
        {
            pattern = Pattern.Animate(new Pattern.GradientFrame[]
            {
            new Pattern.GradientFrame(0, new Frame(Colour.Orange)),
            new Pattern.GradientFrame(100, new Frame(Colour.Blue))
            }, "OrangeToBlue", 300, 14);
        }

        protected override void Check(object sender = null, EventArgs e = null)
        {
            Send(); 
        }
    }
}
