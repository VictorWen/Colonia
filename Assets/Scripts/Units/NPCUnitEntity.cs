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
        public NPCAbilityAI AbilityAI { get; private set; }
        public NPCMovementAI MovementAI {get; private set;}

        public NPCUnitEntity(string name, Vector3Int position, NPCMainAI mainAI, NPCAbilityAI abilityAI, NPCMovementAI moveAI, UnitEntityManager manager, UnitEntityScript script) : base(name, false, position, manager, script)
        {
            MainAI = mainAI;
            AbilityAI = abilityAI;
            MovementAI = moveAI;
        }

        public void ExecuteTurn(GameMaster game)
        {
            /*UnitEntity attackTarget = AbilityAI.GetAbilityTarget(this, game.World);
            Vector3Int moveTarget = AbilityAI.GetMovementTarget(this, attackTarget, game.World);
            LinkedList<Vector3Int> movement = MovementAI.GetAbilityMovement(this, moveTarget, game.World);

            foreach (Vector3Int motion in movement)
            {
                Debug.Log("NPC Entity motion: " + motion);
            }
            MoveTo(movement.Last.Value, game.World);

            Ability casting = AttackAI.GetAbilityTelegraph(this, attackTarget, game.World);
            if (casting != null)
            {
                CastAbility(casting, attackTarget.Position, game.World);
            }*/
        }
    }
}