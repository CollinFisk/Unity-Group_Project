using System.Collections;
using UnityEngine;

public class RangedEnemyWeapon : MonoBehaviour
{
    PlayerController player;
    RangedEnemy rangedEnemy;


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
    public float projLifespan;
    public float projVelocity;
    public float reloadCooldown;
    public float rof;


    public int maxDmg;
    public int minDmg;
    public int clip;
    public int clipSize;
    public int knockbackForce;

    void Start()
    {
        rangedEnemy = GetComponentInParent<RangedEnemy>();
        firePoint = transform.GetChild(0);        
        canFire = true;
        GetComponent<Collider2D>().isTrigger = true;

    }
    public void reload()
    {
        if (clip >= clipSize)
            return;

        int reloadCount = clipSize - clip;

        clip += reloadCount;


        reloading = true;
        canFire = false;
        StartCoroutine("reloadingCooldown");
    }

    public void fire()
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
            StartCoroutine("cooldownFire");
        }
        else if (clip == 0)
        {
            reload();
        }
    }

    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(rof);

        canFire = true;
    }

    IEnumerator reloadingCooldown()
    {
        yield return new WaitForSeconds(reloadCooldown);
        reloading = false;
        canFire = true;
    }
}
