using System.Collections;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnemyMeleeWeapon : MonoBehaviour
{
    private GameObject attackZone;
    private string weaponName;
    public bool attacking;
    public int maxDmg;
    public int minDmg;
    public float knockbackForce;


    [SerializeField] private float SwingSpeed;
    [SerializeField] private float SwingCooldown;
    [SerializeField] private float attackLingeringTime;

    private void Start()
    {
        attackZone = transform.GetChild(0).gameObject;
    }
    private void Asleep()
    {
        StartCoroutine("Attack");
    }
    IEnumerator Attack()
    {
        attacking = true;
        yield return new WaitForSeconds(SwingSpeed);
        attackZone.gameObject.SetActive(true);
        yield return new WaitForSeconds(attackLingeringTime);
        attackZone.gameObject.SetActive(false);

        StartCoroutine("cooldownFire");
    }
    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(SwingCooldown);
        attacking = false;
    }
}
