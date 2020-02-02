using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionZone : MonoBehaviour
{
    public delegate void Method(int arg);
    public enum Action { None, Quit, LaunchLevel }

    public Dictionary<Action, Method> dico = new Dictionary<Action, Method>();

    public Action action = Action.None;
    public int arg = 0;
    public Image fillImage;
    public float fillTime = 2.0f;
    public float unfillTime = 0.5f;

    public int playersIn = 0;
    public float FillAmount {
        get { return fillImage.fillAmount; }
        set { fillImage.fillAmount = value; }
    }

    private void Awake()
    {
        FillAmount = 0;

        dico[Action.None] = Nothing;
        dico[Action.Quit] = Quit;
        dico[Action.LaunchLevel] = LaunchLevel;
    }

    private void Update()
    {
        if (FillAmount >= 1)
        {
            GameManager.Instance.PlaySound(SoundEvent.Success);
            dico[action](arg);
            return;
        }

        if (playersIn > 0 && playersIn == GameManager.Instance.playersManager.players.Count)
        {
            FillAmount += Time.deltaTime / fillTime;
        }
        else if (FillAmount > 0)
        {
            FillAmount -= Time.deltaTime / unfillTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            playersIn++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController pc = other.GetComponent<PlayerController>();
        if (pc != null)
        {
            playersIn--;
        }
    }

    public void Nothing(int arg) { }

    public void Quit(int arg)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LaunchLevel(int arg)
    {
        GameManager.Instance.LaunchScene("Level " + arg);
    }
}
