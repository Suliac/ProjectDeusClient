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
            GameObject gameObject = new GameObject(m_nextId);
            m_nextId++;

            // fill our gameobject with components
            switch (args.Type)
            {
                case EObjectType.Player:
                    var healthComponent = new HealthTimeLineComponent();
                    gameObject.AddComponent(healthComponent);
                    break;
                default:
                    break;
            }

            // notify the view that there is a new object to display
            PacketCreateViewObject packet = new PacketCreateViewObject();
            packet.LinkedGameObject = gameObject;
            packet.ObjectId = gameObject.UniqueIdentifier;
            EventManager.Get().EnqueuePacket(0, packet);

            return gameObject;
        }
    }
}
