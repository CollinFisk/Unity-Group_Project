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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            weapon.canFire = true;
            weapon.StartCoroutine("Attack");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            weapon.canFire = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            weapon.StopAllCoroutines();
            weapon.firing = false;
            weapon.canFire = false;
        }
    }
}
