using UnityEngine;

public class BulletDmg : MonoBehaviour
{
    public int bulletDmg;
    public float bulletKnockback;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MeleeEnemy"))
        {
            other.gameObject.GetComponent<MeleeEnemy>().enemyHealth -= bulletDmg;
        }
        if (other.gameObject.CompareTag("RangedEnemy"))
        {
            other.gameObject.GetComponent<RangedEnemy>().enemyHealth -= bulletDmg;
        }
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().health -= bulletDmg;
        }
        if (other.gameObject.CompareTag("Destroyable"))
        {
            if (other.gameObject.GetComponent<ObjectDestruction>() != null)
            {
                other.gameObject.GetComponent<ObjectDestruction>().objectHealth -= bulletDmg;

                other.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletKnockback);

                other.gameObject.GetComponent<ObjectDestruction>().healthbarVisible = true;

                if (other.gameObject.GetComponent<ObjectDestruction>().healthbarVisible == true)
                {
                    other.gameObject.GetComponent<ObjectDestruction>().healthbarVisible = false;
                }

                other.gameObject.GetComponent<ObjectDestruction>().HealthVisible();
            }

            if (other.gameObject.GetComponent<WallDestroy>() != null)
            {
                other.gameObject.GetComponent<WallDestroy>().objectHealth -= bulletDmg;

                other.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * bulletKnockback);

                other.gameObject.GetComponent<WallDestroy>().healthbarVisible = true;

                if (other.gameObject.GetComponent<WallDestroy>().healthbarVisible == true)
                {
                    other.gameObject.GetComponent<WallDestroy>().healthbarVisible = false;
                }

                other.gameObject.GetComponent<WallDestroy>().HealthVisible();
            }
        }
        Destroy(gameObject);
    }
}
