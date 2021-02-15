using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Units.Movement;
using Units.Combat;

namespace Units
{
    public class UnitEntityManager
    {
        public List<UnitEntityData> Units { get; private set; }
        public List<NPCUnitEntityAI> NPCUnits { get; private set; }

        private readonly World world;
        private readonly Dictionary<Vector3Int, UnitEntityData> positions;

        public struct UnitEntityData
        {
            public readonly UnitEntityMovement movement;
            public readonly UnitEntityCombat combat;

            public UnitEntityData(UnitEntityMovement movement, UnitEntityCombat combat)
            {
                this.movement = movement;
                this.combat = combat;
            }
        }

        public UnitEntityManager(World world)
        {
            this.world = world;
            positions = new Dictionary<Vector3Int, UnitEntityData>();
            Units = new List<UnitEntityData>();
            NPCUnits = new List<NPCUnitEntityAI>();
        }

        public void AddUnit(UnitEntityMovement movement, UnitEntityCombat combat, NPCUnitEntityAI ai = null)
        {
            UnitEntityData data = new UnitEntityData(movement, combat);
            Units.Add(data);
            positions.Add(movement.Position, data);

            if (ai != null)
                NPCUnits.Add(ai);

            //unit.SetUnitManager(this);
        }

        public void RemoveUnit(Vector3Int position)
        {
            Units.Remove(positions[position]);
            positions.Remove(position);
        }

        public void UpdateUnitPosition(Vector3Int previousPos, Vector3Int newPos)
        {
            UnitEntityData data = positions[previousPos];
            positions.Remove(previousPos);
            positions.Add(newPos, data);
        }

        public bool GetUnitAt(Vector3Int position, out UnitEntityData data)
        {
            return positions.TryGetValue(position, out data);
        }

        public void NextTurn(GameMaster game)
        {
            foreach (UnitEntityData data in Units)
            {
                data.movement.OnNextTurn(game);
            }
        }

        public void ExecuteNPCTurns(GameMaster game)
        {
            foreach (NPCUnitEntityAI ai in NPCUnits)
            {
                ai.ExecuteTurn(game);
            }
        }
    }
}
