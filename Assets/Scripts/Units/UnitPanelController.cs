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

        [SerializeReference] private UnitPanelGraphics graphics;

        public UnitEntityController SelectedUnit { get; private set; }

        private void Awake()
        {
            graphics = new UnitPanelGraphics(actionPanel, moveButton, attackButton);

            moveButton.onClick.AddListener(MoveAction);
            attackButton.onClick.AddListener(AttackAction);
        }

        public void OnNextTurn()
        {
            if (SelectedUnit != null)
            {
                graphics.UpdateUnitPanel();
                graphics.UpdateActionButtons();
            }
        }

        public void SetSelectedUnit(UnitEntityController newSelectedUnit)
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