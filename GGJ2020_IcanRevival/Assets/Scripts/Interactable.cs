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

        collider.isTrigger = true;
        rigidbody.isKinematic = true;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Drop()
    {
        Batman(holder.holded.transform);
        holder.holded = null;
        holder = null;
        isHolded = false;

        collider.isTrigger = false;
        rigidbody.isKinematic = false;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    /// <summary>Helper function that kill all parents of the target</summary>
    private void Batman(Transform target)
    {
        target.parent = null;
    }

    public IEnumerator ThrowCoroutine(PlayerController player)
    {
        Vector2 dir = player.lookingDirection * player.throwForce * 100;
        float t = player.throwTime;
        while (t > 0)
        {
            t -= Time.deltaTime;
            rigidbody.AddForce(dir);
            yield return new WaitForFixedUpdate();
        }

    }
}
