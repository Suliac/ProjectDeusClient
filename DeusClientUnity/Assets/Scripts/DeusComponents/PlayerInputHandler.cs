using DeusClientCore;
using DeusClientCore.Events;
using DeusClientCore.Packets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{

    public uint PositionComponentId;
    public uint ObjectId;
    public float Distance = 50f;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (GetMouseGamePosition(out hit))
            {
                PacketHandleMovementInput packet = new PacketHandleMovementInput(ObjectId, PositionComponentId, new DeusVector2(hit.point.x, hit.point.z));
                EventManager.Get().EnqueuePacket(0, packet);
            }
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            RaycastHit hit;
            if (GetMouseGamePosition(out hit))
            {
                PacketHandleSkillInput packet = new PacketHandleSkillInput(ObjectId, PositionComponentId, 1, new DeusVector2(hit.point.x, hit.point.z));
                EventManager.Get().EnqueuePacket(0, packet);
            }
        }
    }

    bool GetMouseGamePosition(out RaycastHit hit)
    {
        Ray ray = GameManager.Instance.PlayerCamera.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, Distance);
    }
}
