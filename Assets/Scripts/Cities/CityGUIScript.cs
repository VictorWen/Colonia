using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityGUIScript : MonoBehaviour
{
    private City city;

    public Text cityName;

    //Tabs
    public HorizontalLayoutGroup buttons;
    private TabScript defaultTab;
    private TabScript selectedTab;

    private void Awake()
    {
        TabScript[] tabs = buttons.GetComponentsInChildren<TabScript>();
        for (int i = 0; i < tabs.Length; i++)
        {
            TabScript tab = tabs[i];
            tab.Setup(this);
            if (i == 0)
            {
                defaultTab = tab;
                selectedTab = tab;
            }
        }
        //gameObject.SetActive(false);
    }

    public void ChangeTab(TabScript tab)
    {
        selectedTab.button.interactable = true;
        selectedTab.panel.gameObject.SetActive(false);

        tab.button.interactable = false;
        tab.panel.Enable(city);
        selectedTab = tab;
    }

    public void Enable(City city)
    {
        gameObject.SetActive(true);
        this.city = city;
        cityName.text = city.Name;
        ChangeTab(defaultTab);
    }
}
