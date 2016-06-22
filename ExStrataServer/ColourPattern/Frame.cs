using System;
using System.Linq;
using System.Net;

namespace ExStrataServer.ColourPattern
{
    public class Frame
    {
        public const int ExStrataHeight = 80;
        private Colour[] colours = new Colour[ExStrataHeight];

        public Colour[] Colours { get { return colours; } }

        public Frame()
        {

        }

        public Frame(Colour colour)
        {
            for (int i = 0; i < colours.Length; i++)
            {
                colours[i] = colour;
            }
        }

        public Frame(Colour[] colours)
        {
            this.colours = colours;
        }

        public void SetRow(int pos, Colour colour)
        {
            Colours[pos] = colour;
        }

        public void SetRegion(int startPos, int endPos, Colour colour)
        {
            if (startPos < 0 || endPos > ExStrataHeight) throw new ArgumentException();

            for (int i = startPos; i < endPos; i++)
            {
                Colours[i] = colour;
            }
        }
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
        public static Frame Gradient(GradientColour[] colours, int start = 0, int stop = ExStrataHeight)
        {
            Frame result = new Frame();
            int currentRow = 0;
            for (int i = 0; i < colours.Length - 1; i++)
            {
                float percentage = (float)(colours[i + 1].percentage - colours[i].percentage) / 100;
                int ringsToNextPoint = (int)(ExStrataHeight * percentage);

                float rColorDiff = (colours[i + 1].colour.R - colours[i].colour.R) / (float)ringsToNextPoint,
                    gColorDiff = (colours[i + 1].colour.G - colours[i].colour.G) / (float)ringsToNextPoint,
                    bColorDiff = (colours[i + 1].colour.B - colours[i].colour.B) / (float)ringsToNextPoint;

                for (int j = 0; j < ringsToNextPoint; j++)
                {
                    if (currentRow >= start && currentRow <= stop)
                    {
                        result.SetRow(currentRow, new Colour(
                            Convert.ToByte(colours[i].colour.R + (rColorDiff * j)),
                            Convert.ToByte(colours[i].colour.G + (gColorDiff * j)),
                            Convert.ToByte(colours[i].colour.B + (bColorDiff * j))
                        ));
                    }
                    else
                    {
                        result.SetRow(currentRow, new Colour(0, 0, 0));
                    }
                    currentRow++;
                }
                if (currentRow == 0 && currentRow >= start && currentRow <= stop)
                {
                    result.SetRow(currentRow, colours[i + 1].colour);
                }
                else if (currentRow >= start && currentRow <= stop)
                {
                    result.SetRow(currentRow - 1, colours[i + 1].colour);
                }
            }
            if(result.Colours.Last()== null)
            {
                result.SetRow(79, result.Colours[78]);
            }
            return result;
        }

        public string Serialize(int frameNum)
        {
            string result = String.Empty;
            for (int i = 0; i < colours.Length; i++)
            {
                result += '&' + WebUtility.UrlEncode("pattern[frames][" + frameNum + "][zones][" + (i + 1) + ']') + '=' + colours[i].Serialize();
            }
            return result;

        }

        public string UnencodedSerialize(int frameNum)
        {
            string result = String.Empty;
            for (int i = 0; i < colours.Length; i++)
            {
                result += '&' + "pattern[frames][" + frameNum + "][zones][" + (i + 1) + "]=" + colours[i].Serialize();
            }
            return result;
        }

        public static Frame Empty = new Frame(Colour.Black);
    }
}
