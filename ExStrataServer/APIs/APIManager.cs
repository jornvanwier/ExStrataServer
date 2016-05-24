using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExStrataServer.APIs
{
    public class APIManager : IDisposable
    {
        private List<APIWatcher> loadedAPIs;
        private readonly Type[] allAPIs = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(assembly => assembly.GetTypes())
                       .Where(type => type.IsSubclassOf(typeof(APIWatcher))).ToArray();

        public List<APIWatcher> LoadedAPIs
        {
            get { return loadedAPIs; }
            private set { loadedAPIs = value; }
        }

        public APIManager(params APIWatcher[] apis)
        {
            LoadedAPIs = apis.ToList();
            StartAll();
            Log.Message("API Manager started.");
        }

        private void StartAll()
        {
            foreach(APIWatcher api in LoadedAPIs)
            {
                api.Start();
                Log.Message("Started " + api.Name);
            }
        }

        public void Add(APIWatcher api)
        {
            api.Start();
            LoadedAPIs.Add(api);
        }

        public void Remove(int index)
        {
            LoadedAPIs[index].Dispose();
            LoadedAPIs.RemoveAt(index);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            for (int i = 0; i < LoadedAPIs.Count; i++)
            {
                LoadedAPIs[i].Dispose();
                LoadedAPIs = null;
            }
        }
    }
}
