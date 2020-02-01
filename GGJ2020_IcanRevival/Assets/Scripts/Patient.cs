using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Patient : Interactable
{
    public float MoveSpeed;
    public float AvoidRadius;
    public int Health = 3;
    public float HealthDecreaseSpeed = 10;
    public Sickness sickness; 

    public Vector2? moveTarget;

    private bool healed;
    public Animator animator;
    private Rigidbody2D rb;
    public ContactFilter2D avoidMask;
    private float timer = 0;


    private void Start()
    {
        //animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        avoidMask = new ContactFilter2D();
        avoidMask.useLayerMask = true;
        avoidMask.layerMask = LayerMask.GetMask("Patient");

        animator.SetInteger("Life", Health);

        label.text = sickness.name.ToString();
    }

    public void Update()
    {
        animator.SetInteger("Life", Health);


        if (moveTarget.HasValue) MoveUpdate();
        else AvoidUpdate();

        if(!healed) HealthUpdate();
    }

    public override void InteractWithItem(Interactable itemHolded)
    {
        Item item = itemHolded as Item;

        if (itemHolded != null && enabled)
        {
            sickness.TryCure(item.type, itemHolded.holder);
        }
    }

    private void AvoidUpdate()
    {
        List<Collider2D> collidersToAvoid = new List<Collider2D>();
        
        if (Physics2D.OverlapCircle(rb.position, AvoidRadius, avoidMask, collidersToAvoid) > 0)
        {
            Vector2 direction = Vector2.zero;

            for(int i = 0; i < collidersToAvoid.Count; i++)
            {
                Vector2 colPos = collidersToAvoid[i].transform.position;
                direction += (rb.position - colPos).normalized;
            }

            rb.AddForce(direction.normalized * MoveSpeed, ForceMode2D.Impulse);
        }
    }

    private void MoveUpdate()
    {
        Vector2 direction = moveTarget.Value - rb.position;

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
        }

        if(Health == 0)
        {
            enabled = false;
            label.text = "Dead";
        }

    }
}
