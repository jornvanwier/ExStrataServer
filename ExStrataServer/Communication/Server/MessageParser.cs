using System;
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

            JToken action;
            if (json.TryGetValue("action", out action))
            {
                switch (((string)json["action"]).ToLower())
                {
                    case "login":
                        return Login(json);

                    case "getloadedapis":
                        return GetLoadedAPIs();

                    case "getallapis":
                        return GetAllAPIs();

                    case "getpattern":
                        return GetPattern(json);

                    case "removeapi":
                        return RemoveAPI(json);

                    default:
                        return JsonConvert.SerializeObject(new
                        {
                            success = false,
                            code = 400,
                            error = "Action doesn't exist."
                        });

                }
            }
            else return String.Format(fieldMissing, "action");
        }

        private static string Login(JObject data)
        {
            JToken username;
            if (data.TryGetValue("user", out username))
            {
                JToken password;
                if (data.TryGetValue("pass", out password))
                {
                    Token token = UserManager.Authenticate((string)username, (string)password);

                    if (token != null)
                    {
                        return JsonConvert.SerializeObject(new
                        {
                            success = true,
                            code = 200,
                            token = token.Code,
                            displayName = token.User.Displayname
                        });
                    }
                    else return JsonConvert.SerializeObject(new
                    {
                        success = false,
                        code = 400,
                        error = "Username or password is invalid."
                    });
                }
                else return String.Format(fieldMissing, "pass");
            }
            else return String.Format(fieldMissing, "user");
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

        private static string GetAllAPIs()
        {
            Type[] types = APIManager.AllAPIs;

            List<object> values = new List<object>();

            for (int i = 0; i < types.Length; i++)
            {
                values.Add(new
                {
                    name = types[i].ToString().Split('.').Last().Substring(5),
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

        private static string GetPattern(JObject data)
        {
            JToken patternName;
            if (data.TryGetValue("patternName", out patternName))
            {
                if (!APIManager.AllPatterns.ContainsKey((string)patternName)) return invalidIndex;

                return JsonConvert.SerializeObject(new
                {
                    success = true,
                    code = 200,
                    data = APIManager.AllPatterns[(string)patternName].UnencodedSerialize()
                });
            }

            else return String.Format(fieldMissing, "patternName");
        }

        private static string RemoveAPI(JObject data)
        {
            JToken index;
            if (data.TryGetValue("index", out index))
            {
                int parsedIndex;
                if (Int32.TryParse((string)index, out parsedIndex))
                {
                    APIManager.Remove(parsedIndex);
                    return success;
                }
                else return JsonConvert.SerializeObject(new
                {
                    success = false,
                    code = 400,
                    error = "Index was not an integer."
                });
            }
            else return String.Format(fieldMissing, "index");
        }

        #region defaultMessages

        private static readonly string success = JsonConvert.SerializeObject(new
        {
            success = true,
            code = 200
        });

        private static readonly string invalidJson = JsonConvert.SerializeObject(new
        {
            success = false,
            code = 400,
            error = "JSON was invalid."
        });

        private static readonly string fieldMissing = JsonConvert.SerializeObject(new
        {
            success = false,
            code = 400,
            error = "field {0} was missing."
        });

        private static readonly string invalidIndex = JsonConvert.SerializeObject(new
        {
            success = false,
            code = 400,
            error = "Invalid index."
        });
        #endregion
    }
}
