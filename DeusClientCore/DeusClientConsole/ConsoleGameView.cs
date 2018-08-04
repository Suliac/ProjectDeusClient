using DeusClientCore;
using DeusClientCore.Events;
using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientConsole
{
    public class ConsoleGameView : GamePart<ViewObject>
    {
        private ViewObjectFactory m_objectFactory;

        protected override void OnStart()
        {
            m_objectFactory = new ViewObjectFactory();

            EventManager.Get().AddListener(EPacketType.CreateViewObject, ManagePacket);
            EventManager.Get().AddListener(EPacketType.UpdateViewObject, ManagePacket);
            EventManager.Get().AddListener(EPacketType.DeleteViewObject, ManagePacket);
        }

        protected override void OnStop()
        {
            EventManager.Get().RemoveListener(EPacketType.CreateViewObject, ManagePacket);
            EventManager.Get().RemoveListener(EPacketType.UpdateViewObject, ManagePacket);
            EventManager.Get().RemoveListener(EPacketType.DeleteViewObject, ManagePacket);
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
            RemoveObject(packet.ObjectId);
        }

        private void ManageViewObjectUpdate(PacketUpdateViewObject packet)
        {
            // TODO
        }
    }
}
