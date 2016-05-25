using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

/// <summary>
/// Namespace for Extensions of existing objects
/// </summary>
namespace ExStrataServer
{
    public static class Utilities
    {
        public static bool TryParseJObject(string data, out JObject result)
        {
            try
            {
                result = JObject.Parse(data);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
}
