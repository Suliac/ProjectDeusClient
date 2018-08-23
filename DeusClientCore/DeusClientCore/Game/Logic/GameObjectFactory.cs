using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeusClientCore.Components;
using DeusClientCore.Events;
using DeusClientCore.Packets;

namespace DeusClientCore
{
    public enum EObjectType : byte
    {
        Player = 0,
    }

    public struct GameComponentCreateArgs
    {
        public uint GameComponentId { get; private set; }

        public EComponentType Type { get; private set; }

        public GameComponentCreateArgs(uint componentId, EComponentType componentType)
        {
            GameComponentId = componentId;
            Type = componentType;
        }

    }

    public struct GameObjectCreateArgs
    {
        public uint GameObjectId { get; private set; }

        public EObjectType Type { get; private set; }

        public bool IsLocalPlayer { get; private set; }

        public List<GameComponentCreateArgs> ComponentsInfos { get; private set; }

        public GameObjectCreateArgs(uint gameObjectId, EObjectType type, bool isLocalPlayer, List<GameComponentCreateArgs> componentsInfos)
        {
            GameObjectId = gameObjectId;
            Type = type;
            IsLocalPlayer = isLocalPlayer;
            ComponentsInfos = componentsInfos;
        }
    }

    public class GameObjectFactory
    {
        internal class GameComponentFactory
        {
            public static DeusComponent CreateComponent(GameComponentCreateArgs args)
            {
                switch (args.Type)
                {
                    case EComponentType.HealthComponent:
                        return new HealthTimeLineComponent(args.GameComponentId);
                    case EComponentType.PositionComponent:
                        return new PositionTimeLineComponent(args.GameComponentId);
                    default:
                        return null;
                }
            }
        }

        public DeusGameObject CreateGameObject(GameObjectCreateArgs args)
        {
            // Create all the components
            List<DeusComponent> components = new List<DeusComponent>();
            foreach (var component in args.ComponentsInfos)
                components.Add(GameComponentFactory.CreateComponent(component));

            // Create the gameobject
            DeusGameObject gameObject = new DeusGameObject(args, components);

            // notify the view that there is a new object to display
            PacketCreateViewObject packet = new PacketCreateViewObject();
            packet.LinkedGameObject = gameObject;
            packet.ObjectId = gameObject.UniqueIdentifier;
            EventManager.Get().EnqueuePacket(0, packet);

            return gameObject;
        }
    }
}
