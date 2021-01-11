using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Units.Abilities;

namespace Units.Intelligence
{
    public class BasicAttackAbilityAI : NPCAbilityAI
    {
        public Ability GetNextAbility(NPCUnitEntity self, World world)
        {
            return new Ability("attack", "Basic Attack", 0, 1, false, new AbilityEffect[] { new DamageAbilityEffect(0, true) }, new HexAbilityAOE(1));
        }

        public UnitEntity GetAbilityTarget(UnitEntity self, Ability ability, World world)
        {
            throw new System.NotImplementedException();
/*            // Find all visible enemey UnitEntities
            List<UnitEntity> targets = new List<UnitEntity>();
            foreach (Vector3Int visible in self.VisibleTiles)
            {
                if (world.UnitManager.Positions.ContainsKey(visible) && world.UnitManager.Positions[visible].PlayerControlled)
                {
                    targets.Add(world.UnitManager.Positions[visible]);
                }
            }

            // Target candidate analysis, determine best UnitEntity to target
            // Find the closest UnitEntity
            UnitEntity minDistance = null;
            foreach (UnitEntity target in targets)
            {
                if (minDistance == null || Vector3Int.Distance(self.Position, target.Position) < Vector3Int.Distance(self.Position, minDistance.Position))
                {
                    minDistance = target;
                }
            }
            return minDistance;*/
        }
    }
}
