using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExStrataServer.ColourPattern;
using Newtonsoft.Json.Linq;

namespace ExStrataServer.Communication
{
    public static class ExStrataAPI
    {
        private const string exStrataAPIURI = "http://exstrata.nl/control/api/";
        private const string applicationKey = "ExStrataAPIWatcher";

        private readonly static DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /*  
         For liveControl:
         Subscribe to livecontrol
         Check if we are in queue
            if so, immediately unsubscribe
         If not in queue, play the pattern
         Unsubscribe from livecontrol

         By doing this the server wont unnecessarily block the livecontrol, and we dont need to worry about keeping our token stored
         */

        /// <summary>
        /// Play a pattern on the EX STRATA
        /// </summary>
        /// <param name="pattern">The pattern to be played</param>
        /// <returns>Wether the pattern was successfully played</returns>
        public static bool PlayPattern(Pattern pattern)
        {
            string token = SubscribeToLiveControl();
            Console.WriteLine(token);

            if (token != String.Empty)
            {
                string data = FormatPattern(token, pattern);

                Console.WriteLine(Request.PostJSON(exStrataAPIURI + "play_pattern.php", data, "application/x-www-form-urlencoded"));

                UnsubscribeFromLiveControl(token);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Subscribe to LiveControl on the EX STRATA
        /// </summary>
        /// <returns>The LiveControl token</returns>
        private static string SubscribeToLiveControl()
        {
            string data = Request.GetData(exStrataAPIURI + "subscribe_to_live_control.php");

            Console.WriteLine(data);

            JObject parsedData;
            if(ExtensionMethods.Extensions.TryParseJObject(data, out parsedData))
            {
                if ((bool)parsedData["result"])
                {
                    string token = (string)parsedData["liveControlToken"];

                    if ((int)parsedData["liveControlQueuePosition"] == 0)
                    {
                        return token;
                    }
                    else
                    {
                        Log.AddError("LiveControl queue is not empty");
                        UnsubscribeFromLiveControl(token);

                        return String.Empty;
                    }
                }
                else
                {
                    Log.AddError("SubscribeToLiveControl returned false");
                    return String.Empty;
                }
            }
            else
            {
                Log.AddError("Could not parse LiveControl token data");
                return String.Empty;
            }
        }

        /// <summary>
        /// Unsubscribe from LiveControl on the EX STRATA
        /// </summary>
        /// <param name="token">The LiveControl token</param>
        private static string UnsubscribeFromLiveControl(string token)
        {
            JObject json = JObject.FromObject(new { liveControlToken = token });

            string result = Request.PostJSON(exStrataAPIURI + "unsubscribe_from_live_control.php", json);
            Console.WriteLine(result);
            return result;
        }

        private static string FormatPattern(string token, Pattern pattern)
        {
            if (token == String.Empty) throw new ArgumentException();

            return String.Format("liveControlToken={0}&{1}&applicationKey={2}", token, pattern.Serialize(), applicationKey);
        }
    }
}
