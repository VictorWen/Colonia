using System;
using System.Collections.Generic;
using UnityEngine;
using Units.Combat;

namespace Units
{
    public class UnitEntityController : MonoBehaviour
    {
        [SerializeField] protected GUIMaster gui;
        [SerializeField] protected World world;
        [SerializeField] protected UnitEntityData unitData;

        [SerializeReference] protected UnitEntity unitEntity;

        public UnitEntity Unit { get { return unitEntity; } }

        private bool allowHovering = true;
        private bool hovering = false;

        protected virtual void Awake()
        {
            CreateUnitEntity(false);
            unitEntity.OnMove += UpdateUnitPosition;
        }

        protected virtual void CreateUnitEntity(bool isPlayerControlled)
        {
            Vector3Int gridPos = world.grid.WorldToCell(transform.position);
            transform.position = world.grid.CellToWorld(gridPos);

            UnitEntityCombatData combatData = new UnitEntityCombatData()
            {
                maxMana = unitData.maxMana,
                attack = unitData.attack,
                defence = unitData.defence,
                piercing = unitData.piercing,
                magic = unitData.magic,
                resistance = unitData.resistance
            };

            unitEntity = new UnitEntity(this.name, gridPos, unitData.maxHealth, unitData.sight, world, isPlayerControlled, unitData.movementSpeed, combatData);

            //world.UnitManager.AddUnit(unitEntity);
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
