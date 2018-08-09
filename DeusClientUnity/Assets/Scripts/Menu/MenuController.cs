using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {
    public enum EGameState
    {
        Menu,
        Lobby,
        InGame
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
    public MainMenuController Menu;
    public LobbyMenuController Lobby;

    private void Start()
    {
        ChangeState(EGameState.Menu);
    }

    public static void ChangeState(EGameState state)
    {
        Instance.State = state;

        switch (Instance.State)
        {
            case EGameState.Menu:
                Instance.Menu.gameObject.SetActive(true);
                Instance.Lobby.gameObject.SetActive(false);
                break;
            case EGameState.Lobby:
                Instance.Menu.gameObject.SetActive(false);
                Instance.Lobby.gameObject.SetActive(true);
                break;
            case EGameState.InGame:
                Instance.Menu.gameObject.SetActive(false);
                Instance.Lobby.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }



}
