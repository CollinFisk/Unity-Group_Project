using System.Collections;
using UnityEngine;

public class RangedWeapons : MonoBehaviour
{
    PlayerController player;

    [Header("Object Refrences")]
    public GameObject projectile;
    public Transform firePoint;
    [Header("Meta Attributes")]
    public bool canFire = false;
    public bool holdToAttack = true;
    public bool reloading = false;
    public string weaponName;

    [Header("Weapon Stats")]
    public float projLifespan;
    public float projVelocity;
    public float reloadCooldown;
    public float rof;


    public int maxDmg;
    public int minDmg;
    public int clip;
    public int clipSize;
    public int ammo;
    public int maxAmmo;
    public int knockbackForce;

    void Start()
    {
        firePoint = transform.GetChild(0);
    }

    void Update()
    {
        if (player)
        {
            if (player.playerWeapons[0] != gameObject)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }

    public void equip(PlayerController p)
    {
        player = p;
        transform.gameObject.SetActive(true);
        player.currentRangedWeapon = this;
        if (player.GetComponent<PlayerController>().playerWeapons[0] != null)
        {
            for (var i = player.GetComponent<PlayerController>().playerWeapons.Length; i > 0; i--)
            {
                if (player.GetComponent<PlayerController>().playerWeapons[i - 1] != null && i < player.GetComponent<PlayerController>().playerWeapons.Length)
                {
                    player.GetComponent<PlayerController>().playerWeapons[i] = player.GetComponent<PlayerController>().playerWeapons[i - 1];
                    player.GetComponent<PlayerController>().playerWeapons[i - 1] = null;
                }
            }
        }
        player.GetComponent<PlayerController>().playerWeapons[0] = gameObject;
        player.GetComponent<PlayerController>().filledWeaponSlots++;

        transform.SetParent(player.weaponSlot);
        transform.SetPositionAndRotation(player.weaponSlot.position, player.weaponSlot.rotation);
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

        if (player.GetComponent<PlayerController>().playerWeapons[0] != null)
        {
            player.GetComponent<PlayerController>().playerWeapons[0].SetActive(true);
        }
        player.GetComponent<PlayerController>().filledWeaponSlots--;

        transform.SetParent(null);

        GetComponent<Collider2D>().isTrigger = false;

        player = null;

    }

    public void reload()
    {
        if (clip >= clipSize)
            return;

        int reloadCount = clipSize - clip;

        if (ammo < reloadCount)
        {
            clip += ammo;
            ammo = 0;
        }
        else
        {
            clip += reloadCount;
            ammo -= reloadCount;
        }

        reloading = true;
        canFire = false;
        StartCoroutine("reloadingCooldown");
    }

    public void fire()
    {
        if (clip > 0 && canFire && !reloading && ammo >= 0)
        {
            GameObject p = Instantiate(projectile, firePoint.position, transform.rotation);
            p.GetComponent<Rigidbody2D>().AddForce(firePoint.transform.up * projVelocity);
            Destroy(p, projLifespan);
            clip--;
            canFire = false;
            //player.attackingAnim = true;
            StartCoroutine("cooldownFire");
        }
    }

    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(rof);
        //player.weaponAnim.SetBool("isFiring", false);

        if (clip > 0)
            canFire = true;
    }

    IEnumerator reloadingCooldown()
    {
        yield return new WaitForSeconds(reloadCooldown);
        reloading = false;
        canFire = true;
    }
}
