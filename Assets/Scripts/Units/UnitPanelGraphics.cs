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

        private UnitPanel unitPanel;
        private TempUnitEntity selectedUnit;

        public UnitPanelGraphics(UnitPanel unitPanel, GameObject panel, Button moveButton, Button attackButton)
        {
            this.unitPanel = unitPanel;
            unitPanel.OnSelect += SetSelectedUnit;
            unitPanel.OnSelect += ShowUnitPanel;
            unitPanel.OnDeselect += RemoveCallbacks;
            unitPanel.OnDeselect += HideUnitPanel;
            selectedUnit = null;

            this.panel = panel;
            this.moveButton = moveButton;
            this.attackButton = attackButton;

            foreach (Text t in panel.GetComponentsInChildren<Text>())
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
            panel.SetActive(true);
        }

        public void HideUnitPanel()
        {
            panel.SetActive(false);
        }

        public void SetSelectedUnit()
        {
            // Remove previous callbacks
            RemoveCallbacks();

            // Set local information
            selectedUnit = unitPanel.SelectedUnit;

            // Add new callbacks
            selectedUnit.Movement.OnMove += UpdateActionButtons;
            selectedUnit.Combat.OnAttack += UpdateActionButtons;

            // Update graphics
            UpdateUnitPanel();
            UpdateActionButtons();
        }

        public void RemoveCallbacks()
        {
            if (selectedUnit != null)
            {
                selectedUnit.Movement.OnMove -= UpdateActionButtons;
                selectedUnit.Combat.OnAttack -= UpdateActionButtons;
            }
        }

        public void UpdateUnitPanel()
        {
            namePlate.text = selectedUnit.Name;
            statusText.text = selectedUnit.Combat.GetStatusDescription();
        }

        public void UpdateActionButtons()
        {
            moveButton.interactable = selectedUnit.Movement.CanMove;
            attackButton.interactable = selectedUnit.Combat.CanAttack;
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