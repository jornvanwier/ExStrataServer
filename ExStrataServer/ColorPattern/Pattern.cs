using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExStrataServer.ColorPattern
{
    public class Pattern
    {
        private List<Frame> frames;
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public List<Frame> Frames
        {
            get { return frames; }
        }

        public Pattern(string name, List<Frame> frames)
        {
            Name = name;

            this.frames = frames;
        }

        public Pattern(string name)
        {
            Name = name;

            frames = new List<Frame>();
        }

        public void Add(Frame frame)
        {
            frames.Add(frame);
        }

        // TODO add methods for defining pretty patterns
    }
}
