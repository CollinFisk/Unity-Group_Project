using System.Collections;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnemyMeleeWeapon : MonoBehaviour
{
    public MeleeEnemy enemy;
    public AttackHitboxForMeleeEnemy hitbox;
    public GameObject attackZone;

    public string weaponName;
    public float SwingSpeed;
    public float SwingCooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        enemy = GetComponentInParent<MeleeEnemy>();
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(SwingSpeed);
        attackZone.SetActive(true);
        yield return new WaitForSeconds(1);
        attackZone.SetActive(false);



        StartCoroutine("cooldownFire");
    }
    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(SwingCooldown);
        if (hitbox.PlayerInCanAttackHitbox)
        {
            StartCoroutine(Attack());
        }
    }
}
