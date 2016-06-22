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

        public static byte[] StringToBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string BytesToString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
