﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketTextMessage : Packet
    {
        public string MessageText { get; set; }

        public PacketTextMessage() : base(EPacketType.Text)
        {
        }

        public override ushort EstimateCurrentSerializedSize()
        {
            // nb we serialize the size of the string that's why we need 4 bytes before the string
            return (ushort)(sizeof(UInt16) + (MessageText.Length + 1) );
        }

        public override void OnDeserialize(byte[] buffer, int index)
        {
            // get the size of the string
            UInt16 dataSize = 0;
            Serializer.DeserializeData(buffer, ref index, out dataSize);

            string message;
            Serializer.DeserializeData(buffer, ref index, out message, (int)dataSize);
            MessageText = message;
        }

        public override byte[] OnSerialize()
        {
            List<byte> result = new List<byte>();

            //uint dataSize = (uint)(MessageText.Length + 1); // +1 to add the \0 of string
            result.AddRange(Serializer.SerializeData((UInt16)(MessageText.Length + 1)));

            //  then we add the string 
            result.AddRange(Serializer.SerializeData(MessageText));

            return result.ToArray();
        }
    }
}
