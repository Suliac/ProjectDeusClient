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
        GameManager.Instance.SetCamera(true);
        PacketHandleClickUI packet = new PacketHandleClickUI();

        foreach(Transform child in GameManager.GameObjectContainer.transform)
        {
            Destroy(child.gameObject);
        }
        
        packet.UIClicked = PacketHandleClickUI.UIButton.LeaveGameButton;
        EventManager.Get().EnqueuePacket(0, packet);
        MenuController.ChangeState(MenuController.EGameState.Menu);
    }
}
