using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    protected Text label;

    public int hovering = 0;
    
    public bool isGrabbable = false;
    public bool isHolded = false;
    public PlayerController holder;
    public float weight = 1.0f;

    public new Collider2D collider;
    public new Rigidbody2D rigidbody;

    private void Awake()
    {
        label = GetComponentInChildren<Text>();
        UpdateHoverStatus();

        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
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
       
    public void Grab(PlayerController player)
    {
        holder = player;
        isHolded = true;

        collider.enabled = false;
        rigidbody.simulated = false;
    }

    public void Drop(PlayerController player)
    {
        holder = null;
        isHolded = false;

        collider.enabled = true;
        rigidbody.simulated = true;
    }
}
