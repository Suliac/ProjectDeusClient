using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketMovementUpdateRequest : Packet
    {
        public DeusVector2 DestinationPos { get; set; }
        public uint ComponentId{ get; set; }

        public PacketMovementUpdateRequest() : base(EPacketType.UpdateMovementRequest)
        {
        }

        public PacketMovementUpdateRequest(DeusVector2 dest, uint componentId) : this()
        {
            DestinationPos = dest;
            ComponentId = componentId;
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            return (ushort)(sizeof(uint) + DestinationPos.EstimateCurrentSerializedSize());
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            uint componentId = 0;
            Serializer.DeserializeData(buffer, ref index, out componentId);
            ComponentId = componentId;

            Serializer.DeserializeData(buffer, ref index, DestinationPos);
        }

        public override byte[] OnSerialize()
        {
            List<byte> result = new List<byte>();

            result.AddRange(Serializer.SerializeData(ComponentId));
            result.AddRange(Serializer.SerializeData(DestinationPos));

            return result.ToArray(); 
        }
    }
}
