using UnityEngine;
using UnityEngine.UI;
using Units;

namespace Items
{
    public class HeroInventoryPanel : MonoBehaviour
    {
        public GridLayoutGroup gridLayout;
        public ItemGUI itemPrefab;
        public GameObject tooltipPanel;

        public VerticalLayoutGroup itemActionMenu;
        public Button itemActionButtonPrefab;

        private UnitEntityController selectedUnit;

        public void Enable(UnitEntityController unit)
        {
            selectedUnit = unit;
            OpenInventory();
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void OpenInventory()
        {
            // Clear gridLayout of old Items
            foreach (ItemGUI item in gridLayout.GetComponentsInChildren<ItemGUI>())
            {
                Destroy(item.gameObject);
            }

            // Fill gridLayout with new Items
            foreach (Item i in selectedUnit.Unit.Inventory.Items)
            {
                GameObject itemObject = Instantiate(itemPrefab.gameObject);
                itemObject.transform.SetParent(gridLayout.transform);
                itemObject.GetComponent<ItemGUI>().SetItem(i, tooltipPanel, gameObject, selectedUnit, itemActionMenu, itemActionButtonPrefab);
            }
        }
    }
}