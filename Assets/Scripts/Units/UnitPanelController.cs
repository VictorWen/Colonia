using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Units.Abilities;
using Items;

namespace Units
{
    [ExecuteInEditMode]
    public class UnitPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject actionPanel;
        [SerializeField] private Button moveButton;
        [SerializeField] private Button attackButton;
        [SerializeField] private Button abilityButton;
        [SerializeField] private Button itemsButton;

        [SerializeField] private GameObject infoPanel;

        [SerializeField] private AbilityMenuScript abilityMenu;

        [SerializeReference] private HeroInventoryPanel inventoryPanel;

        [SerializeReference] private UnitPanelGraphics graphics;
        [SerializeReference] private UnitInfoGraphics infoGraphics;

        public UnitEntityPlayerController SelectedUnit { get; private set; }

        private void Awake()
        {
            graphics = new UnitPanelGraphics(actionPanel, moveButton, attackButton, abilityButton, itemsButton);
            infoGraphics = new UnitInfoGraphics(infoPanel);

            moveButton.onClick.AddListener(MoveAction);
            attackButton.onClick.AddListener(AttackAction);
            abilityButton.onClick.AddListener(ShowAbilityMenu);
            itemsButton.onClick.AddListener(ShowInventory);
        }

        public void OnNextTurn()
        {
            if (SelectedUnit != null)
            {
                graphics.UpdateUnitPanel();
                graphics.UpdateActionButtons();
            }
        }

        public void SetSelectedUnit(UnitEntityPlayerController newSelectedUnit)
        {
            if (newSelectedUnit != null)
            {
                if (SelectedUnit != null)
                {
                    SelectedUnit.Deselect();
                    graphics.OnDeselect();
                }
                SelectedUnit = newSelectedUnit;
                graphics.OnSelect(newSelectedUnit.Unit);
            }
            else
            {
                SelectedUnit = null;
                graphics.OnDeselect();
            }
        }

        public void ShowAbilityMenu()
        {
            abilityMenu.Enable(SelectedUnit);
        }

        public void ShowInventory()
        {
            inventoryPanel.Enable(SelectedUnit);
        }

        public void ShowUnitInfo(UnitEntity unit)
        {
            infoGraphics.SetSelectedUnit(unit);
            infoGraphics.ShowUnitInfoPanel();
        }

        public void HideUnitInfo()
        {
            infoGraphics.HideUnitInfoPanel();
        }

        public void MoveAction()
        {
            SelectedUnit.MoveAction();
        }

        public void AttackAction()
        {
            SelectedUnit.AttackAction();
        }
    }
}