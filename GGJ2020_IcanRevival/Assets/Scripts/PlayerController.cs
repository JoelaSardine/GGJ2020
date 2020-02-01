using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private const string INTERACTION_COLLIDER = "InteractionCollider";

    public enum inputName {
        Grab,
        Use,
        Throw
    };

    public Dictionary<inputName, bool> inputStatus = new Dictionary<inputName, bool> {
        { inputName.Grab, false },
        { inputName.Use, false },
        { inputName.Throw, false }
    };
    
    private bool _movementEnabled = true;
    public bool movementEnabled {
        get { return _movementEnabled; }
        set {
            rigidbody.velocity = Vector2.zero;
            animator.SetBool("Moving", false);
            _movementEnabled = value;
        }
    }

    public float deadZoneSquared = 0.1f;
    public float movementForce = 2.5f;
    public float throwForce = 2.5f;
    public float throwTime = 0.1f;

    [Header("Debug")]
    public int playerId;
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
    public Interactable hovered { get { return hoveredList.Count > 0 ? hoveredList[0] : null; } }
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
        if (device.Action1 != inputStatus[inputName.Grab])
        {
            inputStatus[inputName.Grab] = device.Action1;
            OnGrabButton(inputStatus[inputName.Grab]);
        }
        if (device.Action3 != inputStatus[inputName.Use])
        {
            inputStatus[inputName.Use] = device.Action3;
            OnUseButton(inputStatus[inputName.Use]);
        }
        if (device.RightBumper != inputStatus[inputName.Throw])
        {
            inputStatus[inputName.Throw] = device.RightBumper;
            OnThrowButton(inputStatus[inputName.Throw]);
        }

        if (movementEnabled)
        {
            Vector2 movingInput = device.LeftStick;
            float len = movingInput.sqrMagnitude;
            if (len > 1)
            {
                movingInput.Normalize();
            }
            else if (len < deadZoneSquared)
            {
                movingInput = Vector2.zero;
            }

            Vector2 lookingInput = device.RightStick;

            if (len < deadZoneSquared)
            {
                animator.SetBool("Moving", false);
            }
            else
            {
                animator.SetBool("Moving", true);

                if (lookingInput.sqrMagnitude < deadZoneSquared)
                {
                    lookingInput = movingInput;
                }
            }
            
            if (lookingInput.sqrMagnitude > deadZoneSquared)
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
        rigidbody.AddForce(movingDirection * movementForce * 100);
        interactionCollider.transform.localPosition = lookingDirection * interactionRange;
    }

    private void OnGrabButton(bool isDown)
    {
        if (!isDown)
        {
            return;
        }

        // Grab / Drop

        if (holded != null)
        {
            holded.Drop();
        }

        if (hovered != null && hovered.isGrabbable)
        {
            hovered.Hover(false);
            Grab(hovered);
            hoveredList.Remove(holded);

        }
    }

    public void Grab(Interactable item)
    {
        if (item != null && item.isGrabbable)
        {
            item.Grab(this);
            holded = item;

            holded.transform.SetParent(interactionCollider.transform);
            holded.transform.localPosition = Vector3.zero;
        }
    }

    private void OnUseButton(bool isDown)
    {
        if (!isDown)
        {
            return;
        }

        // Grab / Drop

        Machine hoveredMachine = hovered as Machine;
        if(hoveredMachine != null)
        {
            if (holded != null)
                hoveredMachine.InteractWithItem(holded);
            else
                hoveredMachine.Interact(this);
        }
    }

    private void OnThrowButton(bool isDown)
    {
        if (holded != null)
        {
            StartCoroutine(holded.ThrowCoroutine(this));
            holded.Drop();
        }
    }

    public void SetName(string n, int id)
    {
        playerId = id;
        name = n;
        playerName.text = n;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable target = collision.GetComponent<Interactable>();
        if (target != null && target.holder != this)
        {
            hoveredList.Add(target);

            if (hoveredList.Count == 1)
            {
                target.Hover(true);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Interactable target = collision.GetComponent<Interactable>();
        if (target != null && target.holder != this)
        {
            if (target == hovered)
            {
                target.Hover(false);
                hoveredList.Remove(target);
                hovered?.Hover(true);
            }
            else
            {
                hoveredList.Remove(target);
            }
        }
    }
}
