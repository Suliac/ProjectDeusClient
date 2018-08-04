using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketObjectLeave : Packet
    {
        // TODO : add cell hash ?

        public uint GameObjectId { get; set; }

        public PacketObjectLeave() : base(EPacketType.ObjectLeave)
        {
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            return sizeof(uint);
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            uint objectId;
            Serializer.DeserializeData(buffer, ref index, out objectId);
            GameObjectId = objectId;
        }

        public override byte[] OnSerialize()
        {
            List<byte> result = new List<byte>();

            // serialize id
            result.AddRange(Serializer.SerializeData(GameObjectId));
            
            return result.ToArray();
        }
    }
}
