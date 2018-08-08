using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DeusClientCore.Events;
using DeusClientCore.Packets;

namespace DeusClientCore
{
    public class DeusUdpConnection : DeusConnection
    {
        /// <summary>
        /// The UDP communication interface
        /// </summary>
        private UdpClient m_udpClient;

        /// <summary>
        /// Server connection information : ip adress and port
        /// </summary>
        private IPEndPoint m_endPoint;

        /// <summary>
        /// The number of ACK we send back when we receive a packet
        /// </summary>
        private const int NUMBER_ACK_SEND_BACK = 3;

        /// <summary>
        /// Try each N milliseconds to resend packet if the pcket isn't acked
        /// </summary>
        private const double PACKET_DELAY_CHECK_ACK_MS = 100;

        /// <summary>
        /// Array of acked packet's ids
        /// </summary>
        private uint[] m_ackedPackets;

        /// <summary>
        /// Index to write into the array of packet's ids
        /// </summary>
        private uint m_ackedPacketsIndex = 0;

        /// <summary>
        /// Packets to re-enqueue, used to work with our little reliable-UDP (but not ordered !) protocol
        ///
        ///		 +------------------+
        ///		 |				    |
        ///		 |  m_packetsToSend -----+
        ///		 |				    |	 |
        ///		 +-+----------------+	 |
        /// 	   |				     |
        ///ACKED---+----->SEND-----+     |
        /// 	   |			   |     |
        ///		   +->CAN'T SEND---+     |
        ///		                   | 	 |
        ///		+------------------v-+	 |
        ///		|				     |	 |
        ///	 +--+ m_packetsToRequeue |   |
        ///	 |	|				     |   |
        ///	 |	+--------------------+   |
        ///	 |							 |
        ///	 +---------------------------+
        ///	
        /// </summary>
        protected BlockingCollection<Tuple<double, Packet>> m_packetsToRequeue;

        public DeusUdpConnection(IPEndPoint serverEndPoint)
        {
            m_ackedPackets = new uint[256];
            m_endPoint = serverEndPoint;
            m_udpClient = new UdpClient();
            m_packetsToRequeue = new BlockingCollection<Tuple<double, Packet>>();
        }

        /// <summary>
        /// Check if there are pending datas
        /// </summary>
        /// <returns></returns>
        protected override bool AreThereAnyPendingDatas()
        {
            return m_udpClient.Available > 0;
        }

        /// <summary>
        /// Clean udp client on dispose
        /// </summary>
        protected override void OnEnd()
        {
            if (m_udpClient != null)
            {
                m_udpClient.Close();
                m_udpClient.Dispose();
            }
        }

        /// <summary>
        /// Init of the UDP based connection :
        /// -> Nothing to do, we cannot use Stream as UDP isn't stream-based
        /// </summary>
        protected override void OnInit()
        {
            // nothing to do, we cannot use Stream as UDP isn't stream-based
        }

        /// <summary>
        /// Enqueue all the <see cref="Packet"/> received except the <see cref="PacketAck"/>, and update our list of ACKed packet
        /// For the non-ack packet send <see cref="NUMBER_ACK_SEND_BACK"/> <see cref="PacketAck"/> back, where <see cref="NUMBER_ACK_SEND_BACK"/> is a const number set in <see cref="DeusUdpConnection"/>
        /// </summary>
        /// <param name="packet">The packet just received</param>
        protected override void OnPacketDeserialized(Packet packet)
        {
            if (packet.Type == EPacketType.Ack)
            {
                // check if packet isn't already acked
                if (!m_ackedPackets.Any(idAcked => idAcked == (packet as PacketAck).PacketIdToAck))
                {
                    //Console.WriteLine($"Received ACK for packet id : {(packet as PacketAck).PacketIdToAck.ToString()}");
                    m_ackedPackets[m_ackedPacketsIndex] = (packet as PacketAck).PacketIdToAck;
                    m_ackedPacketsIndex++;

                    // reset acked packet index : when 255 ack are received, we estimate that 
                    // the first ones arn't needed anymore, so we just override them
                    if (m_ackedPacketsIndex > 255)
                        m_ackedPacketsIndex = 0;
                }
            }
            else
            {
                // send ack back
                for (int i = 0; i < NUMBER_ACK_SEND_BACK; i++)
                {
                    PacketAck ackPacket = new PacketAck();
                    ackPacket.PacketIdToAck = packet.Id;
                    SendPacket(ackPacket);
                }

                // enqueue the packet
                EventManager.Get().EnqueuePacket(0, packet);
            }

        }

        /// <summary>
        /// Read bytes with UDP connection
        /// </summary>
        /// <param name="receiveBuffer">The buffer we fill with received datas</param>
        /// <returns>The number of bytes read</returns>
        protected override int OnReceiving(ref byte[] receiveBuffer)
        {
            receiveBuffer = m_udpClient.Receive(ref m_endPoint);
            return receiveBuffer.Length;
        }

        /// <summary>
        /// Send bytes with UDP connection
        /// </summary>
        /// <param name="sendBuffer">The datas to send</param>
        protected override void OnSending(Packet packetToSend)
        {
            //Console.WriteLine($"Send packet with id : {packetToSend.Id.ToString()}");

            // Serialize our packet into a byte[]
            byte[] sendBuffer = Packet.Serialize(packetToSend);

            // Send bytes
            m_udpClient.Send(sendBuffer, sendBuffer.Length, m_endPoint);

            // then re-put our packet into packet to send queue
            // in case of the packet isn't received by the client
            // and need to be resend. We don't requeue ACK
            if (packetToSend.Type != EPacketType.Ack)
                RequeuePacket(packetToSend);
        }

        /// <summary>
        /// Take a <see cref="Packet"/> from our <see cref="System.Collections.Concurrent.BlockingCollection{Packet}"/>
        /// For an UDP connection, check if a packet isn't already acked and manage to reenqueue packets if we try to resend them too early
        /// </summary>
        /// <param name="packetTaken">The <see cref="Packet"/> we took</param>
        /// <returns><see cref="true"/> if we successfully took a packet from the <see cref="System.Collections.Concurrent.BlockingCollection{Packet}"/>, return <see cref="false"/> otherwise</returns>
        protected override bool OnTryTakePacket(out Tuple<double, Packet> packet)
        {
            Tuple<double, Packet> tmpPacket = null;
            bool success = false;

            do
            {
                if (m_packetsToSend.TryTake(out tmpPacket))
                {
                    double currentMs = (new TimeSpan(DateTime.UtcNow.Ticks)).TotalMilliseconds;

                    // check if we reached the time to resend a packet otherwise re-enqueue it
                    if ((tmpPacket.Item1 > 0 && tmpPacket.Item1 + PACKET_DELAY_CHECK_ACK_MS <= currentMs)
                        || tmpPacket.Item1 == 0)
                    {
                        // check if the packet is already acked and just need to be popped
                        // if so, just don't do anything and go to the next packet
                        // otherwise we can take it
                        if (!m_ackedPackets.Any(idAcked => idAcked == tmpPacket.Item2.Id))
                            success = true;
                    }
                    else
                    {
                        m_packetsToRequeue.Add(tmpPacket);
                        tmpPacket = null;
                    }
                }

                // continue until there isn't packet to send left or we successfully get a packet to send
            } while (!success && m_packetsToSend.Count > 0);

            if (!success)
                tmpPacket = null;

            packet = tmpPacket;
            return success;

        }

        /// <summary>
        /// Add packet to the Requeue queue
        /// </summary>
        /// <param name="packet">The packet to requeue</param>
        public void RequeuePacket(Packet packet)
        {
            double timeStamp = (new TimeSpan(DateTime.UtcNow.Ticks)).TotalMilliseconds;
            m_packetsToRequeue.Add(new Tuple<double, Packet>(timeStamp, packet));
        }

        /// <summary>
        /// After sending packet, we want to requeue the ones in the requeue queue
        /// </summary>
        protected override void OnAfterSend()
        {
            while (m_packetsToRequeue.Count > 0)
            {
                Tuple<double, Packet> packet = m_packetsToRequeue.Take();
                m_packetsToSend.Add(packet);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            if (m_udpClient != null)
            {
                m_udpClient.Close();
                m_udpClient.Dispose();
            }

            // clean our message queue
            if (m_packetsToSend != null)
            {
                m_packetsToSend.Dispose();
                m_packetsToSend = null;
            }
        }
    }
}
