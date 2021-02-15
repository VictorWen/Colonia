using System.Collections.Generic;
using UnityEngine;
using Units.Movement;

namespace Units.Combat
{
    public class UnitEntityCombatGraphics
    {
        private World world;
        private UnitEntityCombat combat;
        private UnitEntityMovement movement;
        private UnitEntityConfig config;

        private HashSet<Vector3Int> shownAttackables;

        public UnitEntityCombatGraphics(World world, UnitEntityCombat combat, UnitEntityMovement movement, UnitEntityConfig config)
        {
            this.world = world;
            this.combat = combat;
            this.movement = movement;
            this.config = config;

            combat.OnAttack += OnAttack;

            shownAttackables = new HashSet<Vector3Int>();
        }

        public void OnAttack()
        {
            ClearAttackables();
        }

        public void ShowAttackables(HashSet<Vector3Int> attackables)
        {
            ClearAttackables();
            foreach (Vector3Int attackable in attackables)
            {
                world.movement.SetTile(attackable, config.attackTile);
            }
            shownAttackables = new HashSet<Vector3Int>(attackables);
        }

        public void ClearAttackables()
        {
            foreach (Vector3Int tilePos in shownAttackables)
            {
                world.movement.SetTile(tilePos, null);
            }
            shownAttackables = new HashSet<Vector3Int>();
        }
    }
}
