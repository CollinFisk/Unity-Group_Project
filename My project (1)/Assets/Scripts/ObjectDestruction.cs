using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDestruction : MonoBehaviour
{

    public PlayerController player;
    public GameObject healthBar;
    public GameObject[] potentialDrops;
    public int objectHealth;
    public int maxDrops;
    public int minDrops = 1;

    public float timeVisible;

    public bool healthbarVisible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.GetComponent<Slider>().maxValue = objectHealth;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.SetActive(healthbarVisible);
        healthBar.GetComponent<Slider>().value = objectHealth;

        if(objectHealth <= 0)
        {
            Destroy(gameObject);
            DropGoodies();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            healthbarVisible = true;
            objectHealth -= (int)(Random.Range(player.currentRangedWeapon.minDmg, player.currentRangedWeapon.maxDmg) * 0.75f);
            gameObject.GetComponent<Rigidbody2D>().AddForce(collision.gameObject.transform.up * player.currentRangedWeapon.GetComponent<RangedWeapons>().knockbackForce * 0.5f);
            Destroy(collision.gameObject);

            if (healthbarVisible == true)
            {
                StopCoroutine("HealthVisible");
            }

            StartCoroutine("HealthVisible");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "AttackZone" && collision.GetComponentInParent<MeleeWeapons>().canDmg == true)
        {
            healthbarVisible = true;
            objectHealth -= (int)(Random.Range(player.currentMeleeWeapon.minDmg, player.currentMeleeWeapon.maxDmg) * 2);
            gameObject.GetComponent<Rigidbody2D>().AddForce(collision.gameObject.transform.up * collision.GetComponentInParent<MeleeWeapons>().knockbackForce);
            collision.GetComponentInParent<MeleeWeapons>().canDmg = false;

            if(healthbarVisible == true)
            {
                StopCoroutine("HealthVisible");
            }

            StartCoroutine("HealthVisible");
        }
    }

    public void DropGoodies()
    {
        int amtDropped = (int)Random.Range(minDrops, maxDrops);
        Debug.Log(amtDropped);
        for(var i = 0; i <= amtDropped; i++)
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
            
            if(rollSize <= 20)
            {
                drop.GetComponent<ResourceScript>().dropWorth = (int)(drop.GetComponent<ResourceScript>().dropWorth * 2f);
                drop.transform.localScale *= 2f;
            }
            else if(rollSize <= 70)
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

    IEnumerator HealthVisible()
    {
        yield return new WaitForSeconds(1f);

        healthbarVisible = false;
    }
}
