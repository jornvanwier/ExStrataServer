using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ExStrataServer.ColourPattern
{
    public class Pattern
    {
        private List<Frame> frames;
        private string name;
        private int delay;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<Frame> Frames
        {
            get { return frames; }
        }

        public int Delay
        {
            get { return delay; }
            set { delay = value; }
        }

        public Pattern(string name, int delay, List<Frame> frames)
        {
            Name = name;
            Delay = delay;

            this.frames = frames;
        }

        public Pattern(string name, int delay)
        {
            Name = name;
            Delay = delay;

            frames = new List<Frame>();
        }

        public void Add(Frame frame)
        {
            frames.Add(frame);
        }

        public string Serialize()
        {
            string result = "";
            string ampersand = "";
            for (int i = 0; i < frames.Count; i++)
            {
                if (i == 0)
                    ampersand = "";
                else
                    ampersand = "&";
                result += ampersand + WebUtility.UrlEncode("pattern[frames][" + i + "][ms]") + "=" + (Delay * i) + "\n";
                result += frames[i].Serialize(i);
            }
            return result + "";
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }



        // TODO add methods for defining pretty patterns zoals gradients
        // TODO add toString method
    }
}
