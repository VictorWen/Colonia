using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Units.Abilities;
using Items;

namespace Units
{
    public class UnitPanelScript : MonoBehaviour
    {
        public GameObject unitPanel;
        private Text namePlate;
        private Text statusText;

        public GameObject unitInfoPanel;
        private Text infoNamePlate;
        private Text infoStatusText;

        private UnitEntity hoveredUnit;

        public AbilityMenuScript abilityMenu;

        public HeroInventoryPanel heroInventory;

        public Button moveButton;
        public Button attackButton;
        public Button abilityButton;
        public Button itemsButton;

        private UnitEntityGUI selectedUnit;

        private void Awake()
        {
            foreach (Text t in unitPanel.GetComponentsInChildren<Text>())
            {
                if (t.name == "Name Plate")
                    namePlate = t;
                else if (t.name == "Status Text")
                    statusText = t;
            }

            foreach (Text t in unitInfoPanel.GetComponentsInChildren<Text>())
            {
                if (t.name == "Name Plate")
                    infoNamePlate = t;
                if (t.name == "Status Text")
                    infoStatusText = t;
            }
        }

        //TODO: reorganize unitPanelScript
        public void SetSelectedUnit(UnitEntityGUI selected)
        {
            selectedUnit = selected;
            moveButton.onClick.RemoveAllListeners();

            if (selected != null)
            {
                unitPanel.SetActive(true);
                HideUnitInfo();
                namePlate.text = selected.Unit.Name;
                UpdateGUI();
            }
            else
            {
                unitPanel.SetActive(false);
            }
        }

        public void ShowUnitInfo(UnitEntity unit)
        {
            if (selectedUnit == null || selectedUnit.Unit != unit)
            {
                hoveredUnit = unit;
                unitInfoPanel.SetActive(true);
                infoNamePlate.text = unit.Name;
                infoStatusText.text = unit.GetStatusDescription();
            }
        }

        public void HideUnitInfo()
        {
            unitInfoPanel.SetActive(false);
        }

        public void ShowAbilityMenu(UnitEntityGUI unit)
        {
            abilityMenu.Enable(unit);
        }

        public void HideAbilityMenu()
        {
            abilityMenu.gameObject.SetActive(false);
        }

        public void UpdateGUI()
        {
            moveButton.onClick.RemoveAllListeners();
            attackButton.onClick.RemoveAllListeners();
            abilityButton.onClick.RemoveAllListeners();
            itemsButton.onClick.RemoveAllListeners();
            if (selectedUnit != null)
            {
                statusText.text = selectedUnit.Unit.GetStatusDescription();
                moveButton.interactable = selectedUnit.Unit.CanMove;
                attackButton.interactable = selectedUnit.Unit.CanAttack;
                abilityButton.interactable = selectedUnit.Unit.CanAttack;
                itemsButton.interactable = selectedUnit.Unit.CanAttack;
                if (selectedUnit.Unit.CanMove)
                {
                    moveButton.onClick.AddListener(selectedUnit.MoveAction);
                }
                if (selectedUnit.Unit.CanAttack)
                {
                    attackButton.onClick.AddListener(selectedUnit.AttackAction);
                    abilityButton.onClick.AddListener(() => abilityMenu.Enable(selectedUnit));
                }
                itemsButton.onClick.AddListener(() => selectedUnit.ShowInventory());
            }
            
            if (unitInfoPanel.activeInHierarchy)
            {
                infoNamePlate.text = hoveredUnit.Name;
                infoStatusText.text = hoveredUnit.GetStatusDescription();
            }
        }
    }
}