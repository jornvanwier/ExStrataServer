using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExStrataServer.ColourPattern;

namespace ExStrataServer.APIs
{
    public class APIManager
    {
        private static List<APIWatcher> loadedAPIs;
        private static readonly Type[] allAPIs = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(assembly => assembly.GetTypes())
                       .Where(type => type.IsSubclassOf(typeof(APIWatcher))).ToArray();
        private static Dictionary<string, Pattern> allPatterns;
        private static List<List<Parameter>> allParameters;

        public static List<APIWatcher> LoadedAPIs
        {
            get { return loadedAPIs; }
            private set { loadedAPIs = value; }
        }

        public static Type[] AllAPIs
        {
            get { return allAPIs; }
        }

        public static Dictionary<string, Pattern> AllPatterns
        {
            get { return allPatterns; }
            private set { allPatterns = value; }
        }

        public static List<List<Parameter>> AllParameters
        {
            get { return allParameters; }
            private set { allParameters = value; }
        }

        public static void Initialize(params APIWatcher[] apis)
        {
            SetPatternsAndParameters();

            LoadedAPIs = apis.ToList();
            StartAll();
            Log.Message("API Manager started.");
        }

        private static void StartAll()
        {
            foreach (APIWatcher api in LoadedAPIs)
            {
                api.Start();
                Log.Message("Started " + api.Name);
            }
        }

        public static void Add(APIWatcher api)
        {
            Log.Message("Adding API " + api.Name);
            api.Start();
            LoadedAPIs.Add(api);
        }

        public static bool Add(int index, Parameter[] parameters)
        {
            try
            {
                object[] parsedParams = ParseParameter(parameters);
                if (parsedParams.Contains(null))
                    return false;
                APIWatcher api = (APIWatcher)Activator.CreateInstance(AllAPIs[index], parsedParams);
                Add(api);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Remove(int index)
        {
            if (index > 0 && index < LoadedAPIs.Count)
            {
                Log.Message("Removing API " + LoadedAPIs[index].Name);
                LoadedAPIs[index].Dispose();
                LoadedAPIs.RemoveAt(index);
                return true;
            } else return false;
        }

        public static void Dispose()
        {
            Dispose(true);
        }

        private static void Dispose(bool disposing)
        {
            for (int i = 0; i < LoadedAPIs.Count; i++)
            {
                LoadedAPIs[i].Dispose();
                LoadedAPIs = null;
            }
        }

        private static void SetPatternsAndParameters()
        {
            AllPatterns = new Dictionary<string, Pattern>();
            AllParameters = new List<List<Parameter>>();

            for (int i = 0; i < AllAPIs.Length; i++)
            {
                APIWatcher watcher = (APIWatcher)Activator.CreateInstance(AllAPIs[i]);
                AllPatterns.Add(watcher.Name, watcher.GetPattern());
                AllParameters.Add(watcher.Parameters);
            }
        }

        private static object[] ParseParameter(Parameter[] parameters)
        {
            List<object> result = new List<object>();

            for (int i = 0; i < parameters.Length; i++)
            {
                switch (parameters[i].Type)
                {
                    case "int":
                        int iResult;
                        if (Int32.TryParse(parameters[i].Value, out iResult))
                            result.Add(iResult);
                        else
                            return null;
                        break;

                    case "colour":
                    case "string":
                        result.Add(parameters[i].Value);
                        break;

                    case "bool":
                        bool bResult;
                        if (Boolean.TryParse(parameters[i].Value, out bResult))
                            result.Add(bResult);
                        else
                            return null;
                        break;

                    default:
                        return null;
                }
            }
            return result.ToArray();
        }
        
    }
}
