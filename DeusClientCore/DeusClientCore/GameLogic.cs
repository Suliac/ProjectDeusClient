using DeusClientCore.Events;
using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    public class GameLogic : IUpdatable, IDisposable
    {
        public void Dispose()
        {
            EventManager.Get().RemoveListener(Packets.EPacketType.CreateGameAnswer, ManagePacket);
            EventManager.Get().RemoveListener(Packets.EPacketType.GetGameAnswer, ManagePacket);
            EventManager.Get().RemoveListener(Packets.EPacketType.JoinGameAnswer, ManagePacket);
            EventManager.Get().RemoveListener(Packets.EPacketType.LeaveGameAnswer, ManagePacket);
            EventManager.Get().RemoveListener(Packets.EPacketType.HandleClickUI, ManagePacket);
        }

        public void Start()
        {
            EventManager.Get().AddListener(Packets.EPacketType.CreateGameAnswer, ManagePacket);
            EventManager.Get().AddListener(Packets.EPacketType.GetGameAnswer, ManagePacket);
            EventManager.Get().AddListener(Packets.EPacketType.JoinGameAnswer, ManagePacket);
            EventManager.Get().AddListener(Packets.EPacketType.LeaveGameAnswer, ManagePacket);
            EventManager.Get().AddListener(Packets.EPacketType.HandleClickUI, ManagePacket);
        }

        public void Stop()
        {
            Dispose();
        }

        public void Update(decimal deltatimeMs)
        {
            // TODO : call update for each GameObject ?
        }

        private void ManagePacket(object sender, SocketPacketEventArgs e)
        {
            if (e.Packet is PacketCreateGameAnswer)
            {
                ManageCreateGameAnswerPacket((PacketCreateGameAnswer)e.Packet);
            }
            else if (e.Packet is PacketGetGameAnswer)
            {
                ManageGetGameAnswerPacket((PacketGetGameAnswer)e.Packet);
            }
            else if (e.Packet is PacketJoinGameAnswer)
            {
                ManageJoinGameAnswerPacket((PacketJoinGameAnswer)e.Packet);
            }
            else if (e.Packet is PacketLeaveGameAnswer)
            {
                ManageLeaveGameAnswerPacket((PacketLeaveGameAnswer)e.Packet);
            }
            else if (e.Packet is PacketHandleClickUI)
            {
                ManageHandleUIPacket((PacketHandleClickUI)e.Packet);
            }
        }

        private void ManageCreateGameAnswerPacket(PacketCreateGameAnswer packet)
        {
            Console.WriteLine("Create game : " + (packet.IsSuccess ? "success" : "failed"));
        }

        private void ManageGetGameAnswerPacket(PacketGetGameAnswer packet)
        {
            Console.WriteLine("Game availables : " + packet.GamesIds.Count);
        }

        private void ManageJoinGameAnswerPacket(PacketJoinGameAnswer packet)
        {
            Console.WriteLine("Join game : " + packet.GameJoinedId);
        }

        private void ManageLeaveGameAnswerPacket(PacketLeaveGameAnswer packet)
        {
            Console.WriteLine("Leaved game : " + (packet.IsSuccess ? "success" : "failed"));
        }

        private void ManageHandleUIPacket(PacketHandleClickUI packet)
        {
            Console.WriteLine("Handle Game Input : ");
            // TODO : check state du jeu -> le joueur peut avoir cliqué sur le bouton?
            switch (packet.UIClicked)
            {
                case PacketHandleClickUI.UIButton.JoinGameButton:
                    PacketJoinGameRequest newPacketJoin = new PacketJoinGameRequest();
                    newPacketJoin.GameJoinedId = packet.GameIdToJoin;
                    EventManager.Get().EnqueuePacket(0, newPacketJoin);
                    break;
                case PacketHandleClickUI.UIButton.CreateGameButton:
                    EventManager.Get().EnqueuePacket(0, new PacketCreateGameRequest());
                    break;
                case PacketHandleClickUI.UIButton.LeaveGameButton:
                    EventManager.Get().EnqueuePacket(0, new PacketLeaveGameRequest());
                    break;
                case PacketHandleClickUI.UIButton.GetGameButton:
                    EventManager.Get().EnqueuePacket(0, new PacketGetGameRequest());
                    break;
                case PacketHandleClickUI.UIButton.SendTextButton:
                    PacketTextMessage newPacketText = new PacketTextMessage();
                    newPacketText.MessageText = packet.TextMessage;
                    EventManager.Get().EnqueuePacket(0, newPacketText);
                    break;
                default:
                    break;
            }
        }

    }
}
