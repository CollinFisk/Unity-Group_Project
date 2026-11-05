using System.Collections;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class MeleeWeapons : MonoBehaviour
{
    public PlayerController player;
    public GameObject attackZone;

    public bool canFire = false;
    public bool holdToAttack = true;
    public bool reloading = false;

    public bool canDmg = true;
    public string weaponName;
    public int maxDmg;
    public int minDmg;
    public float rof;
    public float knockbackForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Update()
    {
        if (attackZone.activeSelf == true)
        {
            //attackZone.SetActive(false);
        }
    }

    public void equip(PlayerController p)
    {
        player = p;

        player.currentMeleeWeapon = this;

        transform.SetPositionAndRotation(player.weaponSlot.position, player.weaponSlot.rotation);
        transform.SetParent(player.weaponSlot);
        canFire = true;

        GetComponent<Collider2D>().isTrigger = true;
    }

    public void unequip()
    {
        player.currentRangedWeapon = null;

        transform.SetParent(null);

        GetComponent<Collider2D>().isTrigger = false;

        player = null;

    }

    public void hit()
    {
        if (canFire && !reloading)
        {
            StartCoroutine("cooldownFire");
        }
    }

    IEnumerator cooldownFire()
    {
        attackZone.SetActive(true);

        yield return new WaitForSeconds(rof);
        //player.weaponAnim.SetBool("isFiring", false);

        attackZone.SetActive(false);
        canFire = true;
        canDmg = true;
    }
}
