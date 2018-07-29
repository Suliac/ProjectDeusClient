using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketJoinGameAnswer : PacketAnswer
    {
        public uint GameJoinedId { get; set; }

        public PacketJoinGameAnswer() : base(EPacketType.JoinGameAnswer)
        {
        }

        public override ushort EstimateAnswerCurrentSerializedSize()
        {
            return sizeof(uint);
        }

        public override void OnAnswerDeserialize(byte[] buffer, int index)
        {
            uint gameId = 0;
            Serializer.DeserializeData(buffer, ref index, out gameId);
            GameJoinedId = gameId;
        }

        public override byte[] OnAnswerSerialize()
        {
            return Serializer.SerializeData(GameJoinedId);
        }
    }
}
