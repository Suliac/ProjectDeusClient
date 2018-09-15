using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public static class Parameters
    {
        //public static int DEFAULT_LOCAL_LAG_MS = 200;
        
        /// <summary>
        /// Try each N milliseconds to resend packet if the pcket isn't acked
        /// </summary>
        public const double PACKET_DELAY_CHECK_ACK_MS = 100;

        public const uint PING_NUMBER_PACKET = 4;
    }
}
