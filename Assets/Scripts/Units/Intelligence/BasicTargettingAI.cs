using System;
using System.Collections.Generic;
using Units.Abilities;
using UnityEngine;

namespace Units.Intelligence
{
    public class BasicTargettingAI : NPCTargetingAI
    {   
        /// <summary>
        /// Assumes that the Ability is a damage dealing ability, though this is not necessarily true.
        /// </summary>
        public override Vector3Int GetAbilityTarget(NPCUnitEntityAI self, Ability ability, World world)
        {
            return self.Position;
/*            // Find all visible enemey UnitEntities
            List<UnitEntity> targets = new List<UnitEntity>();
            foreach (Vector3Int visible in self.VisibleTiles)
            {
                UnitEntity unitAt = world.GetUnitAt(visible);
                if (unitAt != null && self.IsEnemy(unitAt))
                {
                    targets.Add(unitAt);
                }
            }

            // Target candidate analysis, determine best UnitEntity to target
            // Find the closest UnitEntity
            Vector3Int minDistance = self.Position;
            foreach (UnitEntity entity in targets)
            {
                if (minDistance.Equals(self.Position) || Vector3Int.Distance(self.Position, entity.Position) < Vector3Int.Distance(self.Position, minDistance))
                {
                    minDistance = entity.Position;
                }
            }
            return minDistance;*/
        }

        public override Vector3Int GetManeuverTarget(NPCUnitEntityAI self, Ability ability, Vector3Int abilityTarget, World world)
        {
            return self.Position;
/*            BFSPathfinder bfs = new BFSPathfinder(self.Position, self.MovementSpeed, world, true);
            Vector3Int minDistance = abilityTarget;
            foreach (Vector3Int withinRange in ability.GetReachingTiles(self, abilityTarget, world))
            {
                if (bfs.Reachables.Contains(withinRange) && !withinRange.Equals(abilityTarget) &&
                    (minDistance.Equals(abilityTarget) || Vector3Int.Distance(self.Position, withinRange) < Vector3Int.Distance(self.Position, minDistance)))
                {
                    minDistance = withinRange;
                }
            }

            return minDistance;*/
        }

        public override Vector3Int GetRetreatTarget(NPCUnitEntityAI self, World world)
        {
            BFSPathfinder bfs = self.Movement.GetMoveables();
            int random = world.RNG.Next(bfs.Reachables.Count);
            int index = 0;
            foreach (Vector3Int tile in bfs.Reachables)
            {
                if (index == random)
                    return tile;
                index++;
            }
            return self.Position;
        }

        public override Vector3Int GetWanderTarget(NPCUnitEntityAI self, World world)
        {
            BFSPathfinder bfs = self.Movement.GetMoveables();
            int random = world.RNG.Next(bfs.Reachables.Count);
            int index = 0;
            foreach (Vector3Int tile in bfs.Reachables)
            {
                if (index == random)
                    return tile;
                index++;
            }
            return self.Position;
        }
    }
}
