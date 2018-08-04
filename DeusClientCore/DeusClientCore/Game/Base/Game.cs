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

        private GameLogic m_logic;
        
        public Game()
        {
            m_logic = new GameLogic();
        }

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

            // Init game logic
            m_logic.Start();
        }

        /// <summary>
        /// Called at each frame, update the state of the game
        /// </summary>
        /// <param name="deltatime">The time elapsed from the last call</param>
        public void Update(decimal deltatimeMs)
        {
            // update our objects and manage gamelogic
            m_logic.Update(deltatimeMs);

            // update the event manager
            EventManager.Get().Update(deltatimeMs);
        }

        /// <summary>
        /// Stop the game, disconnect from the server and clean everything
        /// </summary>
        public void Stop()
        {
            m_deusClient.Dispose();

            // Stop game logic
            m_logic.Stop();

            EventManager.Get().Stop();
        }
    }
}
