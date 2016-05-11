using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExStrataServer.ColourPattern
{
    public class Frame
    {
        private Colour[] colours = new Colour[80];
        private int delay;

        public Colour[] Colours { get { return Colours; } }

        public int Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        public Frame(int delay)
        {
            this.delay = delay;
        }

        public Frame(int delay, Colour[] Colours)
        {
            this.delay = delay;
            this.colours = Colours;
        }

        public void SetRow(int pos, Colour Colour)
        {
            Colours[pos] = Colour;
        }

        public void SetRegion(int startPos, int endPos, Colour Colour)
        {
            if (startPos < 0 || endPos > 80) throw new ArgumentException();

            for (int i = startPos; i < endPos; i++)
            {
                Colours[i] = Colour;
            }
        }
    }
}
