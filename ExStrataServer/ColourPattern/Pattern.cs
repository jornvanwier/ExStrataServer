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

            this.frames = frames;
        }

        public Pattern(string name, int delay)
        {
            Name = name;

            frames = new List<Frame>();
        }

        public void Add(Frame frame)
        {
            frames.Add(frame);
        }

        // TODO add methods for defining pretty patterns zoals gradients
        // TODO add toString method
    }
}
