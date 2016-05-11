using System;
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
        private const string ExStrataAPIURI = "http://www.exstrata.nl/control/api/";

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

            Console.WriteLine(pattern.ToJSON());

            if (token != String.Empty)
            {
                JObject json = JObject.FromObject(new
                {
                    liveControlToken = token,
                    pattern = pattern.ToJSON()
                });

                Console.WriteLine(Request.PostJSON(ExStrataAPIURI + "play_pattern.php", json));

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
            string data = Request.GetData(ExStrataAPIURI + "subscribe_to_live_control.php");

            try
            {
                JObject parsedData = JObject.Parse(data);

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
            catch (Exception exception)
            {
                Log.AddError("Could not parse LiveControl token data: " + exception.Message);
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

            return Request.PostJSON(ExStrataAPIURI + "unsubscribe_from_live_control.php", json);
        }
    }
}
