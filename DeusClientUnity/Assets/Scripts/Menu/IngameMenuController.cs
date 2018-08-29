using DeusClientCore.Events;
using DeusClientCore.Packets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenuController : IMenuController
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ClickLeave()
    {
        PacketHandleClickUI packet = new PacketHandleClickUI();
        packet.UIClicked = PacketHandleClickUI.UIButton.LeaveGameButton;
        EventManager.Get().EnqueuePacket(0, packet);
        MenuController.ChangeState(MenuController.EGameState.Menu);
    }
}
