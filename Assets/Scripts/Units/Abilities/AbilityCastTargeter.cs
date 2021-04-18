using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Units.Abilities
{
    public class AbilityCastTargeter
    {
        public bool IsActive { get; private set; }

        private readonly Ability ability;
        private readonly GUIStateManager gui;
        private readonly World world;
        private readonly UnitEntityPlayerController unit;
        private readonly UnitEntityConfig config;

        private readonly HashSet<Vector3Int> range;
        private HashSet<Vector3Int> area;

        public AbilityCastTargeter(Ability ability, GUIStateManager gui, World world, UnitEntityPlayerController unit, UnitEntityConfig config)
        {
            this.ability = ability;
            this.gui = gui;
            this.world = world;
            this.unit = unit;
            this.config = config;

            range = ability.GetWithinRange(unit.Unit, world);
            area = new HashSet<Vector3Int>();
        }

        public IEnumerator TargetAndCastAbilityCoroutine()
        {
            IsActive = true;
            bool hasSelected = false;
            gui.SetState(GUIStateManager.ABILITY);
            while (IsActive && !hasSelected && unit.Status == UnitEntityPlayerController.WaitingStatus.ABILITY && !Input.GetMouseButtonUp(1))
            {
                hasSelected = CheckForValidClick();
                yield return null;
            }

            if (IsActive)
                Disable();
            yield break;
        }

        public void Disable()
        {
            ClearArea();
            world.SetMovementTile(unit.Unit.Position, config.selectTile);
            gui.SetState(GUIStateManager.UNIT);
            IsActive = false;
        }

        private bool CheckForValidClick()
        {
            ClearArea();
            Vector3Int gridPos = GetMousePosition();

            if (range.Contains(gridPos))
            {
                GetAndHighlightAOE(gridPos);

                if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    unit.Unit.Combat.CastAbility(ability, gridPos);
                    return true;
                }
            }
            return false;
        }

        private void ClearArea()
        {
            foreach (Vector3Int tile in area)
            {
                world.SetMovementTile(tile, null);
            }
        }

        private Vector3Int GetMousePosition()
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return world.grid.WorldToCell(worldPos);
        }

        private void GetAndHighlightAOE(Vector3Int gridPos)
        {
            area = ability.GetAreaOfEffect(unit.Unit.Position, gridPos, world);
            foreach (Vector3Int tile in area)
            {
                world.SetMovementTile(tile, config.attackTile);
            }
        }
    }
}
