using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenUI : MonoBehaviour
{
    public Transform worldObjectToFollow;
    public Vector3 offset;

    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        Vector3 worldtoScreen = camera.WorldToScreenPoint(worldObjectToFollow.position);
        transform.position = worldtoScreen + offset;
    }
}
