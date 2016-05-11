using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExStrataServer.ColourPattern
{
    public class Colour
    {
        #region colours

        public static Colour Black = new Colour(0, 0, 0);
        public static Colour Silver = new Colour(192, 192, 192);
        public static Colour Gray = new Colour(128, 128, 128);
        public static Colour White = new Colour(255, 255, 255);
        public static Colour Maroon = new Colour(128, 0, 0);
        public static Colour Red = new Colour(255, 0, 0);
        public static Colour Purple = new Colour(128, 0, 128);
        public static Colour Fuchsia = new Colour(255, 0, 255);
        public static Colour Green = new Colour(0, 128, 0);
        public static Colour Lime = new Colour(0, 255, 0);
        public static Colour Olive = new Colour(128, 128, 0);
        public static Colour Yellow = new Colour(255, 255, 0);
        public static Colour Navy = new Colour(0, 0, 128);
        public static Colour Blue = new Colour(0, 0, 255);
        public static Colour Teal = new Colour(0, 128, 128);
        public static Colour Aqua = new Colour(0, 255, 255);
        public static Colour Orange = new Colour(255, 165, 0);
        public static Colour Aliceblue = new Colour(240, 248, 255);
        public static Colour Antiquewhite = new Colour(250, 235, 215);
        public static Colour Aquamarine = new Colour(127, 255, 212);
        public static Colour Azure = new Colour(240, 255, 255);
        public static Colour Beige = new Colour(245, 245, 220);
        public static Colour Bisque = new Colour(255, 228, 196);
        public static Colour Blanchedalmond = new Colour(255, 228, 196);
        public static Colour Blueviolet = new Colour(138, 43, 226);
        public static Colour Brown = new Colour(165, 42, 42);
        public static Colour Burlywood = new Colour(222, 184, 135);
        public static Colour Cadetblue = new Colour(95, 158, 160);
        public static Colour Chartreuse = new Colour(127, 255, 0);
        public static Colour Chocolate = new Colour(210, 105, 30);
        public static Colour Coral = new Colour(255, 127, 80);
        public static Colour Cornflowerblue = new Colour(100, 149, 237);
        public static Colour Cornsilk = new Colour(255, 248, 220);
        public static Colour Crimson = new Colour(220, 20, 60);
        public static Colour Darkblue = new Colour(0, 0, 139);
        public static Colour Darkcyan = new Colour(0, 139, 139);
        public static Colour Darkgoldenrod = new Colour(184, 134, 11);
        public static Colour Darkgray = new Colour(169, 169, 169);
        public static Colour Darkgreen = new Colour(0, 100, 0);
        public static Colour Darkgrey = new Colour(169, 169, 169);
        public static Colour Darkkhaki = new Colour(189, 183, 107);
        public static Colour Darkmagenta = new Colour(139, 0, 139);
        public static Colour Darkolivegreen = new Colour(85, 107, 47);
        public static Colour Darkorange = new Colour(255, 140, 0);
        public static Colour Darkorchid = new Colour(153, 50, 204);
        public static Colour Darkred = new Colour(139, 0, 0);
        public static Colour Darksalmon = new Colour(233, 150, 122);
        public static Colour Darkseagreen = new Colour(143, 188, 143);
        public static Colour Darkslateblue = new Colour(72, 61, 139);
        public static Colour Darkslategray = new Colour(47, 79, 79);
        public static Colour Darkslategrey = new Colour(47, 79, 79);
        public static Colour Darkturquoise = new Colour(0, 206, 209);
        public static Colour Darkviolet = new Colour(148, 0, 211);
        public static Colour Deeppink = new Colour(255, 20, 147);
        public static Colour Deepskyblue = new Colour(0, 191, 255);
        public static Colour Dimgray = new Colour(105, 105, 105);
        public static Colour Dimgrey = new Colour(105, 105, 105);
        public static Colour Dodgerblue = new Colour(30, 144, 255);
        public static Colour Firebrick = new Colour(178, 34, 34);
        public static Colour Floralwhite = new Colour(255, 250, 240);
        public static Colour Forestgreen = new Colour(34, 139, 34);
        public static Colour Gainsboro = new Colour(220, 220, 220);
        public static Colour Ghostwhite = new Colour(248, 248, 255);
        public static Colour Gold = new Colour(255, 215, 0);
        public static Colour Goldenrod = new Colour(218, 165, 32);
        public static Colour Greenyellow = new Colour(173, 255, 47);
        public static Colour Grey = new Colour(128, 128, 128);
        public static Colour Honeydew = new Colour(240, 255, 240);
        public static Colour Hotpink = new Colour(255, 105, 180);
        public static Colour Indianred = new Colour(205, 92, 92);
        public static Colour Indigo = new Colour(75, 0, 130);
        public static Colour Ivory = new Colour(255, 255, 240);
        public static Colour Khaki = new Colour(240, 230, 140);
        public static Colour Lavender = new Colour(230, 230, 250);
        public static Colour Lavenderblush = new Colour(255, 240, 245);
        public static Colour Lawngreen = new Colour(124, 252, 0);
        public static Colour Lemonchiffon = new Colour(255, 250, 205);
        public static Colour Lightblue = new Colour(173, 216, 230);
        public static Colour Lightcoral = new Colour(240, 128, 128);
        public static Colour Lightcyan = new Colour(224, 255, 255);
        public static Colour Lightgoldenrodyellow = new Colour(250, 250, 210);
        public static Colour Lightgray = new Colour(211, 211, 211);
        public static Colour Lightgreen = new Colour(144, 238, 144);
        public static Colour Lightgrey = new Colour(211, 211, 211);
        public static Colour Lightpink = new Colour(255, 182, 193);
        public static Colour Lightsalmon = new Colour(255, 160, 122);
        public static Colour Lightseagreen = new Colour(32, 178, 170);
        public static Colour Lightskyblue = new Colour(135, 206, 250);
        public static Colour Lightslategray = new Colour(119, 136, 153);
        public static Colour Lightslategrey = new Colour(119, 136, 153);
        public static Colour Lightsteelblue = new Colour(176, 196, 222);
        public static Colour Lightyellow = new Colour(255, 255, 224);
        public static Colour Limegreen = new Colour(50, 205, 50);
        public static Colour Linen = new Colour(250, 240, 230);
        public static Colour Mediumaquamarine = new Colour(102, 205, 170);
        public static Colour Mediumblue = new Colour(0, 0, 205);
        public static Colour Mediumorchid = new Colour(186, 85, 211);
        public static Colour Mediumpurple = new Colour(147, 112, 219);
        public static Colour Mediumseagreen = new Colour(60, 179, 113);
        public static Colour Mediumslateblue = new Colour(123, 104, 238);
        public static Colour Mediumspringgreen = new Colour(0, 250, 154);
        public static Colour Mediumturquoise = new Colour(72, 209, 204);
        public static Colour Mediumvioletred = new Colour(199, 21, 133);
        public static Colour Midnightblue = new Colour(25, 25, 112);
        public static Colour Mintcream = new Colour(245, 255, 250);
        public static Colour Mistyrose = new Colour(255, 228, 225);
        public static Colour Moccasin = new Colour(255, 228, 181);
        public static Colour Navajowhite = new Colour(255, 222, 173);
        public static Colour Oldlace = new Colour(253, 245, 230);
        public static Colour Olivedrab = new Colour(107, 142, 35);
        public static Colour Orangered = new Colour(255, 69, 0);
        public static Colour Orchid = new Colour(218, 112, 214);
        public static Colour Palegoldenrod = new Colour(238, 232, 170);
        public static Colour Palegreen = new Colour(152, 251, 152);
        public static Colour Paleturquoise = new Colour(175, 238, 238);
        public static Colour Palevioletred = new Colour(219, 112, 147);
        public static Colour Papayawhip = new Colour(255, 239, 213);
        public static Colour Peachpuff = new Colour(255, 218, 185);
        public static Colour Peru = new Colour(205, 133, 63);
        public static Colour Pink = new Colour(255, 192, 203);
        public static Colour Plum = new Colour(221, 160, 221);
        public static Colour Powderblue = new Colour(176, 224, 230);
        public static Colour Rosybrown = new Colour(188, 143, 143);
        public static Colour Royalblue = new Colour(65, 105, 225);
        public static Colour Saddlebrown = new Colour(139, 69, 19);
        public static Colour Salmon = new Colour(250, 128, 114);
        public static Colour Sandybrown = new Colour(244, 164, 96);
        public static Colour Seagreen = new Colour(46, 139, 87);
        public static Colour Seashell = new Colour(255, 245, 238);
        public static Colour Sienna = new Colour(160, 82, 45);
        public static Colour Skyblue = new Colour(135, 206, 235);
        public static Colour Slateblue = new Colour(106, 90, 205);
        public static Colour Slategray = new Colour(112, 128, 144);
        public static Colour Slategrey = new Colour(112, 128, 144);
        public static Colour Snow = new Colour(255, 250, 250);
        public static Colour Springgreen = new Colour(0, 255, 127);
        public static Colour Steelblue = new Colour(70, 130, 180);
        public static Colour Tan = new Colour(210, 180, 140);
        public static Colour Thistle = new Colour(216, 191, 216);
        public static Colour Tomato = new Colour(255, 99, 71);
        public static Colour Turquoise = new Colour(64, 224, 208);
        public static Colour Violet = new Colour(238, 130, 238);
        public static Colour Wheat = new Colour(245, 222, 179);
        public static Colour Whitesmoke = new Colour(245, 245, 245);
        public static Colour Yellowgreen = new Colour(154, 205, 50);
        public static Colour Rebeccapurple = new Colour(102, 51, 153);

        #endregion

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

        public Colour(byte red, byte green, byte blue)
        {
            R = red;
            G = green;
            B = blue;
        }

        /// <summary>
        /// Format the Colour in the format expected by the EX STRATA.
        /// </summary>
        /// <returns>The formatted string.</returns>
        public string ToJSON(int i)
        {
            return String.Format("'{0}':'{1},{2},{3}'", i, R, G, B);
        }

    }
}
