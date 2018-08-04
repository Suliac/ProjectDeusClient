using DeusClientCore.Events;
using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientCore
{
    /// <summary>
    /// <see cref="GameLogic"/> manage all the logic of the game, clien side.
    /// It contains a <see cref="List{GameObject}"/> and when its Update(), Start or Stop() methods are called, they also called the methods for the all <see cref="GameObject"/>
    /// </summary>
    public class GameLogic : GamePart<GameObject>
    {
        private GameObjectFactory m_objectFactory;
        
        protected override void OnStart()
        {
            m_objectFactory = new GameObjectFactory();

            // Games lobby management
            EventManager.Get().AddListener(Packets.EPacketType.CreateGameAnswer     , ManagePacket);
            EventManager.Get().AddListener(Packets.EPacketType.GetGameAnswer        , ManagePacket);
            EventManager.Get().AddListener(Packets.EPacketType.JoinGameAnswer       , ManagePacket);
            EventManager.Get().AddListener(Packets.EPacketType.LeaveGameAnswer      , ManagePacket);

            // Game view
            EventManager.Get().AddListener(Packets.EPacketType.HandleClickUI        , ManagePacket);

            // Game logic
            EventManager.Get().AddListener(Packets.EPacketType.ObjectEnter          , ManagePacket);
            EventManager.Get().AddListener(Packets.EPacketType.ObjectLeave          , ManagePacket);
        }

        protected override void OnStop()
        {
            // Games lobby management
            EventManager.Get().RemoveListener(Packets.EPacketType.CreateGameAnswer  , ManagePacket);
            EventManager.Get().RemoveListener(Packets.EPacketType.GetGameAnswer     , ManagePacket);
            EventManager.Get().RemoveListener(Packets.EPacketType.JoinGameAnswer    , ManagePacket);
            EventManager.Get().RemoveListener(Packets.EPacketType.LeaveGameAnswer   , ManagePacket);

            // Game view
            EventManager.Get().RemoveListener(Packets.EPacketType.HandleClickUI     , ManagePacket);

            // Game logic
            EventManager.Get().RemoveListener(Packets.EPacketType.ObjectEnter       , ManagePacket);
            EventManager.Get().RemoveListener(Packets.EPacketType.ObjectLeave       , ManagePacket);
        }

        #region Events managements
        protected override void ManagePacket(object sender, SocketPacketEventArgs e)
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
            else if (e.Packet is PacketObjectEnter)
            {
                ManageObjectEnter((PacketObjectEnter)e.Packet);
            }
            else if (e.Packet is PacketObjectLeave)
            {
                ManageObjectLeave((PacketObjectLeave)e.Packet);
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

        private void ManageObjectEnter(PacketObjectEnter packet)
        {
            GameObject gameObject = m_objectFactory.CreateGameObject(new GameObjectCreateArgs(packet.ObjectType));
            AddObject(gameObject);
        }

        private void ManageObjectLeave(PacketObjectLeave packet)
        {
            // delete from our
            RemoveObject(packet.GameObjectId);

            // notify the view that there is a new object to display
            PacketDeleteViewObject deleteViewObjectRequest = new PacketDeleteViewObject();
            deleteViewObjectRequest.ObjectId = packet.GameObjectId;
            EventManager.Get().EnqueuePacket(0, deleteViewObjectRequest);
        } 
        #endregion
    }
}
