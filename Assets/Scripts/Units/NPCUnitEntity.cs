using System.Collections.Generic;
using Units.Intelligence;
using Units.Abilities;
using UnityEngine;

namespace Units
{
    public class NPCUnitEntity : UnitEntity
    {
        //TODO: change back to private
        public NPCMainAI MainAI { get; private set; }
        public NPCTargetingAI TargetingAI { get; private set; }
        public NPCMovementAI MovementAI {get; private set;}

        protected override void Awake()
        {
            MainAI = GetComponent<NPCMainAI>();
            TargetingAI = GetComponent<NPCTargetingAI>();
            MovementAI = GetComponent<NPCMovementAI>();
            base.Awake();
        }

        public void ExecuteTurn(GameMaster game)
        {
            UpdateVision(game.World);
            AIState aiState = MainAI.GetState(this, game.World);
            
            switch(aiState)
            {
                case AIState.ABILITY:
                    Ability ability = MainAI.GetNextAbility(this, game.World);
                    Vector3Int abilityTarget = TargetingAI.GetAbilityTarget(this, ability, game.World);
                    Vector3Int maneuverTarget = TargetingAI.GetManeuverTarget(this, ability, abilityTarget, game.World);

                    ExecuteMovement(MovementAI.GetAbilityMovement(this, ability, maneuverTarget, game.World), game.World);

                    bool targetWithinRange = ability.GetWithinRange(this, game.World).Contains(abilityTarget);
                    if (ability != null && targetWithinRange)
                    {
                        CastAbility(ability, abilityTarget, game.World);
                    }

                    break;
                case AIState.RETREAT:
                    Vector3Int retreatTarget = TargetingAI.GetRetreatTarget(this, game.World);
                    ExecuteMovement(MovementAI.GetRetreatMovement(this, retreatTarget, game.World), game.World);

                    break;
                case AIState.WANDER:
                    Vector3Int wanderTarget = TargetingAI.GetWanderTarget(this, game.World);
                    ExecuteMovement(MovementAI.GetWanderMovement(this, wanderTarget, game.World), game.World);

                    break;
            }
        }

        private void ExecuteMovement(LinkedList<Vector3Int> movement, World world)
        {
            foreach (Vector3Int motion in movement)
            {
                Debug.Log("NPC Entity motion: " + motion);
            }
            MoveTo(movement.Last.Value, world);
        }
    }
}