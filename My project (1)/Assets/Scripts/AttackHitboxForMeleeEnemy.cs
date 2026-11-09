using UnityEngine;

public class AttackHitboxForMeleeEnemy : MonoBehaviour
{
    MeleeEnemy enemy;
    EnemyMeleeWeapon weapon;

    public bool PlayerInCanAttackHitbox;
    void Start()
    {
        enemy = GetComponentInParent<MeleeEnemy>();
        weapon = enemy.transform.GetChild(0).GetComponent<EnemyMeleeWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerInCanAttackHitbox = true;
            weapon.StartCoroutine("Attack");
        }
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInCanAttackHitbox = true;
            weapon.StartCoroutine("Attack");
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInCanAttackHitbox = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInCanAttackHitbox = false;
            weapon.StopAllCoroutines();
        }
    }
}
