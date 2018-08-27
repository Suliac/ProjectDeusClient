using DeusClientCore.Components;
using DeusClientCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketObjectEnter : Packet
    {
        public uint GameObjectId { get; set; }
        public EObjectType ObjectType { get; set; }
        public bool IsLocalPlayer { get; set; }

        public List<ISerializableComponent> Components { get; set; }

        public PacketObjectEnter() : base(EPacketType.ObjectEnter)
        {
            Components = new List<ISerializableComponent>();
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            ushort sizeOfComponents = sizeof(byte); // the number of item is saved before as an uint8
            foreach (var component in Components)
                sizeOfComponents += component.EstimateCurrentSerializedSize();

            // +1 for 1 byte for ObjectType(uint8_t)
            return (ushort)(sizeof(uint) + sizeof(EObjectType) + sizeof(bool) + sizeOfComponents);
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            // object id
            uint objectId;
            Serializer.DeserializeData(buffer, ref index, out objectId);
            GameObjectId = objectId;

            // object type
            EObjectType type = (EObjectType)((int)buffer[index]);
            index++;

            bool isLocalPlayer = false;
            Serializer.DeserializeData(buffer, ref index, out isLocalPlayer);
            IsLocalPlayer = isLocalPlayer;

            // Deserialize components
            byte componentsNumber = 0;
            Serializer.DeserializeData(buffer, ref index, out componentsNumber);

            for (int i = 0; i < componentsNumber; i++)
            {
                ISerializableComponent tmpComponent = GameComponentFactory.DeserializeComponent(buffer, ref index);
                Components.Add(tmpComponent);
            }
        }

        public override byte[] OnSerialize()
        {
            //List<byte> result = new List<byte>();

            //// serialize id
            //result.AddRange(Serializer.SerializeData(GameObjectId));

            //// serialize object type
            //result.Add((byte)ObjectType);

            //result.AddRange(Serializer.SerializeData(IsLocalPlayer));

            //// serialize component
            //result.AddRange(Serializer.SerializeData((byte)Components.Count));
            //for (int i = 0; i < Components.Count; i++)
            //{
            //    result.AddRange(Serializer.SerializeData(Components[i]));
            //}

            //return result.ToArray();
            throw new DeusException("Don't try to send this message");
        }
    }
}
