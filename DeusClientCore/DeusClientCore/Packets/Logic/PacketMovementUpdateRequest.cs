using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketMovementUpdateRequest : Packet
    {
        public DeusVector2 DirMovement { get; set; }

        public PacketMovementUpdateRequest() : base(EPacketType.UpdateMovementRequest)
        {
        }

        public PacketMovementUpdateRequest(DeusVector2 dir) : this()
        {
            DirMovement = dir;
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            return DirMovement.EstimateCurrentSerializedSize();
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            Serializer.DeserializeData(buffer, ref index, DirMovement);
        }

        public override byte[] OnSerialize()
        {
            return Serializer.SerializeData(DirMovement);
        }
    }
}
