using DeusClientCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketMovementUpdateAnswer : PacketAnswer
    {
        public uint ObjectId { get; set; }
        public uint ComponentId { get; set; }
        public DeusVector2 PositionOrigin { get; set; }
        public uint OriginTimestampMs { get; set; }
        public DeusVector2 Destination { get; set; }
        public uint DestinationTimestampMs { get; set; }
        
        public PacketMovementUpdateAnswer() : base(EPacketType.UpdateMovementAnswer)
        {
        }
        
        public override ushort EstimateAnswerCurrentSerializedSize()
        {
            return (ushort)(sizeof(uint) + sizeof(uint) 
                + (PositionOrigin.EstimateCurrentSerializedSize() + sizeof(uint))
                + (Destination.EstimateCurrentSerializedSize() + sizeof(uint)));
        }

        public override void OnAnswerDeserialize(byte[] buffer, int index)
        {
            // 1 - ObjectId
            uint tmpObjectId = 0;
            Serializer.DeserializeData(buffer, ref index, out tmpObjectId);
            ObjectId = tmpObjectId;

            // 2 - ComponentId
            uint tmpComponentId = 0;
            Serializer.DeserializeData(buffer, ref index, out tmpComponentId);
            ComponentId = tmpComponentId;

            // 3 - Origin
            DeusVector2 origin = new DeusVector2();
            Serializer.DeserializeData(buffer, ref index, origin);
            PositionOrigin = origin;

            // 5 - Timestamp origin
            uint tmpSrcMs = 0;
            Serializer.DeserializeData(buffer, ref index, out tmpSrcMs);
            OriginTimestampMs = tmpSrcMs;

            // 5 - Destination
            DeusVector2 dest = new DeusVector2();
            Serializer.DeserializeData(buffer, ref index, dest);
            Destination = dest;

            // 5 - Timestamp destination
            uint tmpDestMs = 0;
            Serializer.DeserializeData(buffer, ref index, out tmpDestMs);
            DestinationTimestampMs = tmpDestMs;
        }

        public override byte[] OnAnswerSerialize()
        {
            throw new DeusException("Don't try to serialize this");
        }
    }
}
