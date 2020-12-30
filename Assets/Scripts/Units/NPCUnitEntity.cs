using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Intelligence;

namespace Units
{
    public class NPCUnitEntity : UnitEntity
    {
        //TODO: change back to private
        public readonly NPCIntelligence ai;

        public NPCUnitEntity(string name, Vector3Int position, NPCIntelligence ai, UnitEntityManager manager, UnitEntityScript script) : base(name, false, position, manager, script)
        {
            this.ai = ai;
        }

        public void ExecuteTurn(GameMaster game)
        {
            ai.GetMovement(this, ai.GetTarget(this, game.World), game.World);
        }
    }
}