using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

public enum MiniGameType
{
    None,
    Rythm,
    Mash,
    Hold,
}

public class Minigame : MonoBehaviour
{
    public GameObject Jauge;

    public UnityEvent OnFinished;

    private float completion;
    private MiniGameType currentMinigame;
    private TextMeshProUGUI name;
    private Sequence nameTween;

    private float duration;
    private float completionPerInput;

    private float rythmTimer;

    private void Start()
    {
        name = GetComponentInChildren<TextMeshProUGUI>();
        enabled = false;
    }

    private void Update()
    {
        rythmTimer += Time.deltaTime;
    }

    public void ReceiveInput(bool Hold)
    {
        switch (currentMinigame)
        {
            case MiniGameType.Hold:
                if (Hold)
                {
                    completion += completionPerInput * Time.deltaTime;
                }
                else
                {
                    completion = 0;
                }
                break;

            case MiniGameType.Rythm:
                if (!Hold)
                {
                    float timer = rythmTimer % nameTween.Duration(false);

                    if (timer > 0.3f)
                    {
                        completion += completionPerInput;
                    }
                    else
                    {
                        completion = 0;
                    }
                }
                break;

            case MiniGameType.Mash:
                if (!Hold)
                {
                    completion += completionPerInput;
                }
                break;
        }

        Jauge.transform.localPosition = Vector3.right * -100f * (1 - completion);

        if (completion >= 1)
        {
            OnFinished.Invoke();
            enabled = false;
        }
    }

    public void SetMiniGame(UnityEvent onFinished, MiniGameType miniGame, float value)
    {
        OnFinished = onFinished;
        nameTween.Kill();
        nameTween = DOTween.Sequence();

        currentMinigame = miniGame;
        enabled = true;
        gameObject.SetActive(true);

        duration = value;
        completionPerInput = 1.0f / duration;
        completion = 0;

        switch (currentMinigame)
        {
            case MiniGameType.Hold:
                name.text = "HOLD";
                nameTween.Append(name.transform.DOPunchScale(Vector3.one * 0.4f, 5f, 0, 0)).SetLoops(-1);
                break;

            case MiniGameType.Rythm:
                name.text = "RYTHM";
                nameTween.Append(name.transform.DOPunchScale(Vector3.one * 0.3f, 0.2f));
                nameTween.PrependInterval(0.4f).SetLoops(-1);
                rythmTimer = 0;
                break;

            case MiniGameType.Mash:
                name.text = "MASH";
                nameTween.Append(name.transform.DOPunchScale(Vector3.one * 0.3f, 0.125f)).SetLoops(-1);
                break;

            case MiniGameType.None:
                OnFinished.Invoke();
                enabled = false;
                break;
        }

        nameTween.Play();
    }
    
    private void OnDisable()
    {
        currentMinigame = MiniGameType.None;
        gameObject.SetActive(false);
        nameTween.Kill();
    }
}
