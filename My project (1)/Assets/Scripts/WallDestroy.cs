//using System.Collections;
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WallDestroy : MonoBehaviour
{

    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject healthCanvas;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject[] potentialDrops;
    [SerializeField] private int maxDestroyDrops;
    [SerializeField] private int minDestroyDrops = 1;
    [SerializeField] private int maxHurtDrops;
    [SerializeField] private int minHurtDrops = 1;
    [SerializeField] private bool HealthDepletedThird = false;
    [SerializeField] private bool HealthDepletedTwoThirds = false;


    public int objectHealth;
    [SerializeField] private int objectMaxHealth;
    public bool healthbarVisible = false;
    [SerializeField] private float timeVisible;

    void Start()
    {
        healthBar.GetComponent<Slider>().maxValue = objectHealth;
    }

    void Update()
    {
        healthCanvas.GetComponent<Transform>().transform.position = gameObject.transform.position;

        healthBar.SetActive(healthbarVisible);
        healthBar.GetComponent<Slider>().value = objectHealth;

        if (objectHealth < objectMaxHealth)
        {
            if(objectHealth * 3 < objectMaxHealth * 2 && HealthDepletedThird == false)
            {
                OnHealthDepletedThird();
            }
            if (objectHealth * 3 < objectMaxHealth && HealthDepletedTwoThirds == false)
            {
                OnHealthDepletedTwoThirds();
            }
            if (objectHealth <= 0)
            {
                Destroy(parent);
                DropGoodies(minDestroyDrops, maxDestroyDrops);
            }
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

    private void OnHealthDepletedThird()
    {
        HealthDepletedThird = true;
        DropGoodies(minHurtDrops, maxHurtDrops);
        //Change The Texture Here
    }
    private void OnHealthDepletedTwoThirds()
    {
        HealthDepletedTwoThirds = true;
        DropGoodies(minHurtDrops, maxHurtDrops);
        //Change The Texture Here
    }

    public void HealthVisible()
    {
        healthbarVisible = true;
        Invoke(nameof(HealthInvisible), timeVisible);
    }
    private void HealthInvisible()
    {
        healthbarVisible = false;
    }
}
