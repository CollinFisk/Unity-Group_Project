using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RangedEnemyWeapon : MonoBehaviour
{
    [Header("Object Refrences")]
    public GameObject projectile;
    public Transform firePoint;

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
    public int knockbackForce;

    void Start()
    {
        firePoint = transform.GetChild(0);        
        canFire = true;
        GetComponent<Collider2D>().isTrigger = true;

    }
    public void Reload()
    {
        if (clip >= clipSize)
            return;

        int reloadCount = clipSize - clip;

        clip += reloadCount;


        reloading = true;
        canFire = false;
        StartCoroutine(ReloadingCooldown());
    }

    public void Fire()
    {
        if (clip > 0 && canFire && !reloading)
        {
            GameObject p = Instantiate(projectile, firePoint.position, transform.rotation);
            p.GetComponent<Rigidbody2D>().AddForce(firePoint.transform.up * projVelocity);
            p.GetComponent<BulletDmg>().bulletDmg = (int)Random.Range(minDmg, maxDmg);
            p.GetComponent<BulletDmg>().bulletKnockback = knockbackForce;
            Destroy(p, projLifespan);
            clip--;
            canFire = false;
            StartCoroutine(CooldownFire());
        }
        else if (clip == 0)
        {
            Reload();
        }
    }

    IEnumerator CooldownFire()
    {
        yield return new WaitForSeconds(rof);

        canFire = true;
    }

    IEnumerator ReloadingCooldown()
    {
        yield return new WaitForSeconds(reloadCooldown);
        reloading = false;
        canFire = true;
    }
}
