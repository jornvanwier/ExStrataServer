﻿using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace ExStrataUWP.ColourPattern
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

        public int Length
        {
            get { return frames.Count; }
        }

        public Pattern(string name, int delay, List<Frame> frames)
        {
            Name = name;
            Delay = delay;

            this.frames = frames;
            if (frames.Last() != Frame.Empty)
            {
                frames.Add(Frame.Empty);
            }
        }

        public Pattern(string name, int delay)
        {
            Name = name;
            Delay = delay;

            frames = new List<Frame>() { Frame.Empty };

        }

        public void Add(Frame frame)
        {
            frames.Insert(frames.Count-1, frame);
        }

        public struct GradientFrame
        {
            public int percentage;
            public Frame frame;
            public GradientFrame(int percentage, Frame frame)
            {
                this.percentage = percentage;
                this.frame = frame;
            }
        }
        public static Pattern Animate(GradientFrame[] frames, string name, int delay, int length)
        {
            Pattern result = new Pattern(name, delay);
            for (int i = 0; i < frames.Length - 1; i++)
            {
                float percentage = (float)(frames[i + 1].percentage - frames[i].percentage) / 100;
                int framesToNextPoint = (int)((length - 1) * percentage);

                List<float> rColorDifs = new List<float>(),
                    gColorDifs = new List<float>(),
                    bColorDifs = new List<float>();

                for (int j = 0; j < frames[i].frame.Colours.Length; j++)
                {
                    rColorDifs.Add((frames[i + 1].frame.Colours[j].R - frames[i].frame.Colours[j].R) / (float)framesToNextPoint);
                    gColorDifs.Add((frames[i + 1].frame.Colours[j].G - frames[i].frame.Colours[j].G) / (float)framesToNextPoint);
                    bColorDifs.Add((frames[i + 1].frame.Colours[j].B - frames[i].frame.Colours[j].B) / (float)framesToNextPoint);
                }

                for (int k = 0; k < framesToNextPoint; k++)
                {
                    List<Colour> colours = new List<Colour>();
                    for (int l = 0; l < rColorDifs.Count; l++)
                    {
                        colours.Add(new Colour(
                            Convert.ToByte(frames[i].frame.Colours[l].R + (rColorDifs[l] * k)),
                            Convert.ToByte(frames[i].frame.Colours[l].G + (gColorDifs[l] * k)),
                            Convert.ToByte(frames[i].frame.Colours[l].B + (bColorDifs[l] * k))
                            ));
                    }
                    result.Add(new Frame(colours.ToArray()));
                }
                result.Add(frames[i + 1].frame);
            }
            return result;
        }

        public string Serialize()
        {
            string result = "", ampersand = "";
            for (int i = 0; i < frames.Count; i++)
            {
                if(i > 0) ampersand = "&";

                result += ampersand + WebUtility.UrlEncode("pattern[frames][" + i + "][ms]") + '=' + (Delay * i);
                result += frames[i].Serialize(i);
            }
            return result + "";
        }
        public string UnencodedSerialize()
        {
            string result = "", ampersand = "";
            for (int i = 0; i < frames.Count; i++)
            {
                if (i > 0) ampersand = "&";

                result += ampersand + "pattern[frames][" + i + "][ms]" + '=' + (Delay * i);
                result += frames[i].UnencodedSerialize(i);
            }
            return result + "";
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        
        public void Save()
        {
            string defaultLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Patterns");

            if (!Directory.Exists(defaultLocation))
            {
                Directory.CreateDirectory(defaultLocation);
            }

            using (StreamWriter sw = File.AppendText(defaultLocation + "/" + Name + ".pattern"))
            {
                sw.WriteLine(UnencodedSerialize());
            }
        }



        // TODO add methods for defining pretty patterns zoals gradients
        // TODO add toString method
    }
}
