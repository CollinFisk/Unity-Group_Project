using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;
using System.Collections;
using System;
using System.Linq;

public class ShopTemplate : MonoBehaviour
{
    public PlayerController player;

    public GameObject infoPanel;
    //public GameObject itemSpawn;
    public GameObject itemOpen;

    [Serializable]
    public class shopTabs
    {
        public GameObject tab;
        public GameObject[] tabItems;
    }

    public shopTabs[] tabs;
    public GameObject[] purchasables;

    public GameObject itemWindow;

    public int menuIndex;
    public bool infoActive;
    public bool printing;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        infoPanel = GameObject.Find("PurchaseInfo");

        foreach (var item in tabs[0].tabItems)
        {
            GameObject p = Instantiate(itemWindow, tabs[0].tab.transform);
        }

        for (int i = 0; i < tabs.Length; i++)
        {
            //tabs[i].gameObject.SetActive(false);
        }

        infoPanel.SetActive(infoActive);
    }

    // Update is called once per frame
    void Update()
    {
        if (printing)
        {
            for (int i = 0; i < tabs.Length; i++)
            {
                //tabs[i].gameObject.SetActive(false);
            }

            infoPanel.SetActive(infoActive);
        }
    }

    public void TabClick(int tab)
    {
        for(int i = 0; i < tabs.Length; i++)
        {
            //tabs[i].gameObject.SetActive(false);
        }
        
        //tabs[tab].gameObject.SetActive(true);
    }

    public void ShowInfo(int item)
    {
        if(!itemOpen || purchasables[item] != itemOpen)
        {
            itemOpen = purchasables[item];
            infoActive = true;
        }
        else
        {
            infoActive = false;
            itemOpen = null;
        }

        infoPanel.SetActive(infoActive);

        
    }

    /*
    public void Buy()
    {
        if ()
        {
            
        }
    }
    */
}
