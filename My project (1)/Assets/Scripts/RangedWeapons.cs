using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RangedWeapons : MonoBehaviour
{
    private PlayerController player;

    [Header("Object Refrences")]
    [SerializeField] private GameObject projectile;

    private Transform firePoint;
    [Header("Meta Attributes")]
    public bool canFire = false;
    public bool holdToAttack = true;
    public bool reloading = false;
    public bool equipped = false;
    public string weaponName;

    [Header("Weapon Stats")]
    [SerializeField] private float projLifespan;
    [SerializeField] private float projVelocity;
    [SerializeField] private float reloadCooldown;
    [SerializeField] private float rof;


    [SerializeField] private int maxDmg;
    [SerializeField] private int minDmg;

    [Header("Public Weapon Stats")]

    public int clip;
    public int clipSize;
    public int ammo;
    public int maxAmmo;
    public int knockbackForce;


    void Start()
    {
        firePoint = transform.GetChild(0);
        canFire = true;

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

        if (player.GetComponent<PlayerController>().playerWeapons[0] != null)
        {
            player.GetComponent<PlayerController>().playerWeapons[0].SetActive(true);
        }
        player.GetComponent<PlayerController>().filledWeaponSlots--;

        transform.SetParent(null);

        GetComponent<Collider2D>().isTrigger = false;

        player = null;

    }

    public void Reload()
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
        Invoke(nameof(ReloadingCooldown), reloadCooldown);
    }

    public void Fire()
    {
        if (clip > 0 && canFire && !reloading && ammo >= 0)
        {
            GameObject p = Instantiate(projectile, firePoint.position, transform.rotation);
            p.GetComponent<Rigidbody2D>().AddForce(firePoint.transform.up * projVelocity);
            p.GetComponent<BulletDmg>().bulletDmg = (int)Random.Range(player.currentRangedWeapon.minDmg, player.currentRangedWeapon.maxDmg);
            p.GetComponent<BulletDmg>().bulletKnockback = knockbackForce;
            Destroy(p, projLifespan);
            clip--;
            canFire = false;
            //player.attackingAnim = true;
            Invoke(nameof(CooldownFire), rof);
        }
    }

    private void CooldownFire()
    {
        if (clip > 0)
            canFire = true;
    }

    private void  ReloadingCooldown()
    {
        reloading = false;
        canFire = true;
    }
}
