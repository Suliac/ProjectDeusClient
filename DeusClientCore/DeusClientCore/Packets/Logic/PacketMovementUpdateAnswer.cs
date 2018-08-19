using DeusClientCore.Exceptions;
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
        public ulong OriginTimestampMs { get; set; }
        public DeusVector2 Destination { get; set; }
        public ulong DestinationTimestampMs { get; set; }
        
        public PacketMovementUpdateAnswer() : base(EPacketType.UpdateMovementAnswer)
        {
        }
        
        public override ushort EstimateCurrentSerializedSize()
        {
            return (ushort)(sizeof(uint) + sizeof(uint) 
                + (PositionOrigin.EstimateCurrentSerializedSize() + sizeof(ulong))
                + (Destination.EstimateCurrentSerializedSize() + sizeof(ulong)));
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

            // 3 - Origin
            Serializer.DeserializeData(buffer, ref index, PositionOrigin);

            // 5 - Timestamp origin
            ulong tmpSrcMs = 0;
            Serializer.DeserializeData(buffer, ref index, out tmpSrcMs);
            OriginTimestampMs = tmpSrcMs;

            // 5 - Destination
            Serializer.DeserializeData(buffer, ref index, Destination);

            // 5 - Timestamp destination
            ulong tmpDestMs = 0;
            Serializer.DeserializeData(buffer, ref index, out tmpDestMs);
            DestinationTimestampMs = tmpDestMs;
        }

        public override byte[] OnSerialize()
        {
            throw new DeusException("Don't try to serialize this");
        }
    }
}
