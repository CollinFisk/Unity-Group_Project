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
    [Header("Context")]
    public PlayerController player;
    public GameObject shopUI;
    public GameObject infoPanel;
    public TextMeshProUGUI costText;
    public GameObject itemSpawn;

    [Header("Shop Functioning")]
    public GameObject itemScrollView;
    public GameObject itemOpen;
    public int itemIndex;
    public Transform contentBound;
    public GameObject openTab;

    [Header("Prefabs")]
    public GameObject button;
    public GameObject tabContent;
    public GameObject itemWindow;

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

    

    public int menuIndex;
    public bool infoActive;
    public bool printing;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        itemSpawn = GetComponentInChildren<Transform>().gameObject;
        infoPanel = GameObject.Find("PurchaseInfo");
        contentBound = shopUI.GetComponentInChildren<Transform>().GetChild(2);
        costText = shopUI.GetComponentInChildren<Transform>().GetChild(4).GetChild(2).GetChild(3).GetComponentInChildren<TextMeshProUGUI>();

        foreach (var tab in tabs)
        {
            tab.tabIndex = System.Array.IndexOf(tabs, tab);

            GameObject t = Instantiate(tabContent, contentBound.GetComponentInChildren<Transform>().GetChild(0).transform);
            t.name = tab.tabName + "Tab";
            tab.tab = t;

            GameObject b = Instantiate(button, shopUI.GetComponentInChildren<Transform>().GetChild(3).GetChild(0).GetChild(0).transform);
            b.GetComponentInChildren<TextMeshProUGUI>().text = tab.tabName;
            b.GetComponent<Button>().onClick.AddListener(() => TabClick(System.Array.IndexOf(tabs, tab)) );

            foreach (var item in tab.items)
            {
                GameObject p = Instantiate(itemWindow, tab.tab.transform);
                p.GetComponentInChildren<TextMeshProUGUI>().text = item.item.name;
                p.GetComponent<Button>().onClick.AddListener(() => ShowInfo(System.Array.IndexOf(tab.items, item)) );
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

    public void TabClick(int tab)
    {
        foreach(var item in tabs)
        {
            if (item.tabIndex == tab && item.tab != openTab)
            {
                contentBound.GetComponent<ScrollRect>().content = tabs[tab].tab.GetComponent<RectTransform>();
                item.tab.gameObject.SetActive(true);
                openTab = item.tab;
            }
            else
            {
                item.tab.gameObject.SetActive(false);
                if(item.tab == openTab)
                {
                    openTab = null;
                }
            }
        }
    }

    public void ShowInfo(int product)
    {
        foreach (var element in tabs)
        {
            if (element.tab == openTab)
            {
                if (element.items[product].item != itemOpen || !itemOpen)
                {
                    itemOpen = element.items[product].item;
                    itemIndex = product;
                    costText.text = "Wood: " + player.wood + "/" + element.items[product].woodCost + "\nMetal: " + player.metal + "/" + element.items[product].metalCost + "\nComponents: " + player.components + "/" + element.items[product].componentCost;
                    shopUI.GetComponentInChildren<Transform>().GetChild(4).GetComponentInChildren<TextMeshProUGUI>().text = element.items[product].item.name;
                    if(element.items[product].item.tag == "RangedWeapon")
                    {
                        shopUI.GetComponentInChildren<Transform>().GetChild(4).GetChild(2).GetChild(1).gameObject.SetActive(false);
                        shopUI.GetComponentInChildren<Transform>().GetChild(4).GetChild(2).GetChild(0).gameObject.SetActive(true);
                        shopUI.GetComponentInChildren<Transform>().GetChild(4).GetChild(2).GetChild(0).GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = "Damage: " + element.items[product].item.GetComponent<RangedWeapons>().minDmg + "-" + element.items[product].item.GetComponent<RangedWeapons>().maxDmg + "\nFire Rate: " + (int)(60f / element.items[product].item.GetComponent<RangedWeapons>().rof) + " rnds/min\nReload: " + element.items[product].item.GetComponent<RangedWeapons>().reloadCooldown + " seconds\nMag: " + element.items[product].item.GetComponent<RangedWeapons>().clipSize;
                    }
                    else if (element.items[product].item.tag == "MeleeWeapon")
                    {
                        shopUI.GetComponentInChildren<Transform>().GetChild(4).GetChild(2).GetChild(1).gameObject.SetActive(false);
                        shopUI.GetComponentInChildren<Transform>().GetChild(4).GetChild(2).GetChild(0).gameObject.SetActive(true);
                        shopUI.GetComponentInChildren<Transform>().GetChild(4).GetChild(2).GetChild(0).GetChild(2).GetComponentInChildren<TextMeshProUGUI>().text = "Damage: " + element.items[product].item.GetComponent<MeleeWeapons>().minDmg + "-" + element.items[product].item.GetComponent<MeleeWeapons>().maxDmg + "\nSwing Speed: " + element.items[product].item.GetComponent<MeleeWeapons>().SwingSpeed + " seconds\nSwing Cooldown: " + element.items[product].item.GetComponent<MeleeWeapons>().SwingCooldown;
                    }
                    infoActive = true;
                }
                else
                {
                    infoActive = false;
                    itemOpen = null;
                }
            }
        }

        infoPanel.SetActive(infoActive);
    }

    
    public void Buy()
    {
        foreach (var element in tabs)
        {
            if (element.tab == openTab)
            {
                Debug.Log(element.items[itemIndex].woodCost < player.wood);
                Debug.Log(element.items[itemIndex].metalCost < player.metal);
                Debug.Log(element.items[itemIndex].componentCost < player.components);
                if (element.items[itemIndex].woodCost < player.wood && element.items[itemIndex].metalCost < player.metal && element.items[itemIndex].componentCost < player.components)
                {
                    GameObject g = Instantiate(itemOpen, itemSpawn.GetComponent<Transform>().position, itemSpawn.GetComponent<Transform>().rotation);
                    g.name = itemOpen.name;

                    player.wood -= element.items[itemIndex].woodCost;
                    player.metal -= element.items[itemIndex].metalCost;
                    player.components -= element.items[itemIndex].componentCost;
                }
            }
        }
    }
    //*/
}
