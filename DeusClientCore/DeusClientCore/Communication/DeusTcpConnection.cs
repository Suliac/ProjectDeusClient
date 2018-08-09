using DeusClientCore.Events;
using DeusClientCore.Packets;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace DeusClientCore
{
    public class DeusTcpConnection : DeusConnection
    {
        /// <summary>
        /// TCP communication interface
        /// </summary>
        private TcpClient m_tcpClient;

        /// <summary>
        /// The stream we use to communicate
        /// </summary>
        private NetworkStream m_networkStream;

        /// <summary>
        /// Default constructor for <see cref="DeusTcpConnection"/>
        /// </summary>
        /// <param name="client">The <see cref="TcpClient"/> we use to communicate</param>
        public DeusTcpConnection(TcpClient client)
            : base()
        {
            m_tcpClient = client;
            m_networkStream = null;
        }

        /// <summary>
        /// Return if there are any pending datas on the <see cref="NetworkStream"/>
        /// </summary>
        /// <returns><see cref="true"/> if there are pending datas or <see cref="false"/> otherwise</returns>
        protected override bool AreThereAnyPendingDatas()
        {
            return m_networkStream.DataAvailable;
        }

        protected override void OnAfterSend()
        {
            // nothing to do
        }

        /// <summary>
        /// We dispose our stream and socket
        /// </summary>
        protected override void OnEnd()
        {
            // dispose our stream
            if (m_networkStream != null)
            {
                m_networkStream.Close();
                m_networkStream.Dispose();
            }

            // dispose our socket
            if (m_tcpClient != null)
            {
                m_tcpClient.Close();
                m_tcpClient.Dispose();
            }
        }

        /// <summary>
        /// We use the NetworkStream object, so we init it from our TcpClient
        /// </summary>
        protected override void OnInit()
        {
            m_networkStream = null;
            m_networkStream = m_tcpClient.GetStream();
        }

        /// <summary>
        /// For TCP connection, we enqueue all the packet received
        /// </summary>
        /// <param name="packet">The packet just serialized</param>
        protected override void OnPacketDeserialized(Packet packet)
        {
            Console.WriteLine($"TCP Enqueue{packet.Type}");
            EventManager.Get().EnqueuePacket(0, packet);
        }

        /// <summary>
        /// Read from the <see cref="NetworkStream"/>
        /// </summary>
        /// <param name="receiveBuffer">The buffer we fill with received datas</param>
        /// <returns>The number of bytes read</returns>
        protected override int OnReceiving(ref byte[] receiveBuffer)
        {
            return m_networkStream.Read(receiveBuffer, 0, receiveBuffer.Length);
        }

        /// <summary>
        /// Write to the <see cref="NetworkStream"/>
        /// </summary>
        /// <param name="sendBuffer">The bytes we want to send</param>
        protected override void OnSending(Packet packetToSend)
        { 
            // Serialize our packet into a byte[]
            byte[] sendBuffer = Packet.Serialize(packetToSend);

            m_networkStream.Write(sendBuffer, 0, sendBuffer.Length);
        }

        /// <summary>
        /// Take a <see cref="Packet"/> from our <see cref="System.Collections.Concurrent.BlockingCollection{Packet}"/>
        /// For a TCP connection, we just need to take a packet if we can
        /// </summary>
        /// <param name="packetTaken">The <see cref="Packet"/> we took with the enqueued timestamp in MS</param>
        /// <returns><see cref="true"/> if we successfully took a packet from the <see cref="System.Collections.Concurrent.BlockingCollection{Packet}"/>, return <see cref="false"/> otherwise</returns>
        protected override bool OnTryTakePacket(out Tuple<double, Packet> packet)
        {
            return m_packetsToSend.TryTake(out packet);
        }

        public override void Dispose()
        {
            base.Dispose();

            if(m_networkStream != null)
            {
                m_networkStream.Close();
                m_networkStream.Dispose();
            }

            if(m_tcpClient !=null)
            {
                m_tcpClient.Close();
                m_tcpClient.Dispose();
            }
        }

    }
}
