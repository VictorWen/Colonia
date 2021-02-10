using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Abilities;

namespace Units.Intelligence
{
    /// <summary>
    /// AI for calculating how the UnitEntity should move to a target location
    /// </summary>
    public abstract class NPCMovementAI : MonoBehaviour
    {
        public abstract LinkedList<Vector3Int> GetAbilityMovement(UnitEntity self, Ability ability, Vector3Int target, World world);

        public abstract LinkedList<Vector3Int> GetRetreatMovement(UnitEntity self, Vector3Int target, World world);

        public abstract LinkedList<Vector3Int> GetWanderMovement(UnitEntity self, Vector3Int target, World world);
    }
}