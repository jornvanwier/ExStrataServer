using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                result += '&' + "pattern[frames][" + frameNum + "][zones][" + (i + 1) + ']' + '=' + colours[i].Serialize();
            }
            return result;
        }

        public static Frame Empty = new Frame(Colour.Black);
    }
}
