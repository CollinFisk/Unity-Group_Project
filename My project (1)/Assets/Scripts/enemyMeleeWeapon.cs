using System.Collections;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnemyMeleeWeapon : MonoBehaviour
{
    public MeleeEnemy enemy;
    public GameObject attackZone;

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

    void Update()
    {
        if (attackZone.activeSelf == true)
        {
            //attackZone.SetActive(false);
        }
    }

    public void hit()
    {
        if (canFire)
        {
            StartCoroutine("cooldownFire");
        }
    }

    IEnumerator cooldownFire()
    {
        attackZone.SetActive(true);

        yield return new WaitForSeconds(rof);

        attackZone.SetActive(false);
        canFire = true;
        canDmg = true;
    }
}
