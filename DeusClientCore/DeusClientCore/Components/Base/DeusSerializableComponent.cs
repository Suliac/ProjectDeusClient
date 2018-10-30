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

            ComponentType = (EComponentType)packetsBuffer[index];
            index++;

            bool thereIsOrigin = false;
            Serializer.DeserializeData(packetsBuffer, ref index, out thereIsOrigin);

            if (thereIsOrigin)
            {
                DataTimed<T> tmpOrigin = new DataTimed<T>();
                Serializer.DeserializeData(packetsBuffer, ref index, out tmpOrigin);
                Origin = tmpOrigin;
            }
            else
            {
                Origin = null;
            }

            bool thereIsDestination = false;
            Serializer.DeserializeData(packetsBuffer, ref index, out thereIsDestination);

            if (thereIsDestination)
            {
                DataTimed<T> tmpDestination = new DataTimed<T>(default(T), 0);
                Serializer.DeserializeData(packetsBuffer, ref index, out tmpDestination);
                Destination = tmpDestination;
            }
            else
            {
                Destination = null;
            }
        }

        public byte[] Serialize()
        {
            List<byte> result = new List<byte>();

            result.AddRange(Serializer.SerializeData(ComponentId));
            result.AddRange(Serializer.SerializeData((byte)ComponentType));

            bool thereIsOrigin = Origin != null;
            result.AddRange(Serializer.SerializeData(thereIsOrigin));
            if(thereIsOrigin)
                result.AddRange(Serializer.SerializeData(Origin));

            bool thereIsDestination = Destination != null;
            result.AddRange(Serializer.SerializeData(thereIsDestination));
            if (thereIsDestination)
                result.AddRange(Serializer.SerializeData(Destination));

            return result.ToArray();
        }

        public ushort EstimateCurrentSerializedSize()
        {
            // sizeof(bool) for isThereDestination & isThereOrigin
            return (ushort)(sizeof(uint) + sizeof(byte) + sizeof(bool) + Origin.EstimateCurrentSerializedSize() + sizeof(bool) + Destination.EstimateCurrentSerializedSize());
        }

    }
}
