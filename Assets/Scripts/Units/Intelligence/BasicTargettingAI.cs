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
        public Vector3Int GetAbilityTarget(UnitEntity self, Ability ability, World world)
        {
            // Find all visible enemey UnitEntities
            List<UnitEntity> targets = new List<UnitEntity>();
            foreach (Vector3Int visible in self.VisibleTiles)
            {
                if (world.UnitManager.Positions.ContainsKey(visible) && self.IsEnemy(world.UnitManager.Positions[visible]))
                {
                    targets.Add(world.UnitManager.Positions[visible]);
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
            return minDistance;
        }

        public Vector3Int GetManeuverTarget(UnitEntity self, Ability ability, Vector3Int abilityTarget, World world)
        {
            BFSPathfinder bfs = new BFSPathfinder(self.Position, self.MovementSpeed, world, true);
            Vector3Int minDistance = abilityTarget;
            foreach (Vector3Int withinRange in ability.GetReachingTiles(self, abilityTarget, world))
            {
                if (bfs.Reachables.Contains(withinRange) && !withinRange.Equals(abilityTarget) &&
                    (minDistance.Equals(abilityTarget) || Vector3Int.Distance(self.Position, withinRange) < Vector3Int.Distance(self.Position, minDistance)))
                {
                    minDistance = withinRange;
                }
            }

            return minDistance;
        }

        public Vector3Int GetRetreatTarget(UnitEntity self, World world)
        {
            throw new NotImplementedException();
        }

        public Vector3Int GetWanderTarget(UnitEntity self, World world)
        {
            throw new NotImplementedException();
        }
    }
}
