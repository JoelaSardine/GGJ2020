using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private const string INTERACTION_COLLIDER = "InteractionCollider";
    
    
    private bool _movementEnabled = true;
    public bool movementEnabled {
        get { return _movementEnabled && !miniGame.enabled; }
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

    public List<Renderer> playerRenderers = new List<Renderer>();
    public List<GameObject> playerHairs = new List<GameObject>();

    [Header("Debug")]
    public int playerId;
    public int colorId;
    public Vector2 movingDirection = Vector2.zero;
    public Vector2 lookingDirection = Vector2.zero;

    private new Rigidbody2D rigidbody;
    public Animator animator;
    private float interactionRange;
    private Collider2D interactionCollider;
    private TextMeshProUGUI playerName;
    [HideInInspector] public Minigame miniGame;

    public InputDevice device;
    public string deviceMeta;

    public List<Interactable> hoveredList = new List<Interactable>();
    public Interactable hovered { get { return hoveredList.Count > 0 ? hoveredList[0] : null; } }
    public Interactable holded;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
        miniGame = transform.Find("Canvas").GetComponentInChildren<Minigame>();

        interactionCollider = transform.Find(INTERACTION_COLLIDER).GetComponent<Collider2D>();
        interactionRange = interactionCollider.transform.localPosition.magnitude;
        //miniGame.enabled = false;

        playerName = transform.Find("Canvas").GetComponentInChildren<TextMeshProUGUI>();

        int rdm = Random.Range(0, playerHairs.Count);
        for (int i = 0; i < playerHairs.Count; i++)
        {
            playerHairs[i].SetActive(i == rdm);
        }
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

    private void FixedUpdate()
    {
        Move();
    }

    private void ManageInput()
    {
        if (device.Action3 && miniGame.enabled) miniGame.ReceiveInput(true);

        if (device.Action1.HasChanged) // A - LClick
        {
            OnGrabButton(device.Action1.WasPressed);
        }
        if (device.Action2.HasChanged) // B
        {
            OnGrabButton(device.Action2.WasPressed, true);
        }
        if (device.Action3.HasChanged) // X - RClick
        {
            OnUseButton(device.Action3.WasPressed);
        }
        if (device.Action4.HasChanged) // Y - MClick
        {
            OnThrowButton(device.Action4.WasPressed);
        }
        else if (device.RightBumper.HasChanged) // RB
        {
            OnThrowButton(device.RightBumper.WasPressed);
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
        }
    }

    private void Move()
    {
        rigidbody.AddForce(movingDirection * movementForce * 100);

        this.transform.LookAt(new Vector3(this.transform.position.x + lookingDirection.x, this.transform.position.y + lookingDirection.y, this.transform.position.z), Vector3.forward);

        //interactionCollider.transform.localPosition = lookingDirection * interactionRange;
    }

    private void OnGrabButton(bool isDown, bool dropOnly = false)
    {
        if (!isDown)
        {
            return;
        }

        if (miniGame.enabled)
        {
            miniGame.enabled = false;
            return;
        }

        // Grab / Drop

        if (holded != null)
        {
            holded.Drop();
            interactionCollider.isTrigger = true;
        }

        if (dropOnly) return;

        if (hovered != null && hovered.isGrabbable)
        {
            hovered.Hover(false);
            Grab(hovered);
            interactionCollider.isTrigger = false;
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

        if (miniGame.enabled)
        {
            miniGame.ReceiveInput(false);
            return;
        }

        // Grab / Drop

        Interactable hoveredInteractable = hovered;
        if(hoveredInteractable != null)
        {
            if (holded != null)
                hoveredInteractable.InteractWithItem(holded);
            else
                hoveredInteractable.Interact(this);
        }
    }

    private void OnThrowButton(bool isDown)
    {
        if (holded != null)
        {
            interactionCollider.isTrigger = true;
            StartCoroutine(holded.ThrowCoroutine(this));
            holded.Drop();
            
        }
    }

    public void SetName(string n, int id, int colorId)
    {
        playerId = id;
        name = n;
        playerName.text = n;
        this.colorId = colorId;
    }

    public void SetColor(int id)
    {
        colorId = id;
        foreach (var renderer in playerRenderers)
        {
            renderer.material = GameManager.Instance.PlayerMaterials[id];
        }
        playerName.color = GameManager.Instance.PlayerColors[id];
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
