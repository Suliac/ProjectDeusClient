using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public class DataTimed<T> : ISerializable
    {
        public T Data;
        public uint TimeStampMs;

        public DataTimed()
        {

        }

        public DataTimed(T data, uint timeStampMs)
        {
            Data = data;
            TimeStampMs = timeStampMs;
        }

        public void Deserialize(byte[] packetsBuffer, ref int index)
        {
            Serializer.DeserializeData(packetsBuffer, ref index, out Data);
            Serializer.DeserializeData(packetsBuffer, ref index, out TimeStampMs);
        }

        public byte[] Serialize()
        {
            List<byte> result = new List<byte>();

            result.AddRange(Serializer.SerializeData(Data));
            result.AddRange(Serializer.SerializeData(TimeStampMs));

            return result.ToArray();
        }

        public ushort EstimateCurrentSerializedSize()
        {
            return (ushort)(Marshal.SizeOf(Data) + sizeof(uint));
        }

        public override string ToString()
        {
            return $"[{TimeStampMs}]{Data}";
        }
    }
}
