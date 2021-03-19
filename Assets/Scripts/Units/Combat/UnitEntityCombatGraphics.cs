using System.Collections.Generic;
using UnityEngine;
using Units.Movement;

namespace Units.Combat
{
    public class UnitEntityCombatGraphics
    {
        private World world;
        private IUnitEntityCombat combat;
        private IUnitEntityMovement movement;
        private UnitEntityConfig config;

        public HashSet<Vector3Int> ShownAttackables { get; private set; }

        public UnitEntityCombatGraphics(World world, IUnitEntityCombat combat, IUnitEntityMovement movement, UnitEntityConfig config)
        {
            this.world = world;
            this.combat = combat;
            this.movement = movement;
            this.config = config;

            combat.OnAttack += OnAttack;

            ShownAttackables = new HashSet<Vector3Int>();
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
            ShownAttackables = new HashSet<Vector3Int>(attackables);
        }

        public void ClearAttackables()
        {
            foreach (Vector3Int tilePos in ShownAttackables)
            {
                world.movement.SetTile(tilePos, null);
            }
            ShownAttackables = new HashSet<Vector3Int>();
        }
    }
}
