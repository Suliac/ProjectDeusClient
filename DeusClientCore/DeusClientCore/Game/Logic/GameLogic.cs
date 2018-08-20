using DeusClientCore.Components;
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
    /// It contains a <see cref="List{GameObject}"/> and when its Update(), Start or Stop() methods are called, they also called the methods for the all <see cref="DeusGameObject"/>
    /// </summary>
    public class GameLogic : GamePart<DeusGameObject>
    {
        private GameObjectFactory m_objectFactory;

        protected override void OnStart()
        {
            m_objectFactory = new GameObjectFactory();

            // Games lobby management
            EventManager.Get().AddListener(Packets.EPacketType.CreateGameAnswer, ManagePacket);
            EventManager.Get().AddListener(Packets.EPacketType.GetGameAnswer, ManagePacket);
            EventManager.Get().AddListener(Packets.EPacketType.JoinGameAnswer, ManagePacket);
            EventManager.Get().AddListener(Packets.EPacketType.LeaveGameAnswer, ManagePacket);

            // Game view
            EventManager.Get().AddListener(Packets.EPacketType.HandleClickUI, ManagePacket);
            EventManager.Get().AddListener(Packets.EPacketType.HandleMovementInputs, ManagePacket);

            // Game logic
            EventManager.Get().AddListener(Packets.EPacketType.ObjectEnter, ManagePacket);
            EventManager.Get().AddListener(Packets.EPacketType.ObjectLeave, ManagePacket);
            EventManager.Get().AddListener(Packets.EPacketType.UpdateHealth, ManagePacket);
            EventManager.Get().AddListener(Packets.EPacketType.UpdateMovementAnswer, ManagePacket);
        }

        protected override void OnStop()
        {
            // Games lobby management
            EventManager.Get().RemoveListener(Packets.EPacketType.CreateGameAnswer, ManagePacket);
            EventManager.Get().RemoveListener(Packets.EPacketType.GetGameAnswer, ManagePacket);
            EventManager.Get().RemoveListener(Packets.EPacketType.JoinGameAnswer, ManagePacket);
            EventManager.Get().RemoveListener(Packets.EPacketType.LeaveGameAnswer, ManagePacket);

            // Game view
            EventManager.Get().RemoveListener(Packets.EPacketType.HandleClickUI, ManagePacket);

            // Game logic
            EventManager.Get().RemoveListener(Packets.EPacketType.ObjectEnter, ManagePacket);
            EventManager.Get().RemoveListener(Packets.EPacketType.ObjectLeave, ManagePacket);
            EventManager.Get().RemoveListener(Packets.EPacketType.UpdateHealth, ManagePacket);
            EventManager.Get().RemoveListener(Packets.EPacketType.UpdateMovementAnswer, ManagePacket);
        }

        #region Events managements
        protected override void ManagePacket(object sender, SocketPacketEventArgs e)
        {
            /////////////////////// LOBBY 
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

            /////////////////////// UI 
            else if (e.Packet is PacketHandleClickUI)
            {
                ManageHandleUIPacket((PacketHandleClickUI)e.Packet);
            }
            else if (e.Packet is PacketHandleMovementInput)
            {
                ManageHandleMovementRequest((PacketHandleMovementInput)e.Packet);
            }

            /////////////////////// LOGIC
            else if (e.Packet is PacketObjectEnter)
            {
                ManageObjectEnter((PacketObjectEnter)e.Packet);
            }
            else if (e.Packet is PacketObjectLeave)
            {
                ManageObjectLeave((PacketObjectLeave)e.Packet);
            }
            else if (e.Packet is PacketHealthUpdate)
            {
                ManageUpdateHealth((PacketHealthUpdate)e.Packet);
            }
            else if (e.Packet is PacketMovementUpdateAnswer)
            {
                ManagePacketMovementAnswer((PacketMovementUpdateAnswer)e.Packet);
            }
        }

        #region Lobby
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
        #endregion

        #region View Requests
        private void ManageHandleUIPacket(PacketHandleClickUI packet)
        {
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
                case PacketHandleClickUI.UIButton.ReadyButton:
                    EventManager.Get().EnqueuePacket(0, new PacketPlayerReady());
                    break;
                default:
                    break;
            }
        }

        private void ManageHandleMovementRequest(PacketHandleMovementInput packet)
        {
            // Send requets to the server
            PacketMovementUpdateRequest movUpdateRequest = new PacketMovementUpdateRequest(packet.DestinationWanted, packet.ComponentId);
            EventManager.Get().EnqueuePacket(0, movUpdateRequest);
        }
        #endregion

        #region Logic
        private void ManageObjectEnter(PacketObjectEnter packet)
        {
            DeusGameObject gameObject = m_objectFactory.CreateGameObject(new GameObjectCreateArgs(packet.GameObjectId, packet.ObjectType, packet.IsLocalPlayer));
            Console.WriteLine($"Create Game Object | Id obj : {packet.GameObjectId} | Is local player : {packet.IsLocalPlayer}");
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

        private void ManageUpdateHealth(PacketHealthUpdate packet)
        {
            DeusComponent component = FindComponent(packet.ObjectId, packet.ComponentId);
            if (component != null && component is HealthTimeLineComponent)
            {
                (component as HealthTimeLineComponent).InsertData(packet.NewHealthAmount, packet.NewHealthTimestamp);

                // Notify the view, that component value has just changed : use this only if your component isn't getting directly informations
                PacketUpdateViewObject feedBackPacket = new PacketUpdateViewObject();
                feedBackPacket.ObjectId = packet.ObjectId;
                feedBackPacket.ComponentId = packet.ComponentId;
                feedBackPacket.NewValue = packet.NewHealthAmount;
                EventManager.Get().EnqueuePacket(0, feedBackPacket);
            }
        }

        private void ManagePacketMovementAnswer(PacketMovementUpdateAnswer packet)
        {
            Console.WriteLine($"Manage update movement | Origin ({packet.PositionOrigin.X},{packet.PositionOrigin.Y}) : {packet.OriginTimestampMs} | Destination ({packet.Destination.X},{packet.Destination.Y}) : {packet.DestinationTimestampMs}");
            Console.WriteLine($"Current Ms : {TimeHelper.GetUnixMsTimeStamp()}");
            UpdateTimelineComponent<PositionTimeLineComponent, DeusVector2>(packet.ObjectId, packet.ComponentId, packet.PositionOrigin, packet.OriginTimestampMs, packet.Destination, packet.DestinationTimestampMs);
        }
        #endregion
        #endregion

        private DeusComponent FindComponent(uint objectId, uint componentId)
        {
            return m_holdedObjects.FirstOrDefault(go => go.UniqueIdentifier == objectId)?.GetComponent(componentId) ?? null;
        }

        private void UpdateTimelineComponent<T, U>(uint objectId, uint componentId, U originValue, uint originTimestampMs, U destinationValue, uint destinationTimestampMs) where T : TimeLineComponent<U>
        {
            DeusComponent component = FindComponent(objectId, componentId);
            if (component != null && component is T)
            {
                (component as T).InsertData(originValue, originTimestampMs);
                (component as T).InsertData(destinationValue, destinationTimestampMs);
            }
        }
    }
}
