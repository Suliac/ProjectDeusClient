using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public enum EGameState
    {
        Connection,
        Menu,
        Lobby,
        InGame
    }

    [Serializable]
    public struct MenuForState
    {
        public EGameState State;
        public IMenuController Menu;
    }

    public static MenuController Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public EGameState State;
    public List<MenuForState> Menus = new List<MenuForState>();

    private void Start()
    {
        ChangeState(EGameState.Connection);
    }

    private LobbyMenuController GetLobby()
    {
        for (int i = 0; i < Instance.Menus.Count; i++)
            if (EGameState.Lobby == Instance.Menus[i].State)
                return (Instance.Menus[i].Menu as LobbyMenuController);

        return null;
    }

    public static void ChangeState(EGameState state)
    {
        Instance.State = state;

        for (int i = 0; i < Instance.Menus.Count; i++)
            Instance.Menus[i].Menu.gameObject.SetActive(state == Instance.Menus[i].State);
    }

    public static void ToLobby(uint gameId, Dictionary<uint, string> playerInfos)
    {
        ChangeState(EGameState.Lobby);

        Instance.GetLobby().SetGameId(gameId);
        Instance.GetLobby().SetAlreadyHerePlayer(playerInfos);
    }

}
