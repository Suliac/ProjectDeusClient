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
	

    public float distance = 50f;
    //replace Update method in your class with this one
    void Update()
    {
        //if mouse button (left hand side) pressed instantiate a raycast
        if (Input.GetMouseButtonDown(0))
        {
            //create a ray cast and set it to the mouses cursor position in game
            Ray ray = GameManager.Instance.PlayerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distance))
            {
                PacketHandleMovementInput packet = new PacketHandleMovementInput(ObjectId, PositionComponentId, new DeusVector2(hit.point.x, hit.point.z));
                EventManager.Get().EnqueuePacket(0, packet);
            }
        }
    }
}
