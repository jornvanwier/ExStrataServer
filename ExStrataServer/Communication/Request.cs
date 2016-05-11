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
            string result;
            WebRequest request = WebRequest.Create(url);
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
            }

            return result;
        }

        public static string PostData(string url, string data)
        {
            throw new NotImplementedException();
        }

        public static string PostData(string url, JObject data)
        {
            return PostData(url, data.ToString());
        }
    }
}
