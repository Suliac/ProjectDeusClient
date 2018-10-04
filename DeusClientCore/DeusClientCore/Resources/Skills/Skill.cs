using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Resources
{
    public class Skill
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public bool IsCircle { get; set; }
        public List<SkillEffect> Effects { get; set; }
        public float CastTime { get; set; }
        public ushort MaxScope { get; set; }
        public ushort Level { get; set; }
        public ushort ManaCost { get; set; }

        public override string ToString()
        {
            return $"Id : {Id} | Name : {Name} | Circle : {IsCircle} | Cast time : {CastTime} | Max scope : {MaxScope} | Level : {Level} | Manacost : {ManaCost} | {string.Join(" | ", Effects.Select(effect => effect.ToString()))}";
        }
    }
}
