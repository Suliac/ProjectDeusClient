using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeusClientCore;
using System.Threading;

public class GameManager : MonoBehaviour
{
    private Game m_game;
    private GameView m_view;

    private bool stopped = true;
    // Use this for initialization
    void Start()
    {
        m_view = new UnityGameView();
        m_game = new Game();

        m_game.Start("127.0.0.1", 27015);
        m_view.Start();
        stopped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopped)
        {
            m_game.Update((decimal)Time.deltaTime);
            m_view.Update((decimal)Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.S))
            {
                m_game.Stop();
                m_view.Stop();
                stopped = true;
            } 
        }
    }
    
    

}
