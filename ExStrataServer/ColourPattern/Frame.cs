using System;
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

        public Frame(Colour[] Colours)
        {
            this.colours = Colours;
        }

        public void SetRow(int pos, Colour Colour)
        {
            Colours[pos] = Colour;
        }

        public void SetRegion(int startPos, int endPos, Colour Colour)
        {
            if (startPos < 0 || endPos > ExStrataHeight) throw new ArgumentException();

            for (int i = startPos; i < endPos; i++)
            {
                Colours[i] = Colour;
            }
        }
        
    }
}
