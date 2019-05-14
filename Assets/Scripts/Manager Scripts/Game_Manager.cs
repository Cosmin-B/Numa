using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{

    public delegate void GameManagereventHandler();
    public event GameManagereventHandler MenuToggleEvent;
    public event GameManagereventHandler InventoryUIToggleEvent;
    public event GameManagereventHandler RestartLevelEvent;
    public event GameManagereventHandler GoToMenuSceneEvent;
    public event GameManagereventHandler GameOverEvent;

    public bool isGameOver;
    public bool isInvetoryUIOn;
    public bool isMenuOn;

    private Game_SoundManager _soundManager = null;

    private static Game_Manager _instance = null;

    public static Game_Manager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(_instance);
            _instance = this;
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        Debug.Assert(_soundManager == null);

        _soundManager = new Game_SoundManager();
    }

    public void Start()
    {
        Player_SoundHolder.Init();
    }

    public Game_SoundManager GetSoundManager()
    {
        return _soundManager;
    }

    public void CallEventMenuToggle()
    {
        if(MenuToggleEvent !=null)
        {
            MenuToggleEvent();
        }
    }

    public void CallEventInventoryUIToggle()
    {
        if(InventoryUIToggleEvent !=null)
        {
            InventoryUIToggleEvent();
        }
    }

    public void CallEventRestartLevel()
    {
        if (RestartLevelEvent != null)
        {
            RestartLevelEvent();
        }
    }

    public void CallEventGoToMenuScene()
    {
        if (GoToMenuSceneEvent != null)
        {
            GoToMenuSceneEvent();
        }
    }

    public void CallEventGameOver()
    {
        if (GameOverEvent != null)
        {
            isGameOver = true;
            GameOverEvent();
        }
    }

}
