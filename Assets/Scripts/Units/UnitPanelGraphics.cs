﻿using Units.Combat;
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
        private readonly Image image;

        // Action buttons
        private readonly Button moveButton;
        private readonly Button attackButton;
        private readonly Button abilityButton;
        private readonly Button itemsButton;

        private UnitEntity selectedUnit;
        private Sprite unitSprite;

        public UnitPanelGraphics(GameObject panel, Button moveButton, Button attackButton, Button abilityButton, Button itemsButton)
        { 
            selectedUnit = null;

            this.panel = panel;
            this.moveButton = moveButton;
            this.attackButton = attackButton;
            this.abilityButton = abilityButton;
            this.itemsButton = itemsButton;

            foreach (Text t in panel.GetComponentsInChildren<Text>())
            {
                if (t.name == "Name Plate")
                    namePlate = t;
                else if (t.name == "Status Text")
                    statusText = t;
            }

            foreach (Image img in panel.GetComponentsInChildren<Image>())
            {
                if (img.name == "Image")
                    image = img;
            }
        }

        public void OnSelect(PlayerUnitEntityController unit)
        {
            SetSelectedUnit(unit);
            ShowUnitPanel();
        }

        public void OnDeselect()
        {
            RemoveCallbacks();
            HideUnitPanel();
        }

        public void SetSelectedUnit(PlayerUnitEntityController ctrl)
        {
            UnitEntity unit = ctrl.Unit;
            RemoveCallbacks();
            selectedUnit = unit;
            unitSprite = ctrl.GetComponent<SpriteRenderer>().sprite;

            // Add new callbacks
            selectedUnit.OnMove += UpdateActionButtons;
            selectedUnit.Combat.OnAttack += UpdateActionButtons;
            selectedUnit.OnStatusChanged += UpdateUnitPanel;

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
                selectedUnit.OnStatusChanged -= UpdateUnitPanel;
            }
        }

        public void UpdateUnitPanel()
        {
            namePlate.text = selectedUnit.Name;
            statusText.text = selectedUnit.GetStatus();
            image.sprite = unitSprite;
        }

        public void UpdateActionButtons()
        {
            moveButton.interactable = selectedUnit.Movement.CanMove;
            attackButton.interactable = selectedUnit.Combat.CanAttack;
            abilityButton.interactable = selectedUnit.Combat.CanAttack;
            itemsButton.interactable = selectedUnit.Combat.CanAttack;
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