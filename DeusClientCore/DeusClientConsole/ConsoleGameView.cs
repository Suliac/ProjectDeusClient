﻿using DeusClientCore;
using DeusClientCore.Components;
using DeusClientCore.Events;
using DeusClientCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeusClientConsole
{
    public class ConsoleGameView : GameView
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
            Console.WriteLine($"Create View Object | Id obj : {packet.LinkedGameObject.UniqueIdentifier} | Is local player : {packet.LinkedGameObject.IsLocalPlayer}");
            // Create our view object
            ViewObject viewObject = m_objectFactory.CreateViewObject(new ViewObjectCreateArgs(packet.LinkedGameObject));
            AddObject(viewObject);
        }

        private void ManageViewObjectDeletion(PacketDeleteViewObject packet)
        {
            RemoveObject(packet.ObjectId);
        }

        private void ManageViewObjectUpdate(PacketUpdateViewObject packet)
        {
            ViewObject viewObject = m_holdedObjects.FirstOrDefault(vo => vo.UniqueIdentifier == packet.ObjectId);
            if(viewObject != null)
            {
               DeusViewComponent component = viewObject.Get(packet.ComponentId);
                if(component != null)
                {
                    component.UpdateViewValue(packet.NewValue);
                }
            }
        }
    }
}
