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

        public DeusClient(TcpClient tcpClient)
        {
            EventManager.Get().AddListener(EPacketType.Connected, InitUdp); // subscribe to event 'Connected'
            EventManager.Get().AddListener(EPacketType.GetGameRequest, SendUdpMessage);
            EventManager.Get().AddListener(EPacketType.CreateGameRequest, SendTcpMessage);
            EventManager.Get().AddListener(EPacketType.JoinGameRequest, SendTcpMessage);
            EventManager.Get().AddListener(EPacketType.LeaveGameRequest, SendTcpMessage);
            EventManager.Get().AddListener(EPacketType.Text, SendUdpMessage);

            m_tcpConnection = new DeusTcpConnection(tcpClient); // launch the SendAndReceive() Task
        }

        public void Dispose()
        {
            EventManager.Get().RemoveListener(EPacketType.Connected, InitUdp);

            m_tcpConnection.Dispose();
            m_udpConnection.Dispose();
        }

        public void SendPacket(Packet packet, bool isTcp)
        {
            if (isTcp)
                m_tcpConnection.SendPacket(packet);
            else
                throw new NotImplementedException();
        }

        private void InitUdp(object sender, SocketPacketEventArgs e)
        {
            IPAddress addr;
            if (IPAddress.TryParse((e.Packet as PacketClientConnected).AddrUdp, out addr))
            {
                Console.WriteLine("UDP addr : " + addr.ToString() + " | port : " + (e.Packet as PacketClientConnected).PortUdp);
                m_udpConnection = new DeusUdpConnection(new IPEndPoint(addr, (int)(e.Packet as PacketClientConnected).PortUdp));
            }
            else
                throw new Exception("Cannot parse IPAdress, abort the UDP connection...");
        }

        private void SendTcpMessage(object sender, SocketPacketEventArgs e)
        {
            m_tcpConnection.SendPacket(e.Packet);
        }

        private void SendUdpMessage(object sender, SocketPacketEventArgs e)
        {
            m_udpConnection.SendPacket(e.Packet);
        }
    }
}
