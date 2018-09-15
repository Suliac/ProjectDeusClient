using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketPingAnswer : Packet
    {
        public uint SenderLocalTime { get; private set; }

        public PacketPingAnswer() : base(EPacketType.PingAnswer)
        {
            SenderLocalTime = TimeHelper.GetUnixMsTimeStamp();
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            return sizeof(uint);
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            uint tmpTime = 0;
            Serializer.DeserializeData(buffer, ref index, out tmpTime);
            SenderLocalTime = tmpTime;
        }

        public override byte[] OnSerialize()
        {
            return Serializer.SerializeData(SenderLocalTime);
        }
    }
}
