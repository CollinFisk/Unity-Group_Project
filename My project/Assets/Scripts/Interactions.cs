using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    public PlayerController player;
    public bool canInteract = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

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
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag( "MeleeWeapon") || other.CompareTag( "RangedWeapon") || other.CompareTag( "Ammo") || other.CompareTag( "Health"))
        {
            canInteract = false;
            player.pickupObj = null;
        }
    }
}