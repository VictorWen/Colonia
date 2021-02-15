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
        [SerializeField] private GameObject unitPanel;
        [SerializeField] private Button moveButton;
        [SerializeField] private Button attackButton;

        [SerializeReference] private UnitPanelGraphics graphics;

        private UnitEntityController selectedUnit;

        private void Awake()
        {
            graphics = new UnitPanelGraphics(unitPanel, moveButton, attackButton);
        }

        public void RemoveListeners()
        {
            moveButton.onClick.RemoveAllListeners();
        }

        public void OnNextTurn()
        {
            if (selectedUnit != null)
            {
                graphics.UpdateUnitPanel();
                graphics.UpdateActionButtons();
            }
        }

        public void SetSelectedUnit(UnitEntityController selected)
        {
            selectedUnit = selected;

            RemoveListeners();
            moveButton.onClick.AddListener(selected.MoveAction);
            attackButton.onClick.AddListener(selected.AttackAction);

            if (selected != null)
            {
                graphics.ShowUnitPanel();
                //HideUnitInfo();
                graphics.SetSelectedUnit(selected.name, selected.Combat, selected.Movement);
            }
            else
            {
                graphics.HideUnitPanel();
            }
        }

/*        public void ShowUnitInfo(UnitEntityController unit)
        {
            if (selectedUnit == null || selectedUnit.Unit != unit)
            {
                hoveredUnit = unit;
                unitInfoPanel.SetActive(true);
                infoNamePlate.text = unit.Name;
                infoStatusText.text = unit.GetStatusDescription();
            }
        }*/
    }
}