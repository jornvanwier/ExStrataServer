using System;
using ExStrataServer.ColourPattern;

namespace ExStrataServer.APIs
{
    public class WatchTest : APIWatcher
    {
        private const string name = "Test";
        private const string description = "Test API, moet verwijderd worden voor release.";

        public WatchTest()
        {
            Name = name;
            duration = 10;
        }

        public WatchTest(int delay, int patternDuration = 10) : base(delay, name, description, patternDuration)
        {
            pattern = GetPattern();
        }

        protected override void Check(object sender = null, EventArgs e = null)
        {
            base.Check();

            Send(); 
        }

        public sealed override Pattern GetPattern()
        {
            return Pattern.Animate(new[]{
                new Pattern.GradientFrame(0, Frame.Gradient(new[]
                {
                    new Frame.GradientColour(0, Colour.Red),
                    new Frame.GradientColour(100, Colour.Blue)
                })),
                new Pattern.GradientFrame(100, Frame.Gradient(new[]
                {
                    new Frame.GradientColour(0, Colour.Green),
                    new Frame.GradientColour(100, Colour.Orange)
                }))
            }, "Animation", duration);
        }
    }
}
