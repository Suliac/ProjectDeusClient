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

    public class SkillModel
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

        public override string ToString()
        {
            return $"Id : {Id} | Name : {Name} | Circle : {IsCircle} | Cast time : {CastTime} | Max scope : {MaxScope} | Radius : {Radius} | Level : {Level} | Manacost : {ManaCost} | {string.Join(" | ", Effects.Select(effect => effect.ToString()))}";
        }
    }

    public class SkillInfos : SkillModel
    {
        public uint LaunchTime { get; set; }
        public SkillState State { get; set; }
        public DeusVector2 Position { get; set; }

        public SkillInfos(PacketUseSkillAnswer packet)
        {
            SkillModel model = ResourcesHandler.GetSkill(packet.SkillId);

            Id = model.Id;
            Name = model.Name;
            IsCircle = model.IsCircle;
            Effects = model.Effects;
            CastTime = model.CastTime;
            MaxScope = model.MaxScope;
            Radius = model.Radius;
            Level = model.Level;
            ManaCost = model.ManaCost;

            LaunchTime = packet.SkillLaunchTimestampMs;
            State = SkillState.NotLaunched;
            Position = packet.SkillLaunchPosition;
        }

    }
}
