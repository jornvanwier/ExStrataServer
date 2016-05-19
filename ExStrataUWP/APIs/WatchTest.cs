using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExStrataUWP.ColourPattern;

namespace ExStrataUWP.APIs
{
    public class WatchTest : APIWatcher
    {
        public WatchTest(int delay) : base(delay, "TestAPI")
        {
            pattern = Pattern.Animate(new Pattern.GradientFrame[]{
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

        protected override void Check(object sender = null, EventArgs e = null)
        {
            Send(); 
        }
    }
}
