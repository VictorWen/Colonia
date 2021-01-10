using System.Collections.Generic;
using Units.Intelligence;
using Units.Abilities;
using UnityEngine;

namespace Units
{
    public class NPCUnitEntity : UnitEntity
    {
        //TODO: change back to private
        public NPCTargetingAI TargetingAI { get; private set; }
        public NPCMovementAI MovementAI {get; private set;}
        public NPCAttackAI AttackAI { get; private set; }

        public NPCUnitEntity(string name, Vector3Int position, NPCMovementAI ai, UnitEntityManager manager, UnitEntityScript script) : base(name, false, position, manager, script)
        {
            //this.ai = ai;
        }

        public void ExecuteTurn(GameMaster game)
        {
            UnitEntity attackTarget = TargetingAI.GetAttackTarget(this, game.World);
            Vector3Int moveTarget = TargetingAI.GetMovementTarget(this, attackTarget, game.World);
            LinkedList<Vector3Int> movement = MovementAI.GetMovementAction(this, moveTarget, game.World);
            foreach (Vector3Int motion in movement)
            {
                Debug.Log("NPC Entity motion: " + motion);
            }
            MoveTo(movement.Last.Value, game.World);
            Ability casting = AttackAI.GetAbilityTelegraph(this, attackTarget, game.World);
            if (casting != null)
            {
                CastAbility(casting, attackTarget.Position, game.World);
            }
        }
    }
}