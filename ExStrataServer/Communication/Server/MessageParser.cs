﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vtortola.WebSockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ExStrataServer.APIs;
using ExStrataServer.ColourPattern;

namespace ExStrataServer.Communication.Server
{
    class MessageParser
    {

        public static string Parse(string text)
        {
            JObject json;
            if (!Utilities.TryParseJObject(text, out json))
            {
                return invalidJson;
            }

            switch (((string)json["action"]).ToLower())
            {
                case "getloadedapis":
                    return GetLoadedAPIs();

                case "getpattern":
                    JToken data;
                    if (json.TryGetValue("data", out data))
                    {
                        return GetPattern(data);
                    }
                    else return String.Format(fieldMissing, "data");

                default:
                    return String.Format(fieldMissing, "action");

            }
        }

        private static string GetLoadedAPIs()
        {
            List<APIWatcher> loadedAPIs = APIManager.LoadedAPIs;

            List<object> values = new List<object>();

            for (int i = 0; i < loadedAPIs.Count; i++)
            {
                values.Add(new
                {
                    name = loadedAPIs[i].Name,
                    description = loadedAPIs[i].Description,
                    index = i
                });
            }

            return JsonConvert.SerializeObject(new
            {
                success = true,
                code = 200,
                data = values
            });
        }

        private static string GetPattern(JToken data)
        {
            if (!APIManager.AllPatterns.ContainsKey((string)data)) return invalidIndex;

            return JsonConvert.SerializeObject(new
            {
                success = true,
                code = 200,
                data = APIManager.AllPatterns[(string)data].UnencodedSerialize()
            });
        }

        #region defaultErrors

        private static readonly string invalidJson = JsonConvert.SerializeObject(JObject.FromObject(new
        {
            success = false,
            code = 400,
            error = "JSON was invalid."
        }));

        private static readonly string fieldMissing = JsonConvert.SerializeObject(JObject.FromObject(new
        {
            success = false,
            code = 400,
            error = "field {0} was missing."
        }));

        private static readonly string invalidIndex = JsonConvert.SerializeObject(new
        {
            success = false,
            code = 400,
            error = "Invalid index."
        });
        #endregion
    }
}
