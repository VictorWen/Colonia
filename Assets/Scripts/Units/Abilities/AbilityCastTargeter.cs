using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Units.Abilities
{
    public class AbilityCastTargeter
    {
        private readonly Ability ability;
        private readonly World world;
        private readonly UnitEntity unit;
        private readonly UnitEntityConfig config;

        private readonly HashSet<Vector3Int> range;
        private HashSet<Vector3Int> area;

        public AbilityCastTargeter(Ability ability, World world, UnitEntity unit, UnitEntityConfig config)
        {
            this.ability = ability;
            this.world = world;
            this.unit = unit;
            this.config = config;

            range = ability.GetWithinRange(unit, world);
            area = new HashSet<Vector3Int>();
        }

        public IEnumerator TargetAndCastAbilityCoroutine()
        {
            bool hasSelected = false;
            while (!hasSelected)
            {
                if (Input.GetMouseButtonUp(1)) // Canceled if right clicked
                    break;

                hasSelected = CheckForValidClick();
                yield return null;
            }

            ClearArea();
            world.movement.SetTile(unit.Position, config.selectTile);
            yield break;
        }

        private bool CheckForValidClick()
        {
            ClearArea();
            Vector3Int gridPos = GetMousePosition();

            if (range.Contains(gridPos))
            {
                GetAndHighlightAOE(gridPos);

                if (Input.GetMouseButtonUp(0))
                {
                    unit.Combat.CastAbility(ability, gridPos);
                    return true;
                }
            }
            return false;
        }

        private void ClearArea()
        {
            foreach (Vector3Int tile in area)
            {
                world.movement.SetTile(tile, null);
            }
        }

        private Vector3Int GetMousePosition()
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return world.grid.WorldToCell(worldPos);
        }

        private void GetAndHighlightAOE(Vector3Int gridPos)
        {
            area = ability.GetAreaOfEffect(unit.Position, gridPos, world);
            foreach (Vector3Int tile in area)
            {
                world.movement.SetTile(tile, config.attackTile);
            }
        }
    }
}
