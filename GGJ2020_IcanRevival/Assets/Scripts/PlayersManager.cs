﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using System;

public class PlayersManager : MonoBehaviour
{
    public GameObject playerPrefab;

    public List<PlayerController> players = new List<PlayerController>();
    private List<bool> takenPlayerIds = new List<bool>();

    public int deviceCount = 1;
    public bool isInitialized = false;

    private List<PlayerController> playersToRemove = new List<PlayerController>();

    public void Init()
    {
        isInitialized = true;

        switch (GameManager.Instance.gamePhase)
        {
            case GamePhase.Lobby:
                break;
            case GamePhase.Game:
                LevelManager levelManager = GameManager.Instance.levelManager;
                int maxPlayers = GameManager.Instance.PlayerColors.Length;

                deviceCount = InputManager.Devices.Count;
                for (int i = 0; i < deviceCount && i < maxPlayers;  ++i)
                {
                    int id = i;
                    Vector3 spawn = levelManager.spawns[id].position;
                    GameObject go = Instantiate(playerPrefab, spawn, Quaternion.identity, transform);
                    PlayerController pc = go.GetComponent<PlayerController>();
                    pc.device = InputManager.Devices[id];
                    pc.deviceMeta = pc.device.Meta;
                    pc.SetName("player" + id, id);
                    
                    players.Add(pc);
                }
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        switch (GameManager.Instance.gamePhase)
        {
            case GamePhase.Lobby:
                UpdateLobby();
                break;
            default:
                break;
        }

        if (deviceCount != InputManager.Devices.Count)
        {
            deviceCount = InputManager.Devices.Count;
        }
    }

    /// <summary>
    /// Link devices to players
    /// </summary>
    private void UpdateLobby()
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
        int id = GetPlayerId();

        PlayerZone lobbyZone = GameManager.Instance.lobbyManager.playersZones[id];
        GameObject go = Instantiate(playerPrefab, lobbyZone.spawnPos, Quaternion.identity, transform);
        PlayerController pc = go.GetComponent<PlayerController>();
        pc.device = device;
        pc.deviceMeta = device.Meta;
        pc.SetName("player" + id, id);
        lobbyZone.Enable(pc);

        players.Add(pc);
    }

    private void RemovePlayer(PlayerController player)
    {
        PlayerZone lobbyZone = GameManager.Instance.lobbyManager.playersZones[player.playerId];
        lobbyZone.Enable(null);

        takenPlayerIds[player.playerId] = false;
        Destroy(player.gameObject);
        players.Remove(player);
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
}
