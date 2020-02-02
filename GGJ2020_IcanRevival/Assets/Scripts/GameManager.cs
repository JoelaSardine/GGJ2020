using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GamePhase { None, Game, Lobby }
public enum SoundEvent { Success, Grab, Mash, Throw, Drop } 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int maxPlayerCount = 4;
    public Color[] PlayerColors = new Color[] {
        Color.blue, Color.red, Color.green, Color.yellow
    };
    public Material[] PlayerMaterials = new Material[4] { null, null, null, null };

    public GamePhase gamePhase = GamePhase.None;
    public PlayersManager playersManager;
    public LobbyManager lobbyManager;
    public LevelManager levelManager;

    private Dictionary<SoundEvent, AudioSource> sounds = new Dictionary<SoundEvent, AudioSource>();
    
    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            if (playersManager)
                Destroy(playersManager.gameObject);

            if (lobbyManager)
            {
                GameManager.Instance.lobbyManager = lobbyManager;
                lobbyManager.Init();
                GameManager.Instance.playersManager.OnBackToLobby(lobbyManager);
            }

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

        if (lobbyManager)
        {
            lobbyManager.Init();
        }

        Transform soundContainer = transform.Find("Sounds");
        sounds[SoundEvent.Mash] = soundContainer.Find("Mash").GetComponent<AudioSource>();
        sounds[SoundEvent.Grab] = soundContainer.Find("Grab").GetComponent<AudioSource>();
        sounds[SoundEvent.Success] = soundContainer.Find("Success").GetComponent<AudioSource>();
        sounds[SoundEvent.Throw] = soundContainer.Find("Throw").GetComponent<AudioSource>();
        sounds[SoundEvent.Drop] = soundContainer.Find("Drop").GetComponent<AudioSource>();
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
                /*if (lobbyManager == null || playersManager == null)
                {
                    Debug.LogError("Please link LobbyManager and PlayersManager !");
                    return;
                }
                lobbyManager.Init();*/
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

    public void PlaySound(SoundEvent sound)
    {
        sounds[sound].Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("Lobby");
            GameManager.Instance.ChangePhase(GamePhase.Lobby);
        }
    }
}
