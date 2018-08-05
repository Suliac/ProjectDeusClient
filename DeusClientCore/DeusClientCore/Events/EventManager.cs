using DeusClientCore.Packets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeusClientCore.Events
{
    public class EventManager : IExecutable
    {
        #region Singleton
        private static EventManager m_instance = null;

        public static EventManager Get()
        {
            if (m_instance == null)
                m_instance = new EventManager();

            return m_instance;
        }
        #endregion

        /// <summary>
        /// Dictionnary of Packet type with all the deleguate associated
        /// </summary>
        public Dictionary<EPacketType, EventHandler<SocketPacketEventArgs>> m_onMessageReceived;

        /// <summary>
        /// Number of packet's queue we have
        /// </summary>
        const int NUMBER_QUEUES = 2;

        /// <summary>
        /// 
        /// </summary>
        const int MAX_DURATION_PROCESSEVENTS_MS = 5;

        /// <summary>
        /// The index of the current active queue
        /// </summary>
        int m_activeQueue = 0;

        object m_activeQueueIndexLocker = new object();

        /// <summary>
        /// Table of packet's queues : we have many to handle easily the insert and read&
        /// </summary>
        BlockingCollection<Tuple<int, Packet>>[] m_packetQueue = new BlockingCollection<Tuple<int, Packet>>[NUMBER_QUEUES];

        public bool Stopped { get; private set; }

        private EventManager()
        {
            Stopped = false;
            m_onMessageReceived = new Dictionary<EPacketType, EventHandler<SocketPacketEventArgs>>();
            m_packetQueue[0] = new BlockingCollection<Tuple<int, Packet>>();
            m_packetQueue[1] = new BlockingCollection<Tuple<int, Packet>>();
        }

        public void Start()
        {
            Stopped = false;

            if (m_onMessageReceived != null)
                m_onMessageReceived.Clear();

            m_onMessageReceived = new Dictionary<EPacketType, EventHandler<SocketPacketEventArgs>>();
            m_packetQueue[0] = new BlockingCollection<Tuple<int, Packet>>();
            m_packetQueue[1] = new BlockingCollection<Tuple<int, Packet>>();
        }

        public void Stop()
        {
            if (m_onMessageReceived != null)
                m_onMessageReceived.Clear();

            // clear blocking queue
            foreach (var packetQueue in m_packetQueue)
            {
                if (packetQueue != null)
                {
                    while (packetQueue.Count > 0)
                    {
                        Tuple<int, Packet> item;
                        packetQueue.TryTake(out item);
                    }
                } 
            }

            Stopped = true;
        }

        public void Update(decimal deltatimeMs)
        {
            if (Stopped)
                return;

            Stopwatch stopWatch = Stopwatch.StartNew(); // start timer
            int queueToProcess = m_activeQueue;

            // Changing active queue
            lock (m_activeQueueIndexLocker)
            {
                // our old active queue which get packet, become the queue process
                m_activeQueue = (m_activeQueue + 1) % NUMBER_QUEUES;

                // clear the next active queue
                while (m_packetQueue[m_activeQueue].Count > 0)
                {
                    Tuple<int, Packet> item;
                    m_packetQueue[m_activeQueue].TryTake(out item);
                }
            }

            // process the queue
            while (m_packetQueue[queueToProcess].Count > 0)
            {
                // pop item
                Tuple<int, Packet> packet;
                m_packetQueue[queueToProcess].TryTake(out packet);

                // trigger event of type
                if (m_onMessageReceived.ContainsKey(packet.Item2.Type))
                {
                    m_onMessageReceived[packet.Item2.Type](this, new SocketPacketEventArgs(packet.Item2, packet.Item1));
                }

                // if process events take too long, break
                if (stopWatch.ElapsedMilliseconds > MAX_DURATION_PROCESSEVENTS_MS)
                    break;

            } // while

            if (m_packetQueue[queueToProcess].Count > 0)
            {
                // We have to put the element of queue to process before the content of active queue
                var tempQueue = new BlockingCollection<Tuple<int, Packet>>();
                lock (m_activeQueueIndexLocker)
                {
                    // put active queue content into tmp queue
                    while (m_packetQueue[m_activeQueue].Count > 0)
                        tempQueue.Add(m_packetQueue[m_activeQueue].Take());

                    // put all left event into active queue
                    while (m_packetQueue[queueToProcess].Count > 0)
                        m_packetQueue[m_activeQueue].Add(m_packetQueue[queueToProcess].Take());

                    // reput all old active content after
                    while (tempQueue.Count > 0)
                        m_packetQueue[m_activeQueue].Add(tempQueue.Take());
                } // lock
            } // if
        }

        public void AddListener(EPacketType type, EventHandler<SocketPacketEventArgs> listener)
        {
            if (!m_onMessageReceived.ContainsKey(type))
                m_onMessageReceived.Add(type, listener);
            else
                m_onMessageReceived[type] += listener;
        }
        public bool RemoveListener(EPacketType type, EventHandler<SocketPacketEventArgs> listener)
        {
            if (!m_onMessageReceived.ContainsKey(type))
                return false;
            else
                m_onMessageReceived[type] -= listener;

            return true;
        }

        public void EnqueuePacket(int idSource, Packet packet)
        {
            lock (m_activeQueueIndexLocker)
            {
                m_packetQueue[m_activeQueue].Add(new Tuple<int, Packet>(idSource, packet));
            }
        }
    }
}
