using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }
    public bool singletons = false;
    public LevelController LevelController { get; set; }

    public enum GameState { MENU = 0, STARTGAME, INGAME }
    [SerializeField] private GameState gameState;
    public GameState PreviousGameState { get; set; }


    public void init()
    {
        Debug.Log("GameManager");
        singletons = true;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public GameState CurrentGameState
    {
        get { return gameState; }
        set
        {
            PreviousGameState = gameState;
            gameState = value;
        }
    }
}
