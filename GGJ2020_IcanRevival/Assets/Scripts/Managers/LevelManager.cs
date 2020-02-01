using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<Transform> spawns = new List<Transform>();
    public bool shuffleSpawns = false;

    private void Awake()
    {
        if (shuffleSpawns)
        {
            ShuffleSpawns();
        }
    }

    private void ShuffleSpawns()
    {
        List<Transform> newList = new List<Transform>(spawns.Count);
        while (spawns.Count > 0)
        {
            int r = Random.Range(0, spawns.Count);
            newList.Add(spawns[r]);
            spawns.RemoveAt(r);
        }
        spawns = newList;
    }

    public void Init()
    {
    }
}
