using DeusClientCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketJoinGameAnswer : PacketAnswer
    {
        public uint GameJoinedId { get; set; }
        public Dictionary<UInt32, string> PlayerInfos { get; set; }

        public PacketJoinGameAnswer() : base(EPacketType.JoinGameAnswer)
        {
        }

        public override ushort EstimateAnswerCurrentSerializedSize()
        {
            ushort sizeStr = 0;
            foreach (var playerInfo in PlayerInfos)
            {
                sizeStr += sizeof(byte); // size of the str is saved
                sizeStr += (ushort)(playerInfo.Value.Length + 1);
            }

            // 4 bytes	for gameId joined
            // 2 bytes	for number of playerinfos
            // 4 bytes	for each player id
            // X bytes	for each player nickname
            return (ushort)(sizeof(uint) + sizeof(UInt16) + (PlayerInfos.Count * sizeof(UInt32)) + sizeStr);
        }

        public override void OnAnswerDeserialize(byte[] buffer, int index)
        {
            // 1 - game id
            uint gameId = 0;
            Serializer.DeserializeData(buffer, ref index, out gameId);
            GameJoinedId = gameId;

            // 2 - dictionnary size
            ushort playerInfosSize = 0;
            Serializer.DeserializeData(buffer, ref index, out playerInfosSize);

            PlayerInfos = new Dictionary<uint, string>();

            // 3 - all players infos
            for (int i = 0; i < playerInfosSize; i++)
            {
                // 3.1 Player Id
                uint playerId = 0;
                Serializer.DeserializeData(buffer, ref index, out playerId);

                // 3.2.1 size of nickname
                byte dataSize = buffer[index];
                index++;

                // 3.2.2 nickname
                string tmpNickname;
                Serializer.DeserializeData(buffer, ref index, out tmpNickname, (int)dataSize);

                PlayerInfos.Add(playerId, tmpNickname);
            }

        }

        public override byte[] OnAnswerSerialize()
        {
            throw new DeusException("Don't try to serialize this");
        }
    }
}
