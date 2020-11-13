using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cities
{
    public class CityGUIScript : MonoBehaviour
    {
        private City city;

        public Text cityName;

        public GUIMaster gui;

        //Tabs
        public HorizontalLayoutGroup buttons;
        private TabScript defaultTab;
        private TabScript selectedTab;
        private TabScript[] tabs;

        public void Awake()
        {
            tabs = buttons.GetComponentsInChildren<TabScript>();
            for (int i = 0; i < tabs.Length; i++)
            {
                TabScript tab = tabs[i];
                tab.button.onClick.AddListener(() => ChangeTab(tab));
                if (i == 0)
                {
                    defaultTab = tab;
                    selectedTab = tab;
                }
            }
            //gameObject.SetActive(false);
        }

        public void OnEnable()
        {
            UpdateGUI();
        }

        public void ChangeTab(TabScript tab)
        {
            selectedTab.button.interactable = true;
            selectedTab.panel.gameObject.SetActive(false);

            tab.button.interactable = false;
            tab.panel.Enable(city, gui);
            selectedTab = tab;
        }

        public void OpenCityGUI(City city)
        {
            gameObject.SetActive(true);
            this.city = city;
            cityName.text = city.Name;
            ChangeTab(defaultTab);
            gui.GUIState.SetState(GUIStateManager.CITY);
        }

        public void CloseCityGUI()
        {
            gameObject.SetActive(false);
            gui.GUIState.SetState(GUIStateManager.MAP);
        }

        public void UpdateGUI()
        {
            selectedTab.panel.UpdateGUI();
        }

    }
}