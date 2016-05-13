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
        public static string GetData(string url)
        {
            WebRequest request = WebRequest.Create(url);

            return GetResponse(request);
        }

        public static string PostJSON(string url, string data, string ContentType = "application/json")
        {
            WebRequest request = WebRequest.Create(url);
            request.ContentType = ContentType;
            request.Method = "POST";

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(data);
                writer.Flush();
            }

            return GetResponse(request);
        }

        public static string PostJSON(string url, JObject data, string ContentType = "application/json")
        {
            return PostJSON(url, data.ToString(), ContentType);
        }

        private static string GetResponse(WebRequest request)
        {
            try
            {
                string result;
                using (WebResponse response = request.GetResponse())
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
                if (e.Response == null) Log.Error("Could not get response: " + e.Message);
                else Log.Error(String.Format("Could not get response: ({0}) {1}", 
                    ((HttpWebResponse)e.Response).StatusCode, 
                    ((HttpWebResponse)e.Response).StatusDescription));

                return String.Empty;
            }
        }
    }
}
