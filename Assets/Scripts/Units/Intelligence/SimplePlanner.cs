using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Units.Abilities;

namespace Units.Intelligence
{
    public class SimplePlanner : INPCPlanner
    {
        public LinkedList<Vector3Int> GetMovementPath(UnitEntity self, Dictionary<Vector3Int, float> posScores)
        {
            List<KeyValuePair<Vector3Int, float>> scores = posScores.ToList();
            scores.Sort(PositioningCompare);
            return self.Movement.GetMoveables().GetPathTo(scores[0].Key);
        }

        private int PositioningCompare(KeyValuePair<Vector3Int, float> pair1, KeyValuePair<Vector3Int, float> pair2)
        {
            if (pair1.Value > pair2.Value)
                return -1;
            else if (pair1.Value < pair2.Value)
                return 1;
            else
                return 0;
        }

        public void ExecuteAbility(UnitEntity self, World world)
        {
            List<string> abilities = self.Combat.Abilities;

            Dictionary<string, float> abilityScores = new Dictionary<string, float>();
            Dictionary<string, Vector3Int> targets = new Dictionary<string, Vector3Int>();

            foreach (string ability in abilities)
            {
                abilityScores.Add(ability, 0);
                targets.Add(ability, self.Position);
            }

            CalculateAbilityScores(self, world, abilityScores, targets);

            List<KeyValuePair<string, float>> scores = abilityScores.ToList();
            scores.Sort(CompareAbilityScore);
            if (scores[0].Value > 0)
            {
                string id = scores[0].Key;
                Debug.Log(self.Name + " casting " + id);
                Ability ability = GlobalAbilityDictionary.GetAbility(id);
                self.Combat.CastAbility(ability, targets[id]);
            }
        }

        private int CompareAbilityScore(KeyValuePair<string, float> pair1, KeyValuePair<string, float> pair2)
        {
            if (pair1.Value > pair2.Value)
                return -1;
            else if (pair1.Value < pair2.Value)
                return 1;
            else
                return 0;
        }

        private void CalculateAbilityScores(UnitEntity self, World world, Dictionary<string, float> scores, Dictionary<string, Vector3Int> targets)
        {
            foreach (Vector3Int tile in world.GetAdjacents(self.Position))
            {
                UnitEntity unit = world.UnitManager.GetUnitAt<UnitEntity>(tile);
                if (unit != null && unit.Combat.IsEnemy(self.Combat)){
                    scores["attack"] = 2;
                    targets["attack"] = tile;
                    break;
                }
            }

            Ability fireball = GlobalAbilityDictionary.GetAbility("fireball");
            foreach (Vector3Int tile in fireball.GetWithinRange(self, world))
            {
                int count = 0;
                foreach (Vector3Int area in fireball.GetAreaOfEffect(self.Position, tile, world))
                {
                    UnitEntity unit = world.UnitManager.GetUnitAt<UnitEntity>(area);
                    if (unit != null && unit.Combat.IsEnemy(self.Combat))
                    {
                        count++;
                    }
                }

                float score = 2.5f * count;
                if (scores["fireball"] < score)
                {
                    scores["fireball"] = score;
                    targets["fireball"] = tile;
                }
            }
        }
    }
}
