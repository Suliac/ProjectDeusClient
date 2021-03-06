﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketCreateGameRequest : Packet
    {
        public PacketCreateGameRequest() : base(EPacketType.CreateGameRequest)
        {
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            // nothing to do, already done in base class
            return 0;
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            // nothing to do, already done in base class
        }

        public override byte[] OnSerialize()
        {
            // nothing to do, already done in base class
            return new byte[0];
        }
    }
}
