using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vtortola.WebSockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ExStrataServer.APIs;

namespace ExStrataServer.Communication.Server
{
    class MessageParser
    {
        public static string Parse(string text, Action<WebSocket> action)
        {
            GetLoadedAPIs();

            JObject data;
            if (!Utilities.TryParseJObject(text, out data))
            {
                return JsonConvert.SerializeObject(JObject.FromObject(new
                {
                    success = false,
                    code = 400,
                    error = "Data was not JSON."
                }));
            }

            switch (((string)data["action"]).ToLower())
            {
                case "getloadedapis":
                    return GetLoadedAPIs();

                default:
                    return JsonConvert.SerializeObject(JObject.FromObject(new
                    {
                        success = false,
                        code = 400,
                        error = "Action does not exist."
                    }));

            }
        }

        private static string GetLoadedAPIs()
        {
            List<APIWatcher> loadedAPIs = APIManager.LoadedAPIs;

            Dictionary<int, string> apiIndexName = new Dictionary<int, string>();

            for (int i = 0; i < loadedAPIs.Count; i++)
            {
                apiIndexName.Add(i, loadedAPIs[i].Name);
            }

            return JsonConvert.SerializeObject(JObject.FromObject(new
            {
                success = true,
                code = 200,
                data = apiIndexName
            }));
        }
    }
}
