using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase { None, Game, Lobby }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Color[] PlayerColors = new Color[] {
        Color.blue, Color.red, Color.green, Color.yellow
    };
    public GamePhase gamePhase = GamePhase.None;
    public PlayersManager playersManager;
    public LobbyManager lobbyManager;
    public LevelManager levelManager;

    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            if (lobbyManager)
                GameManager.Instance.lobbyManager = lobbyManager;
            if (levelManager)
                GameManager.Instance.levelManager = levelManager;
            
            Destroy(this);
        }
        else
        {
            Init();
        }
    }

    private void Init()
    {
        GameManager.Instance = this;
        
        ChangePhase(gamePhase);
    }

    private void ChangePhase(GamePhase newPhase)
    {
        switch (newPhase)
        {
            case GamePhase.Game:
                break;

            case GamePhase.Lobby:
                if (lobbyManager == null || playersManager == null)
                {
                    Debug.LogError("Please link LobbyManager and PlayersManager !");
                    return;
                }
                lobbyManager.Init();
                break;

            default:
                break;
        }
    }

    private void Update()
    {
    }
}
