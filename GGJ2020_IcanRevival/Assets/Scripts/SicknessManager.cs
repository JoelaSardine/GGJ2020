using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public enum SicknessType
{
    None,
    CancerDuCul,
    Coronavirus,
    FlemmingiteAigue,
}

[Serializable]
public class Sickness
{
    public SicknessType name;
    public ItemType cure;

    public bool TryCure(ItemType itemUsed)
    {
        return cure == itemUsed;
    }
}

public class SicknessManager : MonoBehaviour
{
    public Sickness[] sicknesses;

    public Sickness GetRandomsickness()
    {
        return sicknesses[Random.Range(0, sicknesses.Length)];
    }
}
