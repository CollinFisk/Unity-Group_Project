using UnityEngine;

public class enemyMeleeWeaponAttackPoint : MonoBehaviour
{
    public PlayerController player;

    public int maxDmg;
    public int minDmg;
    public float knockbackForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            player.health -= Random.Range(minDmg, maxDmg);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(other.gameObject.transform.up * knockbackForce);
        }
        if (other.gameObject.CompareTag("Destroyable"))
        {
            if (other.GetComponent<ObjectDestruction>() != null)
            {
                other.GetComponent<ObjectDestruction>().objectHealth -= (int)(Random.Range(player.currentMeleeWeapon.minDmg, player.currentMeleeWeapon.maxDmg) * 2);
                gameObject.GetComponent<Rigidbody2D>().AddForce(other.gameObject.transform.up * other.GetComponentInParent<MeleeWeapons>().knockbackForce);

                other.GetComponent<ObjectDestruction>().healthbarVisible = true;

                if (other.GetComponent<ObjectDestruction>().healthbarVisible == true)
                {
                    StopCoroutine("HealthVisible");
                }

                StartCoroutine("HealthVisible");
            }
        }
    }
}
