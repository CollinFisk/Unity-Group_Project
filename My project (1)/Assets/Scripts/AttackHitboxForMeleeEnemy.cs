using UnityEngine;

public class AttackHitboxForMeleeEnemy : MonoBehaviour
{
    MeleeEnemy enemy;
    EnemyMeleeWeapon weapon;
    void Start()
    {
        enemy = GetComponentInParent<MeleeEnemy>();
        weapon = enemy.transform.GetChild(0).GetComponent<EnemyMeleeWeapon>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (!weapon.attacking)
            {
                weapon.StartCoroutine("Attack");
            }
        }
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!weapon.attacking)
            {
                weapon.StartCoroutine("Attack");
            }
        }
    }
}
