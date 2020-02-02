using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.Events;

public enum SicknessType
{
    None,
    AnalCancer,
    Coronavirus,
    Fever,
    HeartAttack,
    Transfusion,
    Influenza,
}

[Serializable]
public class Sickness
{
    public float weight = 1;
    public bool Urgente;
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
        this.Urgente = sickness.Urgente;
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

    private float totalWeight;

    private void Start()
    {
        for(int i = 0; i < sicknesses.Length; i++)
        {
            totalWeight += sicknesses[i].weight;
        }
    }

    public Sickness GetRandomsickness()
    {
        Sickness sicknessToReturn = sicknesses[0];
        float value = totalWeight * Random.value;

        for (int i = 0; i < sicknesses.Length; i++)
        {
            sicknessToReturn = sicknesses[i];
            value -= sicknessToReturn.weight;
            if (value <= 0) break;
        }

        return new Sickness(sicknessToReturn);
    }
}
