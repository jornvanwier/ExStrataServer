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

        public List<APIWatcher> LoadedAPIs
        {
            get { return loadedAPIs; }
        }

        public APIManager(params APIWatcher[] apis)
        {
            loadedAPIs = apis.ToList();
            Log.Message("API Manager started.");
        }

        public void StartAll()
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
            loadedAPIs.Add(api);
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
                loadedAPIs = null;
            }
        }
    }
}
