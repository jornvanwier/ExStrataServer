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
        public static Frame GetFrame(GradientColour[] colours, int start = 0, int stop = Frame.ExStrataHeight)
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
            return result;
        }

        public struct GradientFrame
        {
            public int percentage;
            public Frame frame;
            public GradientFrame(int percentage, Frame frame)
            {
                this.percentage = percentage;
                this.frame = frame;
            }
        }
        public static Pattern GetPattern(GradientFrame[] frames, string name, int delay, int length)
        {
            if (length > 14)
            {
                throw new ArgumentException("The length of a pattern can not be more than 14");
            }
            Pattern result = new Pattern(name, delay);
            int currentFrame = 0;
            for (int i = 0; i < frames.Length - 1; i++)
            {
                float percentage = (float)(frames[i + 1].percentage - frames[i].percentage) / 100;
                int framesToNextPoint = (int)((length - 1) * percentage);

                List<float> rColorDifs = new List<float>(),
                    gColorDifs = new List<float>(),
                    bColorDifs = new List<float>();

                for (int j = 0; j < frames[i].frame.Colours.Length; j++)
                {
                    rColorDifs.Add((frames[i + 1].frame.Colours[j].R - frames[i].frame.Colours[j].R) / (float)framesToNextPoint);
                    gColorDifs.Add((frames[i + 1].frame.Colours[j].G - frames[i].frame.Colours[j].G) / (float)framesToNextPoint);
                    bColorDifs.Add((frames[i + 1].frame.Colours[j].B - frames[i].frame.Colours[j].B) / (float)framesToNextPoint);
                }

                for (int k = 0; k < framesToNextPoint; k++)
                {
                    List<Colour> colours = new List<Colour>();
                    for (int l = 0; l < rColorDifs.Count; l++)
                    {
                        colours.Add(new Colour(
                            Convert.ToByte(frames[i].frame.Colours[l].R + (rColorDifs[l] * k)),
                            Convert.ToByte(frames[i].frame.Colours[l].G + (gColorDifs[l] * k)),
                            Convert.ToByte(frames[i].frame.Colours[l].B + (bColorDifs[l] * k))
                            ));
                    }
                    result.Add(new Frame(colours.ToArray()));
                }
                result.Add(frames[i+1].frame);
            }
            return result;
        }
    }
}
