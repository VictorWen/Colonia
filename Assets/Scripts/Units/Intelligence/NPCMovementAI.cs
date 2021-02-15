using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Abilities;
using Units.Movement;

namespace Units.Intelligence
{
    /// <summary>
    /// AI for calculating how the UnitEntity should move to a target location
    /// </summary>
    public abstract class NPCMovementAI
    {
        public abstract LinkedList<Vector3Int> GetAbilityMovement(UnitEntityMovement self, Ability ability, Vector3Int target, World world);

        public abstract LinkedList<Vector3Int> GetRetreatMovement(UnitEntityMovement self, Vector3Int target, World world);

        public abstract LinkedList<Vector3Int> GetWanderMovement(UnitEntityMovement self, Vector3Int target, World world);
    }
}