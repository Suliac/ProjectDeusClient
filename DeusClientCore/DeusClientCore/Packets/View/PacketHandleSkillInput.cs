using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketHandleSkillInput : PacketView
    {
        public uint SkillId { get; set; }

        public DeusVector2 SkillPosition { get; set; }

        public PacketHandleSkillInput(uint objectId, uint componentId, uint skillId, DeusVector2 position) : base(EPacketType.HandleSkillInputs)
        {
            SkillPosition = position;
            SkillId = skillId;

            ObjectId = objectId;
            ComponentId = componentId;
        }
    }
}
