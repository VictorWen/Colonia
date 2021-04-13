using System;
using System.Collections.Generic;
using UnityEngine;

namespace Units
{
    public class UnitEntityController : MonoBehaviour
    {
        [SerializeField] protected GUIMaster gui;
        [SerializeField] protected World world;
        [SerializeField] protected UnitEntity unitEntity;

        public UnitEntity Unit { get { return unitEntity; } }

        private bool allowHovering = true;
        private bool hovering = false;

        protected virtual void Awake()
        {
            Vector3Int gridPos = world.grid.WorldToCell(transform.position);
            transform.position = world.grid.CellToWorld(gridPos);

            // TODO: Placeholder unitEntity, should be constructed in the model
            unitEntity = new UnitEntity(name, gridPos, 100, 4, world, false, 3);
            unitEntity.OnMove += UpdateUnitPosition;
            world.UnitManager.AddUnit(unitEntity);

            unitEntity.OnDeath += OnDeath;
        }

        protected virtual void OnDeath()
        {
            world.UnitManager.RemoveUnit(unitEntity);
            Destroy(gameObject);
        }

        private void UpdateUnitPosition()
        {
            transform.position = world.grid.CellToWorld(unitEntity.Position);
        }

        private void OnMouseEnter()
        {
            if (gui.GUIState.UnitControl && allowHovering)
            {
                HoverInfo(true);
            }
        }

        private void OnMouseExit()
        {
            if (hovering && allowHovering)
            {
                HoverInfo(false);
            }
        }

        protected void AllowHovering(bool enable)
        {
            allowHovering = enable;
            if (!enable && hovering)
            {
                HoverInfo(false);
            }
        }

        public void HoverInfo(bool show)
        {
            hovering = show;
            if (show)
                gui.unitPanel.ShowUnitInfo(unitEntity);
            else
                gui.unitPanel.HideUnitInfo();
        }
    }
}
