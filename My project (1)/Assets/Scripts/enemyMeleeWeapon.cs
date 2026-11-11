using System.Collections;
using UnityEngine;

public class EnemyMeleeWeapon : MonoBehaviour
{
    private GameObject attackZone;
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

    public IEnumerator Attack()
    {
        attacking = true;
        yield return new WaitForSeconds(SwingSpeed);
        attackZone.SetActive(true);
        yield return new WaitForSeconds(attackLingeringTime);
        attackZone.SetActive(false);

        StartCoroutine(CooldownFire());
    }
    IEnumerator CooldownFire()
    {
        yield return new WaitForSeconds(SwingCooldown);
        attacking = false;
    }
}
