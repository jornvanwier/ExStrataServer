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

         By doing this the server wont unnecessarily block the livecontrol, and we dont need to worry about keeping our token stored.
         */

        /// <summary>
        /// Play a pattern on the EX STRATA.
        /// </summary>
        /// <param name="pattern">The pattern to be played.</param>
        /// <returns>Whether the pattern was successfully played.</returns>
        public static async Task<bool> PlayPattern(Pattern pattern)
        {
            if (pattern.Length > 15)
            {
                Log.Error("Pattern cannot be longer than 14 frames");
                pattern.Frames = pattern.Frames.GetRange(0, 15);
            }

            string token = await SubscribeToLiveControl();

            if (token != String.Empty)
            {
                string serializedPattern = FormatPattern(token, pattern);


                /* 
                For some reason, play_pattern doesn't want to give a response when playing the pattern.
                Because of this, we send the request and don't wait for any response. 
                This leaves the code unable to determine if the pattern was played successfully, sadly.

                In the past, the responses did work. Further investigation is required to determine what caused the sudden disappearance of these responses.
                */
                await Request.PostDataAsync(exStrataAPIURI + "play_pattern.php", serializedPattern, waitForResponse: false, contentType: "application/x-www-form-urlencoded");

                await UnsubscribeFromLiveControl(token);
                return true;
            }
            else
            {
                Log.Error("Could not obtain LiveControl token");
                return false;
            }
        }

        /// <summary>
        /// Subscribe to LiveControl on the EX STRATA.
        /// </summary>
        /// <returns>The LiveControl token.</returns>
        private static async Task<string> SubscribeToLiveControl()
        {
            string data = await Request.GetDataAsync(exStrataAPIURI + "subscribe_to_live_control.php");

            Log.RawData(data);

            JObject parsedData;
            if(Utilities.TryParseJObject(data, out parsedData))
            {
                if ((bool)parsedData["result"])
                {
                    string token = (string)parsedData["liveControlToken"];

                    if ((int)parsedData["liveControlQueuePosition"] == 0)
                    {
                        Log.Message("Obtained token: " + token);

                        return token;
                    }
                    else
                    {
                        Log.Error("LiveControl queue is not empty");
                        await UnsubscribeFromLiveControl(token);

                        return String.Empty;
                    }
                }
                else
                {
                    Log.Error("SubscribeToLiveControl returned false");
                    return String.Empty;
                }
            }
            else
            {
                Log.Error("Could not parse LiveControl token data");
                return String.Empty;
            }
        }

        /// <summary>
        /// Unsubscribe from LiveControl on the EX STRATA.
        /// </summary>
        /// <param name="token">The LiveControl token.</param>
        private static async Task<string> UnsubscribeFromLiveControl(string token)
        {
            JObject json = JObject.FromObject(new
            {
                liveControlToken = token
            });

            string result = await Request.PostDataAsync(exStrataAPIURI + "unsubscribe_from_live_control.php", json);
            Log.RawData(result);
            return result;
        }

        private static string FormatPattern(string token, Pattern pattern)
        {
            if (token == String.Empty) throw new ArgumentException("Token is invalid.");

            return String.Format("liveControlToken={0}&applicationKey={2}&{1}", token, pattern.Serialize(), applicationKey);
        }
    }
}
