using System.Collections.Generic;
using Units.Intelligence;
using Units.Abilities;
using UnityEngine;
using Units.Movement;
using Units.Combat;
using Tiles;

namespace Units
{
    public class NPCIntelligence : MonoBehaviour, INPCCombatAI
    {
        public GUIMaster gui;

        private INPCStateMachine stateMachine;
        [SerializeField] private INPCSurveyer surveyer = null;
        [SerializeField] private INPCPlanner planner = null;
        private UnitEntity unit;

        private void Start()
        {
            // TODO: Placeholder?
            unit = GetComponent<UnitEntityController>().Unit;

            gui.AddNPCIntelligence(this);
        }

        public void ExecuteCombat(GameMaster game)
        {
            Dictionary<Vector3Int, float> positioningScores = surveyer.SurveyPositioning(unit, game.World);
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