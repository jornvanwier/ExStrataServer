using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vtortola.WebSockets;
using Newtonsoft.Json.Linq;

namespace ExStrataServer.Communication.Server
{
    class MessageParser
    {
        public static bool Parse(string text, Action<WebSocket>)
        {
            JObject data;
            if (!Utilities.TryParseJObject(text, out data)) return false;

            switch(((string)data["action"]).ToLower())
            {
                case "getloadedapis":
                    
            }
        }
    }
}
