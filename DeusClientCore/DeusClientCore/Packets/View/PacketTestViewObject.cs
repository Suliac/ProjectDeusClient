using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketTestViewObject : Packet
    {
        EObjectType ObjectType { get; set; }

        public PacketTestViewObject() : base(EPacketType.TestViewObject)
        {
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
