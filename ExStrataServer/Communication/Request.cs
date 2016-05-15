using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;




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
            else
            {
                // We don't need the result of this call
                Task delayTask = GetResponseAsync(request);
                return String.Empty;
            }
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
            else Log.Error(String.Format("Could not get response: ({0}) {1}",
                ((HttpWebResponse)e.Response).StatusCode,
                ((HttpWebResponse)e.Response).StatusDescription));
        }
    }
}
