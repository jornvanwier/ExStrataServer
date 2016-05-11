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

        public static string PostJSON(string url, string data)
        {
            WebRequest request = WebRequest.Create(url);
            request.ContentType = "application/json";
            request.Method = "POST";

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(data);
            }

            return GetResponse(request);
        }

        public static string PostJSON(string url, JObject data)
        {
            return PostJSON(url, data.ToString());
        }

        private static string GetResponse(WebRequest request)
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
    }
}
