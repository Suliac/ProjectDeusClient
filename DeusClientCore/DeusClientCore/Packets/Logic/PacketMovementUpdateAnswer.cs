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
            throw new NotImplementedException();
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            throw new NotImplementedException();
        }

        public override byte[] OnSerialize()
        {
            throw new NotImplementedException();
        }
    }
}
