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
            public int percentage;
            public Colour colour;
            public GradientColour(int percentage, Colour colour)
            {
                this.percentage = percentage;
                this.colour = colour;
            }
        }
        public static Frame GetFrame(GradientColour[] colours)
        {
            Frame result = new Frame();
            int currentRow = 0;
            for (int i = 0; i < colours.Length - 1; i++)
            {
                float percentage = (float)(colours[i + 1].percentage - colours[i].percentage) / 100;
                int ringsToNextPoint = (int)(Frame.ExStrataHeight * percentage);

                float rColorDiff = (colours[i + 1].colour.R - colours[i].colour.R) / (float)ringsToNextPoint,
                    gColorDiff = (colours[i + 1].colour.G - colours[i].colour.G) / (float)ringsToNextPoint,
                    bColorDiff = (colours[i + 1].colour.B - colours[i].colour.B) / (float)ringsToNextPoint;

                for (int j = 0; j < ringsToNextPoint; j++)
                {
                    result.SetRow(currentRow, new Colour(
                        Convert.ToByte(colours[i].colour.R + (rColorDiff * j)),
                        Convert.ToByte(colours[i].colour.G + (gColorDiff * j)),
                        Convert.ToByte(colours[i].colour.B + (bColorDiff * j))
                    ));
                    currentRow++;
                }
                if (currentRow == 0)
                {
                    result.SetRow(currentRow, colours[i + 1].colour);
                }
                else {
                    result.SetRow(currentRow - 1, colours[i + 1].colour);
                }
            }
            return result;
        }
    }
}
