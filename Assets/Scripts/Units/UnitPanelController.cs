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

        [SerializeReference] private UnitPanel unitPanel = new UnitPanel();
        [SerializeReference] private UnitPanelGraphics graphics;

        public UnitPanel UnitPanel { get { return unitPanel; } }

        private void Awake()
        {
            graphics = new UnitPanelGraphics(unitPanel, actionPanel, moveButton, attackButton);

            moveButton.onClick.AddListener(unitPanel.MoveAction);
            attackButton.onClick.AddListener(unitPanel.AttackAction);
        }

        public void OnNextTurn()
        {
            if (unitPanel.SelectedUnit != null)
            {
                graphics.UpdateUnitPanel();
                graphics.UpdateActionButtons();
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