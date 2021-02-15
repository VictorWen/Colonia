using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Units.Movement;
using Units.Combat;

namespace Units {
    public class UnitPanelGraphics
    {
        private readonly GameObject unitPanel;
        private readonly Text namePlate;
        private readonly Text statusText;

        /*    public GameObject unitInfoPanel;
            private Text infoNamePlate;
            private Text infoStatusText;*/

        /*    public AbilityMenuScript abilityMenu;

            public HeroInventoryPanel heroInventory;*/

        // Action buttons
        private readonly Button moveButton;
        private readonly Button attackButton;
/*        public Button abilityButton;
        public Button itemsButton;*/

        private string name;
        private UnitEntityCombat combat;
        private UnitEntityMovement movement;

        public UnitPanelGraphics(GameObject unitPanel, Button moveButton, Button attackButton)
        {
            this.unitPanel = unitPanel;
            this.moveButton = moveButton;
            this.attackButton = attackButton;

            foreach (Text t in unitPanel.GetComponentsInChildren<Text>())
            {
                if (t.name == "Name Plate")
                    namePlate = t;
                else if (t.name == "Status Text")
                    statusText = t;
            }
            /*
            foreach (Text t in unitInfoPanel.GetComponentsInChildren<Text>())
            {
                if (t.name == "Name Plate")
                    infoNamePlate = t;
                if (t.name == "Status Text")
                    infoStatusText = t;
            }*/
        }

        public void ShowUnitPanel()
        {
            unitPanel.SetActive(true);
        }

        public void HideUnitPanel()
        {
            unitPanel.SetActive(false);
        }

        public void SetSelectedUnit(string name, UnitEntityCombat combatInfo, UnitEntityMovement movementInfo)
        {
            // Remove previous callbacks
            if (movement != null)
                movement.OnMove -= UpdateActionButtons;
            if (combat != null)
                combat.OnAttack -= UpdateActionButtons;

            // Set local information
            this.name = name;
            this.combat = combatInfo;
            this.movement = movementInfo;

            // Add new callbacks
            movementInfo.OnMove += UpdateActionButtons;
            combatInfo.OnAttack += UpdateActionButtons;

            // Update graphics
            UpdateUnitPanel();
            UpdateActionButtons();
        }

        public void UpdateUnitPanel()
        {
            namePlate.text = name;
            statusText.text = combat.GetStatusDescription();
        }

        public void UpdateActionButtons()
        {
            moveButton.interactable = movement.CanMove;
            attackButton.interactable = combat.CanAttack;
        }
    

/*    public void HideUnitInfo()
    {
        unitInfoPanel.SetActive(false);
    }

    public void HideAbilityMenu()
    {
        abilityMenu.gameObject.SetActive(false);
    }*/

        /*    public void UpdateGUI()
            {
                // Reset Listeners
                moveButton.onClick.RemoveAllListeners();
                attackButton.onClick.RemoveAllListeners();
                abilityButton.onClick.RemoveAllListeners();
                itemsButton.onClick.RemoveAllListeners();

                if (selectedUnit != null)
                {
                    statusText.text = selectedUnit.Unit.GetStatusDescription();

                    // Toggle buttons
                    moveButton.interactable = selectedUnit.Unit.CanMove;
                    attackButton.interactable = selectedUnit.Unit.CanAttack;
                    abilityButton.interactable = selectedUnit.Unit.CanAttack;
                    itemsButton.interactable = selectedUnit.Unit.CanAttack;

                    // Add correct listeners
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

                // Update info
                if (unitInfoPanel.activeInHierarchy)
                {
                    infoNamePlate.text = hoveredUnit.Name;
                    infoStatusText.text = hoveredUnit.GetStatusDescription();
                }
            }*/
}
}