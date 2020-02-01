using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using System;

public class PlayersManager : MonoBehaviour
{
    public enum State { Home, InGame, Rebind }
    public State state = State.Home;

    public GameObject playerPrefab;

    public List<PlayerController> players = new List<PlayerController>();
    private List<bool> takenPlayerIds = new List<bool>();

    public int deviceCount = 1;
    public bool isBindingInputs = false;

    private List<PlayerController> playersToRemove = new List<PlayerController>();

    private void Awake()
    {
    }

    private void Update()
    {
        switch (state)
        {
            case State.Home:
                UpdateHome();
                break;
            case State.InGame:
                UpdateInGame();
                break;
            case State.Rebind:
                UpdateRebind();
                break;
        }

        if (deviceCount != InputManager.Devices.Count)
        {
            deviceCount = InputManager.Devices.Count;
        }
    }

    private void UpdateHome()
    {
        foreach (var device in InputManager.Devices)
        {
            bool alreadyBound = false;

            foreach (var player in players)
            {
                if (device.Meta == player.deviceMeta)
                {
                    alreadyBound = true;

                    if (device.Action2)
                    {
                        playersToRemove.Add(player);
                    }
                    break;
                }
            }
            if (alreadyBound)
            {
                continue;
            }

            if (device.Action1)
            {
                CreateNewPlayer(device);
            }
        }

        foreach (var player in playersToRemove)
        {
            RemovePlayer(player);
        }
        playersToRemove.Clear();
    }

    private void UpdateInGame()
    { }

    private void UpdateRebind()
    { }

    private void CreateNewPlayer(InputDevice device)
    {
        GameObject go = Instantiate(playerPrefab, transform, true);
        PlayerController pc = go.GetComponent<PlayerController>();
        pc.device = device;
        pc.deviceMeta = device.Meta;
        int id = GetPlayerId();
        pc.SetName("player" + id, id);

        players.Add(pc);
    }

    private int GetPlayerId()
    {
        int i = 0;
        for (; i < takenPlayerIds.Count; i++)
        {
            if (!takenPlayerIds[i])
            {
                takenPlayerIds[i] = true;
                return i;
            }
        }
        takenPlayerIds.Add(true);
        return i;
    }

    private void RemovePlayer(PlayerController player)
    {
        takenPlayerIds[player.playerId] = false;
        Destroy(player.gameObject);
        players.Remove(player);
    }
}
