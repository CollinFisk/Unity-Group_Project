using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MeleeWeapons : MonoBehaviour
{
    private PlayerController player;
    private GameObject attackZone;

    public bool canAttack = false;
    public bool equipped = false;
    public string weaponName;
    public int maxDmg;
    public int minDmg;
    public float knockbackForce;

    [SerializeField] private float SwingSpeed;
    [SerializeField] private float SwingCooldown;
    [SerializeField] private float attackLingeringTime;

    void Start()
    {
        attackZone = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (player)
        {
            if (player.playerWeapons[0] != gameObject)
            {
                equipped = false;
            }
            else
                equipped = true;

            gameObject.GetComponent<SpriteRenderer>().enabled = equipped;
        }

    }

    public void Equip(PlayerController p)
    {
        player = p;
        player.currentMeleeWeapon = this;
        transform.gameObject.SetActive(true);
        if (player.GetComponent<PlayerController>().playerWeapons[0] != null)
        {
            for (var i = player.GetComponent<PlayerController>().playerWeapons.Length; i > 0; i--)
            {
                if (player.GetComponent<PlayerController>().playerWeapons[i-1] != null && i < player.GetComponent<PlayerController>().playerWeapons.Length)
                {
                    player.GetComponent<PlayerController>().playerWeapons[i] = player.GetComponent<PlayerController>().playerWeapons[i - 1];
                    player.GetComponent<PlayerController>().playerWeapons[i - 1] = null;
                }
            }
        }
        player.GetComponent<PlayerController>().playerWeapons[0] = gameObject;
        player.GetComponent<PlayerController>().filledWeaponSlots++;

        transform.SetPositionAndRotation(player.weaponSlot.position, player.weaponSlot.rotation);
        transform.SetParent(player.weaponSlot);
        canAttack = true;

        GetComponent<Collider2D>().isTrigger = true;
    }

    public void Unequip()
    {
        player.currentRangedWeapon = null;
        player.GetComponent<PlayerController>().playerWeapons[0] = null;
        for (var i = 1; i < player.GetComponent<PlayerController>().playerWeapons.Length; i++)
        {
            if (player.GetComponent<PlayerController>().playerWeapons[i] != null)
            {
                player.GetComponent<PlayerController>().playerWeapons[i - 1] = player.GetComponent<PlayerController>().playerWeapons[i];
                player.GetComponent<PlayerController>().playerWeapons[i] = null;
            }
        }
        
        if(player.GetComponent<PlayerController>().playerWeapons[0] != null)
        {
            player.GetComponent<PlayerController>().playerWeapons[0].SetActive(true);
        }

        player.GetComponent<PlayerController>().filledWeaponSlots--;

        transform.SetParent(null);

        GetComponent<Collider2D>().isTrigger = false;

        player = null;

    }

    public void StartAttack()
    {
        canAttack = false;
        Invoke(nameof(StopSwing), SwingSpeed);
    }
    private void StopSwing()
    {
        attackZone.SetActive(true);
        Invoke(nameof(AttackCooldown), attackLingeringTime);
    }
    private void AttackCooldown()
    {
        attackZone.SetActive(false);
        Invoke(nameof(ResetCanAttack), SwingCooldown);
    }
    private void ResetCanAttack()
    {
        canAttack = true;
    }
}
