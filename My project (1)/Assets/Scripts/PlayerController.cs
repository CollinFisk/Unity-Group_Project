using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public int health = 5;
    public int maxHealth = 5;
    public int filledWeaponSlots = 0;
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool attacking = false;

    [Header("References")]
    public MeleeWeapons currentMeleeWeapon;
    public RangedWeapons currentRangedWeapon;
    public GameObject[] playerWeapons;
    public Interactions interactions;
    public Transform weaponSlot;
    public Transform interactObject;
    public GameObject pickupObj;
    public GameObject gameManager;

    private Rigidbody2D rb;
    private PlayerInput inputComponent;
    private Vector2 moveInput = Vector2.zero;

    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction attackAction;
    private InputAction reloadAction;
    private InputAction dropAction;
    private InputAction equipNextWeaponAction;

    [Header("Resources")]
    public int wood;
    public int metal;
    public int components;
    public int totalResources;
    public int maxResources;

    void Start()
    {
        if (weaponSlot == null) weaponSlot = transform.GetChild(0);

        gameManager = GameObject.Find("gameManager");
        interactObject = GameObject.Find("PlayerInteractor").transform;
        interactions = interactObject.GetComponent<Interactions>();
        rb = GetComponent<Rigidbody2D>();
        inputComponent = GetComponent<PlayerInput>();

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
            equipNextWeaponAction = inputComponent.actions.FindAction("equipNextWeapon");

            if (moveAction != null) { moveAction.performed += Move; moveAction.canceled += Move; }
            if (interactAction != null) interactAction.performed += Interact;
            if (attackAction != null) { attackAction.performed += Attack; attackAction.canceled += Attack; }
            if (reloadAction != null) reloadAction.performed += Reload;
            if (dropAction != null) dropAction.performed += Drop;
            if (equipNextWeaponAction != null) equipNextWeaponAction.performed += EquipNextWeapon;

        }
    }

    private void OnDisable()
    {
        if (moveAction != null) { moveAction.performed -= Move; moveAction.canceled -= Move; }
        if (interactAction != null) interactAction.performed -= Interact;
        if (attackAction != null) { attackAction.performed -= Attack; attackAction.canceled -= Attack; }
        if (reloadAction != null) reloadAction.performed -= Reload;
        if (dropAction != null) dropAction.performed -= Drop;
        if (equipNextWeaponAction != null) equipNextWeaponAction.performed -= EquipNextWeapon;


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
        totalResources = wood + metal + components; //When adding new resources add them to this
        interactObject.SetPositionAndRotation(transform.position, transform.rotation);
        if (playerWeapons[0] != null)
        {
            Transform activeWeapon = playerWeapons[0].transform;
            activeWeapon.gameObject.SetActive(true);
            //Debug.Log(activeWeapon.gameObject);
            if (activeWeapon.CompareTag("MeleeWeapon"))
            {
                //currentMeleeWeapon.gameObject.SetActive(true);
                currentMeleeWeapon = activeWeapon.GetComponent<MeleeWeapons>();
                currentRangedWeapon = null;
            }
            else if (activeWeapon.CompareTag("RangedWeapon"))
            {
                //currentRangedWeapon.gameObject.SetActive(true);
                currentRangedWeapon = activeWeapon.GetComponent<RangedWeapons>();
                currentMeleeWeapon = null;

                if (currentRangedWeapon != null && currentRangedWeapon.holdToAttack && attacking)
                {
                    currentRangedWeapon.Fire();
                }
            }
        }
        else
        {
            currentMeleeWeapon = null;
            currentRangedWeapon = null;
        }
    }

    private void EquipNextWeapon(InputAction.CallbackContext context)
    {
        if (context.canceled && playerWeapons[0])
        {
            GameObject temp = playerWeapons[0];
            playerWeapons[0] = null;
            for (var i = 1; i < playerWeapons.Length; i++)
            {
                if (playerWeapons[i] != null)
                {
                    playerWeapons[i - 1] = playerWeapons[i];
                    playerWeapons[i] = null;
                }
            }
            playerWeapons[filledWeaponSlots-1] = temp;
        }
    }

    private void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (pickupObj != null && interactions.canInteract)
        {
            if (pickupObj.CompareTag("MeleeWeapon") && playerWeapons.Last() == null)
            {
                if (pickupObj.TryGetComponent<MeleeWeapons>(out var script)) script.Equip(this);
            }
            else if (pickupObj.CompareTag("RangedWeapon"))
            {
                if (pickupObj.TryGetComponent<RangedWeapons>(out var script)) script.Equip(this);
            }
            else if (pickupObj.CompareTag("Health"))
            {
                Destroy(pickupObj);
                health = maxHealth;
            }
            else if (pickupObj.CompareTag("Shop"))
            {
                if (pickupObj.TryGetComponent<ShopTemplate>(out var script))
                {
                    gameManager.GetComponent<GameManager>().OpenMenu(script.menuIndex);
                }
            }

                pickupObj = null;
            interactions.canInteract = false;
        }
    }
    private void Drop(InputAction.CallbackContext context)
    {
        if(currentMeleeWeapon && context.performed)
        {
            currentMeleeWeapon.Unequip();
        }
        if(currentRangedWeapon && context.performed)
        {
            currentRangedWeapon.Unequip();
        }
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (currentRangedWeapon)
        {
            if (currentRangedWeapon.holdToAttack)
                attacking = context.ReadValueAsButton();
            else if (context.performed)
                currentRangedWeapon.Fire();
        }
        else if (currentMeleeWeapon && context.performed && currentMeleeWeapon.canAttack)
        {
            currentMeleeWeapon.StartAttack();
        }
    }

    private void Reload(InputAction.CallbackContext context)
    {
        if (currentRangedWeapon && !currentRangedWeapon.reloading)
        {
            currentRangedWeapon.Reload();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyAttackZone"))
        {
            health--;
        }
        if (collision.gameObject.CompareTag("Projectile"))
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
        if (collision.gameObject.CompareTag("Resource") && totalResources < maxResources)
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