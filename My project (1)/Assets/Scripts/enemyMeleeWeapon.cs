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

    public void StartAttack()
    {
        attacking = true;
        Invoke(nameof(StopSwing), SwingSpeed);
    }
    private void StopSwing()
    {
        attackZone.SetActive(true);
        Invoke(nameof(AttackCooldown), attackLingeringTime);
    }
    private void AttackCooldown()
    {
        attackZone.SetActive(false);
        Invoke(nameof(ResetCanAttack), SwingCooldown);
    }
    private void ResetCanAttack()
    {
        attacking = false;
    }
}
