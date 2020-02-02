using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerZone : MonoBehaviour
{
    public Transform spawn;
    public SpriteRenderer ground;
    public TextMeshProUGUI nameField;
    public TextMeshProUGUI deviceField;
    public PlayerController controller;

    [HideInInspector]
    public Vector3 spawnPos;

    public void Init()
    {
        spawnPos = spawn.position;
        gameObject.SetActive(false);
    }

    public void SetColor(int colorId)
    {
        Color c = GameManager.Instance.PlayerColors[colorId];
        c.a = 0.2f;
        ground.color = c;
    }

    public void UpdateColor()
    {
        if (controller)
        {
            SetColor(controller.colorId);
        }
    }

    public void Enable(PlayerController player)
    {
        if (player == null)
        {
            gameObject.SetActive(false);
            controller = null;
        }
        else
        {
            gameObject.SetActive(true);
            nameField.text = player.name;
            deviceField.text = player.device.Name;
            controller = player;
        }
    }
}
