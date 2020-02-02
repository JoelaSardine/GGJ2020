using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Patient : Interactable
{
    public float MoveSpeed;
    public float AvoidRadius;
    public int Health = 3;
    public float HealthDecreaseSpeed = 10;
    public GameObject EmergencyObj;
    public Sickness sickness;

    private Item item;

    public Vector2? moveTarget;

    [HideInInspector] public bool healed;
    public Animator animator;
    private Rigidbody2D rb;
    private ContactFilter2D avoidMask;
    private LayerMask LookAtMask;
    private float timer = 0;
    private Vector2 direction;


    private void Start()
    {
        EmergencyObj.transform.DOShakeRotation(5, 20).SetLoops(-1);
        rb = GetComponent<Rigidbody2D>();
        avoidMask = new ContactFilter2D();
        avoidMask.useLayerMask = true;
        avoidMask.layerMask = LayerMask.GetMask("Patient");
        LookAtMask = LayerMask.GetMask("Player");

        animator.SetInteger("Life", Health);

        label.text = sickness.name.ToString();
    }

    public void Update()
    {
        if (holder != null) moveTarget = null;

        if (moveTarget.HasValue) MoveUpdate();
        else AvoidUpdate();

        if(!healed) HealthUpdate();
    }

    private void FixedUpdate()
    {
        bool moving = direction.magnitude > 0.1f;
        if (moving)
        {
            animator.transform.parent.rotation = Quaternion.LookRotation(direction, animator.transform.parent.up);
        }

        animator.SetBool("Moving", moving);
        rb.AddForce(direction.normalized * MoveSpeed, ForceMode2D.Impulse);
    }

    public override void InteractWithItem(Interactable itemHolded)
    {
        item = itemHolded as Item;

        if (itemHolded != null && enabled && !healed)
        {
            sickness.TryCure(item.type, itemHolded.holder);
        }
    }

    private void AvoidUpdate()
    {
        List<Collider2D> collidersToAvoid = new List<Collider2D>();
        
        if (Physics2D.OverlapCircle(rb.position, AvoidRadius, avoidMask, collidersToAvoid) > 0)
        {
            direction = Vector2.zero;

            for(int i = 0; i < collidersToAvoid.Count; i++)
            {
                Vector2 colPos = collidersToAvoid[i].transform.position;
                direction += (rb.position - colPos).normalized;
            }
        }
    }

    private void MoveUpdate()
    {
        if (isHolded)
        {
            moveTarget = null;
            direction = Vector2.zero;
            return;
        }

        direction = moveTarget.Value - rb.position;

        rb.AddForce(direction.normalized * MoveSpeed, ForceMode2D.Impulse);

        if(direction.magnitude < 0.1f) 
        {
            moveTarget = null;
        }
    }

    private void HealthUpdate()
    {
        timer += Time.deltaTime;

        if(timer >= HealthDecreaseSpeed)
        {
            ChangeHealth(Health - 1);
            timer = 0;
        }
    }

    public void ChangeHealth(int newHealth)
    {
        if (Health == 0) return;

        Health = newHealth;
        animator.SetInteger("Life", Health);

        if(Health >= 3)
        {
            healed = true;
            label.text = "Healed";
            animator.SetBool("Healed", true);
            if (sickness.Urgente)
            {
                EmergencyObj.SetActive(false);
            }
            item?.Used();
            GameManager.Instance.levelManager.HealPatient();
        }

        if(Health == 0)
        {
            enabled = false;
            label.text = "Dead";
            if (sickness.Urgente)
            {
                EmergencyObj.SetActive(false);
            }
            GameManager.Instance.levelManager.KillPatient();
        }

    }
}
