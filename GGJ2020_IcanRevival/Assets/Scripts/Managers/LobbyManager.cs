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
            Color c = GameManager.Instance.PlayerColors[i];
            c.a = zoneOpacity;
            playersZones[i].SetColor(c);
            playersZones[i].Init();
        }
    }
}
