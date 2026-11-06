using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    PlayerController Player;
    GameManager gameManager;
    EnemyNav nav;
    public Transform player;

    Rigidbody2D rigidbody;
    Collider2D hitBox;

    EnemyMeleeWeapon weapon;

    public int enemyHealth;
    public int enemyMaxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = player.GetComponent<PlayerController>();
        weapon = transform.GetChild(0).GetComponent<EnemyMeleeWeapon>();
        hitBox = transform.GetChild(1).GetComponent<BoxCollider2D>();
        nav = GetComponent<EnemyNav>();
        gameManager = GameObject.Find("gameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHealth <= 0)
        {
            Destroy(transform.gameObject);
            gameManager.enemyCount--;
        }

        if (weapon.firing)
        {
            nav.speed /= 2;
        }
        else
            nav.speed = nav.baseSpeed;
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
