using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;
using System.Collections;
using System;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEditor.UI;

public class ShopTemplate : MonoBehaviour
{
    public PlayerController player;

    public GameObject infoPanel;
    //public GameObject itemSpawn;
    public GameObject itemOpen;
    public GameObject contentBound;
    public GameObject button;
    public GameObject tabContent;

    [Serializable]
    public class shopTabs
    {
        public string tabName;
        public GameObject tab;
        public GameObject[] tabItems;
    }

    public shopTabs[] tabs;
    public GameObject[] purchasables;

    public GameObject itemWindow;

    public int menuIndex;
    public bool infoActive;
    public bool printing;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        infoPanel = GameObject.Find("PurchaseInfo");

        for (var i = 0; i < tabs.Length; i++)
        {
            GameObject b = Instantiate(button, GameObject.Find("TabButtons").transform);
            b.GetComponentInChildren<TextMeshProUGUI>().text = tabs[i].tabName;
            //b.GetComponent<Butt>().;
            foreach (var item in tabs[i].tabItems)
            {
                GameObject p = Instantiate(itemWindow, tabs[i].tab.transform);
                p.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
            }
        }
        
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].tab.gameObject.SetActive(false);
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
            tabs[i].tab.gameObject.SetActive(false);
        }

        if (tabs[tab].tabItems.Length >= 3)
        {
            //contentBound.GetComponent<RectTransform>().rect.height+;
        }
        contentBound.GetComponent<ScrollRect>().content = tabs[tab].tab.GetComponent<RectTransform>();
        tabs[tab].tab.gameObject.SetActive(true);
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
