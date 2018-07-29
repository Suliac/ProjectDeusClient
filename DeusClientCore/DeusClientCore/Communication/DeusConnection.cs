using DeusClientCore.Packets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        /// The list of message to send
        /// </summary>
        protected BlockingCollection<Packet> m_messagesToSend;

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
            m_messagesToSend = new BlockingCollection<Packet>();
            m_cancellationTokenSource = new CancellationTokenSource();
            m_cancellationToken = m_cancellationTokenSource.Token;

            m_sendAndReceiveTask = new Task(SendAndReceive, m_cancellationToken, TaskCreationOptions.LongRunning);
            m_sendAndReceiveTask.Start();
        }

        /// Loop while we don't want to stop 
        /// Try to take packets from m_messageToSend to send them on the network
        /// then try to read some data on the network
        protected abstract void SendAndReceive();

        /// <summary>
        /// Stop the connection
        /// </summary>
        public void Dispose()
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
            if (m_messagesToSend != null)
            {
                m_messagesToSend.Dispose();
                m_messagesToSend = null;
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
            m_messagesToSend.Add(packet);
        }
    }
}
