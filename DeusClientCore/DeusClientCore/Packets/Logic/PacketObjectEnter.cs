using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketObjectEnter : Packet
    {
        // TODO : add cell hash

        public uint GameObjectId { get; set; }
        public EObjectType ObjectType { get; set; }

        public PacketObjectEnter() : base(EPacketType.ObjectEnter)
        {
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            // +1 for 1 byte for ObjectType(uint8_t)
            return sizeof(uint) + 1;
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            uint objectId;
            Serializer.DeserializeData(buffer, ref index, out objectId);
            GameObjectId = objectId;

            // Deerialize id
            EPacketType type = (EPacketType)((int)buffer[index]);
            index++;
        }

        public override byte[] OnSerialize()
        {
            List<byte> result = new List<byte>();

            // serialize id
            result.AddRange(Serializer.SerializeData(GameObjectId));

            // serialize object type
            result.Add((byte)ObjectType);

            return result.ToArray();
        }
    }
}
