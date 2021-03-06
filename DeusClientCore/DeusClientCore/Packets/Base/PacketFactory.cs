﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Packets
{
    public class PacketFactory
    {
        public static Packet CreatePacket(EPacketType packetType)
        {
            Packet packet = null;

            switch (packetType)
            {
                case EPacketType.Text:
                    packet = new PacketTextMessage();
                    break;
                case EPacketType.Ack:
                    packet = new PacketAck();
                    break;
                case EPacketType.Connected:
                    packet = new PacketClientConnected();
                    break;
                case EPacketType.CreateGameRequest:
                    packet = new PacketCreateGameRequest();
                    break;
                case EPacketType.CreateGameAnswer:
                    packet = new PacketCreateGameAnswer();
                    break;
                case EPacketType.JoinGameRequest:
                    packet = new PacketJoinGameRequest();
                    break;
                case EPacketType.JoinGameAnswer:
                    packet = new PacketJoinGameAnswer();
                    break;
                case EPacketType.GetGameRequest:
                    packet = new PacketGetGameRequest();
                    break;
                case EPacketType.GetGameAnswer:
                    packet = new PacketGetGameAnswer();
                    break;
                case EPacketType.LeaveGameRequest:
                    packet = new PacketLeaveGameRequest();
                    break;
                case EPacketType.LeaveGameAnswer:
                    packet = new PacketLeaveGameAnswer();
                    break;
                case EPacketType.ObjectEnter:
                    packet = new PacketObjectEnter();
                    break;
                case EPacketType.ObjectLeave:
                    packet = new PacketObjectLeave();
                    break;
                case EPacketType.NewPlayerJoin:
                    packet = new PacketNewPlayerJoin();
                    break;
                case EPacketType.UpdateMovementAnswer:
                    packet = new PacketMovementUpdateAnswer();
                    break;
                case EPacketType.GameStarted:
                    packet = new PacketGameStarted();
                    break;
                case EPacketType.PingAnswer:
                    packet = new PacketPingAnswer();
                    break;
                case EPacketType.PingRequest:
                    packet = new PacketPingRequest();
                    break;
                case EPacketType.ClockSyncAnswer:
                    packet = new PacketSyncClockAnswer();
                    break;
                default:
                    throw new Exception("Impossible to instantiate the packet");
            }

            return packet;
        }
    }
}
