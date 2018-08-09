using DeusClientCore.Events;
using DeusClientCore.Packets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenuController : MonoBehaviour
{
    public Text Title;
    public uint GameId { get; private set; }
       

    public void SetGameId(uint id)
    {
        GameId = id;
        Title.text = $"Game {id}";
    }

    #region Click events
    public void OnClickQuitGame()
    {
        PacketHandleClickUI packet = new PacketHandleClickUI();
        packet.UIClicked = PacketHandleClickUI.UIButton.LeaveGameButton;
        EventManager.Get().EnqueuePacket(0, packet);
    }

    public void OnClickReady()
    {
        PacketHandleClickUI packet = new PacketHandleClickUI();
        packet.UIClicked = PacketHandleClickUI.UIButton.ReadyButton;
        EventManager.Get().EnqueuePacket(0, packet);
    }

    #endregion
}
