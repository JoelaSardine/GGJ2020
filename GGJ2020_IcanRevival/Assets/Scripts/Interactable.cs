using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interactable : MonoBehaviour
{
    protected TextMeshProUGUI label;

    public int hovering = 0;
    
    public bool isGrabbable = false;
    public bool isHolded = false;
    public PlayerController holder;
    public float weight = 1.0f;

    public new Collider2D collider;
    public new Rigidbody2D rigidbody;

    private void Awake()
    {
        label = GetComponentInChildren<TextMeshProUGUI>();
        label.text = gameObject.name;
        UpdateHoverStatus();

        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
    }

    public virtual void InteractWithItem(Interactable itemHolded)
    {

    }

    public virtual void Interact(PlayerController player)
    {

    }

    public virtual void Used()
    {

    }

    public void ChangeName(string newName)
    {
        gameObject.name = newName;
        label.text = newName;
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
        GameManager.Instance.PlaySound(SoundEvent.Grab);

        holder = player;
        isHolded = true;

        collider.isTrigger = true;
        rigidbody.isKinematic = true;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Drop(bool isThrowed = false)
    {
        GameManager.Instance.PlaySound(isThrowed ? SoundEvent.Throw : SoundEvent.Drop);
        
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
