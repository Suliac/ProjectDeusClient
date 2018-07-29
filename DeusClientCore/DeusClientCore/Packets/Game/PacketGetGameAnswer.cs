using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketGetGameAnswer : PacketAnswer
    {
        public List<uint> GamesIds { get; set; }

        public PacketGetGameAnswer() : base(EPacketType.GetGameAnswer)
        {
            GamesIds = new List<uint>();
        }
        public override ushort EstimateAnswerCurrentSerializedSize()
        {
            // 1 PacketGetGameAnswer uses :
            // - 4 byte						: to save an unsigned int for the length of the tab
            // - GamesIds.length * 4 bytes	: 4 bytes per uint * number of ids
            return (ushort)(sizeof(uint) + (GamesIds.Count * sizeof(uint)));
        }

        public override void OnAnswerDeserialize(byte[] buffer, int index)
        {
            uint tabSize = 0;
            Serializer.DeserializeData(buffer, ref index, out tabSize);

            for (int i = 0; i < tabSize; i++)
            {
                uint gameId = 0;
                Serializer.DeserializeData(buffer, ref index, out gameId);
                GamesIds.Add(gameId);
            }
        }

        public override byte[] OnAnswerSerialize()
        {
            List<byte> result = new List<byte>();

            result.AddRange(Serializer.SerializeData((uint)GamesIds.Count));

            foreach (var id in GamesIds)
                result.AddRange(Serializer.SerializeData(id));

            return result.ToArray();
        }
    }
}
