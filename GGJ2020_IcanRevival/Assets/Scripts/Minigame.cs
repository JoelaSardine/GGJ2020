using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

public enum CureMiniGame
{
    None,
    Rythm,
    Mash,
    Hold,
}

public class Minigame : MonoBehaviour
{
    public GameObject Jauge;
    public Sickness sickness;

    private float completion;
    private CureMiniGame currentMinigame;
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
            case CureMiniGame.Hold:
                if (Hold)
                {
                    completion += completionPerInput * Time.deltaTime;
                }
                else
                {
                    completion = 0;
                }
                break;

            case CureMiniGame.Rythm:
                if (!Hold)
                {
                    float timer = rythmTimer % nameTween.Duration(false);

                    if (timer > 0.4f)
                    {
                        completion += completionPerInput;
                    }
                    else
                    {
                        completion = 0;
                    }
                }
                break;

            case CureMiniGame.Mash:
                if (!Hold)
                {
                    completion += completionPerInput;
                }
                break;
        }

        Jauge.transform.localPosition = Vector3.right * -2.0f * (1 - completion);

        if (completion >= 1)
        {
            sickness.OnCure.Invoke();
            enabled = false;
        }
    }

    public void SetMiniGame(Sickness sickness, CureMiniGame miniGame, float value)
    {
        this.sickness = sickness;
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
            case CureMiniGame.Hold:
                name.text = "HOLD";
                nameTween.Append(name.transform.DOPunchScale(Vector3.one * 0.4f, 5f, 0, 0)).SetLoops(-1);
                break;

            case CureMiniGame.Rythm:
                name.text = "RYTHM";
                nameTween.Append(name.transform.DOPunchScale(Vector3.one * 0.3f, 0.2f));
                nameTween.PrependInterval(0.4f).SetLoops(-1);
                rythmTimer = 0;
                break;

            case CureMiniGame.Mash:
                name.text = "MASH";
                nameTween.Append(name.transform.DOPunchScale(Vector3.one * 0.3f, 0.125f)).SetLoops(-1);
                break;
        }

        nameTween.Play();
    }
    
    private void OnDisable()
    {
        currentMinigame = CureMiniGame.None;
        gameObject.SetActive(false);
        nameTween.Kill();
    }
}
