using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketPingAnswer : Packet
    {
        public uint AnswerToPacketId { get; set; }

        public PacketPingAnswer() : base(EPacketType.PingAnswer)
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
            AnswerToPacketId = tmpTime;
        }

        public override byte[] OnSerialize()
        {
            return Serializer.SerializeData(AnswerToPacketId);
        }
    }
}
