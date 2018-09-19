using DeusClientCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketSyncClockAnswer : Packet
    {
        public uint DistantClockValue { get; set; }

        public PacketSyncClockAnswer() : base(EPacketType.ClockSyncAnswer)
        {
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            return sizeof(uint);
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            uint tmpTime = 0;
            Serializer.DeserializeData(buffer, ref index, out tmpTime);
            DistantClockValue = tmpTime;
        }

        public override byte[] OnSerialize()
        {
            // DON'T
            throw new DeusException("Don't try to serialize this");
            //return Serializer.SerializeData(DistantClockValue);
        }
    }
}
