using DeusClientCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public abstract class PacketView : Packet
    {
        public uint ObjectId;
        public GameObject LinkedGameObject;

        public PacketView(EPacketType type) : base(type)
        {
        }

        public sealed override ushort EstimateCurrentSerializedSize()
        {
            throw new DeusException("YOU... ARE... NOT... PREPARED  (￣ヘ￣)! Do not try to serialize view packets. They are only made to communicate between clients game and view parts.");
        }

        public sealed override void OnDeserialize(byte[] buffer, int index)
        {
            throw new DeusException("YOU... ARE... NOT... PREPARED  (￣ヘ￣)! Do not try to serialize view packets. They are only made to communicate between clients game and view parts.");
        }

        public sealed override byte[] OnSerialize()
        {
            throw new DeusException("YOU... ARE... NOT... PREPARED  (￣ヘ￣)! Do not try to serialize view packets. They are only made to communicate between clients game and view parts.");

        }
    }
}
