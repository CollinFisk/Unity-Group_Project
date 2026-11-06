using System.Collections;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnemyMeleeWeapon : MonoBehaviour
{
    public MeleeEnemy enemy;
    public AttackHitboxForMeleeEnemy hitbox;
    public GameObject attackZone;

    public bool firing = false;
    public bool canFire = false;
    public bool holdToAttack = true;

    public bool canDmg = true;
    public string weaponName;
    public int maxDmg;
    public int minDmg;
    public float rof;
    public float knockbackForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        enemy = GetComponentInParent<MeleeEnemy>();
    }
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(rof);
        StartCoroutine("cooldownFire");
    }
    IEnumerator cooldownFire()
    {
        if (canFire)
        {
            firing = true;
            attackZone.SetActive(true);

            yield return new WaitForSeconds(0.1f);

            attackZone.SetActive(false);

            firing = false;
            Attack();
        }
    }
}
