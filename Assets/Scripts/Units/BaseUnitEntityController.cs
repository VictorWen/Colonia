using System;
using System.Collections.Generic;
using UnityEngine;
using Units.Combat;

namespace Units
{
    public class BaseUnitEntityController : MonoBehaviour
    {
        [SerializeField] protected GUIMaster gui;
        [SerializeField] protected World world;
        [SerializeField] protected UnitEntityData unitData;

        [SerializeReference] protected UnitEntity unitEntity;

        public UnitEntity Unit { get { return unitEntity; } }

        private bool allowHovering = true;
        private bool hovering = false;

        protected virtual void Start()
        {
            if (unitEntity == null)
                CreateUnitEntity(false);
            unitEntity.OnMove += UpdateUnitPosition;
        }

        public void Initialize(Vector3Int position, GUIMaster gui, World world, UnitEntity unitEntity)
        {
            transform.position = world.CellToWorld(position);
            this.gui = gui;
            this.world = world;
            this.unitEntity = unitEntity;
        }

        protected virtual void CreateUnitEntity(bool isPlayerControlled)
        {
            Vector3Int gridPos = world.WorldToCell(transform.position);
            transform.position = world.CellToWorld(gridPos);

            UnitEntityCombatData combatData = new UnitEntityCombatData()
            {
                maxMana = unitData.maxMana,
                attack = unitData.attack,
                defence = unitData.defence,
                piercing = unitData.piercing,
                magic = unitData.magic,
                resistance = unitData.resistance
            };

            unitEntity = new UnitEntity(this.name, gridPos, unitData.maxHealth, unitData.sight, isPlayerControlled, unitData.movementSpeed, world, combatData);
            unitEntity.OnDeath += OnDeath;
        }

        protected virtual void OnDeath()
        {
            world.UnitManager.RemoveUnit(unitEntity);
            Destroy(gameObject);
        }

        private void UpdateUnitPosition()
        {
            transform.position = world.CellToWorld(unitEntity.Position);
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
                gui.unitPanel.ShowUnitInfo(this);
            else
                gui.unitPanel.HideUnitInfo();
        }
    }
}
