using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Events;

public enum SicknessType
{
    None,
    CancerDuCul,
    Coronavirus,
    FlemmingiteAigue,
    CriseCardiaque,
    PriseDeSang,
}

[Serializable]
public class Sickness
{
    public SicknessType name;
    public ItemType cure;
    public MiniGameType gameType;
    public float gameArgument;

    public UnityEvent OnCure;

    public Sickness (Sickness sickness)
    {
        this.name = sickness.name;
        this.cure = sickness.cure;
        this.gameType = sickness.gameType;
        this.gameArgument = sickness.gameArgument;
        OnCure = new UnityEvent();
    }

    public void TryCure(ItemType itemUsed, PlayerController player)
    {
        if(cure == itemUsed) player.miniGame.SetMiniGame(OnCure, gameType, gameArgument);
    }
}

public class SicknessManager : MonoBehaviour
{
    public Sickness[] sicknesses;

    public Sickness GetRandomsickness()
    {
        return new Sickness(sicknesses[Random.Range(0, sicknesses.Length)]);
    }
}
