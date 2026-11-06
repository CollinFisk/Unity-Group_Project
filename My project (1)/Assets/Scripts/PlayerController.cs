using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public int health = 5;
    public int maxHealth = 5;
    public int filledWeaponSlots = 0;
    public float speed = 5f;
    public bool attacking = false;

    [Header("References")]
    public Transform player;
    public MeleeWeapons currentMeleeWeapon;
    public RangedWeapons currentRangedWeapon;
    public GameObject[] playerWeapons;
    public Interactions interactions;
    public Transform weaponSlot;
    public Transform interactObject;
    public GameObject pickupObj;

    private Rigidbody2D rb;
    private PlayerInput inputComponent;
    private Vector2 moveInput = Vector2.zero;

    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction attackAction;
    private InputAction reloadAction;
    private InputAction dropAction;

    [Header("Resources")]
    public int wood;
    public int metal;
    public int components;
    public int totalResources;
    public int maxResources;
    void Awake()
    {
        player = transform;
        rb = GetComponent<Rigidbody2D>();
        inputComponent = GetComponent<PlayerInput>();
    }

    void Start()
    {
        if (weaponSlot == null) weaponSlot = player.GetChild(0);

        interactObject = GameObject.Find("PlayerInteractor").transform;
        interactions = interactObject.GetComponent<Interactions>();
    }

    private void OnEnable()
    {
        if (inputComponent != null && inputComponent.actions != null)
        {
            moveAction = inputComponent.actions.FindAction("Move");
            interactAction = inputComponent.actions.FindAction("Interact");
            attackAction = inputComponent.actions.FindAction("Attack");
            reloadAction = inputComponent.actions.FindAction("Reload");
            dropAction = inputComponent.actions.FindAction("Drop");

            if (moveAction != null) { moveAction.performed += Move; moveAction.canceled += Move; }
            if (interactAction != null) interactAction.performed += Interact;
            if (attackAction != null) { attackAction.performed += Attack; attackAction.canceled += Attack; }
            if (reloadAction != null) reloadAction.performed += Reload;
            if (dropAction != null) dropAction.performed += Drop;

        }
    }

    private void OnDisable()
    {
        if (moveAction != null) { moveAction.performed -= Move; moveAction.canceled -= Move; }
        if (interactAction != null) interactAction.performed -= Interact;
        if (attackAction != null) { attackAction.performed -= Attack; attackAction.canceled -= Attack; }
        if (reloadAction != null) reloadAction.performed -= Reload;
        if (dropAction != null) dropAction.performed -= Drop;

    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        Vector2 lookDir = mousePos - transform.position;
        rb.MoveRotation(Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f);

        interactObject.position = transform.position;
        interactObject.rotation = transform.rotation;

        if (weaponSlot.childCount > 0)
        {
            Transform activeWeapon = weaponSlot.GetChild(0);
            if (activeWeapon.CompareTag("MeleeWeapon"))
            {
                currentMeleeWeapon = activeWeapon.GetComponent<MeleeWeapons>();
                currentRangedWeapon = null;
            }
            else if (activeWeapon.CompareTag("RangedWeapon"))
            {
                currentRangedWeapon = activeWeapon.GetComponent<RangedWeapons>();
                currentMeleeWeapon = null;

                if (currentRangedWeapon != null && currentRangedWeapon.holdToAttack && attacking)
                {
                    currentRangedWeapon.fire();
                }
            }
        }
        else
        {
            currentMeleeWeapon = null;
            currentRangedWeapon = null;
        }
    }

    public void EquipNextWeapon()
    {

    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (pickupObj != null && interactions.canInteract)
        {
            if (pickupObj.CompareTag("MeleeWeapon"))
            {
                var script = pickupObj.GetComponent<MeleeWeapons>();
                if (script != null) script.equip(this);
            }
            else if (pickupObj.CompareTag("RangedWeapon"))
            {
                var script = pickupObj.GetComponent<RangedWeapons>();
                if (script != null) script.equip(this);
            }
            else if (pickupObj.CompareTag("Health"))
            {
                Destroy(pickupObj);
                health = maxHealth;
            }

            pickupObj = null;
            interactions.canInteract = false;
        }
    }
    public void Drop(InputAction.CallbackContext context)
    {
        if(currentMeleeWeapon)
        {
            currentMeleeWeapon.unequip();
        }
        if(currentRangedWeapon)
        {
            currentRangedWeapon.unequip();
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (currentRangedWeapon)
        {
            if (currentRangedWeapon.holdToAttack)
                attacking = context.ReadValueAsButton();
            else if (context.performed)
                currentRangedWeapon.fire();
        }
        else if (currentMeleeWeapon && context.performed)
        {
            currentMeleeWeapon.hit();
        }
    }

    public void Reload(InputAction.CallbackContext context)
    {
        if (currentRangedWeapon && !currentRangedWeapon.reloading)
        {
            currentRangedWeapon.reload();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyAttackZone"))
        {
            health--;
        }
        if (collision.gameObject.CompareTag("Health") && health < maxHealth)
        {
            health = maxHealth;
            Destroy(collision.gameObject);
        }
        if (currentRangedWeapon && collision.gameObject.CompareTag("Ammo") && currentRangedWeapon.ammo < currentRangedWeapon.maxAmmo)
        {
            health = maxHealth;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Resource"))
        {
            if(collision.gameObject.GetComponent<ResourceScript>().givesWood)
            {
                wood = +collision.gameObject.GetComponent<ResourceScript>().dropWorth;
                Destroy(collision.gameObject);

            }
            if (collision.gameObject.GetComponent<ResourceScript>().givesMetal)
            {
                metal += collision.gameObject.GetComponent<ResourceScript>().dropWorth;
                Destroy(collision.gameObject);

            }
            if (collision.gameObject.GetComponent<ResourceScript>().givesComponents)
            {
                components += collision.gameObject.GetComponent<ResourceScript>().dropWorth;
                Destroy(collision.gameObject);

            }
        }

    }
}