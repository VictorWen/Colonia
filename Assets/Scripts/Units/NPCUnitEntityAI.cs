using System.Collections.Generic;
using Units.Intelligence;
using Units.Abilities;
using UnityEngine;
using Units.Movement;
using Units.Combat;

namespace Units
{
    public class NPCUnitEntityAI 
    {

        private NPCMainAI mainAI;
        private NPCTargetingAI targetAI;
        private NPCMovementAI moveAI;

        public Vector3Int Position { get { return new Vector3Int(); } }
        public HashSet<Vector3Int> Visibles { get { return new HashSet<Vector3Int>(); } }

        public IUnitEntityMovement Movement { get; private set; }
        public IUnitEntityCombat Combat { get; private set; }

        public NPCUnitEntityAI(IUnitEntityMovement movement, UnitEntityCombat combat, NPCMainAI mainAI, NPCTargetingAI targetAI, NPCMovementAI moveAI)
        {
            this.Movement = movement;
            this.Combat = combat;
            //this.world = world;
            this.mainAI = mainAI;
            this.targetAI = targetAI;
            this.moveAI = moveAI;
        }

        public void ExecuteTurn(GameMaster game)
        {
            //movement.UpdateVision();
            AIState aiState = mainAI.GetState(this, game.World);
            
            switch(aiState)
            {
                case AIState.ABILITY:
                    Ability ability = mainAI.GetNextAbility(this, game.World);
                    Vector3Int abilityTarget = targetAI.GetAbilityTarget(this, ability, game.World);
                    Vector3Int maneuverTarget = targetAI.GetManeuverTarget(this, ability, abilityTarget, game.World);

                    //ExecuteMovement(moveAI.GetAbilityMovement(Movement, ability, maneuverTarget, game.World), game.World);

/*                    bool targetWithinRange = ability.GetWithinRange(this, game.World).Contains(abilityTarget);
                    if (ability != null && targetWithinRange)
                    {
                        CastAbility(ability, abilityTarget, game.World);
                    }*/

                    break;
                case AIState.RETREAT:
                    Vector3Int retreatTarget = targetAI.GetRetreatTarget(this, game.World);
                    //ExecuteMovement(moveAI.GetRetreatMovement(Movement, retreatTarget, game.World), game.World);

                    break;
                case AIState.WANDER:
                    Vector3Int wanderTarget = targetAI.GetWanderTarget(this, game.World);
                    //ExecuteMovement(moveAI.GetWanderMovement(Movement, wanderTarget, game.World), game.World);

                    break;
            }
        }

        private void ExecuteMovement(LinkedList<Vector3Int> movement, World world)
        {
            foreach (Vector3Int motion in movement)
            {
                Debug.Log("NPC Entity motion: " + motion);
            }
            //this.Movement.MoveTo(movement.Last.Value);
        }
    }
}