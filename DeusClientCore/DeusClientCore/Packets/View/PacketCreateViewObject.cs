using DeusClientCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketCreateViewObject : PacketView
    {
        public EObjectType ObjectType { get; set; }

        public PacketCreateViewObject() : base(EPacketType.CreateViewObject)
        {

        }
    }
}
