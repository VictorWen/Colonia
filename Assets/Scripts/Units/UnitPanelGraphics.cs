using Units.Combat;
using Units.Movement;
using UnityEngine;
using UnityEngine.UI;

namespace Units
{
    public class UnitPanelGraphics
    {
        private readonly GameObject panel;
        private readonly Text namePlate;
        private readonly Text statusText;

        // Action buttons
        private readonly Button moveButton;
        private readonly Button attackButton;
        private readonly Button abilityButton;

        private UnitEntity selectedUnit;

        public UnitPanelGraphics(GameObject panel, Button moveButton, Button attackButton, Button abilityButton)
        { 
            selectedUnit = null;

            this.panel = panel;
            this.moveButton = moveButton;
            this.attackButton = attackButton;
            this.abilityButton = abilityButton;

            foreach (Text t in panel.GetComponentsInChildren<Text>())
            {
                if (t.name == "Name Plate")
                    namePlate = t;
                else if (t.name == "Status Text")
                    statusText = t;
            }
        }

        public void OnSelect(UnitEntity unit)
        {
            SetSelectedUnit(unit);
            ShowUnitPanel();
        }

        public void OnDeselect()
        {
            RemoveCallbacks();
            HideUnitPanel();
        }

        public void SetSelectedUnit(UnitEntity unit)
        {
            // Remove previous callbacks
            RemoveCallbacks();

            // Set local information
            selectedUnit = unit;

            // Add new callbacks
            selectedUnit.OnMove += UpdateActionButtons;
            selectedUnit.Combat.OnAttack += UpdateActionButtons;

            // Update graphics
            UpdateUnitPanel();
            UpdateActionButtons();
        }

        public void RemoveCallbacks()
        {
            if (selectedUnit != null)
            {
                selectedUnit.OnMove -= UpdateActionButtons;
                selectedUnit.Combat.OnAttack -= UpdateActionButtons;
            }
        }

        public void UpdateUnitPanel()
        {
            namePlate.text = selectedUnit.Name;
            statusText.text = selectedUnit.GetStatus();
        }

        public void UpdateActionButtons()
        {
            moveButton.interactable = selectedUnit.Movement.CanMove;
            attackButton.interactable = selectedUnit.Combat.CanAttack;
            abilityButton.interactable = selectedUnit.Combat.CanAttack;
        }

        public void ShowUnitPanel()
        {
            panel.SetActive(true);
        }

        public void HideUnitPanel()
        {
            panel.SetActive(false);
        }
    }
}