﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeusClientCore.Components;
using DeusClientCore.Events;
using DeusClientCore.Packets;

namespace DeusClientCore
{
    public enum EObjectType
    {
        Player
    }

    public struct GameObjectCreateArgs
    {
        public EObjectType Type { get; set; }

        public GameObjectCreateArgs(EObjectType type)
        {
            Type = type;
        }
    }

    public class GameObjectFactory
    {
        private static uint m_nextId = 1;

        public GameObject CreateGameObject(GameObjectCreateArgs args)
        {
            // get id
            uint gameObjectId = m_nextId;
            m_nextId++;

            // Create all the components
            List<DeusComponent> components = new List<DeusComponent>();
            switch (args.Type)
            {
                case EObjectType.Player:
                    var healthComponent = new HealthTimeLineComponent();
                    components.Add(healthComponent);
                    break;
                default:
                    break;
            }

            // Create the gameobject
            GameObject gameObject = new GameObject(gameObjectId, args.Type, components);

            // notify the view that there is a new object to display
            PacketCreateViewObject packet = new PacketCreateViewObject();
            packet.LinkedGameObject = gameObject;
            packet.ObjectId = gameObject.UniqueIdentifier;
            EventManager.Get().EnqueuePacket(0, packet);

            return gameObject;
        }
    }
}