using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeusClientCore;
using System.Threading;

public class GameManager : MonoBehaviour
{
    private Game m_game;

    public UnityGameView DeusGameView;

    private bool stopped = true;

    void Start()
    {
        m_game = new Game();

        m_game.Start("127.0.0.1", 27015);
        stopped = false;
    }

    void Update()
    {
        if (!stopped)
        {
            m_game.Update((decimal)Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.P))
            {
                m_game.Stop();
                DeusGameView.Stop();
                stopped = true;
            } 
        }
    }

    private void OnApplicationQuit()
    {
        if (!stopped)
        {
            m_game.Stop();
            DeusGameView.Stop();
            stopped = true;
        }
    }
}
