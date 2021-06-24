using UnityEngine;
using UnityEngine.UI;
using Units;
using System.Collections.Generic;
using Units.Combat;
using Items.EquipmentItems;

namespace Items
{
    public class HeroInventoryPanel : MonoBehaviour
    {
        public GridLayoutGroup gridLayout;
        public ItemGUI itemPrefab;
        public GameObject tooltipPanel;

        public GameObject headPanel;
        public GameObject bodyPanel;
        public GameObject bootsPanel;
        public GameObject weapon1Panel;
        public GameObject weapon2Panel;
        public GameObject artifactPanel;
        public GridLayoutGroup equipmentGrid;

        public VerticalLayoutGroup itemActionMenu;
        public Button itemActionButtonPrefab;

        private PlayerUnitEntityController selectedUnit;
        private GameObject[] equipmentPanels;

        public void Enable(PlayerUnitEntityController unit)
        {
            selectedUnit = unit;
            OpenInventory();
            gameObject.SetActive(true);
            equipmentPanels = new GameObject[]
            {
                headPanel,
                bodyPanel,
                bootsPanel,
                weapon1Panel,
                weapon2Panel,
                artifactPanel,
            };
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
            //selectedUnit.Unit.Inventory.RemoveZeroCountItems();
            foreach (Item i in selectedUnit.Unit.Inventory.Items)
            {
                CreateItemGUI(i, gridLayout.transform);
            }

            DisplayEquipment();
        }

        public void DisplayEquipment()
        {
            foreach (ItemGUI item in equipmentGrid.GetComponentsInChildren<ItemGUI>())
            {
                Destroy(item.gameObject);
            }

            foreach (KeyValuePair<UnitEntityEquipmentSlotID, EquipmentItem> pair in selectedUnit.Unit.Combat.Equipment)
            {
                switch (pair.Key)
                {
                    case UnitEntityEquipmentSlotID.HEAD:
                        CreateItemGUI(pair.Value, headPanel.transform);
                        break;
                    case UnitEntityEquipmentSlotID.BODY:
                        CreateItemGUI(pair.Value, bodyPanel.transform);
                        break;
                    case UnitEntityEquipmentSlotID.BOOTS:
                        CreateItemGUI(pair.Value, bootsPanel.transform);
                        break;
                    case UnitEntityEquipmentSlotID.WEAPON1:
                        CreateItemGUI(pair.Value, weapon1Panel.transform);
                        break;
                    case UnitEntityEquipmentSlotID.WEAPON2:
                        CreateItemGUI(pair.Value, weapon2Panel.transform);
                        break;
                    case UnitEntityEquipmentSlotID.ARTIFACT:
                        CreateItemGUI(pair.Value, artifactPanel.transform);
                        break;
                }
            }
        }

        private void CreateItemGUI(Item item, Transform parentObject)
        {
            if (item == null)
                return;
            GameObject itemObject = Instantiate(itemPrefab.gameObject);
            itemObject.transform.SetParent(parentObject, false);
            itemObject.GetComponent<ItemGUI>().SetItem(item, tooltipPanel, gameObject, selectedUnit, itemActionMenu, itemActionButtonPrefab);
        }
    }
}