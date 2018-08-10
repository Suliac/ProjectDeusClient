using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketNewPlayerJoin : Packet
    {
        public string PlayerNickname { get; private set; }
        public UInt32 PlayerId { get; private set; }

        public PacketNewPlayerJoin() : base(EPacketType.NewPlayerJoin)
        {

        }

        public PacketNewPlayerJoin(string nickname, UInt32 id) : base(EPacketType.NewPlayerJoin)
        {
            PlayerNickname = nickname;
            PlayerId = id;
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            // 1 PacketClientConnected uses :
            // - 4 bytes					: to save player id
            // - 1 byte						: to save an uint8 for the length of the next string
            // - string size +1 bytes	    : to save the string nickname ('+1' is for the \0)
            return (ushort)(sizeof(UInt32) + sizeof(byte) + (PlayerNickname.Length + 1));
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            UInt32 tmpId = 0;
            Serializer.DeserializeData(buffer, ref index, out tmpId);
            PlayerId = tmpId;

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

            result.AddRange(Serializer.SerializeData(PlayerId));

            byte dataSize = (byte)(PlayerNickname.Length + 1); // +1 to add the \0 of string
            result.Add(dataSize);

            //  then we add the string 
            result.AddRange(Serializer.SerializeData(PlayerNickname));

            return result.ToArray();
        }
    }
}
