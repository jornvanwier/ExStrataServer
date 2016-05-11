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


        public static bool PlayPattern(Pattern pattern)
        {
            SubscribeToLiveControl();
            throw new NotImplementedException();
        }

        /// <summary>
        /// Subscribe to LiveControl on the EX STRATA.
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

                        return "";
                    }
                }
                else
                {
                    Log.AddError("SubscribeToLiveControl returned false");
                    return "";
                }
            }
            catch (Exception exception)
            {
                Log.AddError("Could not parse LiveControl token data: " + exception.Message);
                return "";
            }
        }

        /// <summary>
        /// Unsubscribe from LiveControl on the EX STRATA
        /// </summary>
        /// <param name="token">The LiveControl token</param>
        private static void UnsubscribeFromLiveControl(string token)
        {
            JObject json = new JObject(new { liveControlToken = token });

            Request.PostData(ExStrataAPIURI + "unsubscribe_from_live_control.php", json);
        }
    }
}
