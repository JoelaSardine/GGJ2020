using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private bool waitForLoad = true;

    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            if (playersManager)
                Destroy(playersManager.gameObject);

            if (lobbyManager)
                GameManager.Instance.lobbyManager = lobbyManager;

            if (levelManager)
            {
                GameManager.Instance.levelManager = levelManager;
                levelManager.Init();
                GameManager.Instance.playersManager.OnNewLevel(levelManager);
            }
            
            Destroy(this.gameObject);
        }
        else
        {
            Init();
        }
    }

    private void Init()
    {
        GameManager.Instance = this;

        GetComponentInChildren<InControl.InControlManager>().enabled = true;

        ChangePhase(gamePhase);

        if (!playersManager.isInitialized)
        {
            playersManager.Init();
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(playersManager.gameObject);
        }
    }

    public void ChangePhase(GamePhase newPhase)
    {
        gamePhase = newPhase;
        switch (newPhase)
        {
            case GamePhase.Game:
                /* if (levelManager == null || playersManager == null)
                 {
                     Debug.LogError("Please link LevelManager and PlayersManager !");
                     return;
                 }
                 levelManager.Init();*/
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

    public void LaunchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        GameManager.Instance.ChangePhase(GamePhase.Game);
    }

    private void Update()
    {
    }
}
