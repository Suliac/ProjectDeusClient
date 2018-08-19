using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketHandleMovementInput : PacketView
    {
        public DeusVector2 DestinationWanted { get; set; }

        public PacketHandleMovementInput(uint objectId, uint componentId, DeusVector2 destination) : base(EPacketType.HandleMovementInputs)
        {
            DestinationWanted = destination;
            ObjectId = objectId;
            ComponentId = componentId;
        }
    }
}
