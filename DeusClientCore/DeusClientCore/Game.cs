using DeusClientCore.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public class Game
    {
        /// <summary>
        /// Interface to communicate with client
        /// </summary>
        private DeusClient m_deusClient;

        private GameLogic m_gameLogic;

        private bool m_wantToStop = false;

        /// <summary>
        /// Start the game, connect to the server
        /// </summary>
        /// <param name="addr">The ip address of the server</param>
        /// <param name="port">The default port of the server</param>
        public void Start(string addr, int port)
        {
            // Init event manager
            EventManager.Get().Start();

            // Init connections
            m_deusClient = new DeusClient(new TcpClient(addr, port));
        }

        /// <summary>
        /// Called at each frame, update the state of the game
        /// </summary>
        /// <param name="deltatime">The time elapsed from the last call</param>
        public void Update(decimal deltatime)
        {
            // update our objects and manage gamelogic
            m_gameLogic.Update(deltatime);

            // TODO : update the game view ?

            // update the event manager
            EventManager.Get().Update(deltatime);
        }

        /// <summary>
        /// Stop the game, disconnect from the server and clean everything
        /// </summary>
        public void Stop()
        {
            m_deusClient.Dispose();
            
            EventManager.Get().Stop();
        }
    }
}
