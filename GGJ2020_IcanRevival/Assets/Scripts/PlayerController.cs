using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public enum inputName {
        Grab,
        Use
    };

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
    public Interactable hovered { get { return hoveredList.Count > 0 ? hoveredList[0] : null; } }
    public Interactable holded;

    public Dictionary<inputName, bool> inputStatus = new Dictionary<inputName, bool> {
        { inputName.Grab, false },
        { inputName.Use, false }
    };

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

    private void OnGrabButton(bool isDown)
    {
        if (!isDown)
        {
            return;
        }

        // Grab / Drop

        if (holded != null)
        {
            holded.Drop(this);

            holded.transform.parent = null;
        }

        if (hovered != null && hovered.isGrabbable)
        {
            hovered.Hover(false);
            hovered.Grab(this);
            holded = hovered;
            hoveredList.Remove(holded);

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
        Item itemHolded = holded as Item;
        if(hoveredMachine != null)
        {
            if (itemHolded != null)
            {
                if (hoveredMachine.ItemRequired == itemHolded.type)
                {
                    Debug.Log("Type equal = true");
                }
            }
            else if (hoveredMachine.ItemRequired == ItemType.None)
            {
                Debug.Log("Type equal = true");
            }
        }

        /*if (hovered != null && hovered.isGrabbable)
        {
            hovered.Hover(false);
            hovered.Grab(this);
            holded = hovered;
            hoveredList.Remove(holded);

            holded.transform.SetParent(interactionCollider.transform);
            holded.transform.localPosition = Vector3.zero;
        }*/
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
