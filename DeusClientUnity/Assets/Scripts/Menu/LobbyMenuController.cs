using DeusClientCore.Events;
using DeusClientCore.Packets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenuController : MonoBehaviour
{
    public Text Title;
    public GameObject PlayersHolder;
    public GameObject LinePlayerPrefab;

    public uint IdGameObj = 1;
    public uint IdCompo = 1;
    public int Amount = 50;

    public uint GameId { get; private set; }
    public Dictionary<uint, string> PlayerInfos { get; private set; }

    private Dictionary<uint, Text> m_lines;
    private void Start()
    {
        EventManager.Get().AddListener(EPacketType.NewPlayerJoin, ManagePacket);
        EventManager.Get().AddListener(EPacketType.LeaveGameAnswer, ManagePacket);
    }

    public void SetGameId(uint id)
    {
        GameId = id;
        Title.text = $"Game {id}";
    }

    public void SetAlreadyHerePlayer(Dictionary<uint, string> playerInfos)
    {
        m_lines = new Dictionary<uint, Text>();
        PlayerInfos = playerInfos;
        foreach (var p in playerInfos)
        {
            GameObject tmpInfo = Instantiate(LinePlayerPrefab);
            Text textCompo = tmpInfo.GetComponent<Text>();
            if (textCompo)
                textCompo.text = p.Value;

            m_lines.Add(p.Key, textCompo);

            tmpInfo.transform.SetParent(PlayersHolder.transform);
        }
    }

    protected void ManagePacket(object sender, SocketPacketEventArgs e)
    {
        Debug.Log($"receive {e.Packet.Type}");
        if (e.Packet is PacketNewPlayerJoin)
        {
            ManageNewPlayerJoin(e.Packet as PacketNewPlayerJoin);
        }
        if (e.Packet is PacketLeaveGameAnswer)
        {
            ManagePlayerLeave(e.Packet as PacketLeaveGameAnswer);
        }

    }

    private void ManageNewPlayerJoin(PacketNewPlayerJoin packetNewPlayerJoin)
    {
        PlayerInfos.Add(packetNewPlayerJoin.PlayerId, packetNewPlayerJoin.PlayerNickname);

        GameObject tmpInfo = Instantiate(LinePlayerPrefab);
        Text textCompo = tmpInfo.GetComponent<Text>();
        if (textCompo)
            textCompo.text = packetNewPlayerJoin.PlayerNickname;

        m_lines.Add(packetNewPlayerJoin.PlayerId, textCompo);
        tmpInfo.transform.SetParent(PlayersHolder.transform);
    }

    private void ManagePlayerLeave(PacketLeaveGameAnswer packetLeaveGame)
    {
        PlayerInfos.Remove(packetLeaveGame.PlayerId);

        if (m_lines.ContainsKey(packetLeaveGame.PlayerId))
        {
            Destroy(m_lines[packetLeaveGame.PlayerId].gameObject);
            m_lines.Remove(packetLeaveGame.PlayerId);
        }
    }

    #region Click events
    public void OnClickQuitGame()
    {
        PacketHandleClickUI packet = new PacketHandleClickUI();
        packet.UIClicked = PacketHandleClickUI.UIButton.LeaveGameButton;
        EventManager.Get().EnqueuePacket(0, packet);

        foreach (var p in PlayerInfos)
            if (m_lines.ContainsKey(p.Key))
                Destroy(m_lines[p.Key].gameObject);

        m_lines.Clear();
        PlayerInfos.Clear();
        MenuController.ChangeState(MenuController.EGameState.Menu);
    }

    public void OnClickReady()
    {
        PacketHandleClickUI packet = new PacketHandleClickUI();
        packet.UIClicked = PacketHandleClickUI.UIButton.ReadyButton;
        EventManager.Get().EnqueuePacket(0, packet);
    }

    public void ClickDebug()
    {
        //PacketHealthUpdate packet = new PacketHealthUpdate(IdGameObj, IdCompo, Amount);
        //EventManager.Get().EnqueuePacket(0, packet);


    }
    #endregion
}
