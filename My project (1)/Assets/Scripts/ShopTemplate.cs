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
    public class TabItems
    {
        public GameObject item;
        public int woodCost;
        public int metalCost;
        public int componentCost;
    }//*/

    [Serializable]
    public class ShopTabs
    {
        public string tabName;
        public int tabIndex;
        public GameObject tab;
        public TabItems[] items;
    }

    public ShopTabs[] tabs;
    public GameObject[] purchasables;

    public GameObject itemWindow;

    public int menuIndex;
    public bool infoActive;
    public bool printing;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        infoPanel = GameObject.Find("PurchaseInfo");

        foreach(var tab in tabs)
        {
            GameObject t = Instantiate(tabContent, contentBound.GetComponentInChildren<Transform>().GetChild(0).transform);
            t.name = tab.tabName + "Tab";
            tab.tab = t;

            GameObject b = Instantiate(button, GameObject.Find("TabButtons").transform);
            b.GetComponentInChildren<TextMeshProUGUI>().text = tab.tabName;
            b.GetComponent<Button>().onClick.AddListener(() => TabClick(tab.tabIndex));

            foreach (var item in tab.items)
            {
                GameObject p = Instantiate(itemWindow, tab.tab.transform);
                p.GetComponentInChildren<TextMeshProUGUI>().text = item.item.name;
            }//*/
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
        foreach(var item in tabs)
        {
            if (item.tabIndex == tab)
            {
                contentBound.GetComponent<ScrollRect>().content = tabs[tab].tab.GetComponent<RectTransform>();
                item.tab.gameObject.SetActive(true);
            }
            else
            {
                item.tab.gameObject.SetActive(false);
            }
        }
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
