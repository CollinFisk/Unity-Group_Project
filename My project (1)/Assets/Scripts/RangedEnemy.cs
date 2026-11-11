using UnityEngine;

[RequireComponent(typeof(EnemyNav))]
public class RangedEnemy : MonoBehaviour
{
    GameManager gameManager;
    RangedEnemyWeapon weapon;
    EnemyNav enemyNav;

    public int enemyHealth;
    [SerializeField] private int enemyMaxHealth;

    public GameObject[] potentialDrops;
    public int maxDrops;
    public int minDrops = 1;
    void Start()
    {
        gameManager = GameObject.Find("gameManager").GetComponent<GameManager>();
        weapon = GetComponentInChildren<RangedEnemyWeapon>();
        enemyNav = GetComponent<EnemyNav>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth <= 0)
        {
            Destroy(transform.gameObject);
            gameManager.enemyCount--;
        }

        if (enemyNav.isFollowing && weapon.canFire && !weapon.reloading)
        {
            weapon.Fire();
        }
    }


    public void DropGoodies(int dropMin, int dropMax)
    {
        int amtDropped = (int)Random.Range(dropMin, dropMax);
        for (var i = 0; i <= amtDropped; i++)
        {
            GameObject drop;

            int rollType = (int)Random.Range(0f, 100f);
            int rollSize = (int)Random.Range(0f, 100f);

            if (rollType <= 20)
            {
                drop = Instantiate(potentialDrops[2], gameObject.transform.position, gameObject.transform.rotation);
            }
            else if (rollType <= 60)
            {
                drop = Instantiate(potentialDrops[0], gameObject.transform.position, gameObject.transform.rotation);
            }
            else
            {
                drop = Instantiate(potentialDrops[1], gameObject.transform.position, gameObject.transform.rotation);
            }

            if (rollSize <= 20)
            {
                drop.GetComponent<ResourceScript>().dropWorth = (int)(drop.GetComponent<ResourceScript>().dropWorth * 2f);
                drop.transform.localScale *= 2f;
            }
            else if (rollSize <= 70)
            {
                drop.GetComponent<ResourceScript>().dropWorth = (int)(drop.GetComponent<ResourceScript>().dropWorth * 1f);
                drop.transform.localScale *= 1f;
            }
            else
            {
                drop.GetComponent<ResourceScript>().dropWorth = (int)(drop.GetComponent<ResourceScript>().dropWorth * 0.75f);
                drop.transform.localScale *= 0.75f;
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
            enemyHealth--;
        }
    }
}
