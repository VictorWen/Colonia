﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Units.Combat;
using UnityEngine.EventSystems;

namespace Units
{
    public class BaseUnitEntityController : MonoBehaviour
    {
        [SerializeField] protected GUIMaster gui;
        [SerializeField] protected World world;
        [SerializeField] string unitID;

        [SerializeReference] protected UnitEntity unitEntity;

        public UnitEntity Unit { get { return unitEntity; } }

        private bool allowHovering = true;
        private bool hovering = false;

        protected virtual void Start()
        {
            if (unitEntity == null)
                CreateUnitEntity(false);
            unitEntity.OnMove += UpdateUnitPosition;
            unitEntity.OnDeath += OnDeath;
            unitEntity.UpdateVision();
        }

        public void Initialize(string id, Vector3Int position, GUIMaster gui, World world, UnitEntity unitEntity)
        {
            unitID = id;
            Sprite sprite = Resources.Load<Sprite>("Unit Entity Sprites/" + id);
            GetComponent<SpriteRenderer>().sprite = sprite;

            transform.position = world.CellToWorld(position);
            this.gui = gui;
            this.world = world;
            this.unitEntity = unitEntity;
            world.AddUnitEntityController(unitEntity, this);
        }

        protected virtual void CreateUnitEntity(bool isPlayerControlled)
        {
            Vector3Int gridPos = world.WorldToCell(transform.position);
            transform.position = world.CellToWorld(gridPos);

            UnitEntityData unitData = GlobalUnitEntityDictionary.GetUnitEntityData("Unit Entities", unitID);
            UnitEntityCombatData combatData = UnitEntityCombatData.LoadFromSO(unitData);

            unitEntity = new UnitEntity(this.name, gridPos, unitData.maxHealth, unitData.sight, isPlayerControlled, unitData.movementSpeed, world, combatData);

            Sprite sprite = Resources.Load<Sprite>("Unit Entity Sprites/" + unitID);
            GetComponent<SpriteRenderer>().sprite = sprite;

            world.AddUnitEntityController(unitEntity, this);
        }

        protected virtual void OnDeath()
        {
            if (hovering)
            {
                HoverInfo(false);
            }
            Destroy(gameObject);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void UpdateUnitPosition()
        {
            transform.position = world.CellToWorld(unitEntity.Position);
        }

        private void OnMouseEnter()
        {
            if (gui.GUIState.MapHUD && allowHovering && !EventSystem.current.IsPointerOverGameObject())
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
