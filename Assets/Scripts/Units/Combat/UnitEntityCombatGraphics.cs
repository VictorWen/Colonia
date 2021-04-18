using System.Collections.Generic;
using UnityEngine;
using Units.Movement;

namespace Units.Combat
{
    public class UnitEntityCombatGraphics
    {
        private readonly World world;
        private readonly IUnitEntityCombat combat;
        private readonly UnitEntityConfig config;

        public HashSet<Vector3Int> ShownAttackables { get; private set; }

        public UnitEntityCombatGraphics(UnitEntityGraphics graphics)
        {
            this.world = graphics.World;
            this.combat = graphics.Unit.Combat;
            this.config = graphics.Config;

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
                world.SetMovementTile(attackable, config.attackTile);
            }
            ShownAttackables = new HashSet<Vector3Int>(attackables);
        }

        public void ClearAttackables()
        {
            foreach (Vector3Int tilePos in ShownAttackables)
            {
                world.SetMovementTile(tilePos, null);
            }
            ShownAttackables = new HashSet<Vector3Int>();
        }
    }
}
