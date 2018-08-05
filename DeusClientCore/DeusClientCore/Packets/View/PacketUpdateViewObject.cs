using DeusClientCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketUpdateViewObject : PacketView
    {
        public uint ComponentId;
        public object NewValue;

        public PacketUpdateViewObject() : base(EPacketType.UpdateViewObject)
        {

        }
    }
}
