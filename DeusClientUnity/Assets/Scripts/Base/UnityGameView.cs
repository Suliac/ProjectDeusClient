using DeusClientCore;
using DeusClientCore.Events;
using DeusClientCore.Packets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnityGameView : MonoBehaviour
{
    [SerializeField]
    private List<UnityEngine.GameObject> m_viewObjects;

    private void Awake()
    {
        m_viewObjects = new List<UnityEngine.GameObject>();
    }

    public void Start()
    {
        EventManager.Get().AddListener(EPacketType.CreateViewObject, ManagePacket);
        EventManager.Get().AddListener(EPacketType.UpdateViewObject, ManagePacket);
        EventManager.Get().AddListener(EPacketType.DeleteViewObject, ManagePacket);
    }

    public void Stop()
    {
        EventManager.Get().RemoveListener(EPacketType.CreateViewObject, ManagePacket);
        EventManager.Get().RemoveListener(EPacketType.UpdateViewObject, ManagePacket);
        EventManager.Get().RemoveListener(EPacketType.DeleteViewObject, ManagePacket);
    }

    private void ManagePacket(object sender, SocketPacketEventArgs e)
    {
        Debug.Log($"receive {e.Packet.Type}");
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
        Debug.Log($"Create View Object | Id obj : {packet.LinkedGameObject.UniqueIdentifier} | Is local player : {packet.LinkedGameObject.IsLocalPlayer}");

        // Create our view object
        GameObject viewObject = ViewObjectFactory.CreateViewObject(new ViewObjectCreateArgs(packet.LinkedGameObject));
        m_viewObjects.Add(viewObject);
    }

    private void ManageViewObjectDeletion(PacketDeleteViewObject packet)
    {
        GameObject objectToDelete = m_viewObjects.FirstOrDefault(vo => vo.GetComponent<DeusObjectLinker>() && vo.GetComponent<DeusObjectLinker>().GetDeusObjectId() == packet.ObjectId);
        if (objectToDelete)
        {
            m_viewObjects.Remove(objectToDelete);
            Destroy(objectToDelete);
        }
    }

    private void ManageViewObjectUpdate(PacketUpdateViewObject packet)
    {
        GameObject objectToUpdate = m_viewObjects.FirstOrDefault(vo => vo.GetComponent<DeusObjectLinker>() && vo.GetComponent<DeusObjectLinker>().GetDeusObjectId() == packet.ObjectId);
        if (objectToUpdate)
        {
            DeusComponentLinker component = objectToUpdate.GetComponents<DeusComponentLinker>().FirstOrDefault(dcl =>dcl.GetComponentId() == packet.ComponentId);
            if (component)
            {
                component.UpdateViewValue(packet.NewValue);
            }
        }
    }
}
