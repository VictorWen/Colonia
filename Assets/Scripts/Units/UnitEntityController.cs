using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    public class UnitEntityController : MonoBehaviour
    {
        [SerializeField] protected GUIMaster gui;
        [SerializeField] protected World world;
        [SerializeField] protected BaseUnitEntity unitEntity;

        private bool hovering = false;

        protected virtual void Awake()
        {
            Vector3Int gridPos = world.grid.WorldToCell(transform.position);
            transform.position = world.grid.CellToWorld(gridPos);

            // TODO: Placeholder unitEntity, should be constructed in the model
            unitEntity = new BaseUnitEntity(name, gridPos, 100, 4, world, false, 3);
            world.UnitManager.AddUnit(unitEntity);
        }

        private void OnMouseEnter()
        {
            //TODO: fix second unit panel call
            if (gui.GUIState.UnitControl)
            {
                gui.unitPanel.ShowUnitInfo(unitEntity);
                hovering = true;
            }
        }

        private void OnMouseExit()
        {
            if (hovering)
            {
                gui.unitPanel.HideUnitInfo();
                hovering = false;
            }
        }
    }
}
