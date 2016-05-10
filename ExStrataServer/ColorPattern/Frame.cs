using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExStrataServer.ColorPattern
{
    public class Frame
    {
        private Color[] colors = new Color[80];
        private int delay;

        public Color[] Colors { get { return colors; } }

        public int Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        public Frame(int delay)
        {
            this.delay = delay;
        }

        public Frame(int delay, Color[] colors)
        {
            this.delay = delay;
            this.colors = colors;
        }

        public void SetRow(int pos, Color color)
        {
            colors[pos] = color;
        }

        public void SetRegion(int startPos, int endPos, Color color)
        {
            if (startPos < 0 || endPos > 80) throw new ArgumentException();

            for (int i = startPos; i < endPos; i++)
            {
                colors[i] = color;
            }
        }
    }
}
