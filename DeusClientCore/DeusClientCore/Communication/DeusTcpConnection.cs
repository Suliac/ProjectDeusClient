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
        /// Read from the <see cref="NetworkStream"/>
        /// </summary>
        /// <param name="receiveBuffer">The buffer we fill with received datas</param>
        /// <returns>The number of bytes read</returns>
        protected override int OnReceiving(byte[] receiveBuffer)
        {
            return m_networkStream.Read(receiveBuffer, 0, receiveBuffer.Length);
        }

        /// <summary>
        /// Write to the <see cref="NetworkStream"/>
        /// </summary>
        /// <param name="sendBuffer">The bytes we want to send</param>
        protected override void OnSending(byte[] sendBuffer)
        {
            m_networkStream.Write(sendBuffer, 0, sendBuffer.Length);
        }

        /// <summary>
        /// Override DeusConnection::SendAndReceive()
        /// Loop while we don't want to stop 
        /// Try to take packets from m_messageToSend to send them on the network
        /// then try to read some data on the network
        /// </summary>
        /*protected override void SendAndReceive()
        {
            NetworkStream networkStream = null;

           // try
            //{
                // Init the stream with our TCP connection
                networkStream = m_tcpClient.GetStream();

                // Loop until cancellation is requested
                while (!m_cancellationToken.IsCancellationRequested)
                {
                    Thread.Sleep(5); // Give some sleep time to that poor thread, damn !
                    Packet packetToSend;

                    ///////////////////////////////////////
                    //                SEND               //
                    ///////////////////////////////////////
                    // Try to take and send packet until we don't have one left
                    while (m_messagesToSend.TryTake(out packetToSend))
                    {
                        // Serialize our packet into a byte[]
                        byte[] sendBuffer = Packet.Serialize(packetToSend);

                        // Write into our stream
                        networkStream.Write(sendBuffer, 0, sendBuffer.Length);
                    }

                    ///////////////////////////////////////
                    //             RECEIVE               //
                    ///////////////////////////////////////
                    if (networkStream.DataAvailable)
                    {
                        int readedByteCount = 0;

                        do
                        {
                            byte[] tempBuffer = new byte[DEFAULT_BUFFER_SIZE];

                            // We read the 'DEFAULT_BUFFER_SIZE' first bytes into our tmp buffer
                            readedByteCount = networkStream.Read(tempBuffer, 0, tempBuffer.Length);

                            // then we add our buffer to our packetBuffer
                            packetsBuffer = packetsBuffer.Concat(tempBuffer.Take(readedByteCount)).ToArray();

                            // we continue to read while there is data left and we already fill our temp buffer
                            // because if we already fill completely our tmp buffer, there is data left to receive !
                        } while (networkStream.DataAvailable && readedByteCount == DEFAULT_BUFFER_SIZE);

                        while (packetsBuffer.Length > 0)
                        {
                            // Deserialize our packet
                            Packet packet = Packet.Deserialize(packetsBuffer);
                            if (packet == null)
                                break;

                            // Then we delete all the byte of our deserialized message
                            packetsBuffer = packetsBuffer.Skip((int)packet.SerializedSize).ToArray();

                            // Enqueue our packet received
                            EventManager.Get().EnqueuePacket(0, packet);
                        }
                    } // endif DataAvailable
                } // end while cancellation requested ?
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("error : "+ ex.Message);
            //}
            //finally
            //{
                if(networkStream != null)
                {
                    networkStream.Close();
                    networkStream.Dispose();
                }

                if(m_tcpClient != null)
                {
                    m_tcpClient.Close();
                    m_tcpClient.Dispose();
                }
            //}
        }*/

    }
}
