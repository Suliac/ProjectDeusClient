using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketSyncClockRequest : Packet
    {
        public PacketSyncClockRequest() : base(EPacketType.ClockSyncRequest)
        {
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            // nothing to do here
            return 0;
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            // nothing to do here
        }

        public override byte[] OnSerialize()
        {
            // nothing to do here
            return new byte[0];
        }
    }
}
