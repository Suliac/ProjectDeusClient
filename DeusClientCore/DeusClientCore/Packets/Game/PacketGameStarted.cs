using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketGameStarted : Packet
    {
        public PacketGameStarted() : base(EPacketType.GameStarted)
        {
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            return 0;
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            // nothing to do
        }

        public override byte[] OnSerialize()
        {
            return new byte[0];
        }
    }
}
