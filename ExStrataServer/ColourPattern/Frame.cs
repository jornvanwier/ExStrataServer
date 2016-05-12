﻿using System;
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

        public Frame(Colour[] colours)
        {
            this.colours = colours;
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

        public string Serialize(int frameNum)
        {
            string result = "";
            for (int i = 0; i < colours.Length; i++)
            {
                result += "&" + WebUtility.UrlEncode("pattern[frames][" + frameNum + "][zones][" + (i + 1) + "]") + "=" + colours[i].Serialize();
            }
            return result + "";

        }
        public static Frame BlackFrame = new Frame(new Colour[]{
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black,
            Colour.Black
        });
    }
}
