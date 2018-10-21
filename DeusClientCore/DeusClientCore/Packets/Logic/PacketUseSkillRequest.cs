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

        public uint ComponentId { get; set; }

        public PacketUseSkillRequest(uint componentId, uint skillId, DeusVector2 skillPos) : base(EPacketType.UseSkillRequest)
        {
            ComponentId = componentId;
            SkillId = skillId;
            SkillLaunchPosition = skillPos;
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            return (ushort)(sizeof(uint) + sizeof(uint) + SkillLaunchPosition.EstimateCurrentSerializedSize());
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            uint componentId = 0;
            Serializer.DeserializeData(buffer, ref index, out componentId);
            ComponentId = componentId;

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

            result.AddRange(Serializer.SerializeData(ComponentId));
            result.AddRange(Serializer.SerializeData(SkillId));
            result.AddRange(Serializer.SerializeData<ISerializable>(SkillLaunchPosition));

            return result.ToArray();
        }
    }
}
