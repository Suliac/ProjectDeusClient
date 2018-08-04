using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeusClientCore.Events;
using DeusClientCore.Packets;

namespace DeusClientCore
{
    public class GameView : GamePart
    {
        private static ViewObjectFactory m_objectFactory = null;

        /// <summary>
        /// The renderer has to create it's own <see cref="ViewObjectFactory"/> and register it to the <see cref="GameView"/>
        /// </summary>
        /// <param name="myFactory">The renderer specific object factory</param>
        /// <returns><see cref="true"/> if there wasn't already a factory, otherwise return <see cref="false"/></returns>
        public static bool InitFactory(ViewObjectFactory myFactory)
        {
            if (m_objectFactory != null)
            {
                m_objectFactory = myFactory;
                return true;
            }

            return false;
        }

        public override void Start()
        {
            EventManager.Get().AddListener(Packets.EPacketType.CreateViewObject, ManagePacket);
            EventManager.Get().AddListener(Packets.EPacketType.UpdateViewObject, ManagePacket);
            EventManager.Get().AddListener(Packets.EPacketType.DeleteViewObject, ManagePacket);
        }

        public override void Stop()
        {
            EventManager.Get().RemoveListener(Packets.EPacketType.CreateViewObject, ManagePacket);
            EventManager.Get().RemoveListener(Packets.EPacketType.UpdateViewObject, ManagePacket);
            EventManager.Get().RemoveListener(Packets.EPacketType.DeleteViewObject, ManagePacket);
        }

        protected override void ManagePacket(object sender, SocketPacketEventArgs e)
        {
            if (e.Packet is PacketCreateViewObject)
            {
                ManageViewObjectCreation((PacketCreateViewObject)e.Packet);
            }
            else if (e.Packet is PacketUpdateViewObject)
            {
                ManageViewObjectUpdate((PacketUpdateViewObject)e.Packet);
            }
            else if (e.Packet is PacketDeleteViewObject)
            {
                ManageViewObjectDeletion((PacketDeleteViewObject)e.Packet);
            }
        }

        private void ManageViewObjectCreation(PacketCreateViewObject packet)
        {
            // Create our view object
            ViewObject viewObject = m_objectFactory.CreateViewObject(new ViewObjectCreateArgs(packet.ObjectType, packet.LinkedGameObject));
            AddObject(viewObject);
        }

        private void ManageViewObjectDeletion(PacketDeleteViewObject packet)
        {
            // TODO
        }

        private void ManageViewObjectUpdate(PacketUpdateViewObject packet)
        {
            // TODO
        }
    }
}
