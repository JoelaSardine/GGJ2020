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
        //GetComponentInParent<Canvas>().gameObject.AddComponent<CanvasManager>();    
    }

    private void Update()
    {
        Vector3 worldtoScreen = Camera.main.WorldToScreenPoint(worldObjectToFollow.position);
        transform.position = worldtoScreen + offset;
    }
}
