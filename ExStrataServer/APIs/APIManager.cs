﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExStrataServer.APIs
{
    public class APIManager
    {
        private static List<APIWatcher> loadedAPIs;
        private static readonly Type[] allAPIs = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(assembly => assembly.GetTypes())
                       .Where(type => type.IsSubclassOf(typeof(APIWatcher))).ToArray();

        public static List<APIWatcher> LoadedAPIs
        {
            get { return loadedAPIs; }
            private set { loadedAPIs = value; }
        }

        public static void Initialize(params APIWatcher[] apis)
        {
            LoadedAPIs = apis.ToList();
            StartAll();
            Log.Message("API Manager started.");
        }

        private static void StartAll()
        {
            foreach(APIWatcher api in LoadedAPIs)
            {
                api.Start();
                Log.Message("Started " + api.Name);
            }
        }

        public static void Add(APIWatcher api)
        {
            api.Start();
            LoadedAPIs.Add(api);
        }

        public static void Remove(int index)
        {
            LoadedAPIs[index].Dispose();
            LoadedAPIs.RemoveAt(index);
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
    }
}
