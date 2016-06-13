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
        private const string name = "Test";
        private const string description = "Test API, moet verwijderd worden voor release.";

        public WatchTest() : base()
        {
            Name = name;
        }

        public WatchTest(int delay) : base(delay, name, description)
        {
            pattern = GetPattern();
        }

        protected override void Check(object sender = null, EventArgs e = null)
        {
            base.Check();

            Send(); 
        }

        public override Pattern GetPattern()
        {
            return Pattern.Animate(new[]{
                new Pattern.GradientFrame(0, Frame.Gradient(new Frame.GradientColour[]
                {
                    new Frame.GradientColour(0, Colour.Red),
                    new Frame.GradientColour(100, Colour.Blue)
                })),
                new Pattern.GradientFrame(100, Frame.Gradient(new Frame.GradientColour[]
                {
                    new Frame.GradientColour(0, Colour.Green),
                    new Frame.GradientColour(100, Colour.Orange)
                }))
            }, "Animation", 300, 14);
        }
    }
}
