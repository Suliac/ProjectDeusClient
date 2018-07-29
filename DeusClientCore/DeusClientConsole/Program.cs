using DeusClientCore;
using DeusClientCore.Events;
using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeusClientConsole
{
    class Program
    {
        static bool wantToCancel = false;

        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start("127.0.0.1", 27015);

            Thread myThread = new Thread(new ThreadStart(HandleInput));
            myThread.Start();

            Stopwatch chrono = new Stopwatch();
            chrono.Start();

            while (!wantToCancel)
            {
                long dt = chrono.ElapsedMilliseconds;
                game.Update(dt);
                Thread.Sleep(1);
            }

            game.Stop();
            myThread.Join();
        }

        static void HandleInput()
        {
            while (!wantToCancel)
            {
                Console.WriteLine("0 : Message | 1 : Get games | 2 : Create games | 3 : Join game | 4 : Leave game");
                Console.WriteLine("Your choice : ");
                string choice = Console.ReadLine();

                if (choice == "stop")
                {
                    wantToCancel = true;
                }
                else if (choice == "0")
                {
                    Console.WriteLine("Your message : ");
                    string message = Console.ReadLine();

                    PacketTextMessage packet = new PacketTextMessage();
                    packet.MessageText = message;
                    EventManager.Get().EnqueuePacket(0, packet);

                }
                else if (choice == "1")
                {
                    PacketHandleClickUI packet = new PacketHandleClickUI();
                    packet.UIClicked = PacketHandleClickUI.UIButton.GetGameButton;
                    EventManager.Get().EnqueuePacket(0, packet);
                }
                else if (choice == "2")
                {
                    PacketHandleClickUI packet = new PacketHandleClickUI();
                    packet.UIClicked = PacketHandleClickUI.UIButton.CreateGameButton;
                    EventManager.Get().EnqueuePacket(0, packet);
                }
                else if (choice == "3")
                {
                    Console.WriteLine("Game id : ");
                    string message = Console.ReadLine();

                    uint idGame = 0;
                    if (uint.TryParse(message, out idGame))
                    {
                        PacketHandleClickUI packet = new PacketHandleClickUI();
                        packet.UIClicked = PacketHandleClickUI.UIButton.JoinGameButton;
                        packet.GameIdToJoin = idGame;

                        EventManager.Get().EnqueuePacket(0, packet);
                    }

                }
                else if (choice == "4")
                {
                    PacketHandleClickUI packet = new PacketHandleClickUI();
                    packet.UIClicked = PacketHandleClickUI.UIButton.LeaveGameButton;
                    EventManager.Get().EnqueuePacket(0, packet);
                }
            }
        }


    }
}
