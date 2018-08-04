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

        private List<GamePart> m_gameParts;

        private bool m_wantToStop = false;

        public Game()
        {
            m_gameParts = new List<GamePart>();
            m_gameParts.Add(new GameLogic());
            m_gameParts.Add(new GameView());
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

            // Init game parts : logic and view
            foreach (var gamePart in m_gameParts)
                gamePart.Start();
        }

        /// <summary>
        /// Called at each frame, update the state of the game
        /// </summary>
        /// <param name="deltatime">The time elapsed from the last call</param>
        public void Update(decimal deltatimeMs)
        {
            // update our objects and manage gamelogic & game view
            foreach (var gamePart in m_gameParts)
                gamePart.Update(deltatimeMs);

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
            foreach (var gamePart in m_gameParts)
                gamePart.Stop();

            EventManager.Get().Stop();
        }
    }
}
