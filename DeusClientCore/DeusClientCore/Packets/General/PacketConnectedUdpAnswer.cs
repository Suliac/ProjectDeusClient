using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketConnectedUdpAnswer : Packet
    {
        public string PlayerNickname { get; private set; }

        public PacketConnectedUdpAnswer(string nickname) : base(EPacketType.ConnectedUdpAnswer)
        {
            PlayerNickname = nickname;
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            // 1 PacketClientConnected uses :
            // - 1 byte						: to save an uint8 for the length of the next string
            // - string size +1 bytes	    : to save the string nickname ('+1' is for the \0)
            return (ushort)(sizeof(byte) + (PlayerNickname.Length + 1));
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            // get the size of the string
            byte dataSize = buffer[index];
            index++;
            
            string tmpNickname;
            Serializer.DeserializeData(buffer, ref index, out tmpNickname, (int)dataSize);
            PlayerNickname = tmpNickname;
        }

        public override byte[] OnSerialize()
        {
            List<byte> result = new List<byte>();

            byte dataSize = (byte)(PlayerNickname.Length + 1); // +1 to add the \0 of string
            result.Add(dataSize);

            //  then we add the string 
            result.AddRange(Serializer.SerializeData(PlayerNickname));

            return result.ToArray();
        }
    }
}
