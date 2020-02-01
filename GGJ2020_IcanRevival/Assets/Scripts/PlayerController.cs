using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private const string INPUT_AXIS_HORIZONTAL = "Horizontal";
    private const string INPUT_AXIS_VERTICAL = "Vertical";
    private const string INTERACTION_COLLIDER = "InteractionCollider";


    public int playerId;


    private bool _movementEnabled = true;
    public bool movementEnabled {
        get { return _movementEnabled; }
        set {
            velocity = Vector2.zero;
            animator.SetBool("Moving", false);
            _movementEnabled = value;
        }
    }

    public float speed = 5.0f;
    public Vector2 velocity = Vector2.zero;

    public Vector2 movingDirection = Vector2.zero;
    public Vector2 lookingDirection = Vector2.zero;

    private new Rigidbody2D rigidbody;
    private Animator animator;
    private float interactionRange;
    private Collider2D interactionCollider;
    private Text playerName;

    public InputDevice device;
    public string deviceMeta;

    public List<Interactable> hoveredList = new List<Interactable>();
    public Interactable hovered 
        { get { return hoveredList.Count > 0 ? hoveredList[0] : null; } }
    public Interactable holded;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        interactionCollider = transform.Find(INTERACTION_COLLIDER).GetComponent<Collider2D>();
        interactionRange = interactionCollider.transform.localPosition.magnitude;

        playerName = transform.Find("Canvas").GetComponentInChildren<Text>();
    }

    private void Update()
    {
        //InputDevice inputDevice = (InputManager.Devices.Count > playerNum) ? InputManager.Devices[playerNum] : null;

        if (device == null)
        {
            // No controller for this player number
        }
        else
        {
            ManageInput();
        }
    }

    private void ManageInput()
    {
        if (device.Action1)
        {

        }

        if (movementEnabled)
        {
            Vector2 movingInput = device.LeftStick;
            float len = movingInput.sqrMagnitude;
            if (len > 1)
            {
                movingInput.Normalize();
            }

            Vector2 lookingInput = device.RightStick;

            if (len < float.Epsilon)
            {
                animator.SetBool("Moving", false);
            }
            else
            {
                animator.SetBool("Moving", true);

                if (lookingInput.sqrMagnitude < float.Epsilon)
                {
                    lookingInput = movingInput;
                }
            }
            
            if (lookingInput.sqrMagnitude > float.Epsilon)
            {
                lookingDirection = lookingInput.normalized;
            }
            movingDirection = movingInput;

            animator.SetFloat("Horizontal", lookingDirection.x);
            animator.SetFloat("Vertical", lookingDirection.y);

            Move();
        }
    }

    private void Move()
    {
        rigidbody.velocity = movingDirection * speed;
        interactionCollider.transform.localPosition = lookingDirection * interactionRange;
    }

    public void SetName(string n, int id)
    {
        playerId = id;
        name = n;
        playerName.text = n;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable interactable = collision.GetComponent<Interactable>();
        if (interactable != null)
        {
            hoveredList.Add(interactable);

            if (hoveredList.Count == 1)
            {
                hovered.Hover(true);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Interactable interactable = collision.GetComponent<Interactable>();
        if (interactable != null)
        {
            if (interactable == hovered)
            {
                interactable.Hover(false);
            }
            hoveredList.Remove(interactable);

            hovered?.Hover(true);
        }
    }
}
