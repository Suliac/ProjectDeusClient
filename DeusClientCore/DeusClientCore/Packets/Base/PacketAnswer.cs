using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public abstract class PacketAnswer : Packet
    {
        public bool IsSuccess { get; set; }

        public PacketAnswer(EPacketType type) : base(type)
        {
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            return (ushort)(sizeof(bool) + EstimateAnswerCurrentSerializedSize());
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            bool isSuccess = false;
            Serializer.DeserializeData(buffer, ref index, out isSuccess);
            IsSuccess = isSuccess;

            OnAnswerDeserialize(buffer, index);
        }

        public override byte[] OnSerialize()
        {
            List<byte> result = new List<byte>();

            result.AddRange(Serializer.SerializeData(IsSuccess));

            result.AddRange(OnAnswerSerialize());

            return result.ToArray();
        }

        public abstract void OnAnswerDeserialize(byte[] buffer, int index);
        public abstract byte[] OnAnswerSerialize();
        public abstract ushort EstimateAnswerCurrentSerializedSize();

    }
}
