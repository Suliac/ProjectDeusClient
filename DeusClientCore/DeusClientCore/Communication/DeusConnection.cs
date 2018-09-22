using DeusClientCore.Events;
using DeusClientCore.Packets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public abstract class DeusConnection : IDisposable
    {
        /// <summary>
        /// Default size for receiving datas (set to 512 on server side)
        /// </summary>
        protected const int DEFAULT_BUFFER_SIZE = 512;

        /// <summary>
        /// The task (~thread) where we deal with message to receive and send
        /// </summary>
        protected Task m_sendAndReceiveTask;

        /// <summary>
        /// The list of packets to send with the time it was enqued
        /// </summary>
        protected BlockingCollection<Tuple<double, Packet>> m_packetsToSend;

        /// <summary>
        /// Do we want to stop our task ?
        /// </summary>
        protected CancellationToken m_cancellationToken;

        /// <summary>
        /// Source of our cancellation token
        /// </summary>
        protected CancellationTokenSource m_cancellationTokenSource;

        /// <summary>
        /// Current buffer of packets received but not deserialized
        /// </summary>
        protected byte[] packetsBuffer = new byte[0];

        /// <summary>
        /// Default constructor for DeusConnection
        /// </summary>
        public DeusConnection()
        {
            m_packetsToSend = new BlockingCollection<Tuple<double, Packet>>();
            m_cancellationTokenSource = new CancellationTokenSource();
            m_cancellationToken = m_cancellationTokenSource.Token;

            m_sendAndReceiveTask = new Task(SendAndReceive, m_cancellationToken, TaskCreationOptions.LongRunning);
            m_sendAndReceiveTask.Start();
        }

        /// Loop while we don't want to stop 
        /// Try to take packets from m_messageToSend to send them on the network
        /// then try to read some data on the network
        protected void SendAndReceive()
        {
            //try
            //{
            // Init our TCP or UDP connection
            OnInit();

            // Loop until cancellation is requested
            while (!m_cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(5); // Give some sleep time to that poor thread, damn !
                Tuple<double, Packet> packetToSend;

                ///////////////////////////////////////
                //                SEND               //
                ///////////////////////////////////////
                // Try to take and send packet until we don't have one left
                while (OnTryTakePacket(out packetToSend))
                {
                    // Write with our connection method
                    OnSending(packetToSend.Item2);
                    if (packetToSend.Item2.Type == EPacketType.PingRequest && TimeHelper.PingPacketNfo.ContainsKey(packetToSend.Item2.Id))
                    {
                        TimeHelper.PingPacketNfo[packetToSend.Item2.Id] = TimeHelper.GetUnixMsTimeStamp();
                        //Console.WriteLine($"Send : {TimeHelper.PingPacketNfo[packetToSend.Item2.Id]}"); 
                        TimeHelper.PingPacketSent++;
                    }
                }

                OnAfterSend();

                ///////////////////////////////////////
                //             RECEIVE               //
                ///////////////////////////////////////
                if (AreThereAnyPendingDatas())
                {
                    int readedByteCount = 0;
                    uint recvTimeStamp = TimeHelper.GetUnixMsTimeStamp();
                    do
                    {
                        byte[] tempBuffer = new byte[DEFAULT_BUFFER_SIZE];

                        // We read the 'DEFAULT_BUFFER_SIZE' first bytes into our tmp buffer

                        readedByteCount = OnReceiving(ref tempBuffer);

                        // then we add our buffer to our packetBuffer
                        packetsBuffer = packetsBuffer.Concat(tempBuffer.Take(readedByteCount)).ToArray();

                        // we continue to read while there is data left and we already fill our temp buffer
                        // because if we already fill completely our tmp buffer, there is data left to receive !
                    } while (AreThereAnyPendingDatas() && readedByteCount == DEFAULT_BUFFER_SIZE);

                    while (packetsBuffer.Length > 0)
                    {
                        // Deserialize our packet
                        Packet packet = Packet.Deserialize(packetsBuffer);
                        if (packet == null)
                            break;

                        packet.RecvTimeStamp = recvTimeStamp;

                        // we update info for ping
                        if (packet.Type == EPacketType.PingAnswer
                            && TimeHelper.PingPacketNfo.ContainsKey((packet as PacketPingAnswer).AnswerToPacketId))
                        {

                            //Console.WriteLine($"Recv : {recvTimeStamp}");
                            TimeHelper.PingPacketNfo[(packet as PacketPingAnswer).AnswerToPacketId] = packet.RecvTimeStamp - TimeHelper.PingPacketNfo[(packet as PacketPingAnswer).AnswerToPacketId];
                            TimeHelper.PingPacketRecv++;
                        }
                        // Then we delete all the byte of our deserialized message
                        packetsBuffer = packetsBuffer.Skip((int)packet.SerializedSize).ToArray();

                        // Enqueue our packet received
                        OnPacketDeserialized(packet);
                    }
                } // endif DataAvailable
            } // end while cancellation requested ?
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Error : " + ex.Message);
            //}
            //finally
            //{
            //    OnEnd();
            //}
        }

        /// <summary>
        /// Function to override, called at the start of SendAndReceive()
        /// Let the children set specific init for its connection type
        /// </summary>
        protected abstract void OnInit();

        /// <summary>
        /// Function to override, called during SendAndReceive()
        /// The child has to implement the way to sending data on the network for its connection type
        /// </summary>
        /// <param name="sendBuffer">The buffer of bytes we want to send</param>
        protected abstract void OnSending(Packet packetToSend);

        protected abstract void OnAfterSend();

        /// <summary>
        /// Function to override, called during SendAndReceive()
        /// The child has to implement the way to receive data on the network for its connection type
        /// </summary>
        /// <param name="receiveBuffer">The buffer we fill we received datas</param>
        /// <returns>The number of bytes received</returns>
        protected abstract int OnReceiving(ref byte[] receiveBuffer);

        /// <summary>
        /// Function to override, called during SendAndReceive()
        /// The child has to implement the way to know if there are pending datas
        /// </summary>
        /// <returns><see cref="true"/> if there are pending datas, otherwise send <see cref="false"/></returns>
        protected abstract bool AreThereAnyPendingDatas();

        /// <summary>
        /// What are we doing with our packet just deserialized ?
        /// </summary>
        /// <param name="packet">The packet we get</param>
        protected abstract void OnPacketDeserialized(Packet packet);

        /// <summary>
        /// Cleanup on dispose
        /// </summary>
        protected abstract void OnEnd();

        /// <summary>
        /// Take a <see cref="Packet"/> from our <see cref="System.Collections.Concurrent.BlockingCollection{Packet}"/>
        /// </summary>
        /// <param name="packet">The packet we took with the enqueued timestamp in MS</param>
        /// <returns><see cref="true"/> if we successfully took a packet from the <see cref="System.Collections.Concurrent.BlockingCollection{Packet}"/>, return <see cref="false"/> otherwise</returns>
        protected abstract bool OnTryTakePacket(out Tuple<double, Packet> packet);

        /// <summary>
        /// Stop the connection
        /// </summary>
        public virtual void Dispose()
        {
            if (m_sendAndReceiveTask != null)
            {
                try
                {
                    // Wait for the end of our task
                    m_cancellationTokenSource.Cancel();
                    m_sendAndReceiveTask.Wait();
                }
                catch (Exception ex)
                {

                }

                // dispose our task
                m_sendAndReceiveTask.Dispose();
                m_sendAndReceiveTask = null;
            }

            // clean our message queue
            if (m_packetsToSend != null)
            {
                m_packetsToSend.Dispose();
                m_packetsToSend = null;
            }

            // clean our cancellation token
            if (m_cancellationToken != null)
            {
                m_cancellationTokenSource.Dispose();
                m_cancellationTokenSource = null;
            }
        }

        /// <summary>
        /// Add packet to the queue to send
        /// </summary>
        /// <param name="packet">The packet to send</param>
        public void SendPacket(Packet packet)
        {
            double timeStamp = (new TimeSpan(DateTime.UtcNow.Ticks)).TotalMilliseconds;
            m_packetsToSend.Add(new Tuple<double, Packet>(timeStamp, packet));
        }
    }
}
