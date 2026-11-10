using System.Collections;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class MeleeWeapons : MonoBehaviour
{
    public PlayerController player;
    public GameObject attackZone;

    public bool canFire = false;
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

    public void equip(PlayerController p)
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
        canFire = true;

        GetComponent<Collider2D>().isTrigger = true;
    }

    public void unequip()
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

    IEnumerator hit()
    {
        canFire = false;
        yield return new WaitForSeconds(SwingSpeed);
        StartCoroutine("cooldownFire");
    }
    IEnumerator cooldownFire()
    {
        attackZone.SetActive(true);
        yield return new WaitForSeconds(attackLingeringTime);
        attackZone.SetActive(false);
        yield return new WaitForSeconds(SwingCooldown);
        hit();
        canFire = true;
    }
}
