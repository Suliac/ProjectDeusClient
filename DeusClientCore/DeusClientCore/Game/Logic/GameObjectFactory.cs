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
    
    public struct GameObjectCreateArgs
    {
        public uint GameObjectId { get; private set; }

        public EObjectType Type { get; private set; }

        public bool IsLocalPlayer { get; private set; }

        public List<ISerializableComponent> ComponentsInfos { get; private set; }

        public GameObjectCreateArgs(uint gameObjectId, EObjectType type, bool isLocalPlayer, List<ISerializableComponent> componentsInfos)
        {
            GameObjectId = gameObjectId;
            Type = type;
            IsLocalPlayer = isLocalPlayer;
            ComponentsInfos = componentsInfos;
        }
    }

    public class GameObjectFactory
    {

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
