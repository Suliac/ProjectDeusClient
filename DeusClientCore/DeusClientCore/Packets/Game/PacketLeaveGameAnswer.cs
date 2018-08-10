using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketLeaveGameAnswer : PacketAnswer
    {
        public UInt32 PlayerId { get; private set; }

        public PacketLeaveGameAnswer() : base(EPacketType.LeaveGameAnswer)
        {
        }

        public PacketLeaveGameAnswer(UInt32 playerId) : base(EPacketType.LeaveGameAnswer)
        {
            PlayerId = playerId;
        }

        public override ushort EstimateAnswerCurrentSerializedSize()
        {
            return sizeof(UInt32);
        }

        public override void OnAnswerDeserialize(byte[] buffer, int index)
        {
            UInt32 tmpPlayerId = 0;
            Serializer.DeserializeData(buffer, ref index, out tmpPlayerId);
            PlayerId = tmpPlayerId;
        }

        public override byte[] OnAnswerSerialize()
        {
            return Serializer.SerializeData(PlayerId);
        }
    }
}
