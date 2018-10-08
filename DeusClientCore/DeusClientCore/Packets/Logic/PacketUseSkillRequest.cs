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

        public DeusVector2 SkillLaunchPosition { get; set; }

        public PacketUseSkillRequest(uint skillId, DeusVector2 skillPos) : base(EPacketType.UseSkillRequest)
        {
            SkillId = skillId;
            SkillLaunchPosition = skillPos;
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

            DeusVector2 pos = DeusVector2.Zero;
            Serializer.DeserializeData(buffer, ref index, out pos);
            SkillLaunchPosition = pos;
        }

        public override byte[] OnSerialize()
        {
            List<byte> result = new List<byte>();
            result.AddRange(Serializer.SerializeData(SkillId));
            result.AddRange(Serializer.SerializeData<ISerializable>(SkillLaunchPosition));

            return result.ToArray();
        }
    }
}
