using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Components
{
    public class DeusSerializableTimelineComponent<T> : ISerializableComponent
    {
        public DataTimed<T> Origin { get; set; }
        public DataTimed<T> Destination { get; set; }

        public uint ComponentId { get; set; }
        public EComponentType ComponentType { get; set; }

        public void Deserialize(byte[] packetsBuffer, ref int index)
        {
            uint tmpComponentId = 0;
            Serializer.DeserializeData(packetsBuffer, ref index, out tmpComponentId);
            ComponentId = tmpComponentId;

            byte tmpType;
            Serializer.DeserializeData(packetsBuffer, ref index, out tmpType);
            ComponentType = (EComponentType)tmpType;

            DataTimed<T> tmpOrigin = null;
            Serializer.DeserializeData(packetsBuffer, ref index, out tmpOrigin);
            Origin = tmpOrigin;

            DataTimed<T> tmpDestination = null;
            Serializer.DeserializeData(packetsBuffer, ref index, out tmpDestination);
            Destination = tmpDestination;
        }

        public byte[] Serialize()
        {
            List<byte> result = new List<byte>();

            result.AddRange(Serializer.SerializeData(ComponentId));
            result.AddRange(Serializer.SerializeData((byte)ComponentType));

            result.AddRange(Serializer.SerializeData(Origin));
            result.AddRange(Serializer.SerializeData(Destination));

            return result.ToArray();
        }

        public ushort EstimateCurrentSerializedSize()
        {
            return (ushort)(sizeof(uint) + sizeof(byte) + Origin.EstimateCurrentSerializedSize() + Destination.EstimateCurrentSerializedSize());
        }

    }
}
