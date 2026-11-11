using UnityEngine;

public class MeleeWeaponAttackPoint : MonoBehaviour
{    
    private MeleeWeapons meleeWeapon;

    private void Start()
    {
        meleeWeapon = GetComponentInParent<MeleeWeapons>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("MeleeEnemy") && other == other.GetComponent<CapsuleCollider2D>())
        {
            other.GetComponent<MeleeEnemy>().enemyHealth -= Random.Range(meleeWeapon.minDmg, meleeWeapon.maxDmg);
            other.GetComponent<Rigidbody2D>().AddForce(transform.up * meleeWeapon.knockbackForce);
        }
        if (other.gameObject.CompareTag("RangedEnemy") && other == other.GetComponent<CapsuleCollider2D>())
        {
            other.GetComponent<RangedEnemy>().enemyHealth -= Random.Range(meleeWeapon.minDmg, meleeWeapon.maxDmg);
            other.GetComponent<Rigidbody2D>().AddForce(transform.up * meleeWeapon.knockbackForce);
        }
        if (other.gameObject.CompareTag("Destroyable"))
        {
            if (other.GetComponent<ObjectDestruction>() != null)
            {
                other.GetComponent<ObjectDestruction>().objectHealth -= (int)(Random.Range(meleeWeapon.minDmg, meleeWeapon.maxDmg) * 2);

                other.GetComponent<Rigidbody2D>().AddForce(transform.up * meleeWeapon.knockbackForce);

                other.GetComponent<ObjectDestruction>().healthbarVisible = true;

                if (other.GetComponent<ObjectDestruction>().healthbarVisible == true)
                {
                    other.GetComponent<ObjectDestruction>().healthbarVisible = false;
                }

                other.GetComponent<ObjectDestruction>().HealthVisible();
            }

            if (other.GetComponent<WallDestroy>() != null)
            {
                other.GetComponent<WallDestroy>().objectHealth -= (int)(Random.Range(meleeWeapon.minDmg, meleeWeapon.maxDmg) * 2);

                other.GetComponent<Rigidbody2D>().AddForce(transform.up * meleeWeapon.knockbackForce);

                other.GetComponent<WallDestroy>().healthbarVisible = true;

                if (other.GetComponent<WallDestroy>().healthbarVisible == true)
                {
                    other.GetComponent<WallDestroy>().healthbarVisible = false;
                }

                other.GetComponent<WallDestroy>().HealthVisible();
            }
        }
    }
}
