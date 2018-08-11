using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public interface ISerializable
    {
        byte[] Serialize();

        void Deserialize(byte[] packetsBuffer, ref int index);

        ushort EstimateCurrentSerializedSize();
    }
}
