using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeusClientCore;
using System.Threading;
using System;
using DeusClientCore.Events;
using DeusClientCore.Packets;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    public UnityGameView DeusGameView;

    public Text Adress;
    public Text Port;
    public Text Pseudo;

    public Camera MainCamera;

    public GameObject m_GameObjectContainer;
    public static GameObject GameObjectContainer { get { return Instance.m_GameObjectContainer; } }

    private Game m_game;
    private bool m_stopped = true;

    private uint m_gameId = 0;
    private Dictionary<uint, string> m_playerInfos = new Dictionary<uint, string>();

    public static uint GameId { get { return Instance.m_gameId; } set { Instance.m_gameId = value; } }
    public static Dictionary<uint, string> PlayerInfos { get { return Instance.m_playerInfos; } set { Instance.m_playerInfos = value; } }

    void Start()
    {
        UnitySystemConsoleRedirector.Redirect();
        m_game = new Game();
    }

    public void Connection()
    {
        int port = 0;
        if (!string.IsNullOrEmpty(Adress.text) && int.TryParse(Port.text, out port) && !string.IsNullOrEmpty(Pseudo.text))
        {
            m_game.Start(Adress.text, port, Pseudo.text);
            EventManager.Get().AddListener(EPacketType.Connected, OnConnected);
            m_stopped = false;
        }
    }

    public void SetCamera(bool enable)
    {
        MainCamera.gameObject.SetActive(enable);
    }

    private void OnConnected(object sender, SocketPacketEventArgs e)
    {
        Debug.Log("Connected !");
        MenuController.ChangeState(MenuController.EGameState.Menu);
    }

    void Update()
    {
        if (!m_stopped)
        {
            m_game.Update((decimal)Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.P))
                Stop();
        }
    }

    private void OnApplicationQuit()
    {
        Stop();
    }

    private void Stop()
    {
        if (!m_stopped)
        {
            EventManager.Get().RemoveListener(EPacketType.ConnectedUdpAnswer, OnConnected);

            m_game.Stop();
            DeusGameView.Stop();

            m_stopped = true;
        }
    }
}
