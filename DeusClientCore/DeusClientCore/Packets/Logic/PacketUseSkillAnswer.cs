using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public class PacketUseSkillAnswer : PacketAnswer
    {
        public uint ObjectId { get; set; }
        public uint SkillId { get; set; }
        public DeusVector2 SkillLaunchPosition { get; set; }
        public uint SkillLaunchTimestampMs { get; set; }

        public PacketUseSkillAnswer() : base(EPacketType.UseSkillAnswer)
        {
        }

        public override ushort EstimateAnswerCurrentSerializedSize()
        {
            return (ushort)(sizeof(uint) + sizeof(uint) + SkillLaunchPosition.EstimateCurrentSerializedSize() + sizeof(uint));
        }

        public override void OnAnswerDeserialize(byte[] buffer, int index)
        {
            uint objectId = 0;
            Serializer.DeserializeData(buffer, ref index, out objectId);
            ObjectId = objectId;

            uint skillId = 0;
            Serializer.DeserializeData(buffer, ref index, out skillId);
            SkillId = skillId;

            DeusVector2 pos = DeusVector2.Zero;
            Serializer.DeserializeData(buffer, ref index, out pos);
            SkillLaunchPosition = pos;

            uint tmpSrcMs = 0;
            Serializer.DeserializeData(buffer, ref index, out tmpSrcMs);
            SkillLaunchTimestampMs = tmpSrcMs;
        }

        public override byte[] OnAnswerSerialize()
        {
            List<byte> result = new List<byte>();

            result.AddRange(Serializer.SerializeData(ObjectId));
            result.AddRange(Serializer.SerializeData(SkillId));
            result.AddRange(Serializer.SerializeData<ISerializable>(SkillLaunchPosition));
            result.AddRange(Serializer.SerializeData(SkillLaunchTimestampMs));

            return result.ToArray();
        }
    }
}
