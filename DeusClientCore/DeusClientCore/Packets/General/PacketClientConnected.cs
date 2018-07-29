using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketClientConnected : Packet
    {
        public string AddrUdp { get; set; }
        public uint PortUdp { get; set; }

        public PacketClientConnected() : base(EPacketType.Connected)
        {
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            // 1 PacketClientConnected uses :
            // - 4 byte						: to save an unsigned int for the length of the next string
            // - m_message.size()+1 bytes	: to save the string ip address ('+1' is for the \0)
            // - 4 bytes					: to save the udp port
            return (ushort)(sizeof(uint) + (AddrUdp.Length + 1) + sizeof(uint));
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            // get the size of the string
            uint dataSize = 0;
            Serializer.DeserializeData(buffer, ref index, out dataSize);

            string tmpAddrUdp;
            Serializer.DeserializeData(buffer, ref index, out tmpAddrUdp, (int)dataSize);
            AddrUdp = tmpAddrUdp;

            // then deserialize the port
            uint port = 0;
            Serializer.DeserializeData(buffer, ref index, out port);
            PortUdp = port;
        }

        public override byte[] OnSerialize()
        {
            List<byte> result = new List<byte>();

            byte dataSize = (byte)(AddrUdp.Length + 1); // +1 to add the \0 of string
            result.Add(dataSize);

            //  then we add the string 
            result.AddRange(Serializer.SerializeData(AddrUdp));

            // then serialize the port
            result.AddRange(Serializer.SerializeData(PortUdp));

            return result.ToArray();
        }
    }
}
