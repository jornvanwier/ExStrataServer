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
                    instanceInfo = loadedAPIs[i].InstanceInfo,
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
            if (CheckToken(data))
            {
                Type[] types = APIManager.AllAPIs;
                List<List<Parameter>> parameters = APIManager.AllParameters;

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
            else return notAuthorized;
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
                                else return invalidParameters;
                            }
                            catch
                            {
                                return invalidParameters;
                            }
                        }
                        else return String.Format(fieldMissing, "parameters");
                    }
                    else return invalidIndex;
                }
                else return String.Format(fieldMissing, "index");
            }
            else return notAuthorized;
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

        private static string ListLogs(JObject data)
        {
            if (CheckToken(data))
            {
                return JsonConvert.SerializeObject(new
                {
                    success = true,
                    code = 200,
                    logs = Log.List()
                });
            }
            else return notAuthorized;
        }

        private static string ReadLog(JObject data)
        {
            if (CheckToken(data))
            {
                JToken filename;
                if (data.TryGetValue("filename", out filename))
                {
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
                    else
                    {
                        return JsonConvert.SerializeObject(new
                        {
                            success = true,
                            code = 200,
                            log = result
                        });
                    }
                }
                else return String.Format(fieldMissing, "filename");
            }
            else return notAuthorized;
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
