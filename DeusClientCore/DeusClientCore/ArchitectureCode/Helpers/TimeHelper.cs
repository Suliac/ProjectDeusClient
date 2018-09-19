using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public class TimeHelper
    {
        public static Dictionary<uint, uint> PingPacketNfo = new Dictionary<uint, uint>();
        public static uint PingPacketSent = 0;
        public static uint PingPacketRecv = 0;

        public static uint CurrentPing { get; set; }

        public static uint GetUnixMsTimeStamp()
        {
            return (uint)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
        }

    }
}
