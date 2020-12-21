using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        public Button moveButton;
        public Button attackButton;
        public Button abilityButton;

        private UnitEntityScript selectedUnit;

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
        public void SetSelectedUnit(UnitEntityScript selected)
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
                unitInfoPanel.SetActive(true);
                infoNamePlate.text = unit.Name;
                infoStatusText.text = unit.GetStatusDescription();
            }
        }

        public void HideUnitInfo()
        {
            unitInfoPanel.SetActive(false);
        }

        public void UpdateGUI()
        {
            moveButton.onClick.RemoveAllListeners();
            if (selectedUnit != null)
            {
                statusText.text = selectedUnit.Unit.GetStatusDescription();
                if (selectedUnit.Unit.CanMove)
                {
                    moveButton.interactable = true;
                    moveButton.onClick.AddListener(selectedUnit.MoveAction);
                }
                if (selectedUnit.Unit.CanAttack)
                {
                    attackButton.interactable = true;
                    attackButton.onClick.AddListener(selectedUnit.AttackAction);
                    abilityButton.onClick.AddListener(selectedUnit.AbilityAction);
                }
            }
        }
    }
}