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
using Button = UnityEngine.UI.Button;

public class ShopTemplate : MonoBehaviour
{
    public PlayerController player;

    public GameObject infoPanel;
    //public GameObject itemSpawn;
    public GameObject itemScrollView;
    public GameObject itemOpen;
    public GameObject contentBound;
    public GameObject button;
    public GameObject tabContent;

    [Serializable]
    public class shopTabs
    {
        public string tabName;
        public int tabIndex;
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
            GameObject t = Instantiate(tabContent, contentBound.GetComponentInChildren<Transform>().GetChild(0).transform);
            t.name = tabs[i].tabName + "Tab";
            tabs[i].tab = t;
            
            GameObject b = Instantiate(button, GameObject.Find("TabButtons").transform);
            b.GetComponentInChildren<TextMeshProUGUI>().text = tabs[i].tabName;
            b.GetComponent<Button>().onClick.AddListener(() => TabClick(tabs[i-1].tabName));

            
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

    void Test(string message)
    {
        Debug.Log(message);
    }

    public void TabClick(int tab)
    {
        Debug.Log(tab);
        foreach(var item in tabs)
        {
            if (item. == tab)
            {
                contentBound.GetComponent<ScrollRect>().content = tabs[tab].tab.GetComponent<RectTransform>();
                item.tab.gameObject.SetActive(true);
            }
            else
            {
                item.tab.gameObject.SetActive(false);
            }

        }
        /*for(int i = 0; i < tabs.Length; i++)
        {
            tabs[i].tab.gameObject.SetActive(false);
        }

        contentBound.GetComponent<ScrollRect>().content = tabs[tab].tab.GetComponent<RectTransform>();
        tabs[tab].tab.gameObject.SetActive(true);*/
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
