using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Patient : MonoBehaviour
{
    public float MoveSpeed;
    public float AvoidRadius;
    public int Health = 3;
    public float HealthDecreaseSpeed = 10;
    public string Sickness;

    public Vector2? moveTarget;

    private Animator animator;
    private Text SicknessText;
    private Rigidbody2D rb;
    public ContactFilter2D avoidMask;
    private float timer = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SicknessText = GetComponentInChildren<Text>();
        rb = GetComponent<Rigidbody2D>();
        avoidMask = new ContactFilter2D();
        avoidMask.useLayerMask = true;
        avoidMask.layerMask = LayerMask.GetMask("Patient");

        animator.SetInteger("Life", Health);
        SicknessText.text = Sickness;
        SicknessText.enabled = false;
    }

    public void Update()
    {
        if(moveTarget.HasValue) MoveUpdate();
        else AvoidUpdate();

        HealthUpdate();
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
            ChangeHeatlth(Health - 1);
            timer = 0;
        }
    }

    public void ChangeHeatlth(int newHealth)
    {
        Health = newHealth;
        animator.SetInteger("Life", Health);

        if(Health == 0)
        {
            enabled = false;
            SicknessText.text = "Dead";
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SicknessText.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
         SicknessText.enabled = false;
    }
}
