using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public float zoneOpacity = 0.1f;
    public List<PlayerZone> playersZones = new List<PlayerZone>();
        
    public void Init()
    {
        for (int i = 0; i < playersZones.Count; i++)
        {
            playersZones[i].SetColor(i);
            playersZones[i].Init();
        }
    }

    public void UpdateZoneColors()
    {
        foreach (var zone in playersZones)
        {
            zone.UpdateColor();
        }
    }
}
