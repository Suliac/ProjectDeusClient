using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketMovementUpdateAnswer : Packet
    {
        public uint ObjectId { get; set; }
        public uint ComponentId { get; set; }
        public DeusVector2 PositionOrigin { get; set; }
        public DeusVector2 DirMovement { get; set; }
        public long TimeStampMs { get; set; }

        public PacketMovementUpdateAnswer() : base(EPacketType.UpdateMovementAnswer)
        {
        }

        public PacketMovementUpdateAnswer(uint objectId, uint componentId, DeusVector2 posOrigin, DeusVector2 dir, long timeStampMs) : this()
        {
            ObjectId = objectId;
            ComponentId = componentId;
            PositionOrigin = posOrigin;
            DirMovement = dir;
            TimeStampMs = timeStampMs;
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            return (ushort)(sizeof(uint) + sizeof(uint) + PositionOrigin.EstimateCurrentSerializedSize() + DirMovement.EstimateCurrentSerializedSize() + sizeof(long));
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            // 1 - ObjectId
            uint tmpObjectId = 0;
            Serializer.DeserializeData(buffer, ref index, out tmpObjectId);
            ObjectId = tmpObjectId;

            // 2 - ComponentId
            uint tmpComponentId = 0;
            Serializer.DeserializeData(buffer, ref index, out tmpComponentId);
            ComponentId = tmpComponentId;

            // 3 - PositionOrigin
            Serializer.DeserializeData(buffer, ref index, PositionOrigin);
            
            // 4 - Dir
            Serializer.DeserializeData(buffer, ref index, DirMovement);

            // 5 - Timestamp
            long tmpMs = 0;
            Serializer.DeserializeData(buffer, ref index, out tmpMs);
            TimeStampMs = tmpMs;
        }

        public override byte[] OnSerialize()
        {
            List<byte> results = new List<byte>();

            // 1 - ObjectId
            results.AddRange(Serializer.SerializeData(ObjectId));

            // 2 - ComponentId
            results.AddRange(Serializer.SerializeData(ComponentId));

            // 3 - PositionOrigin
            results.AddRange(Serializer.SerializeData(PositionOrigin));

            // 4 - Dir
            results.AddRange(Serializer.SerializeData(DirMovement));

            // 5 - Timestamp
            results.AddRange(Serializer.SerializeData((long)TimeStampMs));

            return results.ToArray(); 
        }
    }
}
