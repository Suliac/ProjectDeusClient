using DeusClientCore;
using DeusClientCore.Events;
using DeusClientCore.Packets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour {

    public uint PositionComponentId;
    public uint ObjectId;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            PacketHandleMovementInput packet = new PacketHandleMovementInput(ObjectId, PositionComponentId, new DeusVector2(0, 10));
            EventManager.Get().EnqueuePacket(0, packet);
        }

        if(Input.GetKeyUp(KeyCode.Z))
        {
            PacketHandleMovementInput packet = new PacketHandleMovementInput(ObjectId, PositionComponentId, new DeusVector2(0, 0));
            EventManager.Get().EnqueuePacket(0, packet);
        }
    }
}
