using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Events
{
    public class SocketPacketEventArgs : SocketEventArgs
    {
        /// <summary>
        /// The instance of packet
        /// </summary>
        private Packet m_packet;

        /// <summary>
        /// Get the instance of packet
        /// </summary>
        public Packet Packet { get { return m_packet; } }

        /// <summary>
        /// Init a new instance of <see cref="SocketPacketEventArgs"/>
        /// </summary>
        /// <param name="packet">The instance of packet</param>
        /// <param name="idSource">The source of the event</param>
        public SocketPacketEventArgs(Packet packet, int idSource) : base(idSource)
        {
            m_packet = packet;
        }
    }
}
