using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Units.Intelligence
{
    public class SimpleSurveyer : INPCSurveyer
    {
        //TODO: replace with ScriptableObject
        [SerializeField] private float TERRAIN_COMBAT_SCALE = 3f;
        [SerializeField] private float TERRAIN_SIGHT_SCALE = 3f;

        [SerializeField] private float BASE_TARGET_VALUE = 10f;
        [SerializeField] private int MAX_FOLLOW_DISTANCE = 3;

        public override Dictionary<Vector3Int, float> SurveyPositioning(UnitEntity self, World world)
        {
            Dictionary<Vector3Int, float> scores = new Dictionary<Vector3Int, float>();
            InitializePositioningScores(scores, self);

            CalculateTerrainScores(scores, self, world);
            CalculateTargetScores(scores, self, world);

            return scores;
        }

        private void InitializePositioningScores(Dictionary<Vector3Int, float> scores, UnitEntity self)
        {
            BFSPathfinder movement = self.Movement.GetMoveables();
            scores.Add(self.Position, 0);
            foreach (Vector3Int tile in movement.Reachables)
                scores.Add(tile, 0);
        }

        private void CalculateTerrainScores(Dictionary<Vector3Int, float> scores, UnitEntity self, World world)
        {
            List<Vector3Int> tiles = new List<Vector3Int>(scores.Keys);
            foreach (Vector3Int tile in tiles)
            {
                scores[tile] += TERRAIN_COMBAT_SCALE * ((TerrainTile)world.terrain.GetTile(tile)).combatModifier;
                scores[tile] += TERRAIN_SIGHT_SCALE * ((TerrainTile)world.terrain.GetTile(tile)).sightBonus;
            }
        }

        private void CalculateTargetScores(Dictionary<Vector3Int, float> scores, UnitEntity self, World world)
        {
            HashSet<Vector3Int> visibles = self.Visibles;
            foreach (Vector3Int visible in visibles)
            {
                UnitEntity unit = world.UnitManager.GetUnitAt<UnitEntity>(visible);
                if (unit != null && unit.Combat.IsEnemy(self.Combat))
                {
                    List<List<Vector3Int>> ranges = world.GetRangeList(unit.Position, MAX_FOLLOW_DISTANCE);
                    for (int distance = 1; distance < ranges.Count; distance++)
                    {
                        foreach (Vector3Int tile in ranges[distance])
                        {
                            if (scores.ContainsKey(tile))
                            {
                                scores[tile] += BASE_TARGET_VALUE / distance / distance;
                            }
                        }
                    }
                }
            }
        }
    }
}
