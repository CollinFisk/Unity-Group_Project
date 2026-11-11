using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectDestruction : MonoBehaviour
{

    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject healthCanvas;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject[] potentialDrops;
    [SerializeField] private int maxDrops;
    [SerializeField] private int minDrops = 1;
    [SerializeField] private float timeVisible;

    public int objectHealth;
    public bool healthbarVisible = false;

    void Start()
    {
        healthBar.GetComponent<Slider>().maxValue = objectHealth;
    }

    void Update()
    {
        healthCanvas.GetComponent<Transform>().transform.position = gameObject.transform.position;

        healthBar.SetActive(healthbarVisible);
        healthBar.GetComponent<Slider>().value = objectHealth;

        if(objectHealth <= 0)
        {
            Destroy(parent);
            DropGoodies(minDrops, maxDrops);
        }
    }
    public void DropGoodies(int dropMin, int dropMax)
    {
        int amtDropped = (int)Random.Range(dropMin, dropMax);
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
    public IEnumerator HealthVisible()
    {
        yield return new WaitForSeconds(timeVisible);

        healthbarVisible = false;
    }
}
