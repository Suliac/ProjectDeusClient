using DeusClientCore.Events;
using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public class DeusClient : IDisposable
    {
        private DeusTcpConnection m_tcpConnection;
        private DeusUdpConnection m_udpConnection;

        private string m_playerName;

        public DeusClient(TcpClient tcpClient, string playerName)
        {
            m_playerName = playerName;

            EventManager.Get().AddListener(EPacketType.Connected, InitUdp); // subscribe to event 'Connected'

            EventManager.Get().AddListener(EPacketType.GetGameRequest, SendTcpMessage);
            EventManager.Get().AddListener(EPacketType.CreateGameRequest, SendTcpMessage);
            EventManager.Get().AddListener(EPacketType.JoinGameRequest, SendTcpMessage);
            EventManager.Get().AddListener(EPacketType.LeaveGameRequest, SendTcpMessage);
            EventManager.Get().AddListener(EPacketType.Text, SendTcpMessage);
            EventManager.Get().AddListener(EPacketType.PlayerReady, SendTcpMessage);

            EventManager.Get().AddListener(EPacketType.UpdateMovementRequest, SendUdpMessage);

            m_tcpConnection = new DeusTcpConnection(tcpClient); // launch the SendAndReceive() Task
        }

        public void Dispose()
        {
            EventManager.Get().RemoveListener(EPacketType.Connected, InitUdp);

            EventManager.Get().RemoveListener(EPacketType.GetGameRequest, SendUdpMessage);
            EventManager.Get().RemoveListener(EPacketType.CreateGameRequest, SendTcpMessage);
            EventManager.Get().RemoveListener(EPacketType.JoinGameRequest, SendTcpMessage);
            EventManager.Get().RemoveListener(EPacketType.LeaveGameRequest, SendTcpMessage);
            EventManager.Get().RemoveListener(EPacketType.Text, SendTcpMessage);
            EventManager.Get().RemoveListener(EPacketType.PlayerReady, SendTcpMessage);

            EventManager.Get().RemoveListener(EPacketType.UpdateMovementRequest, SendUdpMessage);
            
            m_tcpConnection.Dispose();
            m_udpConnection.Dispose();
        }

        public void SendPacket(Packet packet, bool isTcp)
        {
            if (isTcp)
                m_tcpConnection.SendPacket(packet);
            else
                m_udpConnection.SendPacket(packet);
        }

        private void InitUdp(object sender, SocketPacketEventArgs e)
        {
            IPAddress addr;
            if (IPAddress.TryParse((e.Packet as PacketClientConnected).AddrUdp, out addr))
            {
                Console.WriteLine("UDP addr : " + addr.ToString() + " | port : " + (e.Packet as PacketClientConnected).PortUdp);
                m_udpConnection = new DeusUdpConnection(new IPEndPoint(addr, (int)(e.Packet as PacketClientConnected).PortUdp));

                PacketConnectedUdpAnswer feedback = new PacketConnectedUdpAnswer(m_playerName);
                m_udpConnection.SendPacket(feedback);
            }
            else
                throw new Exception("Cannot parse IPAdress, abort the UDP connection...");
        }

        private void SendTcpMessage(object sender, SocketPacketEventArgs e)
        {
            //Console.WriteLine($"Send TCP {e.Packet.Type}");
            SendPacket(e.Packet, true);
        }

        private void SendUdpMessage(object sender, SocketPacketEventArgs e)
        {
            //Console.WriteLine($"Send UDP {e.Packet.Type}");
            SendPacket(e.Packet, false);

        }
    }
}
