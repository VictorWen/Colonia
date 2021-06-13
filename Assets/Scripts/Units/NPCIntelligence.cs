using System.Collections.Generic;
using Units.Intelligence;
using Units.Abilities;
using UnityEngine;
using Units.Movement;
using Units.Combat;
using Tiles;

namespace Units
{
    public class NPCIntelligence : INPCCombatAI
    {
        private GameMaster game;

        [SerializeField] private INPCStateMachine stateMachine;
        [SerializeField] private INPCSurveyer surveyer = null;
        [SerializeField] private INPCPlanner planner = null;
        private UnitEntity unit;

        private Vector3Int homeSpawner;

        public NPCIntelligence(GameMaster game, INPCStateMachine sm, INPCSurveyer surveyer, INPCPlanner planner, Vector3Int home)
        {
            this.game = game;
            this.game.npcList.Add(this);

            this.stateMachine = sm;
            this.surveyer = surveyer;
            this.planner = planner;

            homeSpawner = home;
        }

        public void AssignUnitEntity(UnitEntity unitEntity)
        {
            if (unit != null)
                unit.OnDeath -= () => game.npcList.Remove(this);
            unit = unitEntity;
            unit.OnDeath += () => game.npcList.Remove(this);
        }

        public void ExecuteCombat(GameMaster game)
        {
            Dictionary<Vector3Int, float> positioningScores;
            switch (stateMachine.GetState(unit, game.World)) {
                case AIState.COMBAT:                
                    positioningScores = surveyer.SurveyPositioning(unit, game.World);
                    break;
                case AIState.WANDER:
                    positioningScores = surveyer.SurveyWandering(unit, game.World);
                    break;
                default:
                    positioningScores = new Dictionary<Vector3Int, float>();
                    break;
            }
            LinkedList<Vector3Int> movementPath = planner.GetMovementPath(unit, positioningScores);

            if (movementPath != null)
            {
                foreach (Vector3Int tile in movementPath)
                {
                    unit.MoveTo(tile);
                }
            }

            planner.ExecuteAbility(unit, game.World);
        }

        public List<Vector3Int> GetTelegraph()
        {
            throw new System.NotImplementedException();
        }
    }
}