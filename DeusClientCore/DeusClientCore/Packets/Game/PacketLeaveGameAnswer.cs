using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketLeaveGameAnswer : PacketAnswer
    {

        public PacketLeaveGameAnswer() : base(EPacketType.LeaveGameAnswer)
        {
        }

        public override ushort EstimateAnswerCurrentSerializedSize()
        {
            // nothing to do, already done in base class
            return 0;
        }

        public override void OnAnswerDeserialize(byte[] buffer, int index)
        {
            // nothing to do, already done in base class
        }

        public override byte[] OnAnswerSerialize()
        {
            // nothing to do, already done in base class
            return new byte[0];
        }
    }
}
