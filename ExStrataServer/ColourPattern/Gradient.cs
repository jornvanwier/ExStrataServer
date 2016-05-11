using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExStrataServer.ColourPattern
{
    public static class Gradient
    {
        public struct GradientColour
        {
            int percentage;
            Colour Colour;
            public GradientColour(int percentage, Colour Colour)
            {
                this.percentage = percentage;
                this.Colour = Colour;
            }
        }
        public static Frame GetFrame(GradientColour[] Colours)
        {

        }
    }
}
