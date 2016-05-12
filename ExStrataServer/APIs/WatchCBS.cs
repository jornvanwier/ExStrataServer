using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ExStrataServer.APIs;
using ExStrataServer.Communication;

namespace ExStrataServer.APIs
{
    class WatchCBS : APIWatcher
    {
        private static string name = "CBS";

        public WatchCBS(int delay) : base(delay, name)
        {
        }

        protected override void Check(object Sender = null, EventArgs e = null)
        {
            JObject result;
            string URI = String.Format("https://cbs.nl/nl-nl/visualisaties/bevolkingsteller/-/media/cbs/Infographics/Bevolkingsteller/{0}_{1}_{2}.json", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string JSON = "{'data':" + Request.GetData(URI) + "}";
            if (ExtensionMethods.Extensions.TryParseJObject(JSON, out result))
            {
                int born=0,
                    died=0;

                JToken stats = result["data"];
                for (int i = 1; i < stats.Count(); i++)
                {
                    int delta = (int)stats[i] - (int)stats[i - 1];
                    if (delta > 0)
                    {
                        born += delta;
                    }else if (delta < 0)
                    {
                        died -= delta;
                    }
                }
                
                Console.WriteLine("born: "+born+" died: "+died);
            }
            else
            {
                Log.AddError("Could not parse CBS data: ");
                return;
            }
        }
    }
}
