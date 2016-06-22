using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExStrataServer.APIs;
using ExStrataServer.ColourPattern;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExStrataServer.Communication.Server
{
    class MessageParser
    {

        public static async Task<string> Parse(string text)
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
                        return GetAllAPIs(json);

                    case "addapi":
                        return AddAPI(json);

                    case "getpattern":
                        return GetPattern(json);

                    case "removeapi":
                        return RemoveAPI(json);

                    case "listlogs":
                        return ListLogs(json);

                    case "readlog":
                        return ReadLog(json);

                    case "directcontrol":
                        return await DirectControl(json);

                    default:
                        return JsonConvert.SerializeObject(new
                        {
                            success = false,
                            code = 400,
                            error = "Action doesn't exist."
                        });

                }
            }
            return String.Format(fieldMissing, "action");
        }

        private static string Login(JObject data)
        {
            JToken username;
            if (!data.TryGetValue("user", out username)) return String.Format(fieldMissing, "user");
            JToken password;
            if (!data.TryGetValue("pass", out password)) return String.Format(fieldMissing, "pass");

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
            return JsonConvert.SerializeObject(new
            {
                success = false,
                code = 400,
                error = "Username or password is invalid."
            });
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
                    instanceInfo = loadedAPIs[i].InstanceInfo,
                    displayDelay = loadedAPIs[i].DisplayDelay,
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

        private static string GetAllAPIs(JObject data)
        {
            if (!CheckToken(data)) return notAuthorized;

            Type[] types = APIManager.AllAPIs;

            List<object> values = new List<object>();

            for (int i = 0; i < types.Length; i++)
            {
                values.Add(new
                {
                    name = types[i].ToString().Split('.').Last().Substring(5),
                    index = i,
                    parameters = APIManager.AllParameters[i]
                });
            }

            return JsonConvert.SerializeObject(new
            {
                success = true,
                code = 200,
                data = values
            });
        }

        private static string AddAPI(JObject data)
        {
            if (CheckToken(data))
            {
                JToken index;
                if (data.TryGetValue("index", out index))
                {
                    int parsedIndex;
                    if (Int32.TryParse((string)index, out parsedIndex))
                    {
                        if (parsedIndex < 0 || parsedIndex >= APIManager.AllAPIs.Length) return invalidIndex;

                        JToken parameters;
                        if (data.TryGetValue("parameters", out parameters))
                        {
                            try
                            {
                                Parameter[] parsedParameters = parameters.ToObject<Parameter[]>();

                                if (APIManager.Add(parsedIndex, parsedParameters))
                                {
                                    return success;
                                }
                                return invalidParameters;
                            }
                            catch
                            {
                                return invalidParameters;
                            }
                        }
                        return String.Format(fieldMissing, "parameters");
                    }
                    return invalidIndex;
                }
                return String.Format(fieldMissing, "index");
            }
            return notAuthorized;
        }

        private static string GetPattern(JObject data)
        {
            JToken patternName;
            if (!data.TryGetValue("patternName", out patternName)) return String.Format(fieldMissing, "patternName");

            if (!APIManager.AllPatterns.ContainsKey((string)patternName)) return invalidIndex;

            return JsonConvert.SerializeObject(new
            {
                success = true,
                code = 200,
                data = APIManager.AllPatterns[(string)patternName].UnencodedSerialize()
            });
        }

        private static string RemoveAPI(JObject data)
        {
            JToken index;
            if (!data.TryGetValue("index", out index)) return String.Format(fieldMissing, "index");
            int parsedIndex;
            if (!Int32.TryParse((string) index, out parsedIndex)) return invalidIndex;

            if (APIManager.Remove(parsedIndex)) return success;
            return invalidIndex;
        }

        private static string ListLogs(JObject data)
        {
            if (!CheckToken(data)) return notAuthorized;

            return JsonConvert.SerializeObject(new
            {
                success = true,
                code = 200,
                logs = Log.List()
            });
        }

        private static string ReadLog(JObject data)
        {
            if (!CheckToken(data)) return notAuthorized;
            JToken filename;
            if (!data.TryGetValue("filename", out filename)) return String.Format(fieldMissing, "filename");
            string result = Log.Read((string)filename);

            if(result == String.Empty)
            {
                return JsonConvert.SerializeObject(new
                {
                    success = false,
                    code = 400,
                    error = "Could not read file."
                });
            }
            return JsonConvert.SerializeObject(new
            {
                success = true,
                code = 200,
                log = result
            });
        }

        private static async Task<string> DirectControl(JObject data)
        {
            if (!CheckToken(data)) return notAuthorized;

            JToken patternIndex;
            if (!data.TryGetValue("pattern", out patternIndex)) return String.Format(fieldMissing, "pattern");
            if (!APIManager.AllPatterns.ContainsKey((string) patternIndex)) return invalidIndex;

            Pattern pattern = APIManager.AllPatterns[(string)patternIndex];

            // Discard result
            if (await ExStrataAPI.PlayPattern(pattern))
            {
                Log.APISend("Direct Control", pattern.Name);
                return success;
            }
            Log.APISend("Direct Control", pattern.Name, false);
            return JsonConvert.SerializeObject(new
            {
                success = false,
                code = 400,
                error = "Failed to play pattern."
            });
        }

        private static bool CheckToken(JObject data)
        {
            return GetUser(data) != null;
        }

        private static User GetUser(JObject data)
        {
            JToken token;
            if (data.TryGetValue("token", out token))
            {
                return UserManager.CheckToken((string)token);
            }

            return null;
        }

        #region defaultMessages

        private static readonly string success = JsonConvert.SerializeObject(new
        {
            success = true,
            code = 200
        });

        private static readonly string notAuthorized = JsonConvert.SerializeObject(new
        {
            success = false,
            code = 400,
            error = "Token is not supplied or invalid."
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

        private static readonly string invalidParameters = JsonConvert.SerializeObject(new
        {
            success = false,
            code = 400,
            error = "Parameters are in wrong format."
        });
        #endregion
    }
}
