using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketHealthUpdate : Packet
    {
        public uint ObjectId { get; private set; }

        public uint ComponentId { get; private set; }

        public int NewHealthAmount { get; private set; }
        public uint NewHealthTimestamp { get; private set; }

        public PacketHealthUpdate() : base(EPacketType.UpdateHealth)
        {
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            return sizeof(uint) + sizeof(uint) + sizeof(int) + sizeof(uint);
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            uint objectId = 0;
            Serializer.DeserializeData(buffer, ref index, out objectId);
            ObjectId = objectId;

            uint componentId = 0;
            Serializer.DeserializeData(buffer, ref index, out componentId);
            ComponentId = componentId;

            int health = 0;
            Serializer.DeserializeData(buffer, ref index, out health);
            NewHealthAmount = health;

            uint healthMs = 0;
            Serializer.DeserializeData(buffer, ref index, out healthMs);
            NewHealthTimestamp = healthMs;
        }

        public override byte[] OnSerialize()
        {
            List<byte> tmpResult = new List<byte>();

            tmpResult.AddRange(Serializer.SerializeData(ObjectId));

            tmpResult.AddRange(Serializer.SerializeData(ComponentId));

            tmpResult.AddRange(Serializer.SerializeData(NewHealthAmount));

            return tmpResult.ToArray();
        }
    }
}
