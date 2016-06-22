using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ExStrataServer.Communication
{
    public static class Request
    {
        public static async Task<string> GetDataAsync(string url)
        {
            WebRequest request = WebRequest.Create(url);

            return await GetResponseAsync(request);
        }

        public static async Task<string> PostDataAsync(string url, string data, bool waitForResponse = true, string contentType = "application/json")
        {
            WebRequest request = WebRequest.Create(url);
            request.ContentType = contentType;
            request.Method = "POST";

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(data);
                writer.Flush();
            }

            if(waitForResponse) return await GetResponseAsync(request);
            // We don't need the result of this call
            Task delayTask = GetResponseAsync(request);
            return String.Empty;
        }

        public static async Task<string> PostDataAsync(string url, JObject data, bool waitForResponse = true, string contentType = "application/json")
        {
            return await PostDataAsync(url, data.ToString(), waitForResponse, contentType);
        }

        private static async Task<string> GetResponseAsync(WebRequest request)
        {
            try
            {
                string result;
                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                    }
                }

                return result;
            }
            catch(WebException e)
            {
                HandleWebException(e);

                return String.Empty;
            }
        }

        private static void HandleWebException(WebException e)
        {
            if (e.Response == null) Log.Error("Could not get response: " + e.Message);
            else Log.Error(
                $"Could not get response: ({((HttpWebResponse) e.Response).StatusCode}) {((HttpWebResponse) e.Response).StatusDescription}");
        }
    }
}
