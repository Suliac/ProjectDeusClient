using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore.Events
{
    /// <summary>
    /// Args for the socket's events
    /// </summary>
    public class SocketEventArgs : EventArgs
    {
        /// <summary>
        /// Unique identifier of the source of the event
        /// </summary>
        private int m_idSource;

        /// <summary>
        /// Get the unique identifier of the source of the event
        /// </summary>
        public int IdSource { get { return m_idSource; } }

        /// <summary>
        /// Init new instance of <see cref="SocketEventArgs"/>
        /// </summary>
        /// <param name="idSource">Unique identifier of the source of the event</param>
        public SocketEventArgs(int idSource)
        {
            m_idSource = idSource;
        }
    }
}
