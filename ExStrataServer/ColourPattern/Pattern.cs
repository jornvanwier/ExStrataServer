using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string ToJSON()
        {
            string result = "{\n\t'frames':[\n";
            for (int i = 0; i < frames.Count; i++)
            {
                result += "{\n\t\t\t'ms':" + (i*delay)+ ",\n" + frames[i].ToJSON() + "\n\t\t\t}";

                if (i != frames.Count - 1)
                    result += ",";
            }
            return result + "\n\t]\n}";
        }



        // TODO add methods for defining pretty patterns zoals gradients
        // TODO add toString method
    }
}
