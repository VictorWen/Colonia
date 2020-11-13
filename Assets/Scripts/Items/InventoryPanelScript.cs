using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cities
{
    public class InventoryPanelScript : CityPanelScript
    {
        public GridLayoutGroup gridLayout;
        public ItemGUI itemPrefab;
        public GameObject tooltipPanel;
        public InputField searchText;

        private Inventory loaded;

        public void OpenInventory()
        {
            Clear();
            foreach (Item i in loaded.Items)
            {
                GameObject itemObject = Instantiate(itemPrefab.gameObject);
                itemObject.transform.SetParent(gridLayout.transform);
                itemObject.GetComponent<ItemGUI>().SetItem(i, tooltipPanel);
            }
        }

        public override void Enable(City city, GUIMaster gui)
        {
            //TODO: Formalize opening city inventory
            loaded = gui.Game.GlobalInventory;
            base.Enable(city, gui);
        }

        private void OnEnable()
        {
            OpenInventory();
        }

        private void Clear()
        {
            foreach (ItemGUI item in gridLayout.GetComponentsInChildren<ItemGUI>())
            {
                Destroy(item.gameObject);
            }
        }

        public void UpdateSearch()
        {
            string search = searchText.text.ToLowerInvariant();
            foreach (ItemGUI item in gridLayout.GetComponentsInChildren<ItemGUI>())
            {
                if (!item.title.text.ToLowerInvariant().Contains(search))
                {
                    item.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                }
                else
                {
                    item.GetComponent<Image>().color = new Color(1, 1, 1);
                }
            }
        }

        public void SortByName()
        {
            loaded.Sort(Inventory.ItemSortID.NAME);
            OpenInventory();
        }
    }
}