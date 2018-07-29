using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketJoinGameRequest : Packet
    {
        public uint GameJoinedId { get; set; }

        public PacketJoinGameRequest() : base(EPacketType.JoinGameRequest)
        {
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            return sizeof(uint);
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            uint gameId = 0;
            Serializer.DeserializeData(buffer, ref index, out gameId);
            GameJoinedId = gameId;
        }

        public override byte[] OnSerialize()
        {
            return Serializer.SerializeData(GameJoinedId);
        }
    }
}
