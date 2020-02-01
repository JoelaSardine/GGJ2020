using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    protected Text label;

    public int hovering = 0;
    
    private void Awake()
    {
        label = GetComponentInChildren<Text>();
        UpdateHoverStatus();
    }

    private void Update()
    {

    }

    public virtual void Hover(bool state)
    {
        hovering += state ? 1 : -1;
        UpdateHoverStatus();
    }

    public void UpdateHoverStatus()
    {
        label.enabled = hovering > 0;
    }
}
