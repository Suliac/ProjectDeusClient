using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public enum EPacketType : byte
    {
        Error                       = 0,

        // General
        Text                        = 1,
        Disconnect                  = 2,
        Ack                         = 3,
        Connected                   = 4,
        ConnectedUdpAnswer          = 5,

        // Lobby : Games management
        CreateGameRequest           = 10,
        CreateGameAnswer            = 11,
        JoinGameRequest             = 12,
        JoinGameAnswer              = 13,
        GetGameRequest              = 14,
        GetGameAnswer               = 15,
        LeaveGameRequest            = 16,
        LeaveGameAnswer             = 17,
        DeleteGameRequest           = 18,
        NewPlayer                   = 19,
        PlayerReady                 = 20,
        PlayerNotReady			    = 21,
		StartGame				    = 22,

        // Game Logic
        ObjectEnter                 = 50,
        ObjectLeave                 = 51,
        ObjectChangeCell            = 52,
        UpdateHealth                = 53,

        // Game view
        HandleClickUI               = 100,
        CreateViewObject            = 101,
        UpdateViewObject            = 102,
        DeleteViewObject            = 103,
        TestViewObject              = 104,
        TestDeleteViewObject        = 105,
    };

    public abstract class Packet
    {
        private static UInt32 m_nextId = 1;

        const UInt16 HEADER_SIZE = 7;
        
        public UInt32 Id { get; private set; }
        public UInt16 SerializedSize { get; private set; }
        public EPacketType Type { get; private set; }

        public Packet(EPacketType type)
        {
            Id = m_nextId;
            Type = type;

            m_nextId++;
        }

        public static byte[] Serialize(Packet packetToSend)
        {
            List<byte> result = new List<byte>();

            // serialize id
            result.AddRange(Serializer.SerializeData(packetToSend.Id));

            // serialize type
            result.Add((byte)packetToSend.Type);

            // serialize size
            ushort serializedSize = (ushort)((ushort)packetToSend.EstimateCurrentSerializedSize() + (ushort)HEADER_SIZE);
            result.AddRange(Serializer.SerializeData(serializedSize));

            // Specific serialization
            result.AddRange(packetToSend.OnSerialize());

            return result.ToArray();
        }

        public static Packet Deserialize(byte[] packetsBuffer)
        {
            int index = 0;
            Packet packetDeserialize = null;

            // Deerialize id
            uint uniqueId;
            Serializer.DeserializeData(packetsBuffer, ref index, out uniqueId);

            // Deerialize id
            EPacketType type = (EPacketType)((int)packetsBuffer[index]);
            index++;

            // Deerialize id
            ushort serializedSize;
            Serializer.DeserializeData(packetsBuffer, ref index, out serializedSize);

            packetDeserialize = PacketFactory.CreatePacket(type);

            packetDeserialize.Id = uniqueId;
            packetDeserialize.Type = type;
            packetDeserialize.SerializedSize = serializedSize;

            packetDeserialize.OnDeserialize(packetsBuffer, index);

            return packetDeserialize;
        }

        public abstract byte[] OnSerialize();

        public abstract void OnDeserialize(byte[] buffer, int index);

        public abstract ushort EstimateCurrentSerializedSize();
    }
}
