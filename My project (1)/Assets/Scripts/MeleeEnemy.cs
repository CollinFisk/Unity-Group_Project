using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    PlayerController Player;
    GameManager gameManager;
    public Transform player;

    Rigidbody2D rigidbody;
    EnemyNav enemyNav;
    Collider2D hitBox;

    EnemyMeleeWeapon weapon;

    public int enemyHealth;
    public int enemyMaxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitBox = transform.GetComponent<BoxCollider2D>();
        Player = player.GetComponent<PlayerController>();
        weapon = transform.GetChild(0).GetComponent<EnemyMeleeWeapon>();
        enemyNav = transform.GetComponent<EnemyNav>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHealth <= 0)
        {
            Destroy(transform.gameObject);
            gameManager.enemiesKilled++;
        }
        if (enemyNav.isFollowing)
        {
            if (enemyNav.canAttack)
            {
                weapon.canFire = true;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.health--;
        }
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
            enemyHealth--;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.health--;
        }
    }
}
