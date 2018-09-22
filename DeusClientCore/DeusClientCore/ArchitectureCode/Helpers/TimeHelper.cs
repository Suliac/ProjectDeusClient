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

        /// <summary>
        /// First item : client time, second one : distant/server time
        /// </summary>
        private static Tuple<uint, uint> SyncNfos = new Tuple<uint, uint>(1, 1);

        public static uint CurrentPing { get; set; }

        public static uint GetUnixMsTimeStamp()
        {
            uint currentLocalTime = (uint)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

            //  Local time Saved    ->    Corresponding distant time
            //  Current local time  ->                ?
            long distantTime = (long)currentLocalTime * SyncNfos.Item2 / SyncNfos.Item1;
            return (uint)distantTime;
        }

        public static void Sync(uint localTime, uint distantTime)
        {
            SyncNfos = new Tuple<uint, uint>(localTime, distantTime + CurrentPing);
            Console.WriteLine($"Current localtime : {SyncNfos.Item1} | Distant : {SyncNfos.Item2} | Diff : {(long)SyncNfos.Item1 - (long)SyncNfos.Item2}");
        }
    }
}
