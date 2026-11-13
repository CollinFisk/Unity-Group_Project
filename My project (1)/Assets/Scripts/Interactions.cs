using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    public PlayerController player;
    public bool canInteract = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MeleeWeapon"))
        {
            canInteract = true;

            if (other.gameObject.transform.IsChildOf(player.transform))
                player.pickupObj = null;
            else
                player.pickupObj = other.gameObject;
        }
        if (other.CompareTag("RangedWeapon"))
        {
            canInteract = true;

            if (other.gameObject.transform.IsChildOf(player.transform))
                player.pickupObj = null;
            else
                player.pickupObj = other.gameObject;
        }

        if (other.CompareTag("Ammo"))
        {
            canInteract = true;
            player.pickupObj = other.gameObject;
        }

        if (other.CompareTag("Health"))
        {
            player.pickupObj = other.gameObject;

            if (player.health < player.maxHealth)
            {
                canInteract = true;
            }
            else
            {
                canInteract = false;
            }
        }

        if (other.CompareTag("Shop"))
        {
            canInteract = true;
            player.pickupObj = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag( "MeleeWeapon") || other.CompareTag( "RangedWeapon") || other.CompareTag( "Ammo") || other.CompareTag( "Health") || other.CompareTag("Shop"))
        {
            canInteract = false;
            player.pickupObj = null;
        }
    }
}