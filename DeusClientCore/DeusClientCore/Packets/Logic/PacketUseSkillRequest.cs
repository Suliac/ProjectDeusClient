using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public class PacketUseSkillRequest : Packet
    {
        public uint SkillId { get; set; }

        public PacketUseSkillRequest() : base(EPacketType.UseSkillRequest)
        {
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            return sizeof(uint);
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            uint skill = 0;
            Serializer.DeserializeData(buffer, ref index, out skill);
            SkillId = skill;
        }

        public override byte[] OnSerialize()
        {
            return Serializer.SerializeData(SkillId);
        }
    }
}
