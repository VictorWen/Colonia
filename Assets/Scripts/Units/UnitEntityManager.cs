using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Units
{
    public class UnitEntityManager
    {
        public List<UnitEntity> Units { get; private set; }
        public List<NPCUnitEntity> NPCUnits { get; private set; }

        private readonly World world;
        private readonly Dictionary<Vector3Int, UnitEntity> positions;

        public UnitEntityManager(World world)
        {
            this.world = world;
            positions = new Dictionary<Vector3Int, UnitEntity>();
            Units = new List<UnitEntity>();
            NPCUnits = new List<NPCUnitEntity>();
        }

        public void AddUnit(UnitEntity unit)
        {
            Units.Add(unit);
            if (unit is NPCUnitEntity npc)
                NPCUnits.Add(npc);
            positions.Add(GetPositionFor(unit), unit);
            unit.SetUnitManager(this);
        }

        public void RemoveUnit(UnitEntity unit)
        {
            Units.Remove(unit);
            if (unit is NPCUnitEntity npc)
                NPCUnits.Remove(npc);
            positions.Remove(GetPositionFor(unit));
        }

        public void SetUnitPosition(UnitEntity unit, Vector3Int destination)
        {
            positions.Remove(GetPositionFor(unit));
            positions.Add(destination, unit);

            unit.transform.position = world.grid.CellToWorld(destination);
        }

        public UnitEntity GetUnitAt(Vector3Int position)
        {
            if (positions.ContainsKey(position))
                return positions[position];
            return null;
        }

        public Vector3Int GetPositionFor(UnitEntity unit)
        {
            return world.grid.WorldToCell(unit.transform.position);
        }
    }
}
