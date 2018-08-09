using DeusClientCore.Events;
using DeusClientCore.Packets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject GamesHolder;
    public GameObject GameLinePrefab;

    // Use this for initialization
    void Start()
    {
        EventManager.Get().AddListener(EPacketType.GetGameAnswer, ManagePacket);
        EventManager.Get().AddListener(EPacketType.JoinGameAnswer, ManagePacket);
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void ManagePacket(object sender, SocketPacketEventArgs e)
    {
        Debug.Log($"receive {e.Packet.Type}");
        if (e.Packet is PacketGetGameAnswer)
        {
            ManageGetGamesAnswer(e.Packet as PacketGetGameAnswer);
        }
        else if (e.Packet is PacketJoinGameAnswer)
        {
            ManageJoinGameAnswer(e.Packet as PacketJoinGameAnswer);
        }

    }

    private void ManageGetGamesAnswer(PacketGetGameAnswer packet)
    {
        foreach (var gameId in packet.GamesIds)
        {
            GameObject newGame = Instantiate(GameLinePrefab);
            newGame.transform.SetParent(GamesHolder.transform);

            Text gameBtnText = newGame.GetComponentInChildren<Text>();
            if(gameBtnText)
                gameBtnText.text = $"Game {gameId}";

            Button gameBtn = newGame.GetComponent<Button>();
            if (gameBtn)
                gameBtn.onClick.AddListener(delegate { OnClickJoinGames(gameId); });
        }

    }

    private void ManageJoinGameAnswer(PacketJoinGameAnswer packet)
    {
        MenuController.ChangeState(MenuController.EGameState.Lobby);
        MenuController.Instance.Lobby.SetGameId(packet.GameJoinedId);
    }

    #region Click events
    public void OnClickCreateGame()
    {
        PacketHandleClickUI packet = new PacketHandleClickUI();
        packet.UIClicked = PacketHandleClickUI.UIButton.CreateGameButton;
        EventManager.Get().EnqueuePacket(0, packet);
    }

    public void OnClickGetGames()
    {
        PacketHandleClickUI packet = new PacketHandleClickUI();
        packet.UIClicked = PacketHandleClickUI.UIButton.GetGameButton;
        EventManager.Get().EnqueuePacket(0, packet);
    }

    public void OnClickJoinGames(uint gameId)
    {
        PacketHandleClickUI packet = new PacketHandleClickUI();
        packet.UIClicked = PacketHandleClickUI.UIButton.JoinGameButton;
        packet.GameIdToJoin = gameId;

        EventManager.Get().EnqueuePacket(0, packet);
    } 
    #endregion

}
