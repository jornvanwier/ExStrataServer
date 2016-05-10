using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExStrataServer.ColorPattern
{
    public class Color
    {
        private byte red, green, blue;

        public byte R
        {
            get { return red; }
            set { red = value; }
        }

        public byte G
        {
            get { return green; }
            set { green = value; }
        }

        public byte B
        {
            get { return blue; }
            set { blue = value; }
        }

        public Color(byte red, byte green, byte blue)
        {
            R = red;
            G = green;
            B = blue;
        }

        /// <summary>
        /// Format the color in the format expected by the EX STRATA.
        /// </summary>
        /// <returns>The formatted string.</returns>
        public override string ToString()
        {
            return String.Format("{0},{1},{2}", R, G, B);
        }
    }
}
