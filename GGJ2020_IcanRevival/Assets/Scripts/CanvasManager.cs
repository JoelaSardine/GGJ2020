using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    private Camera camera;
    private Canvas canvas;

    void Start()
    {
        camera = Camera.main;
        canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        if (canvas.worldCamera == null) canvas.worldCamera = camera;
    }
}
