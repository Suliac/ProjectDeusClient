using DeusClientCore.Exceptions;
using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Resources
{
    public enum SkillState
    {
        NotLaunched = 0,
        Casting = 1,
        Launched = 2,
        Finished = 3
    }

    public class SkillModel : ISerializable
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public bool IsCircle { get; set; }
        public List<SkillEffect> Effects { get; set; }
        public float CastTime { get; set; }
        public ushort MaxScope { get; set; }
        public ushort Radius { get; set; }
        public ushort Level { get; set; }
        public ushort ManaCost { get; set; }

        public virtual void Deserialize(byte[] packetsBuffer, ref int index)
        {
            uint skillId = 0;
            Serializer.DeserializeData(packetsBuffer, ref index, out skillId);

            SkillModel model = ResourcesHandler.GetSkill(skillId);
        }

        public virtual ushort EstimateCurrentSerializedSize()
        {
            return sizeof(uint);
        }

        public virtual byte[] Serialize()
        {
            return Serializer.SerializeData(Id);
        }

        public override string ToString()
        {
            return $"Id : {Id} | Name : {Name} | Circle : {IsCircle} | Cast time : {CastTime} | Max scope : {MaxScope} | Radius : {Radius} | Level : {Level} | Manacost : {ManaCost} | {string.Join(" | ", Effects.Select(effect => effect.ToString()))}";
        }

        protected void LoadSkill(uint skillId)
        {
            SkillModel model = ResourcesHandler.GetSkill(skillId);

            Id = model.Id;
            Name = model.Name;
            IsCircle = model.IsCircle;
            Effects = model.Effects;
            CastTime = model.CastTime;
            MaxScope = model.MaxScope;
            Radius = model.Radius;
            Level = model.Level;
            ManaCost = model.ManaCost;
        }
    }

    public class SkillInfos : SkillModel
    {
        public uint LaunchTime { get; set; }
        public SkillState State { get; set; }
        public DeusVector2 Position { get; set; }

        public SkillInfos(PacketUseSkillAnswer packet)
        {
            LoadSkill(packet.SkillId);

            LaunchTime = packet.SkillLaunchTimestampMs;
            State = SkillState.NotLaunched;
            Position = packet.SkillLaunchPosition;
        }

        public override void Deserialize(byte[] packetsBuffer, ref int index)
        {
            base.Deserialize(packetsBuffer, ref index);

            uint launchTime = 0;
            Serializer.DeserializeData(packetsBuffer, ref index, out launchTime);
            LaunchTime = launchTime;

            Position.Deserialize(packetsBuffer, ref index);
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            return (ushort)(base.EstimateCurrentSerializedSize() + sizeof(uint) + Position.EstimateCurrentSerializedSize());
        }

        public override byte[] Serialize()
        {
            List<byte> result = new List<byte>();
            base.Serialize();

            result.AddRange(Serializer.SerializeData(LaunchTime));
            result.AddRange(Serializer.SerializeData(Position));

            return result.ToArray();
        }

    }
}
