using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExStrataServer.ColourPattern;

namespace ExStrataServer.Communication
{
    public static class ExStrataAPI
    {
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Subscribe to LiveControl on the EX STRATA.
        /// </summary>
        /// <returns>The LiveControl token</returns>
        private static string SubscribeToLiveControl()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unsubscribe from LiveControl on the EX STRATA
        /// </summary>
        /// <param name="token">The LiveControl token</param>
        private static void UnsubscribeFromLiveControl(string token)
        {
            throw new NotImplementedException();
        }
    }
}
