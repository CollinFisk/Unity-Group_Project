using UnityEngine;

public class enemyMeleeWeaponAttackPoint : MonoBehaviour
{
    EnemyMeleeWeapon weapon;
    private void Start()
    {
        weapon = GetComponentInParent<EnemyMeleeWeapon>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().health -= Random.Range(weapon.minDmg, weapon.maxDmg);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * weapon.knockbackForce);
        }
        if (other.gameObject.CompareTag("Destroyable"))
        {
            if (other.GetComponent<ObjectDestruction>() != null)
            {
                other.GetComponent<ObjectDestruction>().objectHealth -= (int)(Random.Range(weapon.minDmg, weapon.maxDmg) * 2);
                other.GetComponent<Rigidbody2D>().AddForce(transform.up * weapon.knockbackForce);

                other.GetComponent<ObjectDestruction>().healthbarVisible = true;

                if (other.GetComponent<ObjectDestruction>().healthbarVisible == true)
                {
                    other.GetComponent<ObjectDestruction>().StopCoroutine("HealthVisible");
                }

                other.GetComponent<ObjectDestruction>().StartCoroutine("HealthVisible");
            }
        }
    }
}
