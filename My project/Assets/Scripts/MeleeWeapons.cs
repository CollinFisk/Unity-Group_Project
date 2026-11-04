using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class MeleeWeapons : MonoBehaviour
{
    public PlayerController player;

    public bool canFire = false;
    public bool holdToAttack = true;
    public bool reloading = false;
    public string weaponName;
    public float rof;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        yield return new WaitForSeconds(rof);
        //player.weaponAnim.SetBool("isFiring", false);
        canFire = true;
    }
}
