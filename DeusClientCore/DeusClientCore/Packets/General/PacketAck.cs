using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketAck: Packet
    {
        public uint PacketIdToAck { get; set; }

        public PacketAck() : base(EPacketType.Ack)
        {
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            return sizeof(uint);
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            uint id = 0;
            Serializer.DeserializeData(buffer, ref index, out id);
            PacketIdToAck = id;
        }

        public override byte[] OnSerialize()
        {
            return Serializer.SerializeData(PacketIdToAck);
        }
    }
}
