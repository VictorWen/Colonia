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
        [SerializeField] private GameObject actionPanel = null;
        [SerializeField] private Button moveButton = null;
        [SerializeField] private Button attackButton = null;
        [SerializeField] private Button abilityButton = null;
        [SerializeField] private Button restButton = null;
        [SerializeField] private Button itemsButton = null;

        [SerializeField] private GameObject infoPanel = null;

        [SerializeField] private AbilityMenuScript abilityMenu = null;

        [SerializeReference] private HeroInventoryPanel inventoryPanel = null;

        [SerializeReference] private UnitPanelGraphics graphics = null;
        [SerializeReference] private UnitInfoGraphics infoGraphics = null;

        public PlayerUnitEntityController SelectedUnit { get; private set; }

        private void Awake()
        {
            graphics = new UnitPanelGraphics(actionPanel, moveButton, attackButton, abilityButton, restButton, itemsButton);
            infoGraphics = new UnitInfoGraphics(infoPanel);

            moveButton.onClick.AddListener(MoveAction);
            attackButton.onClick.AddListener(AttackAction);
            abilityButton.onClick.AddListener(ShowAbilityMenu);
            restButton.onClick.AddListener(Rest);
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

        public void SetSelectedUnit(PlayerUnitEntityController newSelectedUnit)
        {
            if (newSelectedUnit != null)
            {
                if (SelectedUnit != null)
                {
                    SelectedUnit.Deselect();
                    graphics.OnDeselect();
                }
                SelectedUnit = newSelectedUnit;
                graphics.OnSelect(newSelectedUnit);
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

        public void Rest()
        {
            SelectedUnit.Unit.Combat.Rest();
        }

        public void ShowUnitInfo(BaseUnitEntityController unit)
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