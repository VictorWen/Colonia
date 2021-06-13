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

        [SerializeField] private float BASE_WANDER_VALUE = 1.5f;
        [SerializeField] private float RANDOM_SCALE = 2.5f;
        [SerializeField] private int MAX_WANDER_DISTANCE = 3;

        public Dictionary<Vector3Int, float> SurveyPositioning(UnitEntity self, World world)
        {
            Dictionary<Vector3Int, float> scores = new Dictionary<Vector3Int, float>();
            InitializePositioningScores(scores, self);

            CalculateTerrainScores(scores, self, world);
            CalculateTargetScores(scores, self, world);

            return scores;
        }

        public Dictionary<Vector3Int, float> SurveyWandering(UnitEntity self, World world)
        {
            Dictionary<Vector3Int, float> scores = new Dictionary<Vector3Int, float>();
            InitializePositioningScores(scores, self);

            CalculateTerrainScores(scores, self, world);
            CalculateDistanceIncrease(scores, self, world);
            CalculateRandomAdditions(scores, self, world);

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
                scores[tile] += TERRAIN_COMBAT_SCALE * world.GetTerrainTile(tile).combatModifier;
                scores[tile] += TERRAIN_SIGHT_SCALE * world.GetTerrainTile(tile).sightBonus;
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
                    CalculateDistanceReduction(scores, unit.Position, world);
                }
            }
        }

        private void CalculateDistanceReduction(Dictionary<Vector3Int, float> scores, Vector3Int position, World world)
        {
            List<List<Vector3Int>> ranges = world.GetRangeList(position, MAX_FOLLOW_DISTANCE);
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

        private void CalculateDistanceIncrease(Dictionary<Vector3Int, float> scores, UnitEntity self, World world)
        {
            List<List<Vector3Int>> ranges = world.GetRangeList(self.Position, MAX_WANDER_DISTANCE);
            for (int distance = 1; distance < ranges.Count; distance++)
            {
                foreach (Vector3Int tile in ranges[distance])
                {
                    if (scores.ContainsKey(tile))
                        scores[tile] += distance * distance * BASE_WANDER_VALUE;
                }
            }
        }

        private void CalculateRandomAdditions(Dictionary<Vector3Int, float> scores, UnitEntity self, World world)
        {
            foreach (Vector3Int tile in self.Movement.GetMoveables().Reachables)
            {
                scores[tile] += (2 * (float) world.RNG.NextDouble() - 1) * RANDOM_SCALE;
            }
        }
    }
}
